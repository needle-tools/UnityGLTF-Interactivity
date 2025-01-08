namespace UnityGLTF.Interactivity.Schema
{
    public class GltfInt_Rad : GltfInteractivityNodeSchema
    {
        public static readonly string TypeName = "math/rad";
        public static readonly string IdInputA = "a";
        public static readonly string IdValueResult = "value";
        
        public GltfInt_Rad()
        {
            Type = TypeName;

            InputValueSockets = new InputValueSocketDescriptor[]
            {
                new InputValueSocketDescriptor()
                {
                    Id = IdInputA,
                    SupportedTypes = new string[]{"float", "float2", "float3", "float4"}
                }
            };
            
            OutputValueSockets = new OutValueSocketDescriptor[]
            {
                new OutValueSocketDescriptor()
                {
                    Id = IdValueResult,
                    SupportedTypes = new string[]{"float", "float2", "float3", "float4"},
                    expectedType = ExpectedType.FromInputSocket(IdInputA)
                }
            };
        }
        
    }
}