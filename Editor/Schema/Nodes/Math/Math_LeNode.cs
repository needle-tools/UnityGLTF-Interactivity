namespace UnityGLTF.Interactivity.Schema
{
    public class Math_LeNode : GltfInteractivityNodeSchema
    {
        public static readonly string TypeName = "math/le";
        public static readonly string IdOut = "value";
        public static readonly string IdValueA = "a";
        public static readonly string IdValueB = "b";

        public Math_LeNode()
        {
            Op = TypeName;
            Configuration =  new ConfigDescriptor[]
            {
            };
            
            InputValueSockets = new InputValueSocketDescriptor[]
            {
                new InputValueSocketDescriptor()
                {
                    Id = IdValueA,
                    SupportedTypes = new string[]{"int", "float","float2","float3","float4"},
                    typeRestriction = TypeRestriction.SameAsInputPort(IdValueB)
                },
                new InputValueSocketDescriptor()
                {
                    Id = IdValueB,
                    SupportedTypes = new string[]{"int", "float","float2","float3","float4"},
                    typeRestriction = TypeRestriction.SameAsInputPort(IdValueA)
                }
            };
            
            OutputValueSockets = new OutValueSocketDescriptor[]
            {
                new OutValueSocketDescriptor()
                {
                    Id = IdOut,
                    SupportedTypes = new string[]{"bool"}
                }
            };
        }
        
    }
}