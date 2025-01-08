using System;
using System.Collections.Generic;
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
           TransformHelpers.GetLocalRotation(unitExporter, unit.target, unit.value);
        }
    }
}