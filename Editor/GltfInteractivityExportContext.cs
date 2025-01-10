using System.Text;
using UnityGLTF.Interactivity.Export;
using UnityGLTF.Interactivity.Schema;

namespace UnityGLTF.Interactivity
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using GLTF.Schema;
    using UnityEngine;
    using Unity.VisualScripting;
    using UnityGLTF;
    using UnityGLTF.Plugins;

    public interface IInteractivityExport
    {
        void OnInteractivityExport(GltfInteractivityExportContext context, GltfInteractivityExportNodes nodes);
    }

    public class GltfInteractivityExportNodes
    {
        public readonly List<GltfInteractivityNode> nodes = new List<GltfInteractivityNode>();

        public GltfInteractivityNode CreateNode(GltfInteractivityNodeSchema schema)
        {
            var newNode = new GltfInteractivityNode(schema);
            nodes.Add(newNode);
            newNode.Index = nodes.Count - 1;
            return newNode;
        }
        
        internal GltfInteractivityExportNodes(GltfInteractivityNode[] nodes)
        {
            this.nodes.AddRange(nodes);
        }
    }
    
    public class GltfInteractivityExportContext: GLTFExportPluginContext
    {
        // TODO: Clear all these values once the scene export is done
        public ScriptMachine ActiveScriptMachine = null;
        public GLTFRoot ActiveGltfRoot = null;

        public GLTFSceneExporter exporter { get; private set; }
        
        internal List<GltfInteractivityExtension.Variable> variables = new List<GltfInteractivityExtension.Variable>();
        internal List<GltfInteractivityExtension.CustomEvent> customEvents = new List<GltfInteractivityExtension.CustomEvent>();
        private List<UnitExporter> nodesToExport = new List<UnitExporter>();
        
        public delegate void OnBeforeSerializationDelegate(GltfInteractivityNode[] nodes);
        public event OnBeforeSerializationDelegate OnBeforeSerialization;
        
        public class InputPortGraph 
        {
            public IUnitInputPort port;
            public ExportGraph graph;
            
            public override int GetHashCode()
            {
                string s = $"{port.key}+{port.unit.ToString()}+{graph.graph.title}+{graph.graph.GetHashCode()}";

                return s.GetHashCode();
            }
            
            public InputPortGraph(IUnitInputPort port, ExportGraph graph)
            {
                this.port = port;
                this.graph = graph;
            }
        }
        
        public class InputportGraphComparer : IEqualityComparer<InputPortGraph>
        {
            public bool Equals(InputPortGraph x, InputPortGraph y)
            {
                return x.port == y.port && x.graph == y.graph;
            }

            public int GetHashCode(InputPortGraph obj)
            {
                return obj.GetHashCode();
            }
        }
        
        public class ExportGraph
        {
            public GameObject gameObject = null;
            public ExportGraph parentGraph = null;
            public FlowGraph graph;
            public Dictionary<IUnit, UnitExporter> nodes = new Dictionary<IUnit, UnitExporter>();
            internal Dictionary<IUnitInputPort, IUnitInputPort> bypasses = new Dictionary<IUnitInputPort, IUnitInputPort>();
            public List<ExportGraph> subGraphs = new List<ExportGraph>();
        }

        public class VariableBasedList
        {
            public readonly GltfInteractivityExportContext Context;
            public readonly int Capacity;
            public readonly int StartIndex = 0;
            public readonly int EndIndex = 0;
            public readonly string ListId;
            public readonly int CountVarId;
            public readonly int CapacityVarId;
            public readonly int ListIndex;
            public readonly int CurrentIndexVarId;
            public readonly int ValueToSetVarId;

            public GltfInteractivityUnitExporterNode.ValueOutputSocketData getCountNodeSocket;
            public GltfInteractivityUnitExporterNode.ValueOutputSocketData getValueNodeSocket;
            public GltfInteractivityUnitExporterNode.FlowInSocketData setValueFlowIn;
            public Unit listCreatorUnit;
            public ExportGraph listCreatorGraph;
            
            public VariableBasedList(GltfInteractivityExportContext context, string listId, int capacity, int gltfType)
            {
                Capacity = capacity;
                Context = context;
                ListId = listId;
                
                CurrentIndexVarId = Context.AddVariableWithIdIfNeeded($"VARLIST_{listId}_CurrentIndex", 0, VariableKind.Scene, GltfInteractivityTypeMapping.TypeIndexByGltfSignature("int"));
                ValueToSetVarId = Context.AddVariableWithIdIfNeeded($"VARLIST_{listId}_ValueToSet", 0, VariableKind.Scene, GltfInteractivityTypeMapping.TypeIndexByGltfSignature("int"));
                
                ListIndex = Context.variables.Count;
                
                CountVarId = Context.AddVariableWithIdIfNeeded($"VARLIST_{listId}_Count", 0, VariableKind.Scene, GltfInteractivityTypeMapping.TypeIndexByGltfSignature("int"));
                CapacityVarId = Context.AddVariableWithIdIfNeeded($"VARLIST_{listId}_Capacity", Capacity, VariableKind.Scene, GltfInteractivityTypeMapping.TypeIndexByGltfSignature("int"));
                
                StartIndex = -1;
                EndIndex = -1;
                for (int i = 0; i < capacity; i++)
                {
                    EndIndex = Context.AddVariableWithIdIfNeeded($"VARLIST_{listId}_{i}", null, VariableKind.Scene, gltfType);
                    if (StartIndex == -1)
                        StartIndex = EndIndex;
                }
            }

            public void ClearList()
            {
                Context.variables[CountVarId].Value = 0;
            }
            
            public void AddItem(object value)
            {
                if (Context.variables[CountVarId].Value is int count)
                {
                    if (count >= Capacity)
                        throw new ArgumentException("List is full");
                    
                    Context.variables[StartIndex + count].Value = value;
                    Context.variables[CountVarId].Value = count + 1;
                }
            }

        }
        
        internal Dictionary<InputPortGraph, InputPortGraph> graphBypasses = new Dictionary<InputPortGraph, InputPortGraph>(new InputportGraphComparer());
        internal Dictionary<(IUnitInputPort port, SubgraphUnit subGraph), (IUnitInputPort port, ExportGraph graph)> bypassesSubGraphs = new Dictionary<(IUnitInputPort, SubgraphUnit), (IUnitInputPort, ExportGraph)>();
        private List<ExportGraph> addedGraphs = new List<ExportGraph>();
        private List<VariableBasedList> addedVariableBasedLists = new List<VariableBasedList>();
        
        internal ExportGraph currentGraphProcessing { get; private set; } = null;
        
        public GltfInteractivityExportPlugin plugin;
        
        public GltfInteractivityExportContext(GltfInteractivityExportPlugin plugin)
        {
            this.plugin = plugin;
        }

        public VariableBasedList GetListByCreator(IUnit listCreatorUnit)
        {
            return addedVariableBasedLists
                .FirstOrDefault(l => l.listCreatorUnit == listCreatorUnit && l.listCreatorGraph == currentGraphProcessing);
        }
        
        public VariableBasedList CreateNewVariableBasedList(int capacity, int gltfType, string listId = null)
        {
            if (string.IsNullOrEmpty(listId))
                listId = Guid.NewGuid().ToString();
            var newVariableBasedList = new VariableBasedList(this, listId, capacity, gltfType);
            addedVariableBasedLists.Add(newVariableBasedList);
            return newVariableBasedList;
        }

        /// <summary>
        /// Get the value of a variable from a VariableUnit.
        /// Materials and GameObjects Values will be converted to their respective indices.
        /// </summary>
        public object GetVariableValue(UnifiedVariableUnit unit, out string varName, out string varType)
        {
            var rawValue = GetVariableValueRaw(unit, out varName, out varType);
            
            if (rawValue is GameObject gameObjectValue)
                rawValue = exporter.GetTransformIndex(gameObjectValue.transform);
            else if (rawValue is Component component)
                rawValue = exporter.GetTransformIndex(component.transform);
            else if (rawValue is Material materialValue)
                rawValue = exporter.GetMaterialIndex(materialValue);

            return rawValue;
        }
        
        /// <summary>
        /// Get the value of a variable from a VariableUnit.
        /// Materials and GameObjects Values will be returned as is.
        /// </summary>
        public object GetVariableValueRaw(UnifiedVariableUnit unit, out string exportVarName, out string varType)
        {
            string varName = unit.name.unit.defaultValues["name"] as string;

            exportVarName = varName;
            object varValue = null;
            varType = null;

            VariableDeclarations varDeclarations = null; 
            
            switch (unit.kind)
            {
                case VariableKind.Flow:
                case VariableKind.Graph:
                    varDeclarations = unit.graph.variables;
                    break;
                case VariableKind.Object:

                    var gameObject = GltfInteractivityNodeHelper.GetGameObjectFromValueInput(unit.@object, unit.defaultValues, this);
                    if (gameObject != null)
                    {
                        varDeclarations = Variables.Object(gameObject);

                        var gameObjectIndex =exporter.GetTransformIndex(gameObject.transform);
                        exportVarName = $"node_{gameObjectIndex}_{varName}";
                    }
                    
                    break;
                case VariableKind.Scene:
                    varDeclarations = Variables.ActiveScene;
                    break;
                case VariableKind.Application:
                    varDeclarations = Variables.Application;
                    break;
                case VariableKind.Saved:
                    varDeclarations = Variables.Saved;
                    break;
            }
            
            if (varDeclarations != null)
            {
                var varDeclaration = varDeclarations.GetDeclaration(varName);
                if (varDeclaration != null)
                {
                    varValue = varDeclaration.value;
                    varType = varDeclaration.typeHandle.Identification;
                }
                else
                {
                    Debug.LogError("Variable not found: " + varName);
                    return null;
                }
            }
            else
            {
                Debug.LogError("Variable not found: " + varName);
                return null;
            }

            if (varType == null)
            {
                return null;
            }

            var typeIndex = GltfInteractivityTypeMapping.TypeIndex(varType);
            if (typeIndex == -1)
            {
                Debug.LogError("Unsupported type for variable: " + varType);
                return null;
            }
            
            return varValue;
        }

        public int GetValueTypeForInput(GltfInteractivityNode[] allNodes, GltfInteractivityNode node, string socketName)
        {
            if (!node.ValueSocketConnectionData.TryGetValue(socketName, out var socketData))
                return -1;

            if (socketData.Node != null)
            {
                var nodeIndex = socketData.Node.Value;
                if (nodeIndex >= 0 && nodeIndex < allNodes.Length)
                {
                    var inputSourceNode = allNodes[nodeIndex];
                    if (inputSourceNode.OutValueSocket.TryGetValue(socketData.Socket,
                            out var sourceNodeOutSocketData))
                    {
                        if (sourceNodeOutSocketData.expectedType != null)
                        {
                            if (sourceNodeOutSocketData.expectedType.typeIndex != null)
                                return sourceNodeOutSocketData.expectedType.typeIndex.Value;

                            if (sourceNodeOutSocketData.expectedType.fromInputPort != null)
                                return GetValueTypeForInput(allNodes, inputSourceNode,
                                    sourceNodeOutSocketData.expectedType.fromInputPort);
                        }
                    }
                    
                }

                return -1;
            }

            return socketData.Type;
        }
        
        public int AddVariableIfNeeded(UnifiedVariableUnit unit)
        {
            var varValue = GetVariableValue(unit, out string varName, out string varType);

            var variableIndex = AddVariableWithIdIfNeeded(varName, varValue, unit.kind, varType);
            return variableIndex;
        }

        public int AddVariableWithIdIfNeeded(string id, object defaultValue, VariableKind varKind, string type)
        {
            return AddVariableWithIdIfNeeded(id, defaultValue, varKind, GltfInteractivityTypeMapping.TypeIndex(type));
        }
        
        public int AddVariableWithIdIfNeeded(string id, object defaultValue, VariableKind varKind, Type type)
        {
            return AddVariableWithIdIfNeeded(id, defaultValue, varKind, GltfInteractivityTypeMapping.TypeIndex(type));
        }
        
        public int AddVariableWithIdIfNeeded(string id, object defaultValue, VariableKind varKind, int gltfTypeIndex)
        {
            if (gltfTypeIndex == -1)
            {
                throw new ArgumentException("Type not supported for variable: " + defaultValue.GetType().Name);
            }
            
            if (addedGraphs.Count > 0)
            {
                switch (varKind)
                {
                    case VariableKind.Flow:
                    case VariableKind.Graph:
                        id = $"graph{addedGraphs.FindIndex(ag => ag == currentGraphProcessing)}_{id}";
                        break;
                }
            }
            
            var index = variables.FindIndex(v => v.Id == id);
            if (index != -1)
                return index;

            GltfInteractivityExtension.Variable newVariable = new GltfInteractivityExtension.Variable();
            newVariable.Id = id;
            
            newVariable.Type = gltfTypeIndex;
            
            newVariable.Value = defaultValue;
            variables.Add(newVariable);
            return variables.Count - 1;
        }
        
        public int AddEventIfNeeded(Unit eventUnit, GltfInteractivityUnitExporterNode.EventValues[] arguments = null)
        {
            var eventId = eventUnit.defaultValues["name"] as string;
            GameObject target = null;

            ValueInput targetValueInput;
            if (eventUnit is TriggerCustomEvent triggerCustomEvent)
                targetValueInput = triggerCustomEvent.target;
            else if (eventUnit is CustomEvent customEvent)
                targetValueInput = customEvent.target;
            else
                throw new ArgumentException("Event Unit type not supported: " + eventUnit.GetType().Name);

            string id = null; 
            target = GltfInteractivityNodeHelper.GetGameObjectFromValueInput(targetValueInput, eventUnit.defaultValues, this);
            if (target != null)
            {
                var targetIndex = exporter.GetTransformIndex(target.transform);
               // id = $"node_{targetIndex}_{eventId}";
            }

            if (id == null)
            {
                id = eventId;
            }
            
            return AddEventWithIdIfNeeded(id, arguments);
        }
        
        public int AddEventWithIdIfNeeded(string id, GltfInteractivityUnitExporterNode.EventValues[] arguments = null)
        {
            var index = customEvents.FindIndex(customEvents => customEvents.Id == id);
            if (index != -1)
            {
                // Compare the arguments
                return index;
            }

            GltfInteractivityExtension.CustomEvent newEvent = new GltfInteractivityExtension.CustomEvent();
            newEvent.Id = id;

            if (arguments != null)
                newEvent.Values = arguments;

            customEvents.Add(newEvent);
            return customEvents.Count - 1;
        }
        
        public void AddHoverabilityExtensionToNode(int nodeIndex)
        {
            if (nodeIndex == -1)
                return;
            
            var nodeExtensions = ActiveGltfRoot.Nodes[nodeIndex].Extensions;
            if (nodeExtensions == null)
            {
                nodeExtensions = new Dictionary<string, IExtension>();
                ActiveGltfRoot.Nodes[nodeIndex].Extensions = nodeExtensions;
            }
            if (!nodeExtensions.ContainsKey(KHR_node_hoverability_Factory.EXTENSION_NAME))
            {
                nodeExtensions.Add(KHR_node_hoverability_Factory.EXTENSION_NAME, new KHR_node_hoverability());
            }
            exporter.DeclareExtensionUsage(KHR_node_hoverability_Factory.EXTENSION_NAME, false);
        }

        public void AddVisibilityExtensionToNode(int nodeIndex)
        {
            if (nodeIndex == -1)
                return;
            
            var nodeExtensions = ActiveGltfRoot.Nodes[nodeIndex].Extensions;
            if (nodeExtensions == null)
            {
                nodeExtensions = new Dictionary<string, IExtension>();
                ActiveGltfRoot.Nodes[nodeIndex].Extensions = nodeExtensions;
            }
            if (!nodeExtensions.ContainsKey(KHR_node_visbility_Factory.EXTENSION_NAME))
            {
                nodeExtensions.Add(KHR_node_visbility_Factory.EXTENSION_NAME, new KHR_node_visbility());
            }
            exporter.DeclareExtensionUsage(KHR_node_visbility_Factory.EXTENSION_NAME, false);
        }
        
        public void AddVisibilityExtensionToAllNodes()
        {
            foreach (var node in ActiveGltfRoot.Nodes)
            {
                var nodeExtensions = node.Extensions;
                if (nodeExtensions == null)
                {
                    nodeExtensions = new Dictionary<string, IExtension>();
                    node.Extensions = nodeExtensions;
                }

                if (!nodeExtensions.ContainsKey(KHR_node_visbility_Factory.EXTENSION_NAME))
                {
                    nodeExtensions.Add(KHR_node_visbility_Factory.EXTENSION_NAME, new KHR_node_visbility());
                }
            }

            exporter.DeclareExtensionUsage(KHR_node_visbility_Factory.EXTENSION_NAME, false);
        }
        
        public void AddSelectabilityExtensionToNode(int nodeIndex)
        {
            if (nodeIndex == -1)
                return;

            var nodeExtensions = ActiveGltfRoot.Nodes[nodeIndex].Extensions;
            if (nodeExtensions == null)
            {
                nodeExtensions = new Dictionary<string, IExtension>();
                ActiveGltfRoot.Nodes[nodeIndex].Extensions = nodeExtensions;
            }
            if (!nodeExtensions.ContainsKey(KHR_node_selectability_Factory.EXTENSION_NAME))
            {
                nodeExtensions.Add(KHR_node_selectability_Factory.EXTENSION_NAME, new KHR_node_selectability());
            }
            exporter.DeclareExtensionUsage(KHR_node_selectability_Factory.EXTENSION_NAME, false);
        }
        
        public void AddSelectabilityExtensionToAllNode()
        {
            foreach (var node in ActiveGltfRoot.Nodes)
            {
                var nodeExtensions = node.Extensions;
                if (nodeExtensions == null)
                {
                    nodeExtensions = new Dictionary<string, IExtension>();
                    node.Extensions = nodeExtensions;
                }

                if (!nodeExtensions.ContainsKey(KHR_node_selectability_Factory.EXTENSION_NAME))
                {
                    nodeExtensions.Add(KHR_node_selectability_Factory.EXTENSION_NAME, new KHR_node_selectability());
                }
            }

            exporter.DeclareExtensionUsage(KHR_node_selectability_Factory.EXTENSION_NAME, false);
        }
        
        internal ExportGraph AddGraph(FlowGraph graph)
        {
            var newExportGraph = new ExportGraph();
            newExportGraph.gameObject = ActiveScriptMachine.gameObject;
            newExportGraph.parentGraph = currentGraphProcessing;
            newExportGraph.graph = graph;
            addedGraphs.Add(newExportGraph);
            if (currentGraphProcessing != null)
                currentGraphProcessing.subGraphs.Add(newExportGraph);
            
            var lastCurrentGraph = currentGraphProcessing;
            currentGraphProcessing = newExportGraph;
            // Topologically sort the graph to establish the dependency order
            LinkedList<IUnit> topologicallySortedNodes = TopologicalSort(graph.units);
            
            newExportGraph.nodes = GltfInteractivityNodeHelper.GetTranslatableNodes(topologicallySortedNodes, this);
            
            nodesToExport.AddRange(newExportGraph.nodes.Select( g => g.Value));
 
            currentGraphProcessing = lastCurrentGraph;
            return newExportGraph;
        }
        
        private void TriggerInterfaceExportCallbacks(ref GltfInteractivityNode[] nodes)
        {
            GltfInteractivityExportNodes nodesExport = new GltfInteractivityExportNodes(nodes);
            
            foreach (var root in exporter.RootTransforms)
            {
                var interfaces = root.GetComponentsInChildren<IInteractivityExport>(true);
                foreach (var callback in interfaces)
                {
                    callback.OnInteractivityExport(this, nodesExport);
                }                
            }

            nodes = nodesExport.nodes.ToArray();
        }
        
        /// <summary>
        /// Called after the scene has been exported to add interactivity data.
        ///
        /// This overload of AfterSceneExport exposes the origins as a parameter to simplify tests.
        /// </summary>
        /// <param name="exporter"> GLTFSceneExporter object used to export the scene</param>
        /// <param name="gltfRoot"> Root GLTF object for the gltf object tree</param>
        /// <param name="visualScriptingComponents"> list of ScriptMachines in the scene.</param>
        internal void AfterSceneExport(GLTFSceneExporter exporter, GLTFRoot gltfRoot, List<ScriptMachine> visualScriptingComponents)
        {
            if (visualScriptingComponents.Count == 0)
            {
                return;
            }
            this.exporter = exporter;
            
            foreach (var scriptMachine in visualScriptingComponents)
            {
                ActiveScriptMachine = scriptMachine;
                FlowGraph flowGraph = scriptMachine.graph;
                AddGraph(flowGraph);
            }
            
            GltfInteractivityNode[] nodesToSerialize = nodesToExport.SelectMany(exportNode => exportNode.Nodes).ToArray();

            for (int i = 0; i < nodesToSerialize.Length; i++)
                nodesToSerialize[i].Index = i;
            
            foreach (var graph in addedGraphs)
            {
                foreach (var exportNode in graph.nodes)
                    exportNode.Value.ResolveDefaultAndLiterals();
            }
            
            foreach (var graph in addedGraphs)
            {
                foreach (var exportNode in graph.nodes)
                    exportNode.Value.ResolveConnections();
            }
            
            RemoveUnconnectedNodes(ref nodesToSerialize);

            TriggerInterfaceExportCallbacks(ref nodesToSerialize);
            
            CheckForImplicitValueConversions(ref nodesToSerialize);
            
            CheckForCircularFlows(ref nodesToSerialize);
            
            PostIndexTopologicalSort(ref nodesToSerialize);  

            StringBuilder sb = new StringBuilder();
            foreach (var g in graphBypasses)
            {
                sb.AppendLine($"INPUT FROM: {g.Key.port.key} [{g.Key.port.unit.ToString()}] ({g.Key.port.graph.title}) TO: {g.Value.port.key} [{g.Value.port.unit.ToString()}] ({g.Value.port.graph.title})");
                
            }
            Debug.Log("GRAPH BYPASSES: "+sb.ToString());
            
            OnBeforeSerialization?.Invoke(nodesToSerialize);
            // Clear the events
            OnBeforeSerialization = null;
            // Create the extension and add nodes to it
            GltfInteractivityExtension extension = new GltfInteractivityExtension();
            extension.Nodes = nodesToSerialize;
            
            // Deactivated for now - not working in Authoring Tool, and also waiting for Spec Discussions about Types
            //extension.Types = CollectAndFilterUsedTypes(nodesToSerialize);
            
            ValidateData(nodesToSerialize);
            
            extension.Types = GltfInteractivityTypeMapping.TypesMapping;
            
            extension.Variables = variables.ToArray();
            extension.CustomEvents = customEvents.ToArray();
            
            gltfRoot.AddExtension(GltfInteractivityExtension.ExtensionName, extension);
            
            exporter.DeclareExtensionUsage(GltfInteractivityExtension.ExtensionName);
        }

        /// <summary>
        /// Called after the scene has been exported to add interactivity data.
        /// </summary>
        /// <param name="exporter"> GLTFSceneExporter object used to export the scene</param>
        /// <param name="gltfRoot"> Root GLTF object for the gltf object tree</param>
        public override void AfterSceneExport(GLTFSceneExporter exporter, GLTFRoot gltfRoot)
        {
            // TODO: Also handle getting non-embeded flow graphs?
            ActiveGltfRoot = gltfRoot;

            var scriptMachines = new List<ScriptMachine>();
            
            foreach (var root in exporter.RootTransforms)
            {
                if (!root) continue;
                var machines = root
                    .GetComponentsInChildren<ScriptMachine>()
                    .Where(x => x.isActiveAndEnabled && x.graph != null);
                scriptMachines.AddRange(machines);
            }
            AfterSceneExport(exporter, gltfRoot, scriptMachines);
        }

        private void PostIndexTopologicalSort(ref GltfInteractivityNode[] nodesToSerialize)
        {
            // Resort the nodes after resolving the connections
            var sorted = PostTopologicalSort(nodesToSerialize);
            var newIndices = new Dictionary<int, int>(); // Key = Old Index, Value = New Index
            
            int newIndex = 0;
            foreach (var oldIndex in sorted)
            {
                newIndices[oldIndex] = newIndex;
                newIndex++;
            }
            
            // Replace old indices with new 
            foreach (var kvp in newIndices)
            {
                var node = nodesToSerialize[kvp.Key];
                node.Index = kvp.Value;
                
                foreach (var valueSocket in node.ValueSocketConnectionData)
                {
                    if (valueSocket.Value.Node != null && valueSocket.Value.Node.HasValue)
                        valueSocket.Value.Node = newIndices[valueSocket.Value.Node.Value];
                }
                foreach (var flowSocket in node.FlowSocketConnectionData)
                {
                    if (flowSocket.Value.Node != null && flowSocket.Value.Node.HasValue)
                        flowSocket.Value.Node = newIndices[flowSocket.Value.Node.Value];
                }
            }
            
            // Resort nodesToSerialize
            Array.Sort(nodesToSerialize, (a, b) => a.Index.CompareTo(b.Index));
        }

        private GltfInteractivityTypeMapping.TypeMapping[] CollectAndFilterUsedTypes(GltfInteractivityNode[] nodes)
        {
            var types = new List<GltfInteractivityTypeMapping.TypeMapping>();
            
            // key = old index, value = new index
            var typesIndexReplacement = new Dictionary<int, int>();
            var usedTypeIndices = new HashSet<int>();

            // Collect used Types
            foreach (var variable in variables.Where(v => v.Type == -1 && v.Value != null))
            {
                var typeIndex = GltfInteractivityTypeMapping.TypeIndex(variable.Value.GetType());
                variable.Type = typeIndex;
                usedTypeIndices.Add(typeIndex);
            }
            
            foreach (var variable in variables.Where(v => v.Type != -1))
                usedTypeIndices.Add(variable.Type);

            foreach (var customEventValue in customEvents.SelectMany(c => c.Values))
                usedTypeIndices.Add(customEventValue.Type);
            
            foreach (var node in nodes)
            {
                foreach (var valueSocket in node.ValueSocketConnectionData)
                    if (valueSocket.Value.Value != null)
                    {
                        if (valueSocket.Value.Type == -1)
                            valueSocket.Value.Type = GltfInteractivityTypeMapping.TypeIndex(valueSocket.Value.GetType());
                        usedTypeIndices.Add(valueSocket.Value.Type);
                    }
                
                foreach (var outSocket in node.OutValueSocket)
                    if (outSocket.Value.expectedType != null)
                    {
                        if (outSocket.Value.expectedType.typeIndex != null)
                            usedTypeIndices.Add(outSocket.Value.expectedType.typeIndex.Value);
                    }
            }
            
            // Create used Type Mapping List and mark the new indices
            foreach (var typeIndex in usedTypeIndices.OrderBy( t => t))
            {
                types.Add(GltfInteractivityTypeMapping.TypesMapping[typeIndex]);
                typesIndexReplacement.Add(typeIndex, types.Count-1);
            }
            
            // Replace the old type indices with the new ones
            foreach (var node in nodes)
            {
                foreach (var valueSocket in node.ValueSocketConnectionData)
                    if (valueSocket.Value.Value != null && valueSocket.Value.Type != -1)
                        valueSocket.Value.Type = typesIndexReplacement[valueSocket.Value.Type];
            }
            
            foreach (var variable in variables.Where( v => v.Type != -1))
                variable.Type = typesIndexReplacement[variable.Type];
            
            foreach (var customEventValue in customEvents.SelectMany(c => c.Values))
                customEventValue.Type = typesIndexReplacement[customEventValue.Type];
            
            return types.ToArray();
        }
        
        private static LinkedList<IUnit> TopologicalSort(IEnumerable<IUnit> nodes)
        {
            var sorted = new LinkedList<IUnit>();
            var visited = new Dictionary<IUnit, bool>();

            foreach (var node in nodes)
            {
                Visit(node, sorted, visited);
            }

            return sorted;
        }

        private static LinkedList<int> PostTopologicalSort(GltfInteractivityNode[] nodes)
        {
            var sorted = new LinkedList<int>();
            var visited = new Dictionary<int, bool>();

            foreach (var node in nodes)
            {
                Visit(node.Index, nodes, sorted, visited);
            }

            return sorted;
        }

        private void CheckForCircularFlows(ref GltfInteractivityNode[] allNodes)
        {
            var nodes = new List<GltfInteractivityNode>(allNodes);
            var visited = new Dictionary<int, bool>(allNodes.Length);
            
            bool Visit(int node, GltfInteractivityNode[] allNodes)
            {
                bool alreadyVisited = false;
                
                if (visited.TryGetValue(node, out alreadyVisited))
                {
                    if (alreadyVisited)
                        return true;
                }
                
                if (!alreadyVisited)
                {
                    visited[node] = true;

                    // Get the dependencies from incoming connections and ignore self-references
                    var currentNode = allNodes[node];
                    foreach (var connection in currentNode.FlowSocketConnectionData)
                    {
                        if (connection.Value.Node != null && connection.Value.Node.HasValue && connection.Value.Node.Value < allNodes.Length)
                        {
                            if (Visit(connection.Value.Node.Value, allNodes))
                            {
                                // Add Events because of cyclic dependency
                                var eventId = AddEventWithIdIfNeeded($"CyclicDependency{connection.Value.Node.ToString()}from{node.ToString()}");
                       
                                var triggerEventNode = new GltfInteractivityNode(new Event_SendNode());
                                triggerEventNode.Index = nodes.Count;
                                triggerEventNode.ConfigurationData["event"].Value = eventId;
                                nodes.Add(triggerEventNode);
                                
                                var receiveEventNode = new GltfInteractivityNode(new Event_ReceiveNode());
                                receiveEventNode.Index = nodes.Count;
                                receiveEventNode.ConfigurationData["event"].Value = eventId;
                                nodes.Add(receiveEventNode);

                                var receiveFlowOut = receiveEventNode.FlowSocketConnectionData[Event_ReceiveNode.IdFlowOut];
                                receiveFlowOut.Node = connection.Value.Node;
                                receiveFlowOut.Socket = connection.Value.Socket;    

                                connection.Value.Node = triggerEventNode.Index;
                                connection.Value.Socket = Event_SendNode.IdFlowIn;
                            }
                        }
                    }

                    visited[node] = false;
                }

                return false;
            }
            
            foreach (var node in allNodes)
                Visit(node.Index, allNodes);

            allNodes = nodes.ToArray();
        }

        private static void RemoveUnconnectedNodes(ref GltfInteractivityNode[] allNodes)
        {
            var nodes = new List<GltfInteractivityNode>(allNodes);
            var visited = new HashSet<int>(nodes.Count);
            var nodesToRemove = new List<GltfInteractivityNode>();
           
            //Collect which nodes has connections
            foreach (var node in nodes)
            {
                if (node.ValueSocketConnectionData.Count > 0)
                {
                    foreach (var valueSocket in node.ValueSocketConnectionData)
                    {
                        if (valueSocket.Value.Node != null && valueSocket.Value.Node != -1)
                        {
                            visited.Add(valueSocket.Value.Node.Value);
                            visited.Add(node.Index);
                        }
                    }
                }

                if (node.FlowSocketConnectionData.Count > 0)
                {
                    foreach (var flowSocket in node.FlowSocketConnectionData)
                    {
                        if (flowSocket.Value.Node != null && flowSocket.Value.Node != -1)
                        {
                            visited.Add(flowSocket.Value.Node.Value);
                            visited.Add(node.Index);
                        }
                    }
                }
            }

            foreach (var node in nodes)
            {
                if (!visited.Contains(node.Index))
                    nodesToRemove.Add(node);
            }
            
            foreach (var node in nodesToRemove)
            {
                var indexToRemove = nodes.IndexOf(node);
                if (indexToRemove == nodes.Count - 1)
                {
                    // Just remove, no other indices are affected
                    nodes.RemoveAt(indexToRemove);
                    continue;
                }
                
                nodes.RemoveAt(indexToRemove);
                // Move last node to the removed node index
                nodes.Insert(indexToRemove, nodes.Last());
                nodes.RemoveAt(nodes.Count - 1);
              
                int replaceIndex = nodes[indexToRemove].Index;
                nodes[indexToRemove].Index = node.Index;
                // Replace old index with new index of the inserted node
                foreach (var n in nodes)
                {
                    foreach (var valueSocket in n.ValueSocketConnectionData)
                    {
                        if (valueSocket.Value.Node == replaceIndex)
                            valueSocket.Value.Node = node.Index;
                    }
                    foreach (var flowSocket in n.FlowSocketConnectionData)
                    {
                        if (flowSocket.Value.Node == replaceIndex)
                            flowSocket.Value.Node = node.Index;
                    }
                }
            }
            
            allNodes = nodes.ToArray();
        }
        
        private static void Visit(int node, GltfInteractivityNode[] allNodes, LinkedList<int> sorted, Dictionary<int, bool> visited)
        {
            bool inProcess;
            bool alreadyVisited = visited.TryGetValue(node, out inProcess);
            
            if (alreadyVisited)
            {
                if (inProcess)
                {
                    // TODO: Should quit the topological sort and cancel the export
                    // throw new ArgumentException("Cyclic dependency found.");
                }
            }
            else
            {
                visited[node] = true;

                // Get the dependencies from incoming connections and ignore self-references
                HashSet<int> dependencies = new HashSet<int>();
                var currentNode = allNodes[node];
                foreach (var connection in currentNode.ValueSocketConnectionData)
                {
                    if (connection.Value.Node != null && connection.Value.Node.HasValue)
                    {
                        dependencies.Add(connection.Value.Node.Value);
                    }
                }

                foreach (var dependency in dependencies)
                {
                    Visit(dependency, allNodes, sorted, visited);
                }

                visited[node] = false;
                sorted.AddLast(node);
            }
        }
        
        private static void Visit(IUnit node, LinkedList<IUnit> sorted, Dictionary<IUnit, bool> visited)
        {
            bool inProcess;
            bool alreadyVisited = visited.TryGetValue(node, out inProcess);

            if (alreadyVisited)
            {
                if (inProcess)
                {
                    // TODO: Should quit the topological sort and cancel the export
                   // throw new ArgumentException("Cyclic dependency found.");
                }
            }
            else
            {
                visited[node] = true;

                // Get the dependencies from incoming connections and ignore self-references
                HashSet<IUnit> dependencies = new HashSet<IUnit>();
                foreach (IUnitConnection connection in node.connections)
                {
                    if (connection.source.unit != node)
                    {
                        dependencies.Add(connection.source.unit);
                    }
                }

                foreach (IUnit dependency in dependencies)
                {
                    Visit(dependency, sorted, visited);
                }

                visited[node] = false;
                sorted.AddLast(node);
            }
        }
        
        private GltfInteractivityNode[] AddTypeConversion(GltfInteractivityNode targetNode, int conversionNodeIndex, string targetInputSocket, int fromType, int toType)
        {
            List<GltfInteractivityNode> newNodes = new List<GltfInteractivityNode>();
            
            var fromTypeSignature = GltfInteractivityTypeMapping.TypesMapping[fromType].GltfSignature;
            var toTypeSignature = GltfInteractivityTypeMapping.TypesMapping[toType].GltfSignature;

            var targetSocketData = targetNode.ValueSocketConnectionData[targetInputSocket];
            GltfInteractivityNode conversionNode = null;
            
            var fromTypeComponentCount = GltfInteractivityTypeMapping.GetComponentCount(fromTypeSignature);
            var toTypeComponentCount = GltfInteractivityTypeMapping.GetComponentCount(toTypeSignature);
            
            void SetupSimpleConversion(GltfInteractivityNodeSchema schema)
            {
                conversionNode= new GltfInteractivityNode(schema);
                conversionNode.Index = conversionNodeIndex;
                newNodes.Add(conversionNode);
                conversionNode.ValueSocketConnectionData["a"] = new GltfInteractivityUnitExporterNode.ValueSocketData()
                {
                    Id = "a",
                    Node = targetSocketData.Node, 
                    Socket = targetSocketData.Socket, 
                    Value = targetSocketData.Value, 
                    Type = targetSocketData.Type
                };
                
                targetSocketData.Node = conversionNodeIndex;
                targetSocketData.Socket = "value";
                targetSocketData.Value = null;
            }
            
            void SetupConversion(GltfInteractivityNodeSchema schema)
            {
                conversionNode= new GltfInteractivityNode(schema);
                conversionNode.Index = conversionNodeIndex;
                newNodes.Add(conversionNode);

                int indexForTargetNodeInput = conversionNodeIndex;
                
                if (fromTypeComponentCount == 1 && toTypeComponentCount > 1)
                {
                    foreach (var input in conversionNode.ValueSocketConnectionData)
                    {
                        input.Value.Node = targetSocketData.Node;
                        input.Value.Socket = targetSocketData.Socket;
                        input.Value.Value = targetSocketData.Value;
                        input.Value.Type = targetSocketData.Type;
                    }
                }
                else if (toTypeComponentCount > 1)
                {
                    GltfInteractivityNodeSchema extractSchema = null;
                    switch (fromTypeComponentCount)
                    {
                        case 2:
                            extractSchema = new Math_Extract2Node();
                            break;
                        case 3:
                            extractSchema = new Math_Extract3Node();
                            break;
                        case 4:
                            extractSchema = new Math_Extract4Node();
                            break;
                    }

                    if (extractSchema == null)
                    {
                       newNodes.Clear();
                    }
                    
                    var extractNode = new GltfInteractivityNode(extractSchema);
                    extractNode.Index = conversionNodeIndex + newNodes.Count;
                    newNodes.Add(extractNode);

                    indexForTargetNodeInput = extractNode.Index;
                    
                    var extractInput = extractNode.ValueSocketConnectionData["a"];
                    extractInput.Node = targetSocketData.Node;
                    extractInput.Socket = targetSocketData.Socket;
                    extractInput.Value = targetSocketData.Value;
                    extractInput.Type = targetSocketData.Type;

                    int inputExtractIndex = 0;
                    foreach (var inputSocket in conversionNode.ValueSocketConnectionData)
                    {
                        if (inputExtractIndex < extractNode.ValueSocketConnectionData.Count - 1)
                        {
                            inputSocket.Value.Node = extractNode.Index;
                            inputSocket.Value.Socket = inputExtractIndex.ToString();
                            inputExtractIndex++;
                        }
                        else
                        {
                            inputSocket.Value.Value = 0f;
                            inputSocket.Value.Type = GltfInteractivityTypeMapping.TypeIndexByGltfSignature("float");
                        }
                    }
                }
                
                targetSocketData.Node = indexForTargetNodeInput;
                targetSocketData.Socket = "value";
                targetSocketData.Value = null;
            }
            
            if (fromTypeSignature == toTypeSignature)
                return null;
            
            var conversionSchema = GltfInteractivityTypeMapping.GetTypeConversionSchema(fromTypeSignature, toTypeSignature);
            if (conversionSchema != null)
            {
                if (conversionSchema.InputValueSockets.Length == 1)
                    SetupSimpleConversion(conversionSchema);
                else
                    SetupConversion(conversionSchema);
            }

            return newNodes.ToArray();
        }

        private void CheckForImplicitValueConversions(ref GltfInteractivityNode[] nodes)
        {
            List<GltfInteractivityNode> newNodeList = new List<GltfInteractivityNode>();
            newNodeList.AddRange(nodes);
            foreach (var node in nodes)
            {
                foreach (var valueSocket in node.ValueSocketConnectionData)
                {
                    node.ValueSocketConnectionData.TryGetValue(valueSocket.Key, out var socketRestriction);
              
                    if (valueSocket.Value.Node == null && valueSocket.Value.Value == null)
                    {
                        // Try to handle nulls

                        if (socketRestriction.typeRestriction != null)
                        {
                            if (socketRestriction.typeRestriction.limitToType != null)
                            {
                                valueSocket.Value.Value = GltfInteractivityTypeMapping.GetNullByType(socketRestriction.typeRestriction.limitToType);
                                valueSocket.Value.Type = GltfInteractivityTypeMapping.TypeIndexByGltfSignature(socketRestriction.typeRestriction.limitToType);
                            }
                            else if (socketRestriction.typeRestriction.fromInputPort != null)
                            {
                                var fromInputPort = socketRestriction.typeRestriction.fromInputPort;
                                var fromInputPortType = GetValueTypeForInput(nodes, node, fromInputPort);
                                if (fromInputPortType != -1)
                                {
                                    valueSocket.Value.Value = GltfInteractivityTypeMapping.GetNullByType(fromInputPortType);
                                    valueSocket.Value.Type = fromInputPortType;
                                }
                            }
                        }
                    }
                    
                    if (socketRestriction != null && socketRestriction.typeRestriction != null)
                    {
                        var valueType = GetValueTypeForInput(nodes, node, valueSocket.Key);
                        if (valueType == -1)
                            continue;
                        if (socketRestriction.typeRestriction.limitToType != null)
                        {
                            var limitToType =
                                GltfInteractivityTypeMapping.TypeIndexByGltfSignature(socketRestriction.typeRestriction
                                    .limitToType);
                            if (limitToType != valueType)
                            {
                                var conversionNode = AddTypeConversion(node, newNodeList.Count, socketRestriction.Id,
                                    valueType, limitToType);
                                if (conversionNode != null)
                                    newNodeList.AddRange(conversionNode);
                            }
                        }
                        else if (socketRestriction.typeRestriction.fromInputPort != null)
                        {
                            var fromInputPort = socketRestriction.typeRestriction.fromInputPort;
                            var fromInputPortType = GetValueTypeForInput(nodes, node, fromInputPort);
                            if (fromInputPortType == -1)
                                continue;
                            if (fromInputPortType != valueType)
                            {
                                var preferType =
                                    GltfInteractivityTypeMapping.PreferType(valueType, fromInputPortType);
                                if (preferType == -1)
                                {
                                    continue;
                                }
                                var conversionNode = AddTypeConversion(node, newNodeList.Count, socketRestriction.Id,
                                    valueType, preferType);
                                if (conversionNode != null)
                                    newNodeList.AddRange(conversionNode);
                            }
                        }
                    }
                }
            }

            nodes = newNodeList.ToArray();
        }

        private void ValidateData(GltfInteractivityNode[] nodes)
        {
            var sb = new StringBuilder();
            
            void NodeAppendLine(GltfInteractivityNode node, string message)
            {
                sb.AppendLine($"Node Index {node.Index} with Schema={node.Schema.Type}: {message}");
            }
            
            foreach (var node in nodes)
            {
                foreach (var valueSocket in node.ValueSocketConnectionData)
                {
                    if (valueSocket.Value.Node == null)
                    {
                        if (valueSocket.Value.Value == null)
                            NodeAppendLine(node, $"Socket {valueSocket.Key} has no connection and no Value");
                        else if (valueSocket.Value.Type == -1)
                            NodeAppendLine(node, $"Socket {valueSocket.Key} has invalid Type (-1). Value-Type: {valueSocket.Value.Value.GetType().Name}");
                    }
                    else if (valueSocket.Value.Node == -1)
                    {
                        NodeAppendLine(node, $"Socket {valueSocket.Key} has invalid Node Index (-1)");
                    }
                }
                
                foreach (var flowSocket in node.FlowSocketConnectionData)
                {
                    if (flowSocket.Value.Node == -1)
                    {
                        NodeAppendLine(node, $"Flow Socket {flowSocket.Key} has invalid Node Index (-1)");
                    }
                }

                if (node.Schema.Type == Pointer_SetNode.TypeName || node.Schema.Type == Pointer_GetNode.TypeName)
                {
                    if (node.ValueSocketConnectionData.TryGetValue(GltfInteractivityNodeHelper.IdPointerNodeIndex, out var valueSocket))
                    {
                        if (valueSocket.Value != null && valueSocket.Node == null && (int)valueSocket.Value == -1)
                            NodeAppendLine(node, $"Node Pointer Node has invalid nodeIndex Value: -1");
                    }
                }
                
                if (node.Schema.Type == Variable_SetNode.TypeName || node.Schema.Type == Variable_GetNode.TypeName)
                {
                    if (node.ConfigurationData.TryGetValue(Variable_SetNode.IdConfigVarIndex, out var varConfig))
                    {
                        if (varConfig.Value == null)
                            NodeAppendLine(node, $"Variable Node has no Variable Index");
                        if (varConfig.Value != null && (int)varConfig.Value == -1)
                            NodeAppendLine(node, $"Variable Node has invalid Variable Index: -1");
                    }
                }

                if (node.Schema.Type == Event_ReceiveNode.TypeName || node.Schema.Type == Event_SendNode.TypeName)
                {
                    if (node.ConfigurationData.TryGetValue("event", out var varConfig))
                    {
                        if (varConfig.Value == null)
                            NodeAppendLine(node, $"Event Node has no Event Index");
                        if (varConfig.Value != null && (int)varConfig.Value == -1)
                            NodeAppendLine(node, $"Event Node has invalid Event Index: -1");
                    }
                }
            }
            
            foreach (var variable in variables)
            {
                if (variable.Type == -1)
                    sb.AppendLine($"Variable with Id >{variable.Id}< has invalid Type (-1)");
            }
            
            foreach (var customEvent in customEvents)
            {
                foreach (var customEventValue in customEvent.Values)
                {
                    if (customEventValue.Type == -1)
                        sb.AppendLine($"Custom Event with Id >{customEvent.Id}< with Value >{customEventValue.Id}< has invalid Value Type (-1)");
                }
            }
            
            if (sb.Length == 0)
                return;
            
            Debug.LogError($"Validation Errors Found: "+ System.Environment.NewLine + sb.ToString());
        }
    }
}
