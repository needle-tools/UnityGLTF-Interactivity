namespace UnityGLTF.Interactivity.Schema
{
    public class Flow_SequenceNode : GltfInteractivityNodeSchema
    {
        public static readonly string TypeName = "flow/sequence";
        public static readonly string IdFlowIn = "in";
       
        public Flow_SequenceNode()
        {
            Op = TypeName;
            Description = "";
            Configuration =  new ConfigDescriptor[]
            {
            };

            InputFlowSockets = new FlowSocketDescriptor[]
            {
                new FlowSocketDescriptor()
                {
                    Id = IdFlowIn,
                    Description = "The flow to be followed when the custom event is fired."
                }
            };   
        }
    }
}