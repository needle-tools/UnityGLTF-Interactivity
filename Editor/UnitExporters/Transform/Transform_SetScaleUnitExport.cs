using System;
using UnityEditor;
using UnityEngine;
using UnityGLTF.Interactivity.Schema;

namespace UnityGLTF.Interactivity.Export
{
    public class Transform_SetScaleUnitExport : IUnitExporter
    {
        public Type unitType { get; }
        private bool worldSpace = false;
        
        
        [InitializeOnLoadMethod]
        private static void Register()
        {
            SetMemberUnitExport.RegisterMemberConvert(typeof(Transform), nameof(Transform.lossyScale), new Transform_SetScaleUnitExport(true));
            SetMemberUnitExport.RegisterMemberConvert(typeof(Transform), nameof(Transform.localScale), new Transform_SetScaleUnitExport(false));
        }
        
        public Transform_SetScaleUnitExport(bool worldSpace)
        {
            this.worldSpace = worldSpace;
        }
        
        public void InitializeInteractivityNodes(UnitExporter unitExporter)
        {
            // TODO: World Space conversion
            
           var unit = unitExporter.unit as Unity.VisualScripting.SetMember;
           
           var setScale = unitExporter.CreateNode(new Pointer_SetNode());
           
           unitExporter.SetupPointerTemplateAndTargetInput(GltfInteractivityNodeHelper.IdPointerNodeIndex,
               unit.target, setScale,
               "/nodes/{" + GltfInteractivityNodeHelper.IdPointerNodeIndex + "}/scale");
           
           unitExporter.MapInputPortToSocketName(unit.assign, Pointer_SetNode.IdFlowIn, setScale);
           unitExporter.MapOutFlowConnectionWhenValid(unit.assigned, Pointer_SetNode.IdFlowOut, setScale);
           unitExporter.MapInputPortToSocketName(unit.input, Pointer_SetNode.IdValue, setScale);
        }
    }
}