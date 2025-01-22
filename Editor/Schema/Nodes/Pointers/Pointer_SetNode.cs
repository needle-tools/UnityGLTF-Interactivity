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
            Configuration = new ConfigDescriptor[]
            {
                new ConfigDescriptor()
                {
                    Id = IdPointer,
                    Type = "pointer",
                }
            };

            OutputFlowSockets = new FlowSocketDescriptor[]
            {
                new FlowSocketDescriptor()
                {
                    Id = IdFlowOut,
                }
            };
            
            InputFlowSockets = new FlowSocketDescriptor[]
            {
                new FlowSocketDescriptor()
                {
                    Id = IdFlowIn,
                }
            };

            InputValueSockets = new InputValueSocketDescriptor[]
            {
                new InputValueSocketDescriptor()
                {
                    Id = IdValue,
                    SupportedTypes = GltfInteractivityTypeMapping.allTypes
                },

            };
        }
        
    }
}