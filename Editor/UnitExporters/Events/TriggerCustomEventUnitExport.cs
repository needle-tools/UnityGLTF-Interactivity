using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityGLTF.Interactivity.Schema;

namespace UnityGLTF.Interactivity.Export
{
    public class TriggerCustomEventUnitExport: IUnitExporter
    {
        [InitializeOnLoadMethod]
        private static void Register()
        {
            UnitExporterRegistry.RegisterExporter(new TriggerCustomEventUnitExport());
        }
        
        public System.Type unitType { get => typeof(TriggerCustomEvent); }
        

        public void InitializeInteractivityNodes(UnitExporter unitExporter)
        {
            GltfInteractivityUnitExporterNode node = unitExporter.CreateNode(new Event_SendNode());
            var customEvent = unitExporter.unit as TriggerCustomEvent;
    
            unitExporter.MapInputPortToSocketName(customEvent.name, Event_SendNode.IdEvent, node);
            unitExporter.MapInputPortToSocketName(customEvent.enter, Event_SendNode.IdFlowIn, node);
            
            var args = new List<GltfInteractivityUnitExporterNode.EventValues>();
            foreach (var arg in customEvent.arguments)
            {
                var argId = arg.key;
                var argTypeIndex = GltfInteractivityTypeMapping.TypeIndex(arg.type);
                var eventValue = new GltfInteractivityUnitExporterNode.EventValues { Id = argId, Type = argTypeIndex };
                args.Add(eventValue);
                
                unitExporter.MapInputPortToSocketName(arg, argId, node);
                var valueSocketData = new GltfInteractivityUnitExporterNode.ValueSocketData()
                {
                    Id = argId,
                    Type = argTypeIndex,
                };
                node.ValueSocketConnectionData.Add(argId, valueSocketData);


            }
            var index = unitExporter.exportContext.AddEventIfNeeded(customEvent, args.ToArray());
            node.ConfigurationData["event"].Value = index;
            
            unitExporter.exportContext.OnBeforeSerialization += (GltfInteractivityNode[] nodes) =>
            {
                var customEvent = unitExporter.exportContext.customEvents[index];

                foreach (var argValue in args)
                {
                    var eventValue = customEvent.Values.FirstOrDefault(x => x.Id == argValue.Id);
                    if (eventValue == null || eventValue.Type != -1)
                        continue;
                    
                    var argTypeIndex = unitExporter.exportContext.GetValueTypeForInput(nodes, node, argValue.Id);
                    eventValue.Type = argTypeIndex;
                }
            };
            
            unitExporter.MapOutFlowConnectionWhenValid(customEvent.exit, Event_SendNode.IdFlowOut, node);
        }
    }
}