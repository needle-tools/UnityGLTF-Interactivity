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
            Op = TypeName;
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
                    Id = IdFlowDone,
                }
            };
            InputValueSockets = new InputValueSocketDescriptor[]
            {
                new InputValueSocketDescriptor()
                {
                    Id = IdValueAnimation,
                    SupportedTypes = new string[] { "int" }
                },
                new InputValueSocketDescriptor()
                {
                    Id = IdValueSpeed,
                    SupportedTypes = new string[] { "float" }
                },
                new InputValueSocketDescriptor()
                {
                    Id = IdValueStartTime,
                    SupportedTypes = new string[] { "float" }
                },
                new InputValueSocketDescriptor()
                {
                    Id = IdValueEndtime,
                    SupportedTypes = new string[] { "float" }
                }
            };
        }
    }
}
