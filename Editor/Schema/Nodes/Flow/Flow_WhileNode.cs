namespace UnityGLTF.Interactivity.Schema
{
    public class Flow_WhileNode : GltfInteractivityNodeSchema
    {
        public static readonly string TypeName = "flow/while";
        public static readonly string IdFlowIn = "in";
        public static readonly string IdLoopBody = "loopBody";
        public static readonly string IdCompleted = "completed";
        public static readonly string IdCondition = "condition";

        public Flow_WhileNode()
        {
            Op = TypeName;
            Description = "";
          
            OutputFlowSockets = new FlowSocketDescriptor[]
            {
                new FlowSocketDescriptor()
                {
                    Id = IdLoopBody,
                },
                new FlowSocketDescriptor()
                {
                    Id = IdCompleted,
                }
            };

            InputValueSockets = new InputValueSocketDescriptor[]
            {
                new InputValueSocketDescriptor()
                {
                    Id = IdCondition,
                    Description = "",
                    SupportedTypes = new string[] { "bool" }
                },
            };
        }
    }
}