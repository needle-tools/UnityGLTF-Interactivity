using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityGLTF.Interactivity.Schema;

namespace UnityGLTF.Interactivity.Export
{
    public class Material_GetTilingOffsetUnitExport : IUnitExporter
    {
        public Type unitType { get => typeof(GetMember); }
        private string property;
        
        [InitializeOnLoadMethod]
        private static void Register()
        {
            InvokeUnitExport.RegisterInvokeExporter(typeof(Material), nameof(Material.GetTextureOffset), new Material_GetTilingOffsetUnitExport("offset"));
            InvokeUnitExport.RegisterInvokeExporter(typeof(Material), nameof(Material.GetTextureScale), new Material_GetTilingOffsetUnitExport("scale"));
            GetMemberUnitExport.RegisterMemberExporter(typeof(Material), nameof(Material.mainTextureOffset), new Material_GetTilingOffsetUnitExport("offset"));
            GetMemberUnitExport.RegisterMemberExporter(typeof(Material), nameof(Material.mainTextureScale), new Material_GetTilingOffsetUnitExport("scale"));
        }
        
        public Material_GetTilingOffsetUnitExport(string property)
        {
            this.property = property;
        }
        
        public void InitializeInteractivityNodes(UnitExporter unitExporter)
        {
            var node = unitExporter.CreateNode(new Pointer_GetNode());
            ValueInput target = null;
            
            if (unitExporter.unit is GetMember getMember)
            {
                target = getMember.target;
                unitExporter.MapValueOutportToSocketName(getMember.value, Pointer_GetNode.IdValue, node);
            }
            else if (unitExporter.unit is InvokeMember invokeMember)
            {
                target = invokeMember.target;
                
                unitExporter.ByPassFlow(invokeMember.enter, invokeMember.exit);
                unitExporter.MapValueOutportToSocketName(invokeMember.result, Pointer_GetNode.IdValue, node);
            }

            node.SetupPointerTemplateAndTargetInput(GltfInteractivityNodeHelper.IdPointerMaterialIndex, target, 
                "/materials/{" + GltfInteractivityNodeHelper.IdPointerMaterialIndex + "}/pbrMetallicRoughness/baseColorTexture/extensions/KHR_texture_transform/"+property,
                GltfTypes.Float2);
        }
    }
}