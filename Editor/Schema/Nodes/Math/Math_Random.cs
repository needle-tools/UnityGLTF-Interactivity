namespace UnityGLTF.Interactivity.Schema
{
    public class Math_Random: GltfInteractivityNodeSchema
    {
        public static readonly string TypeName = "math/random";
        public static readonly string IdValueResult = "value";
        
        public Math_Random()
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