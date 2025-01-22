namespace UnityGLTF.Interactivity.Schema
{
    public class Math_GeNode : GltfInteractivityNodeSchema
    {
        public static readonly string TypeName = "math/ge";
        public static readonly string IdOut = "value";
        public static readonly string IdValueA = "a";
        public static readonly string IdValueB = "b";

        public Math_GeNode()
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