using Unity.VisualScripting;
using UnityEditor;
using UnityGLTF.Interactivity.Schema;

namespace UnityGLTF.Interactivity.Export
{
    public class WaitForSecondsUnitExport : IUnitExporter
    {
        public System.Type unitType { get => typeof( WaitForSecondsUnit); }
        
        [InitializeOnLoadMethod]
        private static void Register()
        {
            //NodeConvertRegistry.RegisterImport(new OnSelectNode());
            UnitExporterRegistry.RegisterExporter(new WaitForSecondsUnitExport());
        }
        
        public void InitializeInteractivityNodes(UnitExporter unitExporter)
        {
            var unit = unitExporter.unit as WaitForSecondsUnit;
            var node = unitExporter.CreateNode(new Flow_SetDelayNode());
            
            unitExporter.MapOutFlowConnectionWhenValid(unit.exit, Flow_SetDelayNode.IdFlowDone, node);
            
            unitExporter.MapInputPortToSocketName(unit.seconds, Flow_SetDelayNode.IdDuration, node);
            unitExporter.MapInputPortToSocketName(unit.enter, Flow_SetDelayNode.IdFlowIn, node);
            // TODO: cancel, done, err, lastDelayIndex ... maybe custom Unit also with a Static Dict. for delay index
        }
    }
}