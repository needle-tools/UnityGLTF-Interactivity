using Unity.VisualScripting;
using UnityEngine;
using UnityGLTF.Interactivity.Export;

namespace UnityGLTF.Interactivity
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Newtonsoft.Json.Linq;
    
    public class GltfInteractivityUnitExporterNode : GltfInteractivityNode
    {
        public UnitExporter Exporter { get; private set; }
        /// <summary>
        /// The constructor takes in a Schema to which the data should be validated against.
        /// </summary>
        /// <param name="schema"> The Schema to which the data is expected to conform. </param>
        public GltfInteractivityUnitExporterNode(UnitExporter exporter, GltfInteractivityNodeSchema schema) : base(schema)
        {
            Exporter = exporter;
        }

        public class ExportSocketData<T> where T : SocketData
        {
            public GltfInteractivityUnitExporterNode node { get; private set;}
            public T data { get; private set; }
            
            public ExportSocketData(GltfInteractivityUnitExporterNode node, T socket)
            {
                this.data = socket;
                this.node = node;
            }
        }

        public class FlowOutSocketData : ExportSocketData<FlowSocketData>
        {
            public FlowOutSocketData(GltfInteractivityUnitExporterNode node, FlowSocketData socket) : base(node, socket)
            {
            }
            
            public FlowOutSocketData MapToControlOutput(ControlOutput controlOutput)
            {
                node.MapOutFlowConnectionWhenValid(controlOutput, data.Id);
                return this;
            }
            
            public FlowOutSocketData ConnectToFlowDestination(FlowInSocketData other)
            {
                node.MapOutFlowConnection(other.node, other.data.Id, data.Id);
                return this;
            }
        }
        
        public class FlowInSocketData : ExportSocketData<FlowSocketData>
        {
            public FlowInSocketData(GltfInteractivityUnitExporterNode node, FlowSocketData socket) : base(node, socket)
            {
            }
            
            public FlowInSocketData MapToControlInput(ControlInput controlInput)
            {
                node.MapInputPortToSocketName(controlInput, data.Id);
                return this;
            }
        }
        
        public class ValueInputSocketData : ExportSocketData<ValueSocketData>
        {
            public ValueInputSocketData(GltfInteractivityUnitExporterNode node, ValueSocketData socket) : base(node, socket)
            {
            }
            
            public ValueInputSocketData MapToInputPort(IUnitInputPort inputPort)
            {
                node.MapInputPortToSocketName(inputPort, data.Id);
                return this;
            }
            
            public ValueInputSocketData ConnectToSource(ValueOutputSocketData other)
            {
                node.MapInputPortToSocketName(other.socket.Id, other.node, data.Id);
                return this;
            }
            
            public ValueInputSocketData SetType(TypeRestriction typeRestriction)
            {
                data.typeRestriction = typeRestriction;
                return this;
            }

            public ValueInputSocketData SetValue(object value)
            {
                node.SetValueInSocket(data.Id, value);
                return this;
            }
        }

        public class ValueOutputSocketData
        {
            public ValueOutSocket socket { get; private set; }
            public GltfInteractivityUnitExporterNode node { get; private set; }
            
            public ValueOutputSocketData(GltfInteractivityUnitExporterNode node, ValueOutSocket socket)
            {
                this.socket = socket;
                this.node = node;    
            }
            
            public ValueOutputSocketData ExpectedType(ExpectedType expectedType)
            {
                socket.expectedType = expectedType;
                return this;
            }
            
            public ValueOutputSocketData MapToPort(IUnitOutputPort outputPort)
            {
                node.MapValueOutportToSocketName(outputPort, socket.Id);
                return this;
            }
        }
            
        public ValueInputSocketData ValueIn(string socketName)
        {
            if (!ValueSocketConnectionData.ContainsKey(socketName))
            {
               ValueSocketConnectionData.Add(socketName, new ValueSocketData { Id = socketName });
            }
            
            var socket = new ValueInputSocketData(this, ValueSocketConnectionData[socketName]);
            return socket;
        }
        
        public FlowOutSocketData FlowOut(string socketName)
        {
            if (!FlowSocketConnectionData.ContainsKey(socketName))
            {
                FlowSocketConnectionData.Add(socketName, new FlowSocketData { Id = socketName });
            }
            var socket = new FlowOutSocketData(this, FlowSocketConnectionData[socketName]);
            return socket;
        }

        public FlowInSocketData FlowIn(string socketName)
        {
            // TODO
            var socket = new FlowInSocketData(this, new FlowSocketData { Id = socketName});
            return socket;
        }
        
        public ValueOutputSocketData ValueOut(string value)
        {
            if (!OutValueSocket.ContainsKey(value))
            {
                OutValueSocket.Add(value, new ValueOutSocket { Id = value });
            }

            return new ValueOutputSocketData(this, OutValueSocket[value]);
        }
        
        public ValueOutputSocketData FirstValueOut()
        {
            return new ValueOutputSocketData(this, OutValueSocket.First().Value);
        }
        
        public void MapOutFlowConnectionWhenValid(ControlOutput controlOutput, string outFlowSocketName)
        {
            Exporter.MapOutFlowConnectionWhenValid(controlOutput, outFlowSocketName, this);
        }

        public void MapOutFlowConnection(GltfInteractivityUnitExporterNode destinationNode, string destinationSocketName, string outFlowSocketName)
        {
            Exporter.MapOutFlowConnection(destinationNode, destinationSocketName, this, outFlowSocketName);
        }
        
        public void SetupPointerTemplateAndTargetInput(string pointerId, ValueInput targetInputPort, string pointerTemplate)
        {
            Exporter.SetupPointerTemplateAndTargetInput(pointerId, targetInputPort, this, pointerTemplate);
        }
        
        public void SetupPointerTemplateAndTargetInput(string pointerId, string pointerTemplate)
        {
            Exporter.SetupPointerTemplateAndTargetInput(pointerId, this, pointerTemplate);
        }

        public void MapValueOutportToSocketName(IUnitOutputPort outputPort, string socketName)
        {
            Exporter.MapValueOutportToSocketName(outputPort, socketName, this);
        }
        
        public void MapInputPortToSocketName(IUnitInputPort inputPort, string socketName)
        {
            Exporter.MapInputPortToSocketName(inputPort, socketName, this);
        }

        public void MapInputPortToSocketName(string sourceSocketName, GltfInteractivityUnitExporterNode sourceNode, string socketName)
        {
            Exporter.MapInputPortToSocketName(sourceSocketName, sourceNode, socketName, this);
        }
        
    }
    
}
