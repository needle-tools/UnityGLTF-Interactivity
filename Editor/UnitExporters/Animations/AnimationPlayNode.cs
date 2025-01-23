using UnityEditor;
using UnityEngine;
using UnityEditor.Animations;
using Unity.VisualScripting;
using UnityGLTF.Interactivity.Schema;

namespace UnityGLTF.Interactivity.Export
{
    internal class AnimationPlayNode : IUnitExporter
    {
        public System.Type unitType
        {
            get => typeof(InvokeMember);
        }

        private readonly string _stateNameKey = "%stateName";

        [InitializeOnLoadMethod]
        private static void Register()
        {
            InvokeUnitExport.RegisterInvokeExporter(typeof(Animation), nameof(Animation.Play),
                new AnimationPlayNode());
        }

        public void InitializeInteractivityNodes(UnitExporter unitExporter)
        {
            InvokeMember unit = unitExporter.unit as InvokeMember;
            GltfInteractivityUnitExporterNode node = unitExporter.CreateNode(new Animation_StartNode());

            GameObject target = GltfInteractivityNodeHelper.GetGameObjectFromValueInput(
                unit.target, unit.defaultValues, unitExporter.exportContext);

            if (target == null)
            {
                UnitExportLogging.AddErrorLog(unit, "Can't resolve target GameObject");
                return;
            }

            // Get the state name from the node
            if (!GltfInteractivityNodeHelper.GetDefaultValue<string>(unit, _stateNameKey, out string stateName))
            {
                UnitExportLogging.AddErrorLog(unit, "Invalid state name.");
                return;
            }
            
            AnimatorState animationState = AnimatorHelper.GetAnimationState(target, stateName);
            int animationId = AnimatorHelper.GetAnimationId(animationState, unitExporter.exportContext);

            if (animationId == -1)
            {
                UnitExportLogging.AddErrorLog(unit, "Animation not found in export context.");
                return;
            }

            node.ValueSocketConnectionData[Animation_StartNode.IdValueAnimation].Value = animationId;
            node.ValueSocketConnectionData[Animation_StartNode.IdValueSpeed].Value = animationState.speed;

            // TODO: Get from clip start from state cycleOffset
            node.ValueSocketConnectionData[Animation_StartNode.IdValueStartTime].Value = 0.0f;

            AnimationClip clip = animationState.motion as AnimationClip;
            node.ValueSocketConnectionData[Animation_StartNode.IdValueEndtime].Value =
                (clip != null && !clip.isLooping) ? clip.length : float.PositiveInfinity;
            
            unitExporter.MapInputPortToSocketName(unit.enter, Animation_StartNode.IdFlowIn, node);
            // There should only be one output flow from the Animator.Play node
            unitExporter.MapOutFlowConnectionWhenValid(unit.exit, Animation_StartNode.IdFlowOut, node);
        }
        
    }
}