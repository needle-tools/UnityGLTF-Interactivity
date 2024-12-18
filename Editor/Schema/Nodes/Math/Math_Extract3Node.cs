namespace UnityGLTF.Interactivity.Schema
{
    public class Math_Extract3Node : GltfInteractivityNodeSchema
    {
        public static readonly string TypeName = "math/extract3";
        public static readonly string IdValueIn = "a";
        public static readonly string IdValueOutX = "0";
        public static readonly string IdValueOutY = "1";
        public static readonly string IdValueOutZ = "2";
        
        public Math_Extract3Node()
        {
            Type = TypeName;
            
            InputValueSockets = new InputValueSocketDescriptor[]
            {
                new InputValueSocketDescriptor()
                {
                    Id = IdValueIn,
                    SupportedTypes = new string[] { "float4" }
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
                },
                new OutValueSocketDescriptor()
                {
                    Id = IdValueOutZ,
                    SupportedTypes = new string[] { "float" }
                },          
            };
        }
        
    }
}