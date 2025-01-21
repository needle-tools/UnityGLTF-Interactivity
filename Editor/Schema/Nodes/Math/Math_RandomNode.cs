namespace UnityGLTF.Interactivity.Schema
{
    public class Math_RandomNode: GltfInteractivityNodeSchema
    {
        public static readonly string TypeName = "math/random";
        public static readonly string IdValueResult = "value";
        
        public Math_RandomNode()
        {
            Op = TypeName;
            
            OutputValueSockets = new OutValueSocketDescriptor[]
            {
                new OutValueSocketDescriptor()
                {
                    Id = IdValueResult,
                    SupportedTypes = new string[]{"float"},
                    expectedType = ExpectedType.Float
                }
            };
        }
    }
}