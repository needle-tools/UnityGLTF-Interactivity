namespace UnityGLTF.Interactivity.Schema
{
    public class Type_FloatToIntNode : GltfInteractivityNodeSchema
    {
        public static readonly string TypeName = "type/floatToInt";
        public static readonly string IdInputA = "a";
        public static readonly string IdValueResult = "value";
        
        public Type_FloatToIntNode()
        {
            Type = TypeName;

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
                    SupportedTypes = new string[]{"int"},
                    expectedType = ExpectedType.Int
                }
            };
        }
        
    }
}