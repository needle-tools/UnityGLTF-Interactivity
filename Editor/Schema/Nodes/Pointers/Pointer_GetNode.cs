namespace UnityGLTF.Interactivity.Schema
{
    public class Pointer_GetNode : GltfInteractivityNodeSchema
    {
        public static readonly string TypeName = "pointer/get";
        public static readonly string IdValue = "value";
        public static readonly string IdPointer = "pointer";

        public Pointer_GetNode()
        {
            Type = TypeName;
            Description = "";
            Configuration = new ConfigDescriptor[]
            {
                new ConfigDescriptor()
                {
                    Id = IdPointer,
                    Type = "pointer",
                    Description = "The pointer to get."
                }
            };
            
            OutputValueSockets = new OutValueSocketDescriptor[]
            {
                new OutValueSocketDescriptor()
                {
                    Id = IdValue,
                    Description = "",
                    SupportedTypes = GltfInteractivityTypeMapping.allTypes
                },

            };
        }
        
    }
}