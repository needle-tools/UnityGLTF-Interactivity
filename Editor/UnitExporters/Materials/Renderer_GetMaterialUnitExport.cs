using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityGLTF.Interactivity.Schema;

namespace UnityGLTF.Interactivity.Export
{
    public class Renderer_GetMaterialUnitExport : IUnitExporter
    {
        public Type unitType { get; }
        
        [InitializeOnLoadMethod]
        private static void Register()
        {
            GetMemberUnitExport.RegisterMemberExporter(typeof(Renderer), nameof(Renderer.material), new Renderer_GetMaterialUnitExport());
        }
        
        public void InitializeInteractivityNodes(UnitExporter unitExporter)
        {
            var unit = unitExporter.unit as GetMember;
     
            var getMesh = unitExporter.CreateNode(new Pointer_GetNode());
         
            unitExporter.SetupPointerTemplateAndTargetInput(GltfInteractivityNodeHelper.IdPointerNodeIndex, 
                unit.target, getMesh,
                "/nodes/{" + GltfInteractivityNodeHelper.IdPointerNodeIndex + "}/mesh");
            
            var getMaterial = unitExporter.CreateNode(new Pointer_GetNode());
            unitExporter.MapInputPortToSocketName(Pointer_GetNode.IdValue, getMesh, GltfInteractivityNodeHelper.IdPointerMeshIndex, getMaterial);
            
            // TODO: support multiple materials/primitives
            unitExporter.SetupPointerTemplateAndTargetInput(GltfInteractivityNodeHelper.IdPointerMeshIndex,
                getMaterial,
                "/meshes/{" + GltfInteractivityNodeHelper.IdPointerMeshIndex + "}/primitives/0/material");
            
            unitExporter.MapValueOutportToSocketName(unit.value, Pointer_GetNode.IdValue, getMaterial);
        
        }
    }
}