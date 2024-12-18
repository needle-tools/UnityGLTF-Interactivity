namespace UnityGLTF.Interactivity.Schema
{
    public class Type_IntToBoolNode : GltfInteractivityNodeSchema
    {
        public static readonly string TypeName = "type/intToBool";
        public static readonly string IdInputA = "a";
        public static readonly string IdValueResult = "value";
        
        public Type_IntToBoolNode()
        {
            Type = TypeName;

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
                    SupportedTypes = new string[]{"bool"},
                    expectedType = ExpectedType.Int
                }
            };
        }
        
    }
}