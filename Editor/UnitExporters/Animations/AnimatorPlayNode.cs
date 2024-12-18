using UnityEditor;
using UnityEngine;
using UnityEditor.Animations;
using Unity.VisualScripting;
using UnityGLTF.Interactivity.Schema;

namespace UnityGLTF.Interactivity.Export
{
    /// <summary>
    /// Adapts the "Animator.Play" InvokeMember Unity node to a world/startAnimation glTF node.
    /// </summary>
    internal class AnimatorPlayNode : IUnitExporter
    {
        public System.Type unitType
        {
            get => typeof(InvokeMember);
        }

        private readonly string _stateNameKey = "%stateName";

        [InitializeOnLoadMethod]
        private static void Register()
        {
            InvokeUnitExport.RegisterInvokeExporter(typeof(Animator), nameof(Animator.Play),
                new AnimatorPlayNode());
        }

        public void InitializeInteractivityNodes(UnitExporter unitExporter)
        {
            InvokeMember unit = unitExporter.unit as InvokeMember;
            GltfInteractivityUnitExporterNode node = unitExporter.CreateNode(new Animation_StartNode());

            GameObject target = GltfInteractivityNodeHelper.GetGameObjectFromValueInput(
                unit.target, unit.defaultValues, unitExporter.exportContext);

            if (target == null)
                return;

            // Get the state name from the node
            if (!GltfInteractivityNodeHelper.GetDefaultValue<string>(unit, _stateNameKey, out string stateName))
                return;
            
            AnimatorState animationState = AnimatorHelper.GetAnimationState(target, stateName);
            int animationId = AnimatorHelper.GetAnimationId(animationState, unitExporter.exportContext);

            if (animationId == -1)
                return;

            node.ValueSocketConnectionData[Animation_StartNode.IdValueAnimation].Value = animationId;
            node.ValueSocketConnectionData[Animation_StartNode.IdValueSpeed].Value = animationState.speed;

            // TODO: Get from clip start from state cycleOffset
            node.ValueSocketConnectionData[Animation_StartNode.IdValueStartTime].Value = 0.0f;

            AnimationClip clip = animationState.motion as AnimationClip;
            node.ValueSocketConnectionData[Animation_StartNode.IdValueEndtime].Value =
                clip != null ? clip.length : float.PositiveInfinity;
            
            unitExporter.MapInputPortToSocketName(unit.enter, Animation_StartNode.IdFlowIn, node);
            // There should only be one output flow from the Animator.Play node
            unitExporter.MapOutFlowConnectionWhenValid(unit.exit, Animation_StartNode.IdFlowOut, node);
        }
        
    }
}