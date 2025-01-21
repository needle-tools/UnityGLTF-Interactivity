namespace UnityGLTF.Interactivity.Schema
{
    public class Math_IsInfNode: GltfInteractivityNodeSchema
    {
        public static readonly string TypeName = "math/isinf";
        public static readonly string IdInputA = "a";
        public static readonly string IdValueResult = "value";
        
        public Math_IsInfNode()
        {
            Op = TypeName;

            InputValueSockets = new InputValueSocketDescriptor[]
            {
                new InputValueSocketDescriptor()
                {
                    Id = IdInputA,
                    SupportedTypes = new string[]{"float"},
                    typeRestriction = TypeRestriction.LimitToFloat
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