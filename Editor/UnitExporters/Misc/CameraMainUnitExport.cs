using System;
using UnityEditor;
using UnityEngine;

namespace UnityGLTF.Interactivity.Export
{
    public class CameraMainUnitExport : IUnitExporter
    {
        public Type unitType
        {
            get => typeof(GetMemberUnitExport);
        }
        
        [InitializeOnLoadMethod]
        private static void Register()
        {
            GetMemberUnitExport.RegisterMemberExporter(typeof(Camera), nameof(Camera.main), new CameraMainUnitExport());
        }
        
        public void InitializeInteractivityNodes(UnitExporter unitExporter)
        {
        }
    }
}