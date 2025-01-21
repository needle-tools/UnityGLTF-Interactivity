namespace UnityGLTF.Interactivity.Schema
{
    public class Event_ReceiveNode: GltfInteractivityNodeSchema
    {
        public static readonly string TypeName = "event/receive";
        public static readonly string IdFlowOut = "out";
        
        public Event_ReceiveNode()
        {
            Op = TypeName;
            Description = "Receive a custom event";
            Configuration = new ConfigDescriptor[]
            {
                new()
                {
                    Id = "event",
                    Type = "int", 
                }
            };
            // TODO custom data
            OutputValueSockets = new OutValueSocketDescriptor[]
            {
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