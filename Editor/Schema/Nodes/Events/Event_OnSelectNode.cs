namespace UnityGLTF.Interactivity.Schema
{

    /// <summary>
    /// node/onSelect Node that fires an event when the node is selected.
    ///
    /// See https://github.com/petermart/glTF-InteractivityGraph-AuthoringTool/blob/483da2161aa3d9c01ef47be8cdb2bec2d1dd3a18/src/authoring/AuthoringNodeSpecs.ts#L419
    /// </summary>
    public class Event_OnSelectNode : GltfInteractivityNodeSchema
    {
        public static readonly string TypeName = "event/onSelect";
        public static readonly string IdConfigNodeIndex = "nodeIndex";
        public static readonly string IdConfigStopPropagation = "stopPropagation";
        public static readonly string IdFlowOut = "out";
        public static readonly string IdValueSelectedNodeIndex = "selectedNodeIndex";
        public static readonly string IdValueSelectionRayOrigin = "selectionRayOrigin";
        public static readonly string IdValueLocalHitLocation = "selectionPoint";
        public static readonly string IdValueControllerIndex = "controllerIndex";

        public Event_OnSelectNode()
        {
            Type = TypeName;
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
                    Id = IdValueSelectedNodeIndex,
                    Description = "The index of the first hit node",
                    SupportedTypes = new string[]{"int"}
                },
                new OutValueSocketDescriptor()
                {
                    Id = IdValueLocalHitLocation,
                    Description = "The local hit offset from the hit node's origin",
                    SupportedTypes = new string[]{"float3"}
                },
                new OutValueSocketDescriptor()
                {
                    Id = IdValueControllerIndex,
                    SupportedTypes = new string[]{"int"}
                },
            };
        }
    }
}
