using JetBrains.Annotations;
using Unity.VisualScripting;

namespace UnityGLTF.Interactivity.Units
{
    [FuzzyOption(typeof(InterpolateMember))]
    [UsedImplicitly]
    public class InterpolateMemberOption : MemberUnitOption<InterpolateMember>
    {
        public InterpolateMemberOption() : base() { }

        public InterpolateMemberOption(InterpolateMember unit) : base(unit) { }

        protected override ActionDirection direction => ActionDirection.Set;

        protected override bool ShowValueOutputsInFooter()
        {
            return false;
        }

        protected override string Label(bool human)
        {
            return base.Label(human).Replace("Set", "Interpolate");
        }

        protected override string Haystack(bool human)
        {
            return base.Haystack(human).Replace("Set", "Interpolate");
        }
    }
}