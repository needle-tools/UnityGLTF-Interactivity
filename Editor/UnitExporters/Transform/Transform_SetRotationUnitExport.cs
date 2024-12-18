using System;
using UnityEditor;
using UnityEngine;
using UnityGLTF.Interactivity.Schema;

namespace UnityGLTF.Interactivity.Export
{
    public class Transform_SetRotationUnitExport : IUnitExporter
    {
        public Type unitType { get; }
        private bool worldSpace = false;
        
        
        [InitializeOnLoadMethod]
        private static void Register()
        {
            SetMemberUnitExport.RegisterMemberConvert(typeof(Transform), nameof(Transform.rotation), new Transform_SetRotationUnitExport(true));
            SetMemberUnitExport.RegisterMemberConvert(typeof(Transform), nameof(Transform.localRotation), new Transform_SetRotationUnitExport(false));
        }
        
        public Transform_SetRotationUnitExport(bool worldSpace)
        {
            this.worldSpace = worldSpace;
        }
        
        public void InitializeInteractivityNodes(UnitExporter unitExporter)
        {
            // TODO: World Space conversion
            
           var unit = unitExporter.unit as Unity.VisualScripting.SetMember;
           
           var setRotation = unitExporter.CreateNode(new Pointer_SetNode());
           
           unitExporter.SetupPointerTemplateAndTargetInput(GltfInteractivityNodeHelper.IdPointerNodeIndex,
              unit.target, setRotation,
               "/nodes/{" + GltfInteractivityNodeHelper.IdPointerNodeIndex + "}/rotation");
           
           
           unitExporter.MapInputPortToSocketName(unit.assign, Pointer_SetNode.IdFlowIn, setRotation);
           unitExporter.MapOutFlowConnectionWhenValid(unit.assigned, Pointer_SetNode.IdFlowOut, setRotation);
           unitExporter.MapInputPortToSocketName(unit.input, Pointer_SetNode.IdValue, setRotation);

        }
    }
}