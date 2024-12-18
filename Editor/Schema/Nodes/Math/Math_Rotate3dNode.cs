namespace UnityGLTF.Interactivity.Schema
{
    public class Math_Rotate3dNode : GltfInteractivityNodeSchema
    {
        public static readonly string TypeName = "math/rotate3d";
        public static readonly string IdInputVector = "a";
        public static readonly string IdInputAxis = "b";
        public static readonly string IdInputAngleRadians = "b";
        public static readonly string IdOutputResult = "value";
        
        public Math_Rotate3dNode()
        {
            Type = TypeName;
            
            OutputValueSockets = new OutValueSocketDescriptor[]
            {
                new OutValueSocketDescriptor()
                {
                    Id = IdOutputResult,
                    SupportedTypes = new []{"float3"},
                }
            };
            
            InputValueSockets = new InputValueSocketDescriptor[]
            {
                new InputValueSocketDescriptor()
                {
                    Id = IdInputVector,
                    SupportedTypes = new []{"float3"},
                },
                new InputValueSocketDescriptor()
                {
                    Id = IdInputAxis,
                    SupportedTypes = new []{"float3"},
                },
                new InputValueSocketDescriptor()
                {
                    Id = IdInputAngleRadians,
                    SupportedTypes = new []{"float"},
                }
            };
        }
        
    }
}