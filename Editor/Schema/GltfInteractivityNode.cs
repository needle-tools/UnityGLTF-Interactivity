using UnityEngine;
using UnityGLTF.Interactivity.Export;

namespace UnityGLTF.Interactivity
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Contains KHR_Interactivity Node data to be serialized into GLTF.
    ///
    /// example:
    /// KHR_interactivity : {
    ///     [...]
    ///     "nodes": [
    ///      {
    ///       "type": "mock/testNode",
    ///          "index": 0,
    ///          "configuration": [
    ///              { "id": "config1", "type": 1, "value": 100 },
    ///              { "id": "config2", "type": 0, "value": false },
    ///          ],
    ///         "flows": [
    ///             { "id": "out", "node": 61, "socket": "in" }
    ///         ],
    ///         "values": [
    ///             {"id": "input1", "type": 4, "value": [ 1, 2, 3 ] },
    ///             {"id": "output1", "type": 4, "node": 61, "socket": "translation" },
    ///         ]
    ///       },
    ///      [...] ]
    ///      [...]
    /// }.
    /// </summary>
    ///
    ///
    public class GltfInteractivityNode
    {
        public int Index;
        public virtual GltfInteractivityNodeSchema Schema { get; protected set; }
        
        // Data to be serialized into Gltf
        public Dictionary<string, ConfigData> ConfigurationData =
            new Dictionary<string, ConfigData>();
        public Dictionary<string, FlowSocketData> FlowSocketConnectionData =
            new Dictionary<string, FlowSocketData>();
        public Dictionary<string, ValueSocketData> ValueSocketConnectionData =
            new Dictionary<string, ValueSocketData>();
        
        public Dictionary<string, ValueOutSocket> OutValueSocket =
            new Dictionary<string, ValueOutSocket>();
        
        public Dictionary<string, string> MetaData = new Dictionary<string, string>();
        
        public void SetFlowOut(string socketId, GltfInteractivityNode targetNode, string targetSocketId)
        {
            if (FlowSocketConnectionData.TryGetValue(socketId, out var socket))
            {
                socket.Node = targetNode.Index;
                socket.Socket = targetSocketId;
            }
            else
            {
                Debug.LogError($"Socket {socketId} not found in node {Schema.Type}");
            }
        }
        
        public void SetValueInSocketSource(string socketId,  GltfInteractivityNode sourceNode, string sourceSocketId, TypeRestriction typeRestriction = null)
        {
            if (ValueSocketConnectionData.TryGetValue(socketId, out var socket))
            {
                socket.Node = sourceNode.Index;
                socket.Socket = sourceSocketId;
                socket.Value = null;
                socket.Type = -1;
                if (typeRestriction != null)
                    socket.typeRestriction = typeRestriction;
            }
            else
            {
                Debug.LogError($"Socket {socketId} not found in node {Schema.Type}");
            }
        }

        public void SetValueInSocket(string socketId, object value, TypeRestriction typeRestriction = null)
        {
            if (ValueSocketConnectionData.TryGetValue(socketId, out var socket))
            {
                socket.Node = null;
                socket.Socket = null;
                socket.Value = value;
                if (value != null)
                    socket.Type =  GltfInteractivityTypeMapping.TypeIndex(value.GetType());
                
                if (typeRestriction != null)
                    socket.typeRestriction = typeRestriction;
            }
            else
            {
                Debug.LogError($"Socket {socketId} not found in node {Schema.Type}");
            }
        }
        
        public GltfInteractivityNode(GltfInteractivityNodeSchema schema)
        {
            Schema = schema;

            foreach (GltfInteractivityNodeSchema.ConfigDescriptor descriptor in Schema.Configuration)
            {
                ConfigurationData.Add(descriptor.Id, new ConfigData()
                {
                    Id = descriptor.Id,
                });
            }

            foreach (GltfInteractivityNodeSchema.InputValueSocketDescriptor descriptor in Schema.InputValueSockets)
            {
                ValueSocketConnectionData.Add(descriptor.Id, new ValueSocketData()
                {
                    Id = descriptor.Id,
                    Type = GltfInteractivityTypeMapping.TypeIndexByGltfSignature(descriptor.SupportedTypes[0]),
                    typeRestriction = descriptor.typeRestriction
                });
            }
            foreach (var  descriptor in Schema.OutputValueSockets)
            {
                if (descriptor.SupportedTypes.Length == 1 && descriptor.expectedType == null)
                    OutValueSocket.Add(descriptor.Id,
                        new ValueOutSocket { Id = descriptor.Id, expectedType = ExpectedType.GtlfType(descriptor.SupportedTypes[0])});
                else
                    OutValueSocket.Add(descriptor.Id,
                        new ValueOutSocket { Id = descriptor.Id, expectedType = descriptor.expectedType });
            }

            foreach (GltfInteractivityNodeSchema.FlowSocketDescriptor descriptor in Schema.OutputFlowSockets)
            {
                FlowSocketConnectionData.Add(descriptor.Id, new FlowSocketData()
                {
                    Id = descriptor.Id
                });
            }
            
            foreach (GltfInteractivityNodeSchema.MetaDataEntry descriptor in Schema.MetaDatas)
            {
                MetaData.Add(descriptor.key, descriptor.value);
            }
        }
        
        public virtual JObject SerializeObject()
        {
            JObject jo = new JObject
            {
                new JProperty("type", Schema.Type),
                new JProperty("index", Index),
                new JProperty("configuration",
                    new JArray(
                        from config in ConfigurationData.Values
                        select config.SerializeObject())
                ),
                // TODO:
       //         new JProperty("metadata",
       //
         //       ),
                new JProperty("values",
                    new JArray(
                        from value in ValueSocketConnectionData.Values
                        select value.SerializeObject())
                ),
                new JProperty("flows",
                    new JArray(
                        from flow in FlowSocketConnectionData.Values where flow.Node != null
                        select flow.SerializeObject()))
            };

            // Remove all empty arrays in the first level of the JSON Object
            jo.SelectTokens("$.*")
              .OfType<JArray>()
              .Where(x => x.Type == JTokenType.Array && !x.HasValues)
              .Select(a => a.Parent)
              .ToList()
              .ForEach(a => a.Remove());

            return jo;
        }

        // TODO: (b/333422987) Add a Validate() method to validate data conforms to the schema

        public abstract class BaseData
        {
            public string Id = string.Empty;
        }

        
        public class ConfigData : BaseData
        {
            // data field holds index in list of types supported in the extension
            public object Value = null;

            public JObject SerializeObject()
            {
                if (Value == null)
                {
                    Debug.LogError($"{nameof(Value)} is null for ConfigData: {Id} ");
                    return null;
                }
                
                var jObject = new JObject
                {
                    new JProperty("id", Id),
                };
                ValueSerializer.Serialize(Value, jObject);
                return jObject;
            }
        }

        /// <summary>
        /// Describes a socket connection's data.
        ///
        /// Only outgoing connections from this node to the next are required to be serialized.
        /// </summary>
        public abstract class SocketData : BaseData
        {
            public string Socket = null;
            public int? Node = null;

            public override string ToString()
            {
                return $"Id: \"{Id}\", Node: {(Node.HasValue ? Node.Value.ToString() : "null")}, Socket: \"{Socket}\"";
            }
        }
        
        public class EventValues : BaseData
        {
            public int Type = -1;
            
            public JObject SerializeObject()
            {
                JObject valueObject = new JObject()
                {
                    new JProperty("id", Id),
                    new JProperty("type", Type),
                };
                
                return valueObject;
            }
            
            public override string ToString()
            {
                return $"{base.ToString()}, Type: {Type}";
            }
        }

        /// <summary>
        /// Describes Flow data for the node.
        ///
        /// Only outgoing connections from this node to the next are required to be serialized.
        /// </summary>
        public class FlowSocketData : SocketData
        {
            public JObject SerializeObject()
            {
                return new JObject
                {
                    new JProperty("id", Id),
                    new JProperty("node", Node),
                    new JProperty("socket", Socket)
                };
            }
        }


        public static class ValueSerializer
        {
           public static void Serialize(object value, JObject valueObject)
            {
                if (value != null)
                {
                    if (value is Color color)
                    {
                        valueObject.Add(new JProperty("value", new JArray(color.r, color.g, color.b, color.a)));
                    }
                    else if (value is Color32 color32)
                    {
                        Color col = color32;
                        valueObject.Add(new JProperty("value", new JArray(col.r, col.g, col.b, col.a)));
                    }                  
                    else if (value is Matrix4x4 m4)
                    {
                        // TODO check if this is the correct row-column order
                        valueObject.Add(new JProperty("value", new JArray(
                            m4.m00, m4.m01, m4.m02, m4.m03,
                            m4.m10, m4.m11, m4.m12, m4.m13,
                            m4.m20, m4.m21, m4.m22, m4.m23,
                            m4.m30, m4.m31, m4.m32, m4.m33)));
                    }
                    else if (value is Vector4 v4)
                    {
                        valueObject.Add(new JProperty("value", new JArray(v4.x, v4.y, v4.z, v4.w)));
                    }
                    else if (value is Vector3 v3)
                    {
                        valueObject.Add(new JProperty("value", new JArray(v3.x, v3.y, v3.z)));
                    }
                    else if (value is Vector2 v2)
                    {
                        valueObject.Add(new JProperty("value", new JArray(v2.x, v2.y)));
                    }
                    else if (value is Quaternion q)
                    {
                        valueObject.Add(new JProperty("value", new JArray(q.x, q.y, q.z, q.w)));
                    }
                    else if (value is bool b)
                    {
                        valueObject.Add(new JProperty("value", b));
                    }
                    else if (value is string s)
                    {
                        valueObject.Add(new JProperty("value", s));
                    }
                    else if (value is int i)
                    {
                        valueObject.Add(new JProperty("value", new JArray(i)));
                    }
                    else if (value is float f)
                    {
                        valueObject.Add(new JProperty("value", new JArray(f)));
                    }
                    else
                    {
                        valueObject.Add(new JProperty("value", value));
                    }
                }
            }    
        }

        public class ValueOutSocket
        {
            public string Id;
            public ExpectedType expectedType;
        }
        
        /// <summary>
        /// Describes value data for the node.
        ///
        /// Either the Value field will be used when the socket is defined by a literal in-line
        /// value or the Node and Socket fields will be used when the socket gets/sets the value
        /// through a connection to another Node's value socket.
        /// </summary>
        public class ValueSocketData : SocketData
        {
            public int Type = -1;
            public object Value = null;
            
            public TypeRestriction typeRestriction = null;

            public JObject SerializeObject()
            {
                JObject valueObject = new JObject()
                {
                    new JProperty("id", Id),
                };

                // Optional fields are only added if non-null
                if (Node != null)
                {
                    valueObject.Add(new JProperty("node", Node));
                }

                if (Socket != null)
                {
                    valueObject.Add(new JProperty("socket", Socket));
                }

                if (Value != null)
                {
                    valueObject.Add(new JProperty("type", Type));

                    ValueSerializer.Serialize(Value, valueObject);
                }
                else if (Node == null)
                {
                    if (Type != -1)
                        Debug.LogError(
                            $"{nameof(Value)} is null for ValueSocketData: \"{Id}\" of type \"{GltfInteractivityTypeMapping.TypesMapping[Type].GltfSignature}\" on node \"{(Node.HasValue ? Node.Value : "<null node>")}\"");
                    else
                        Debug.LogError(
                            $"{nameof(Value)} is null for ValueSocketData: \"{Id}\" on node \"{(Node.HasValue ? Node.Value : "<null node>")}\"");

                }

                return valueObject;
            }

            public override string ToString()
            {
                return $"{base.ToString()}, Value: {Value}";
            }
        }
    }

}
