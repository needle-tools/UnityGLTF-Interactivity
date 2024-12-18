namespace UnityGLTF.Interactivity.Schema
{
    public class Event_OnTickNode: GltfInteractivityNodeSchema
    {
        public static readonly string TypeName = "event/onTick";
        public static readonly string IdFlowOut = "out";
        public static readonly string IdOutTimeSinceStart = "timeSinceStart";
        public static readonly string IdOutTimeSinceLastTick = "timeSinceLastTick";
        
        public Event_OnTickNode()
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
            
            OutputValueSockets = new OutValueSocketDescriptor[]
            {
                new OutValueSocketDescriptor()
                {
                    Id = IdOutTimeSinceStart,
                    SupportedTypes = new string[]{"float"},
                    expectedType = ExpectedType.Float
                },
                new OutValueSocketDescriptor()
                {
                    Id = IdOutTimeSinceLastTick,
                    SupportedTypes = new string[]{"float"},
                    expectedType = ExpectedType.Float
                },
                
            };
            
        }
        
    }
}