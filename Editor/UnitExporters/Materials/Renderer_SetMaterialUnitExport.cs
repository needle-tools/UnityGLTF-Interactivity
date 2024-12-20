using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityGLTF.Interactivity.Schema;

namespace UnityGLTF.Interactivity.Export
{
    public class Renderer_SetMaterialUnitExport : IUnitExporter
    {
        public Type unitType { get; }
        
        [InitializeOnLoadMethod]
        private static void Register()
        {
            SetMemberUnitExport.RegisterMemberConvert(typeof(Renderer), nameof(Renderer.material), new Renderer_SetMaterialUnitExport());
        }
        
        public void InitializeInteractivityNodes(UnitExporter unitExporter)
        {
            var unit = unitExporter.unit as SetMember;

            var getMesh = unitExporter.CreateNode(new Pointer_GetNode());

            unitExporter.SetupPointerTemplateAndTargetInput(GltfInteractivityNodeHelper.IdPointerNodeIndex, 
                unit.target, getMesh,
                "/nodes/{" + GltfInteractivityNodeHelper.IdPointerNodeIndex + "}/mesh");
            
            var setMaterial = unitExporter.CreateNode(new Pointer_SetNode());
            unitExporter.MapInputPortToSocketName(Pointer_GetNode.IdValue, getMesh, GltfInteractivityNodeHelper.IdPointerMeshIndex, setMaterial);
            
            // TODO: support multiple materials/primitives
            unitExporter.SetupPointerTemplateAndTargetInput(GltfInteractivityNodeHelper.IdPointerMeshIndex,
                setMaterial,
                "/meshes/{" + GltfInteractivityNodeHelper.IdPointerMeshIndex + "}/primitives/0/material");
            
            unitExporter.MapInputPortToSocketName(unit.input, Pointer_SetNode.IdValue, setMaterial);

            unitExporter.MapInputPortToSocketName(unit.assign, Pointer_SetNode.IdFlowIn, setMaterial);
            unitExporter.MapValueOutportToSocketName(unit.assigned, Pointer_SetNode.IdFlowOut, setMaterial);
        }
    }
}