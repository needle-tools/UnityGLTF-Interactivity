namespace UnityGLTF.Interactivity.Schema
{
    public class Pointer_GetNode : GltfInteractivityNodeSchema
    {
        public static readonly string TypeName = "pointer/get";
        public static readonly string IdValue = "value";
        public static readonly string IdPointer = "pointer";

        public Pointer_GetNode()
        {
            Op = TypeName;
            Configuration = new ConfigDescriptor[]
            {
                new ConfigDescriptor()
                {
                    Id = IdPointer,
                    Type = "pointer",
                }
            };
            
            OutputValueSockets = new OutValueSocketDescriptor[]
            {
                new OutValueSocketDescriptor()
                {
                    Id = IdValue,
                    SupportedTypes = GltfInteractivityTypeMapping.allTypes
                },

            };
        }
        
    }
}