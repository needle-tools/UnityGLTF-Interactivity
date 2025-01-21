namespace UnityGLTF.Interactivity.Schema
{
    public class Type_FloatToBoolNode : GltfInteractivityNodeSchema
    {
        public static readonly string TypeName = "type/floatToBool";
        public static readonly string IdInputA = "a";
        public static readonly string IdValueResult = "value";
        
        public Type_FloatToBoolNode()
        {
            Op = TypeName;

            InputValueSockets = new InputValueSocketDescriptor[]
            {
                new InputValueSocketDescriptor()
                {
                    Id = IdInputA,
                    SupportedTypes = new string[]{"float"}
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