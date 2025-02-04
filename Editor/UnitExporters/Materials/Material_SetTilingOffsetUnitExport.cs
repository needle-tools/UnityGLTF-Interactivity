using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityGLTF.Interactivity.Schema;

namespace UnityGLTF.Interactivity.Export
{
    public class MaterialSetTilingOffsetNode : IUnitExporter
    {
        public Type unitType { get => typeof(SetMember); }
        private string property;
        
        [InitializeOnLoadMethod]
        private static void Register()
        {
            InvokeUnitExport.RegisterInvokeExporter(typeof(Material), nameof(Material.SetTextureOffset), new MaterialSetTilingOffsetNode("offset"));
            InvokeUnitExport.RegisterInvokeExporter(typeof(Material), nameof(Material.SetTextureScale), new MaterialSetTilingOffsetNode("scale"));
            SetMemberUnitExport.RegisterMemberExporter(typeof(Material), nameof(Material.mainTextureOffset), new MaterialSetTilingOffsetNode("offset"));
            SetMemberUnitExport.RegisterMemberExporter(typeof(Material), nameof(Material.mainTextureScale), new MaterialSetTilingOffsetNode("scale"));
        }
        
        public MaterialSetTilingOffsetNode(string property)
        {
            this.property = property;
        }
        
        public void InitializeInteractivityNodes(UnitExporter unitExporter)
        {
            // Regular pointer/set
            var node = unitExporter.CreateNode(new Pointer_SetNode());
            ValueInput target = null;
            
            if (unitExporter.unit is SetMember setMember)
            {
                target = setMember.target;
                unitExporter.MapInputPortToSocketName(setMember.assign, Pointer_SetNode.IdFlowIn, node);
                unitExporter.MapInputPortToSocketName(setMember.input, Pointer_SetNode.IdValue, node);
                unitExporter.MapOutFlowConnectionWhenValid(setMember.assigned, Pointer_SetNode.IdFlowOut, node);
            }
            else if (unitExporter.unit is InvokeMember invokeMember)
            {
                target = invokeMember.target;
                unitExporter.MapInputPortToSocketName(invokeMember.enter, Pointer_SetNode.IdFlowIn, node);
                // first parameter is the color property name – so based on that we can determine what pointer to set
                // var colorPropertyName = invokeMember.inputParameters[0];
                unitExporter.MapInputPortToSocketName(invokeMember.inputParameters[1], Pointer_SetNode.IdValue, node);
                unitExporter.MapOutFlowConnectionWhenValid(invokeMember.exit, Pointer_SetNode.IdFlowOut, node);
            }

            node.SetupPointerTemplateAndTargetInput(
                GltfInteractivityNodeHelper.IdPointerMaterialIndex,
                target, "/materials/{" + GltfInteractivityNodeHelper.IdPointerMaterialIndex + "}/pbrMetallicRoughness/baseColorTexture/extensions/KHR_texture_transform/"+property,
                GltfTypes.Float2);
        }
    }
}