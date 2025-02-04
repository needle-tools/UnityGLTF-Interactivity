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
            SetMemberUnitExport.RegisterMemberExporter(typeof(Transform), nameof(Transform.rotation), new Transform_SetRotationUnitExport(true));
            SetMemberUnitExport.RegisterMemberExporter(typeof(Transform), nameof(Transform.localRotation), new Transform_SetRotationUnitExport(false));
        }
        
        public Transform_SetRotationUnitExport(bool worldSpace)
        {
            this.worldSpace = worldSpace;
        }
        
        public void InitializeInteractivityNodes(UnitExporter unitExporter)
        {
            // TODO: World Space conversion
            
           var unit = unitExporter.unit as Unity.VisualScripting.SetMember;
           TransformHelpers.SetLocalRotation(unitExporter, unit.target, unit.input, unit.assign, unit.assigned);
           
           if (unit.chainable && unit.output.hasValidConnection)
               unitExporter.ByPassValue(unit.input, unit.output);
        }
    }
}