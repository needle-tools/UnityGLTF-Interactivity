namespace UnityGLTF.Interactivity.Schema
{
    public class Pointer_SetNode : GltfInteractivityNodeSchema
    {
        public static readonly string TypeName = "pointer/set"; 
        public static readonly string IdFlowIn = "in";
        public static readonly string IdFlowOut = "out";
        public static readonly string IdValue = "value";
        public static readonly string IdPointer = "pointer";
        
        public Pointer_SetNode()
        {
            Op = TypeName;
            Description = "";
            Configuration = new ConfigDescriptor[]
            {
                new ConfigDescriptor()
                {
                    Id = IdPointer,
                    Type = "pointer",
                    Description = "The pointer to set."
                }
            };

            OutputFlowSockets = new FlowSocketDescriptor[]
            {
                new FlowSocketDescriptor()
                {
                    Id = IdFlowOut,
                    Description = ""
                }
            };
            
            InputFlowSockets = new FlowSocketDescriptor[]
            {
                new FlowSocketDescriptor()
                {
                    Id = IdFlowIn,
                    Description = ""
                }
            };

            InputValueSockets = new InputValueSocketDescriptor[]
            {
                new InputValueSocketDescriptor()
                {
                    Id = IdValue,
                    Description = "",
                    SupportedTypes = GltfInteractivityTypeMapping.allTypes
                },

            };
        }
        
    }
}