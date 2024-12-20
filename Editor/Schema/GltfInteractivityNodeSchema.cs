namespace UnityGLTF.Interactivity
{
    
    public class ExpectedType
    {
        public string fromInputPort = null;
        public int? typeIndex = null;

        public static ExpectedType Float
        {
            get => GtlfType("float");
        }
        
        public static ExpectedType Float2
        {
            get =>  GtlfType("float2");
        }
        
        public static ExpectedType Float3
        {
            get =>  GtlfType("float3");
        }
        
        public static ExpectedType Float4
        {
            get =>  GtlfType("float4");
        }
        
        public static ExpectedType Int
        {
            get =>  GtlfType("int");
        }
        
        public static ExpectedType Bool
        {
            get =>  GtlfType("bool");
        }
        
        public static ExpectedType Float4x4
        {
            get =>  GtlfType("float4x4");
        }
        
        public static ExpectedType FromInputSocket(string socketName)
        {
            var expectedType = new ExpectedType();
            expectedType.fromInputPort = socketName;
            return expectedType;
        }
            
        public static ExpectedType GtlfType(string gltfType)
        {
            var expectedType = new ExpectedType();
            expectedType.typeIndex = GltfInteractivityTypeMapping.TypeIndexByGltfSignature(gltfType);
            return expectedType;
        }
            
        public static ExpectedType GtlfType(int typeIndex)
        {
            var expectedType = new ExpectedType();
            expectedType.typeIndex = typeIndex;
            return expectedType;
        }
            
        private ExpectedType()
        {
                
        }
    }

    public class TypeRestriction
    {
        public string fromInputPort { get; private set; } = null;
        public string limitToType { get; private set; } = null;
        
        public static TypeRestriction SameAsInputPort(string portName)
        {
            return new TypeRestriction { fromInputPort = portName };
        }
        
        public static TypeRestriction LimitToType(string type)
        {
            return new TypeRestriction { limitToType = type };
        }
        
        public static TypeRestriction LimitToType(int typeIndex)
        {
            return new TypeRestriction { limitToType = GltfInteractivityTypeMapping.TypesMapping[typeIndex].GltfSignature};
        }

        public static TypeRestriction LimitToBool
        {
            get => LimitToType("bool");
        }
        
        public static TypeRestriction LimitToFloat
        {
            get => LimitToType("float");
        }
        
        public static TypeRestriction LimitToInt
        {
            get => LimitToType("int");
        }
        
        public static TypeRestriction LimitToFloat2
        {
            get => LimitToType("float2");
        }
        
        public static TypeRestriction LimitToFloat3
        {
            get => LimitToType("float3");
        }
        
        public static TypeRestriction LimitToFloat4
        {
            get => LimitToType("float4");
        }
        
        public static TypeRestriction LimitToFloat4x4
        {
            get => LimitToType("float4x4");
        }
        
        private TypeRestriction()
        {
        }
    }
    
    /// <summary>
    /// Defines the Schema against which nodes of certain types are expected to conform.
    ///
    /// The format of the Schema follows the latest KHR_Interactivity Schema used externally.
    /// Since the extension hasn't been ratified, we expect this schema to change significantly.
    /// See https://github.com/KhronosGroup/glTF-InteractivityGraph-AuthoringTool/blob/initial-work-merge/src/authoring/AuthoringNodeSpecs.ts
    /// example schema:
    /// {
    ///         type:"node/OnSelect",
    ///         description:"This node will fire when a node is selected.",
    ///         configuration:[
    ///             {
    ///                 id:"nodeIndex",
    ///                 type:"int",
    ///                 description:"The node to add the listener on"
    ///             },
    ///             {
    ///                 id:"stopPropagation",
    ///                 type:"bool",
    ///                 description:"Should the event be propagated up the node parent hierarchy"
    ///             },
    ///         ],
    ///         input:{
    ///             flows:[],
    ///             values:[]
    ///         },
    ///         output:{
    ///             flows:[
    ///                 {
    ///                     id:"out",
    ///                     description:"The flow to be followed when the custom event is fired."
    ///                 }
    ///             ],
    ///             values:[
    ///                 {
    ///                     id: "hitNodeIndex",
    ///                     description: "The index of the first hit node",
    ///                     types: ["int"]
    ///                 },
    ///                 {
    ///                     id: "localHitLocation",
    ///                     description: "The local hit offset from the hit node's origin",
    ///                     types: ["float3"]
    ///                 }
    ///             ]
    ///         }
    ///     }.
    /// </summary>
    public class GltfInteractivityNodeSchema
    {
        // TODO: Should have protected setters
        public static readonly string ExtensionName = "KHR_interactivity";
        public string Type {get; set;}
        public string Description {get; protected set;}
        public ConfigDescriptor[] Configuration {get; set;}
        public FlowSocketDescriptor[] InputFlowSockets {get; set;}
        public FlowSocketDescriptor[] OutputFlowSockets {get; set;}
        public InputValueSocketDescriptor[] InputValueSockets {get; set;}
        public OutValueSocketDescriptor[] OutputValueSockets {get; set;}
        
        public MetaDataEntry[] MetaDatas {get; protected set;}

        public GltfInteractivityNodeSchema()
        {
            Type = string.Empty;
            Description = string.Empty;
            Configuration = new ConfigDescriptor[] { };
            InputFlowSockets = new FlowSocketDescriptor[] { };
            OutputFlowSockets = new FlowSocketDescriptor[] { };
            InputValueSockets = new InputValueSocketDescriptor[] { };
            OutputValueSockets = new OutValueSocketDescriptor[] { };
            MetaDatas = new MetaDataEntry[] { };
        }

        /// <summary> Every descriptive field should have the fields contained here.</summary>
        public class BaseDescriptor
        {
            // A unique identifier within the field's respective container.
            public string Id = string.Empty;

            // A description of the field's intended usage.
            public string Description = string.Empty;
        }

        public class MetaDataEntry
        {
            public string key;
            public string value;
        }
        
        /// <summary> Describes configuration parameters for the node.</summary>
        public class ConfigDescriptor : BaseDescriptor
        {
            // The expected data type of the configuration parameter field.
            public string Type = string.Empty;
        }

        /// <summary>
        /// Describes sockets that control the flow of execution before/after this node.
        /// </summary>
        public class FlowSocketDescriptor : BaseDescriptor
        {
        }

        /// <summary>
        /// Describes sockets that import/export values before/after the node's execution.
        /// </summary>
        public class ValueSocketDescriptor : BaseDescriptor
        {
            // List of data types that can be imported/exported by this value socket.
            public string[] SupportedTypes = { };
        }
        
        public class InputValueSocketDescriptor : ValueSocketDescriptor
        {
            public TypeRestriction typeRestriction = null;
        }
        
        /// <summary>
        /// Describes sockets that import/export values before/after the node's execution.
        /// </summary>
        public class OutValueSocketDescriptor : ValueSocketDescriptor
        {
            // List of data types that can be imported/exported by this value socket.
            public ExpectedType expectedType = null;
        }

        public class ValueTypeRestrictions
        {
            
        }
    }
}
