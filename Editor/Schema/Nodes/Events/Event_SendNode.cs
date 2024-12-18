namespace UnityGLTF.Interactivity.Schema
{
    public class Event_SendNode: GltfInteractivityNodeSchema
    {
        public static readonly string TypeName = "event/send";
        public static readonly string IdFlowOut = "out";
        public static readonly string IdFlowIn = "in";
        public static readonly string IdEvent = "event";
        
        public Event_SendNode()
        {
            Type = TypeName;
            Description = "Send a custom event";
            Configuration = new ConfigDescriptor[]
            {
                new()
                {
                    Id = IdEvent,
                    Type = "int", 
                }
            };
            // TODO custom data
            InputValueSockets = new InputValueSocketDescriptor[]
            {
            };
            
            InputFlowSockets = new FlowSocketDescriptor[]
            {
                new()
                {
                    Id = IdFlowIn,
                }
            };
            
            OutputFlowSockets = new FlowSocketDescriptor[]
            {
                new()
                {
                    Id = IdFlowOut,
                }
            };
        }
    }
}