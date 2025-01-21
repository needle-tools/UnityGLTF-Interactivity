namespace UnityGLTF.Interactivity.Schema
{
    internal class Animation_StopNode : GltfInteractivityNodeSchema
    {
        public static readonly string TypeName = "animation/stop";
        public static readonly string IdFlowIn = "in";
        public static readonly string IdFlowOut = "out";
        public static readonly string IdFlowError = "err";
        public static readonly string IdValueAnimation = "animation";
        
        public Animation_StopNode()
        {
            Op = TypeName;
            Description = "Stops an animation.";
            InputFlowSockets = new FlowSocketDescriptor[]
            {
                new FlowSocketDescriptor()
                {
                    Id = IdFlowIn,
                }
            };
            OutputFlowSockets = new FlowSocketDescriptor[]
            {
                new FlowSocketDescriptor()
                {
                    Id = IdFlowOut,
                },
                new FlowSocketDescriptor()
                {
                    Id = IdFlowError,
                }
            };
            InputValueSockets = new InputValueSocketDescriptor[]
            {
                new InputValueSocketDescriptor()
                {
                    Id = IdValueAnimation,
                    Description = "The index of the animation to play.",
                    SupportedTypes = new string[] { "int" }
                },
            };
        }
    }
}
