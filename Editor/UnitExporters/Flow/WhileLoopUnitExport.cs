using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityGLTF.Interactivity.Schema;

namespace UnityGLTF.Interactivity.Export
{
    public class WhileLoopUnitExport : IUnitExporter
    {
        public Type unitType
        {
            get => typeof(While);
        }
        
        [InitializeOnLoadMethod]
        private static void Register()
        {
            UnitExporterRegistry.RegisterExporter(new WhileLoopUnitExport());
        }
        
        public bool InitializeInteractivityNodes(UnitExporter unitExporter)
        {
            var unit = unitExporter.unit as While;
            var node = unitExporter.CreateNode(new Flow_WhileNode());

            unitExporter.MapInputPortToSocketName(unit.condition, Flow_WhileNode.IdCondition, node);
            unitExporter.MapOutFlowConnectionWhenValid(unit.body, Flow_WhileNode.IdLoopBody, node);
            unitExporter.MapOutFlowConnectionWhenValid(unit.exit, Flow_WhileNode.IdCompleted, node);
            return true;
        }
    }
}