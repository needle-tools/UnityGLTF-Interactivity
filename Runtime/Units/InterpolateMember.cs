using System.Collections.Generic;
using System.Reflection;
using JetBrains.Annotations;
using Unity.VisualScripting;

namespace UnityGLTF.Interactivity.Units
{
    /// <summary>
    /// Interpolates the value of a field or property over time via reflection.
    /// </summary>
    // [SpecialUnit]
    public sealed class InterpolateMember : MemberUnit
    {
        public InterpolateMember() : base() { }

        public InterpolateMember(Member member) : base(member) { }

        /// <summary>
        /// Whether the target should be output to allow for chaining.
        /// </summary>
        [Serialize]
        [InspectableIf(nameof(supportsChaining))]
        public bool chainable { get; set; }

        [DoNotSerialize]
        public bool supportsChaining => member.requiresTarget;

        [DoNotSerialize]
        [Unity.VisualScripting.MemberFilter(Fields = true, Properties = true, ReadOnly = false)]
        public Member setter
        {
            get => member;
            set => member = value;
        }

        [DoNotSerialize]
        [PortLabelHidden]
        public ControlInput assign { get; private set; }

        [DoNotSerialize]
        [PortLabel("Value")]
        [PortLabelHidden]
        public ValueInput input { get; private set; }

        [DoNotSerialize]
        [PortLabel("Duration")]
        [PortLabelHidden]
        public ValueInput duration { get; private set; }
        
        /// <summary>
        /// The target object used when setting the value.
        /// </summary>
        [DoNotSerialize]
        [PortLabel("Target")]
        [PortLabelHidden]
        public ValueOutput targetOutput { get; private set; }

        [DoNotSerialize]
        [PortLabelHidden]
        public ControlOutput assigned { get; private set; }

        protected override void Definition()
        {
            base.Definition();

            assign = ControlInput(nameof(assign), Assign);
            assigned = ControlOutput(nameof(assigned));
            Succession(assign, assigned);

            if (supportsChaining && chainable)
            {
                targetOutput = ValueOutput(member.targetType, nameof(targetOutput));
                Assignment(assign, targetOutput);
            }

            if (member.requiresTarget)
            {
                Requirement(target, assign);
            }

            input = ValueInput(member.type, nameof(input));
            Requirement(input, assign);

            if (member.allowsNull)
            {
                input.AllowsNull();
            }

            input.SetDefaultValue(member.type.PseudoDefault());
        }

        protected override bool IsMemberValid(Member member)
        {
            return member.isAccessor && member.isSettable;
        }

        private object GetAndChainTarget(Flow flow)
        {
            if (member.requiresTarget)
            {
                var target = flow.GetValue(this.target, member.targetType);

                if (supportsChaining && chainable)
                {
                    flow.SetValue(targetOutput, target);
                }

                return target;
            }

            return null;
        }

        private ControlOutput Assign(Flow flow)
        {
            var target = GetAndChainTarget(flow);

            var value = flow.GetConvertedValue(input);

            member.Set(target, value);

            return assigned;
        }

        #region Analytics

        public override AnalyticsIdentifier GetAnalyticsIdentifier()
        {
            var aid = new AnalyticsIdentifier
            {
                Identifier = $"{member.targetType.FullName}.{member.name}(Interpolate)",
                Namespace = member.targetType.Namespace,
            };
            aid.Hashcode = aid.Identifier.GetHashCode();
            return aid;
        }

        #endregion
    }
    
    [InitializeAfterPlugins]
    [UsedImplicitly]
    public static class AddToFinderExtension
    {
        static AddToFinderExtension()
        {
            UnitBase.staticUnitsExtensions.Add(GetStaticOptions);
        }

        private static IEnumerable<IUnitOption> GetStaticOptions()
        {
            CodebaseSubset codebase = typeof(UnitBase).GetField("codebase", (BindingFlags)(-1)).GetValue(null) as CodebaseSubset;
            
            foreach (var member in codebase.members)
            {
                foreach (var memberOption in GetMemberOptions(member))
                {
                    yield return memberOption;
                }
            }
        }
        
        private static IEnumerable<IUnitOption> GetMemberOptions(Member member)
        {
            // Operators are handled with special math units
            // that are more elegant than the raw methods
            if (member.isOperator)
            {
                yield break;
            }

            // Conversions are handled automatically by connections
            if (member.isConversion)
            {
                yield break;
            }
            
            // Interpolation is only available for these types:
            // int, float, Vector2, Vector3, Vector4, Quaternion, Color, Matrix4x4
            
            if (member.type != typeof(int) &&
                member.type != typeof(float) &&
                member.type != typeof(UnityEngine.Vector2) &&
                member.type != typeof(UnityEngine.Vector3) &&
                member.type != typeof(UnityEngine.Vector4) &&
                member.type != typeof(UnityEngine.Quaternion) &&
                member.type != typeof(UnityEngine.Color) &&
                member.type != typeof(UnityEngine.Matrix4x4))
            {
                yield break;
            }
            
            // If the declaring type is Vector2, Vector3, Vector4, Quaternion, Color, or Matrix4x4, we can't interpolate
            // e.g. we can't interpolate Position.x, we can only interpolate the whole Position
            if (member.declaringType == typeof(UnityEngine.Vector2) ||
                member.declaringType == typeof(UnityEngine.Vector3) ||
                member.declaringType == typeof(UnityEngine.Vector4) ||
                member.declaringType == typeof(UnityEngine.Quaternion) ||
                member.declaringType == typeof(UnityEngine.Color) ||
                member.declaringType == typeof(UnityEngine.Matrix4x4) ||
                // these are basically wrappers around the above
                member.declaringType == typeof(UnityEngine.Bounds) ||
                member.declaringType == typeof(UnityEngine.BoundsInt) ||
                member.declaringType == typeof(UnityEngine.Rect))
            {
                yield break;
            }
            
            // Transform is a special case; we only want to allow localPosition, localRotation, and localScale
            if ((member.declaringType == typeof(UnityEngine.Transform) || member.declaringType == typeof(UnityEngine.RectTransform)) &&
                member.name != "localPosition" &&
                member.name != "localRotation" &&
                member.name != "localScale")
            {
                yield break;
            }

            if (member.isAccessor)
            {
                if (member.isPubliclySettable)
                {
                    yield return new InterpolateMember(member).Option();
                }
            }
        }
    }
}
