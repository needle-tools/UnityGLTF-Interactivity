using System;
using System.Collections.Generic;

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
            expectedType.typeIndex = GltfTypes.TypeIndexByGltfSignature(gltfType);
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
            return new TypeRestriction { limitToType = GltfTypes.TypesMapping[typeIndex].GltfSignature};
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
    
    [System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = true)]
    public class InputSocketDescriptionAttribute : Attribute
    {
        public string[] supportedTypes;
        
        public InputSocketDescriptionAttribute(params string[] supportedTypes)
        {
            this.supportedTypes = supportedTypes;
        }
    }

    [System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = true)]
    public class InputSocketDescriptionWithTypeDependencyFromOtherPortAttribute : InputSocketDescriptionAttribute
    {
        public TypeRestriction typeRestriction;
        
        public InputSocketDescriptionWithTypeDependencyFromOtherPortAttribute(string sameAsSocket,
            params string[] supportedTypes) : base(supportedTypes)
        {
            typeRestriction = TypeRestriction.SameAsInputPort(sameAsSocket);
        }
    }


    [System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = true)]
    public class OutputSocketDescriptionAttribute : Attribute
    {
        public string[] supportedTypes;
        
        public OutputSocketDescriptionAttribute( params string[] supportedTypes)
        {
            this.supportedTypes = supportedTypes;
        }
    }

    [System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = true)]
    public class OutputSocketDescriptionWithTypeDependencyFromInputAttribute : OutputSocketDescriptionAttribute
    {
        public ExpectedType expectedType;
        public OutputSocketDescriptionWithTypeDependencyFromInputAttribute(string sameTypeAsInputSocket) : base()
        {
            expectedType = ExpectedType.FromInputSocket(sameTypeAsInputSocket);
        }
    }

    [System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = true)]
    public class FlowInSocketDescriptionAttribute : Attribute
    {
        public FlowInSocketDescriptionAttribute()
        {
        }
    }
    
    [System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = true)]
    public class FlowOutSocketDescriptionAttribute : Attribute
    {
        public FlowOutSocketDescriptionAttribute()
        {
        }
    }
    
    [System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = true)]
    public class ConfigDescriptionAttribute : Attribute
    {
        public string gltfValueType;
        
        public ConfigDescriptionAttribute(string gltfValueType)
        {
            this.gltfValueType = gltfValueType;
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
        public virtual string Op { get; set; } = string.Empty;

        public virtual string Extension { get; protected set; } = null;

        public Dictionary<string, ConfigDescriptor> Configuration { get; set; } = new();
        public Dictionary<string, FlowSocketDescriptor> InputFlowSockets { get; set; } = new();
        public Dictionary<string, FlowSocketDescriptor> OutputFlowSockets {get; set;} = new();
        public Dictionary<string, InputValueSocketDescriptor> InputValueSockets {get; set;} = new();
        public Dictionary<string, OutValueSocketDescriptor> OutputValueSockets {get; set;} = new();
        
        public MetaDataEntry[] MetaDatas {get; protected set;}

        public void CreateDescriptorsFromAttributes(bool clearExisting = false)
        {
            if (clearExisting)
            {
                Configuration.Clear();
                InputFlowSockets.Clear();
                OutputFlowSockets.Clear();
                InputValueSockets.Clear();
                OutputValueSockets.Clear();
            }
            
            var fields = GetType().GetFields();
            foreach (var field in fields)
            {
                if (field.FieldType != typeof(string))
                    return;
                
                var fieldValue = field.GetValue(this) as string;
                
                var attributes = field.GetCustomAttributes(false);
                foreach (var attribute in attributes)
                {
                    
                    if (attribute is ConfigDescriptionAttribute configDescription)
                    {
                        Configuration.Add(fieldValue, new ConfigDescriptor { Type = configDescription.gltfValueType });
                    }
                    else if (attribute is OutputSocketDescriptionWithTypeDependencyFromInputAttribute outputSocketDescriptionWithExpectedType)
                    {
                        var newOut = new OutValueSocketDescriptor
                        {
                            SupportedTypes = outputSocketDescriptionWithExpectedType.supportedTypes,
                            expectedType = outputSocketDescriptionWithExpectedType.expectedType
                        };
                        if (newOut.SupportedTypes == null || newOut.SupportedTypes.Length == 0)
                            newOut.SupportedTypes = GltfTypes.allTypes;

                        OutputValueSockets.Add(fieldValue, newOut);
                    }
                    else if (attribute is InputSocketDescriptionWithTypeDependencyFromOtherPortAttribute inputSocketDescriptionWithTypeRestriction)
                    {
                        var newIn = new InputValueSocketDescriptor
                        {
                            SupportedTypes = inputSocketDescriptionWithTypeRestriction.supportedTypes,
                            typeRestriction = inputSocketDescriptionWithTypeRestriction.typeRestriction
                        };
                        if (newIn.SupportedTypes == null || newIn.SupportedTypes.Length == 0)
                            newIn.SupportedTypes = GltfTypes.allTypes;
                        
                        InputValueSockets.Add(fieldValue, newIn);
                    }
                    else if (attribute is InputSocketDescriptionAttribute inputSocketDescription)
                    {
                        var newIn = new InputValueSocketDescriptor
                        {
                            SupportedTypes = inputSocketDescription.supportedTypes
                        };
                        
                        if (newIn.SupportedTypes == null || newIn.SupportedTypes.Length == 0)
                            newIn.SupportedTypes = GltfTypes.allTypes;
                        
                        InputValueSockets.Add(fieldValue, newIn);
                    }
                    else if (attribute is OutputSocketDescriptionAttribute outputSocketDescription)
                    {
                        var newOut = new OutValueSocketDescriptor
                        {
                            SupportedTypes = outputSocketDescription.supportedTypes
                        };

                        if (newOut.SupportedTypes == null || newOut.SupportedTypes.Length == 0)
                            newOut.SupportedTypes = GltfTypes.allTypes;
                        else if (newOut.SupportedTypes != null && newOut.SupportedTypes.Length == 1)
                        {
                            newOut.expectedType = ExpectedType.GtlfType(newOut.SupportedTypes[0]);
                        }
                        OutputValueSockets.Add(fieldValue, newOut);
                    }
                    else if (attribute is FlowInSocketDescriptionAttribute)
                    {
                        InputFlowSockets.Add(fieldValue, new FlowSocketDescriptor());
                    }
                    else if (attribute is FlowOutSocketDescriptionAttribute)
                    {
                        OutputFlowSockets.Add(fieldValue, new FlowSocketDescriptor());
                    }
                }
            }
        }
        
        public GltfInteractivityNodeSchema()
        {
            MetaDatas = new MetaDataEntry[] { };
            CreateDescriptorsFromAttributes();
        }

        /// <summary> Every descriptive field should have the fields contained here.</summary>
        public class BaseDescriptor
        {
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
