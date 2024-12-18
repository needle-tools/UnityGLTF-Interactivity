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
            
            var node = unitExporter.CreateNode(new Pointer_SetNode());
            
            if (ValueInputHelper.TryGetValueInput(unit.valueInputs, "value", out var valueInput))
            {
                unitExporter.MapInputPortToSocketName(valueInput, Pointer_SetNode.IdValue, node);
            
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
                    }
                }
            }
            unitExporter.MapInputPortToSocketName(unit.enter, Pointer_SetNode.IdFlowIn, node);
            
            unitExporter.SetupPointerTemplateAndTargetInput(
                GltfInteractivityNodeHelper.IdPointerNodeIndex,
                unit.target, node,
                "/nodes/{" + GltfInteractivityNodeHelper.IdPointerNodeIndex + "}/extensions/KHR_node_visibility/visible"
                );
            
            unitExporter.MapOutFlowConnectionWhenValid(unit.exit, Pointer_SetNode.IdFlowOut, node);
        }
    }
}