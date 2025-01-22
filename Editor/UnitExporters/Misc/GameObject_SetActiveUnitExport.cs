using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityGLTF.Interactivity.Schema;

namespace UnityGLTF.Interactivity.Export
{
    public class GameObject_SetActiveUnitExport : IUnitExporter
    {
        public System.Type unitType { get => typeof( InvokeMember); }

        [InitializeOnLoadMethod]
        private static void Register()
        {
            InvokeUnitExport.RegisterInvokeExporter(typeof(GameObject), nameof(GameObject.SetActive),
                new GameObject_SetActiveUnitExport());
        }
        
        public void InitializeInteractivityNodes(UnitExporter unitExporter)
        {
            var unit = unitExporter.unit as InvokeMember;
            
            var visibleNode = unitExporter.CreateNode(new Pointer_SetNode());
            var selectableNode = unitExporter.CreateNode(new Pointer_SetNode());
            
            if (ValueInputHelper.TryGetValueInput(unit.valueInputs, "value", out var valueInput))
            {
                unitExporter.MapInputPortToSocketName(valueInput, Pointer_SetNode.IdValue, visibleNode);
                unitExporter.MapInputPortToSocketName(valueInput, Pointer_SetNode.IdValue, selectableNode);
            
                // Add Extension
                if (!unitExporter.IsInputLiteralOrDefaultValue(valueInput, out var defaultValue))
                {
                    unitExporter.exportContext.AddVisibilityExtensionToAllNodes();
                }
                else
                {
                    if (defaultValue is GameObject go && go != null)
                    {
                        var nodeIndex = unitExporter.exportContext.exporter.GetTransformIndex(go.transform);
                        unitExporter.exportContext.AddVisibilityExtensionToNode(nodeIndex);
                        unitExporter.exportContext.AddSelectabilityExtensionToNode(nodeIndex);
                    }
                }
            }

            visibleNode.FlowIn(Pointer_SetNode.IdFlowIn).MapToControlInput(unit.enter);
            
            visibleNode.SetupPointerTemplateAndTargetInput(
                GltfInteractivityNodeHelper.IdPointerNodeIndex,
                unit.target, "/nodes/{" + GltfInteractivityNodeHelper.IdPointerNodeIndex + "}/extensions/KHR_node_visibility/visible",
                GltfTypes.Bool);
            
            visibleNode.FlowOut(Pointer_SetNode.IdFlowOut)
                .ConnectToFlowDestination(selectableNode.FlowIn(Pointer_SetNode.IdFlowIn));
            selectableNode.SetupPointerTemplateAndTargetInput(
                GltfInteractivityNodeHelper.IdPointerNodeIndex,
                unit.target, "/nodes/{" + GltfInteractivityNodeHelper.IdPointerNodeIndex + "}/extensions/KHR_node_selectability/selectable",
                GltfTypes.Bool);
            
            selectableNode.FlowOut(Pointer_SetNode.IdFlowOut).MapToControlOutput(unit.exit);
        }
    }
}