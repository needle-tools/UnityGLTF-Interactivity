using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityGLTF.Interactivity.Schema;

namespace UnityGLTF.Interactivity.Export
{
    public class CustomEventUnitExport: IUnitExporter
    {
        [InitializeOnLoadMethod]
        private static void Register()
        {
            UnitExporterRegistry.RegisterExporter(new CustomEventUnitExport());
        }
        
        public System.Type unitType { get => typeof(CustomEvent); }

        public void InitializeInteractivityNodes(UnitExporter unitExporter)
        {
            var node = unitExporter.CreateNode(new Event_ReceiveNode());
            var customEvent = unitExporter.unit as CustomEvent;

            node.MapInputPortToSocketName(customEvent.name, "event");
            
            var args = new List<GltfInteractivityUnitExporterNode.EventValues>();
            args.Add( new GltfInteractivityUnitExporterNode.EventValues { Id = "targetNodeIndex", Type = GltfInteractivityTypeMapping.TypeIndex("int") });

            foreach (var arg in customEvent.argumentPorts)
            {
                var argId = arg.key;
                var argTypeIndex = GltfInteractivityTypeMapping.TypeIndex(arg.type);
                args.Add(new  GltfInteractivityUnitExporterNode.EventValues{Id = argId, Type = argTypeIndex});

                node.MapValueOutportToSocketName(arg, argId);
            }
            
            var index = unitExporter.exportContext.AddEventIfNeeded(customEvent, args.ToArray());
            node.ConfigurationData["event"].Value = index;
            node.ValueOut("targetNodeIndex").ExpectedType(ExpectedType.Int);

            // Setup target Node checks
            var eqIdNode = unitExporter.CreateNode(new Math_EqNode());
            eqIdNode.ValueIn(Math_EqNode.IdValueA).ConnectToSource(node.ValueOut("targetNodeIndex")).SetType(TypeRestriction.LimitToInt);
            var currentGameObject = unitExporter.exportContext.currentGraphProcessing.gameObject;
            var transformIndex = unitExporter.exportContext.exporter.GetTransformIndex(currentGameObject.transform);
            eqIdNode.ValueIn(Math_EqNode.IdValueB).SetValue(transformIndex).SetType(TypeRestriction.LimitToInt);            
            
            var branchNode = unitExporter.CreateNode(new Flow_BranchNode());
            branchNode.ValueIn(Flow_BranchNode.IdCondition).ConnectToSource(eqIdNode.FirstValueOut());
            node.FlowOut(Event_ReceiveNode.IdFlowOut)
                .ConnectToFlowDestination(branchNode.FlowIn(Flow_BranchNode.IdFlowIn));

            branchNode.FlowOut(Flow_BranchNode.IdFlowOutTrue).MapToControlOutput(customEvent.trigger);
        }
    }
}