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

            Description = "This node will fire when a node is selected.";
            Configuration =  new ConfigDescriptor[]
            {
                new ConfigDescriptor()
                {
                    Id = IdConfigNodeIndex,
                    Type = "int",
                    Description = "The node to add the listener on"
                },
                new ConfigDescriptor()
                {
                    Id = IdConfigStopPropagation,
                    Type = "bool",
                    Description = "Should the event be propagated up the node parent hierarchy"
                }
            };

            OutputFlowSockets = new FlowSocketDescriptor[]
            {
                new FlowSocketDescriptor()
                {
                    Id = IdFlowOut,
                    Description = "The flow to be followed when the custom event is fired."
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