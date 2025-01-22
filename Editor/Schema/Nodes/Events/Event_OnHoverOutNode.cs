using GLTF.Schema;

namespace UnityGLTF.Interactivity.Schema
{
    public class Event_OnHoverOutNode : GltfInteractivityNodeSchema
    {
        public static readonly string TypeName = "event/onHoverOut";
        public static readonly string IdConfigNodeIndex = "nodeIndex";
        public static readonly string IdConfigStopPropagation = "stopPropagation";
        public static readonly string IdFlowOut = "out";
        public static readonly string IdOutHoverNodeIndex = "hoverNodeIndex";
        public static readonly string IdOutControllerIndex = "controllerIndex";

        public Event_OnHoverOutNode()
        {
            Op = TypeName;
            Extension = KHR_node_hoverability_Factory.EXTENSION_NAME;

            Configuration =  new ConfigDescriptor[]
            {
                new ConfigDescriptor()
                {
                    Id = IdConfigNodeIndex,
                    Type = "int",
                },
                new ConfigDescriptor()
                {
                    Id = IdConfigStopPropagation,
                    Type = "bool",
                }
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
                    Id = IdOutHoverNodeIndex,
                    SupportedTypes = new string[]{"int"}
                },
                new OutValueSocketDescriptor()
                {
                    Id = IdOutControllerIndex,
                    SupportedTypes = new string[]{"int"}
                }
            };
        }
        
        
    }
}