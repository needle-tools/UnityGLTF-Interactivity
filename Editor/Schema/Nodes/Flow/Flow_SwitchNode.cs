namespace UnityGLTF.Interactivity.Schema
{
    public class Flow_SwitchNode : GltfInteractivityNodeSchema
    {
        public static readonly string TypeName = "flow/switch";
        public static readonly string IdFlowIn = "in";
        public static readonly string IdSelection = "selection";
        public static readonly string IdFDefaultFlowOut = "default";

        public Flow_SwitchNode()
        {
            Op = TypeName;
            Configuration = new ConfigDescriptor[]
            {
            };

            InputFlowSockets = new FlowSocketDescriptor[]
            {
                new FlowSocketDescriptor()
                {
                    Id = IdFlowIn,
                },
            };

            OutputFlowSockets = new FlowSocketDescriptor[]
            {
                new FlowSocketDescriptor()
                {
                    Id = IdFDefaultFlowOut
                }
            };

            InputValueSockets = new InputValueSocketDescriptor[]
            {
                new InputValueSocketDescriptor()
                {
                    Id = IdSelection,
                    SupportedTypes = new string[]{"int"}
                }
            };
        }
        
    }
}