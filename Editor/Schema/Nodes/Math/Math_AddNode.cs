namespace UnityGLTF.Interactivity.Schema
{
    public class Math_AddNode : GltfInteractivityNodeSchema
    {
        public static readonly string TypeName = "math/add";
        public static readonly string IdOut = "value";
        public static readonly string IdValueA = "a";
        public static readonly string IdValueB = "b";

        public Math_AddNode()
        {
            Op = TypeName;
            Configuration =  new ConfigDescriptor[]
            {
            };

            OutputFlowSockets = new FlowSocketDescriptor[]
            {
            };

            InputValueSockets = new InputValueSocketDescriptor[]
            {
                new InputValueSocketDescriptor()
                {
                    Id = IdValueA,
                    SupportedTypes = new string[]{"float","float2","float3","float4","float4x4"},
                    typeRestriction = TypeRestriction.SameAsInputPort(IdValueB)
                },
                new InputValueSocketDescriptor()
                {
                    Id = IdValueB,
                    SupportedTypes = new string[]{"float","float2","float3","float4","float4x4"},
                    typeRestriction = TypeRestriction.SameAsInputPort(IdValueA)
                }
            };
            
            OutputValueSockets = new OutValueSocketDescriptor[]
            {
                new OutValueSocketDescriptor()
                {
                    Id = IdOut,
                    SupportedTypes = new string[]{"float","float2","float3","float4","float4x4"},
                    expectedType = ExpectedType.FromInputSocket(IdValueA)
                }
            };
        }
    }
}
