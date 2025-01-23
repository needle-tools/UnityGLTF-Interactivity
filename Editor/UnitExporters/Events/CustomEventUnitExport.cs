using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
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
            var customEvent = unitExporter.unit as CustomEvent;

            if (!customEvent.target.hasDefaultValue && !customEvent.target.hasValidConnection)
            {
                UnitExportLogging.AddErrorLog(customEvent, "Could not find target node for CustomEvent");
                return;
            }
            
            var node = unitExporter.CreateNode(new Event_ReceiveNode());
            node.MapInputPortToSocketName(customEvent.name, "event");
            
            var args = new Dictionary<string, GltfInteractivityUnitExporterNode.EventValues>();
            args.Add("targetNodeIndex", new GltfInteractivityUnitExporterNode.EventValues {Type = GltfTypes.TypeIndex("int") });

            foreach (var arg in customEvent.argumentPorts)
            {
                var argId = arg.key;
                var argTypeIndex = GltfTypes.TypeIndex(arg.type);
                var newArg = new GltfInteractivityUnitExporterNode.EventValues { Type = argTypeIndex };
                args.Add(argId, newArg);
                // TODO: adding default values?

                node.MapValueOutportToSocketName(arg, argId);
            }
            
            var index = unitExporter.exportContext.AddEventIfNeeded(customEvent, args);
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