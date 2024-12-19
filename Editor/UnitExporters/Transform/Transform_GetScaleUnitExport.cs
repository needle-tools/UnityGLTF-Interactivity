using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityGLTF.Interactivity.Schema;

namespace UnityGLTF.Interactivity.Export
{
    public class Transform_GetScaleUnitExport : IUnitExporter
    {
        public Type unitType { get; }
        private bool worldSpace = false;
        
        
        [InitializeOnLoadMethod]
        private static void Register()
        {
            GetMemberUnitExport.RegisterMemberExporter(typeof(Transform), nameof(Transform.lossyScale), new Transform_GetScaleUnitExport(true));
            GetMemberUnitExport.RegisterMemberExporter(typeof(Transform), nameof(Transform.localScale), new Transform_GetScaleUnitExport(false));
        }
        
        public Transform_GetScaleUnitExport(bool worldSpace)
        {
            this.worldSpace = worldSpace;
        }
        
        public void InitializeInteractivityNodes(UnitExporter unitExporter)
        {
            // TODO: World Space conversion
            
           var unit = unitExporter.unit as Unity.VisualScripting.GetMember;
           
           var getScale = unitExporter.CreateNode(new Pointer_GetNode());
           
           unitExporter.SetupPointerTemplateAndTargetInput(GltfInteractivityNodeHelper.IdPointerNodeIndex,
               unit.target, getScale,
               "/nodes/{" + GltfInteractivityNodeHelper.IdPointerNodeIndex + "}/scale");
           
           getScale.OutValueSocket[Pointer_GetNode.IdValue].expectedType = ExpectedType.GtlfType("float3");

           unitExporter.MapValueOutportToSocketName(unit.value, Pointer_GetNode.IdValue, getScale);
  
        }
    }
}