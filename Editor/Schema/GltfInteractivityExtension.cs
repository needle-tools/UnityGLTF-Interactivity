namespace UnityGLTF.Interactivity
{
    using System;
    using System.Linq;
    using GLTF.Schema;
    using Newtonsoft.Json.Linq;
    
    /// <summary>
    /// Defines the KHR_Interactivity extension and holds the data required to serialize to GLTF.
    ///
    /// example:
    /// KHR_interactivity : {
    ///     "types": [
    ///         {"signature": "bool"},
    ///         {"signature": "int"},
    ///         [...]
    ///     ],
    ///     "variables": [
    ///         {
    ///             "id": "variable1",
    ///             "type": 0,
    ///             "value": false
    ///         },
    ///         [...]
    ///     ],
    ///     "customEvents": [
    ///         {
    ///             "id": "Event1",
    ///             "values": [
    ///                 {"id": "input1", "type": 4 },
    ///                 [...],
    ///             ]
    ///         },
    ///         [...]
    ///     ],
    ///     "nodes": [
    ///         {
    ///             "type": "mock/testNode",
    ///             "index": 0,
    ///             "configuration": [
    ///                 { "id": "config1", "type": 1, "value": 100 },
    ///                 { "id": "config2", "type": 0, "value": false },
    ///             ],
    ///             "flows": [
    ///                 { "id": "out", "node": 61, "socket": "in" }
    ///             ],
    ///             "values": [
    ///                 {"id": "input1", "type": 4, "value": [ 1, 2, 3 ] },
    ///                 {"id": "output1", "type": 4, "node": 61, "socket": "translation" },
    ///             ]
    ///         },
    ///      [...] ]
    /// }.
    /// </summary>
    [Serializable]
    internal class GltfInteractivityExtension : IExtension
    {
        public const string ExtensionName = "KHR_interactivity";

        // The list of nodes in the behavior graph
        public GltfInteractivityNode[] Nodes = { };

        // The variables that are accessible to the nodes in the graph.
        public Variable[] Variables = { };

        // The list of custom events that can be sent/received in the behavior graph.
        public CustomEvent[] CustomEvents = { };

        public JProperty Serialize()
        {
            JObject jo = new JObject
            {
                new JProperty("types",
                    new JArray(
                        from type in GltfInteractivityTypeMapping.TypesMapping
                        select type.SerializeObject())),
                new JProperty("variables",
                    new JArray(
                        from variable in Variables
                        select variable.SerializeObject())),
                new JProperty("events",
                    new JArray(
                        from customEvent in CustomEvents
                        select customEvent.SerializeObject())),
                new JProperty("nodes",
                    new JArray(
                        from node in Nodes
                        select node.SerializeObject()))
            };

            JProperty extension =
                new JProperty(GltfInteractivityExtension.ExtensionName, jo);
            return extension;
        }

        public IExtension Clone(GLTFRoot root)
        {
            return new GltfInteractivityExtension()
            {
                Nodes = Nodes,
                Variables = Variables,
                CustomEvents = CustomEvents
            };
        }

        // TODO: (b/333422987) Add a Validate() method to validate data conforms to the schemas


        /// <summary> Variables hold data or references accessible to the behavior graph.</summary>
        public class Variable
        {
            public string Id = string.Empty;
            public int Type;
            public object Value;

            public JObject SerializeObject()
            {
                var jObject = new JObject
                {
                    new JProperty("id", Id),
                    new JProperty("type", Type),
                };
                GltfInteractivityUnitExporterNode.ValueSerializer.Serialize(Value, jObject);

                return jObject;
            }
        }

        /// <summary> Defines the Custom Events can be sent or received in the graph.</summary>
        public class CustomEvent
        {
            public string Id = string.Empty;
            public GltfInteractivityUnitExporterNode.EventValues[] Values = { };

            public JObject SerializeObject()
            {
                return new JObject
                {
                    new JProperty("id", Id),
                    new JProperty("values",
                        new JArray(
                            from value in Values
                            select value.SerializeObject()))
                };
            }
        }
    }
}
