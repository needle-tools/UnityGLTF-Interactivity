namespace UnityGLTF.Interactivity.Schema
{
    public class Type_IntToFloatNode : GltfInteractivityNodeSchema
    {
        public static readonly string TypeName = "type/intToFloat";
        public static readonly string IdInputA = "a";
        public static readonly string IdValueResult = "value";
        
        public Type_IntToFloatNode()
        {
            Op = TypeName;

            InputValueSockets = new InputValueSocketDescriptor[]
            {
                new InputValueSocketDescriptor()
                {
                    Id = IdInputA,
                    SupportedTypes = new string[]{"int"}
                }
            };
            
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