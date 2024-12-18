using System;
using UnityEditor;
using UnityEngine;
using UnityGLTF.Interactivity.Schema;

namespace UnityGLTF.Interactivity.Export
{
    public class Transform_GetRotationUnitExport : IUnitExporter
    {
        public Type unitType { get; }
        private bool worldSpace = false;
        
        
        [InitializeOnLoadMethod]
        private static void Register()
        {
            GetMemberUnitExport.RegisterMemberExporter(typeof(Transform), nameof(Transform.rotation), new Transform_GetRotationUnitExport(true));
            GetMemberUnitExport.RegisterMemberExporter(typeof(Transform), nameof(Transform.localRotation), new Transform_GetRotationUnitExport(false));
        }
        
        public Transform_GetRotationUnitExport(bool worldSpace)
        {
            this.worldSpace = worldSpace;
        }
        
        public void InitializeInteractivityNodes(UnitExporter unitExporter)
        {
            // TODO: World Space conversion
            
           var unit = unitExporter.unit as Unity.VisualScripting.GetMember;
           
           var getRotation = unitExporter.CreateNode(new Pointer_GetNode());
           getRotation.OutValueSocketConnectionData[Pointer_GetNode.IdValue].expectedType = ExpectedType.GtlfType("float4");
           unitExporter.MapValueOutportToSocketName(unit.value, Pointer_GetNode.IdValue, getRotation);
           
           if (GltfInteractivityNodeHelper.IsMainCameraInInput(unit))
           {
               GltfInteractivityNodeHelper.AddPointerConfig(getRotation, "/activeCamera/rotation");
               return;
           }
           unitExporter.SetupPointerTemplateAndTargetInput(GltfInteractivityNodeHelper.IdPointerNodeIndex,
               unit.target, getRotation,
               "/nodes/{" + GltfInteractivityNodeHelper.IdPointerNodeIndex + "}/rotation");
           
        }
    }
}