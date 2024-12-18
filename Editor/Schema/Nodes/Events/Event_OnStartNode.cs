namespace UnityGLTF.Interactivity.Schema
{
    public class Event_OnStartNode : GltfInteractivityNodeSchema
    {
        public static readonly string TypeName = "event/onStart";
        public static readonly string IdFlowOut = "out";
 
        public Event_OnStartNode()
        {
            Type = TypeName;
            Configuration =  new ConfigDescriptor[]
            {
            };

            OutputFlowSockets = new FlowSocketDescriptor[]
            {
                new FlowSocketDescriptor()
                {
                    Id = IdFlowOut,
                }
            };
            
        }
        
    }
}