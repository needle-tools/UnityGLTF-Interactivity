namespace UnityGLTF.Interactivity.Schema
{
    public class Type_BoolToFloatNode : GltfInteractivityNodeSchema
    {
        public static readonly string TypeName = "type/boolToFloat";
        public static readonly string IdInputA = "a";
        public static readonly string IdValueResult = "value";
        
        public Type_BoolToFloatNode()
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
                    SupportedTypes = new string[]{"float"},
                    expectedType = ExpectedType.Int
                }
            };
        }
        
    }
}