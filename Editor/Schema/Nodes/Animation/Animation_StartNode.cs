namespace UnityGLTF.Interactivity.Schema
{
    /// <summary>
    /// The world/startAnimation node plays a global animation
    ///
    /// See https://github.com/petermart/glTF-InteractivityGraph-AuthoringTool/blob/483da2161aa3d9c01ef47be8cdb2bec2d1dd3a18/src/authoring/AuthoringNodeSpecs.ts#L181 .
    /// </summary>
    internal class Animation_StartNode : GltfInteractivityNodeSchema
    {
        public static readonly string TypeName = "animation/start";
        public static readonly string IdFlowIn = "in";
        public static readonly string IdFlowOut = "out";
        public static readonly string IdFlowDone = "done";
        public static readonly string IdValueAnimation = "animation";
        public static readonly string IdValueSpeed = "speed";
        public static readonly string IdValueStartTime = "startTime";
        public static readonly string IdValueEndtime = "endTime";

        public Animation_StartNode()
        {
            Type = TypeName;
            Description = "Plays an animation.";
            InputFlowSockets = new FlowSocketDescriptor[]
            {
                new FlowSocketDescriptor()
                {
                    Id = IdFlowIn,
                    Description = "In-flow to trigger this node."
                }
            };
            OutputFlowSockets = new FlowSocketDescriptor[]
            {
                new FlowSocketDescriptor()
                {
                    Id = IdFlowOut,
                    Description = "The synchronous flow to be followed."
                },
                new FlowSocketDescriptor()
                {
                    Id = IdFlowDone,
                    Description = "The flow to be followed when the animation target time is " +
                        "reached, async."
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
                new InputValueSocketDescriptor()
                {
                    Id = IdValueSpeed,
                    Description = "The speed multiplier of the animation, must be greater than " +
                        "zero/strictly positive, otherwise undefined (but you could in your " +
                        "implementation then default it to 1.0).  We specify backward playing " +
                        "using a negative targetTime.",
                    SupportedTypes = new string[] { "float" }
                },
                new InputValueSocketDescriptor()
                {
                    Id = IdValueStartTime,
                    Description = "Start animation frame must be between the range of 0 to max " +
                        "animation time.",
                    SupportedTypes = new string[] { "float" }
                },
                new InputValueSocketDescriptor()
                {
                    Id = IdValueEndtime,
                    Description = "End animation time, if it is before the start time the " +
                        "animation will be played backwards, if it is +/- Inf the animation " +
                        "will loop until manually stopped.",
                    SupportedTypes = new string[] { "float" }
                }
            };
        }
    }
}
