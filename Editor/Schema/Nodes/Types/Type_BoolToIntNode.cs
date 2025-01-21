namespace UnityGLTF.Interactivity.Schema
{
    public class Type_BoolToIntNode : GltfInteractivityNodeSchema
    {
        public static readonly string TypeName = "type/boolToInt";
        public static readonly string IdInputA = "a";
        public static readonly string IdValueResult = "value";
        
        public Type_BoolToIntNode()
        {
            Op = TypeName;

            InputValueSockets = new InputValueSocketDescriptor[]
            {
                new InputValueSocketDescriptor()
                {
                    Id = IdInputA,
                    SupportedTypes = new string[]{"bool"}
                }
            };
            
            OutputValueSockets = new OutValueSocketDescriptor[]
            {
                new OutValueSocketDescriptor()
                {
                    Id = IdValueResult,
                    SupportedTypes = new string[]{"int"},
                    expectedType = ExpectedType.Int
                }
            };
        }
    }
}