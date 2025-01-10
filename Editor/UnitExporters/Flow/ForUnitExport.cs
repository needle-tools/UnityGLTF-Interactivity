using System;
using UnityEditor;
using UnityGLTF.Interactivity.Schema;

namespace UnityGLTF.Interactivity.Export
{
    public class ForUnitExport : IUnitExporter
    {
        public Type unitType
        {
            get => typeof(Unity.VisualScripting.For);
        }
        
        [InitializeOnLoadMethod]
        private static void Register()
        {
            UnitExporterRegistry.RegisterExporter(new ForUnitExport());
        }
        
        public void InitializeInteractivityNodes(UnitExporter unitExporter)
        {
            var unit = unitExporter.unit as Unity.VisualScripting.For;
            GltfInteractivityUnitExporterNode node = unitExporter.CreateNode(new Flow_ForLoopNode());
            // TODO: set inital index > also... why even using it
            node.ConfigurationData[Flow_ForLoopNode.IdConfigInitialIndex].Value = 0;
            
            unitExporter.MapInputPortToSocketName(unit.enter, Flow_ForLoopNode.IdFlowIn, node);
            unitExporter.MapInputPortToSocketName(unit.firstIndex, Flow_ForLoopNode.IdStartIndex, node);
            unitExporter.MapInputPortToSocketName(unit.lastIndex, Flow_ForLoopNode.IdEndIndex, node);

            node.ValueOut(Flow_ForLoopNode.IdIndex).MapToPort(unit.currentIndex).ExpectedType(ExpectedType.Int);
            
            unitExporter.MapOutFlowConnectionWhenValid(unit.exit, Flow_ForLoopNode.IdCompleted, node);
            unitExporter.MapOutFlowConnectionWhenValid(unit.body, Flow_ForLoopNode.IdLoopBody, node);       
            
        }
    }
}