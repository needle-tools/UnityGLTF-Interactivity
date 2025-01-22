namespace UnityGLTF.Interactivity.Schema.Bitwise
{
    public class Math_LeftShift : GltfInteractivityNodeSchema
    {
        public static readonly string TypeName = "math/lsl";
        public static readonly string IdOut = "value";
        public static readonly string IdValueA = "a";
        public static readonly string IdValueB = "b";

        public Math_LeftShift()
        {
            Op = TypeName;

            InputValueSockets = new InputValueSocketDescriptor[]
            {
                new InputValueSocketDescriptor()
                {
                    Id = IdValueA,
                    SupportedTypes = new string[] { "int" },
                    typeRestriction = TypeRestriction.LimitToInt
                },
                new InputValueSocketDescriptor()
                {
                    Id = IdValueB,
                    SupportedTypes = new string[] { "int" },
                    typeRestriction = TypeRestriction.LimitToInt
                }
            };

            OutputValueSockets = new OutValueSocketDescriptor[]
            {
                new OutValueSocketDescriptor()
                {
                    Id = IdOut,
                    SupportedTypes = new string[] { "int" },
                    expectedType = ExpectedType.Int
                }
            };
        }
    }
}