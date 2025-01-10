using System;
using Unity.VisualScripting;
using UnityEditor;

namespace UnityGLTF.Interactivity.Export
{
    
    // Will be handled in UnitExporter as a Literal 
    
    public class NullUnitExport : IUnitExporter
    {
        public Type unitType
        {
            get => typeof(Null);
        }
        
        [InitializeOnLoadMethod]
        private static void Register()
        {
            UnitExporterRegistry.RegisterExporter(new NullUnitExport());
        }
        
        public void InitializeInteractivityNodes(UnitExporter unitExporter)
        {
        }
    }
}