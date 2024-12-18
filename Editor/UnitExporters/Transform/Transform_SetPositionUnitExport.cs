using System;
using UnityEditor;
using UnityEngine;
using UnityGLTF.Interactivity.Schema;

namespace UnityGLTF.Interactivity.Export
{
    public class Transform_SetPositionUnitExport : IUnitExporter
    {
        public Type unitType { get; }
        private bool worldSpace = false;
        
        
        [InitializeOnLoadMethod]
        private static void Register()
        {
            SetMemberUnitExport.RegisterMemberConvert(typeof(Transform), nameof(Transform.position), new Transform_SetPositionUnitExport(true));
            SetMemberUnitExport.RegisterMemberConvert(typeof(Transform), nameof(Transform.localPosition), new Transform_SetPositionUnitExport(false));
        }
        
        public Transform_SetPositionUnitExport(bool worldSpace)
        {
            this.worldSpace = worldSpace;
        }
        
        public void InitializeInteractivityNodes(UnitExporter unitExporter)
        {
            // TODO: World Space conversion
            
           var unit = unitExporter.unit as Unity.VisualScripting.SetMember;
           
           var setPosition = unitExporter.CreateNode(new Pointer_SetNode());
           
           unitExporter.SetupPointerTemplateAndTargetInput(GltfInteractivityNodeHelper.IdPointerNodeIndex,
               unit.target, setPosition,
               "/nodes/{" + GltfInteractivityNodeHelper.IdPointerNodeIndex + "}/translation");
           
           
           unitExporter.MapInputPortToSocketName(unit.assign, Pointer_SetNode.IdFlowIn, setPosition);
           unitExporter.MapOutFlowConnectionWhenValid(unit.assigned, Pointer_SetNode.IdFlowOut, setPosition);
           unitExporter.MapInputPortToSocketName(unit.input, Pointer_SetNode.IdValue, setPosition);

        }
    }
}