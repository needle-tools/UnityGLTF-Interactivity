namespace UnityGLTF.Interactivity.Schema
{
    public class Math_Extract2Node : GltfInteractivityNodeSchema
    {
        public static readonly string TypeName = "math/extract2";
        public static readonly string IdValueIn = "a";
        public static readonly string IdValueOutX = "0";
        public static readonly string IdValueOutY = "1";
        
        public Math_Extract2Node()
        {
            Op = TypeName;

            InputValueSockets = new InputValueSocketDescriptor[]
            {
                new InputValueSocketDescriptor()
                {
                    Id = IdValueIn,
                    SupportedTypes = new string[] { "float2" }
                }
            };
            
            OutputValueSockets = new OutValueSocketDescriptor[]
            {
                new OutValueSocketDescriptor()
                {
                    Id = IdValueOutX,
                    SupportedTypes = new string[] { "float" }
                },
                new OutValueSocketDescriptor()
                {
                    Id = IdValueOutY,
                    SupportedTypes = new string[] { "float" }
                }
            };
        }
        
    }
}