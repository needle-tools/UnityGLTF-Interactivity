namespace UnityGLTF.Interactivity.Schema
{
    public class Math_RoundNode : GltfInteractivityNodeSchema
    {
        public static readonly string TypeName = "math/round";
        public static readonly string IdInputA = "a";
        public static readonly string IdValueResult = "value";

        public Math_RoundNode()
        {
            Op = TypeName;

            InputValueSockets = new InputValueSocketDescriptor[]
            {
                new InputValueSocketDescriptor()
                {
                    Id = IdInputA,
                    SupportedTypes = new string[] { "float", "float2", "float3", "float4" }
                }
            };

            OutputValueSockets = new OutValueSocketDescriptor[]
            {
                new OutValueSocketDescriptor()
                {
                    Id = IdValueResult,
                    SupportedTypes = new string[] { "float", "float2", "float3", "float4" },
                    expectedType = ExpectedType.FromInputSocket(IdInputA)
                }
            };
        }
    }
}