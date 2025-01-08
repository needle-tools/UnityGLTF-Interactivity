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
           TransformHelpers.SetLocalPosition(unitExporter, unit.target, unit.input, unit.assign, unit.assigned);
           
           if (unit.chainable && unit.output.hasValidConnection)
               unitExporter.ByPassValue(unit.input, unit.output);
        }
    }
}