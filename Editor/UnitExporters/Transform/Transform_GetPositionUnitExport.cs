using System;
using UnityEditor;
using UnityEngine;
using UnityGLTF.Interactivity.Schema;

namespace UnityGLTF.Interactivity.Export
{
    public class Transform_GetPositionUnitExport : IUnitExporter
    {
        public Type unitType { get; }
        private bool worldSpace = false;
        
        
        [InitializeOnLoadMethod]
        private static void Register()
        {
            GetMemberUnitExport.RegisterMemberExporter(typeof(Transform), nameof(Transform.position), new Transform_GetPositionUnitExport(true));
            GetMemberUnitExport.RegisterMemberExporter(typeof(Transform), nameof(Transform.localPosition), new Transform_GetPositionUnitExport(false));
        }
        
        public Transform_GetPositionUnitExport(bool worldSpace)
        {
            this.worldSpace = worldSpace;
        }
        
        public void InitializeInteractivityNodes(UnitExporter unitExporter)
        {
            // TODO: World Space conversion
            
           var unit = unitExporter.unit as Unity.VisualScripting.GetMember;
           
           var getPosition = unitExporter.CreateNode(new Pointer_GetNode());
           getPosition.OutValueSocketConnectionData[Pointer_GetNode.IdValue].expectedType = ExpectedType.GtlfType("float3");
           unitExporter.MapValueOutportToSocketName(unit.value, Pointer_GetNode.IdValue, getPosition);

           if (GltfInteractivityNodeHelper.IsMainCameraInInput(unit))
           {
               GltfInteractivityNodeHelper.AddPointerConfig(getPosition, "/activeCamera/position");
               return;
           }
           
           unitExporter.SetupPointerTemplateAndTargetInput(GltfInteractivityNodeHelper.IdPointerNodeIndex,
               unit.target, getPosition,
               "/nodes/{" + GltfInteractivityNodeHelper.IdPointerNodeIndex + "}/translation");
           
        }
    }
}