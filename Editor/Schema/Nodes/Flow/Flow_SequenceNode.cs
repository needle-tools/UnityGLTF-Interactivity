namespace UnityGLTF.Interactivity.Schema
{
    public class Flow_SequenceNode : GltfInteractivityNodeSchema
    {
        public static readonly string TypeName = "flow/sequence";
        public static readonly string IdFlowIn = "in";
       
        public Flow_SequenceNode()
        {
            Op = TypeName;
            Configuration =  new ConfigDescriptor[]
            {
            };

            InputFlowSockets = new FlowSocketDescriptor[]
            {
                new FlowSocketDescriptor()
                {
                    Id = IdFlowIn,
                }
            };   
        }
    }
}