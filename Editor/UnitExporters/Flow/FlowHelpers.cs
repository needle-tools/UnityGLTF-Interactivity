using System.Collections.Generic;
using Unity.VisualScripting;
using UnityGLTF.Interactivity.Schema;

namespace UnityGLTF.Interactivity.Export
{
    public static class FlowHelpers
    {
        public static bool RequiresCoroutines(ControlInput input, out ControlInput coroutineControlInput)
        {
            coroutineControlInput = null;
            var visited = new HashSet<IUnit>();
            var stack = new Stack<ControlInput>();
            stack.Push(input);

            while (stack.Count > 0)
            {
                var current = stack.Pop();
                if (visited.Contains(current.unit))
                    continue;

                visited.Add(current.unit);

                if (current.requiresCoroutine)
                {
                    coroutineControlInput = current;
                    return true;

                }

                foreach (var controlOutput in current.unit.controlOutputs)
                {
                    if (!controlOutput.hasValidConnection)
                        continue;

                    stack.Push(controlOutput.connection.destination);
                }
            }

            return false;
        }

        public static void CreateCustomForLoop(UnitExporter unitExporter, 
            out GltfInteractivityUnitExporterNode.ValueInputSocketData[] startIndex,
            out GltfInteractivityUnitExporterNode.ValueInputSocketData[] endIndex,
            out GltfInteractivityUnitExporterNode.ValueInputSocketData step,
            out GltfInteractivityUnitExporterNode.FlowInSocketData flowIn,
            out GltfInteractivityUnitExporterNode.ValueOutputSocketData currentIndex,
            out GltfInteractivityUnitExporterNode.FlowOutSocketData loopBodyOut,
            out GltfInteractivityUnitExporterNode.FlowOutSocketData completed)
        {
            startIndex = new GltfInteractivityUnitExporterNode.ValueInputSocketData[2];
            endIndex = new GltfInteractivityUnitExporterNode.ValueInputSocketData[3];
            
            var indexVar = unitExporter.exportContext.AddVariableWithIdIfNeeded("ForLoopIndex"+System.Guid.NewGuid().ToString(), 0, VariableKind.Scene, typeof(int));

            var whileNode = unitExporter.CreateNode(new Flow_WhileNode());
            var setStartIndexVar = VariablesHelpers.SetVariable(unitExporter, indexVar);
           
            flowIn = setStartIndexVar.FlowIn(Variable_SetNode.IdFlowIn);
            setStartIndexVar.FlowOut(Variable_SetNode.IdFlowOut)
                .ConnectToFlowDestination(whileNode.FlowIn(Flow_WhileNode.IdFlowIn));
            completed = whileNode.FlowOut(Flow_WhileNode.IdCompleted);
            
            startIndex[0] = setStartIndexVar.ValueIn(Variable_SetNode.IdInputValue);

            var ascendingCondition = unitExporter.CreateNode(new Math_LeNode());
            startIndex[1] = ascendingCondition.ValueIn("a");
            endIndex[0] = ascendingCondition.ValueIn("b");
            
            VariablesHelpers.GetVariable(unitExporter, indexVar, out var indexVarValue);
            currentIndex = indexVarValue;
            
            var addNode = unitExporter.CreateNode(new Math_AddNode());
            addNode.ValueIn("a").ConnectToSource(indexVarValue).SetType(TypeRestriction.LimitToInt);
            step = addNode.ValueIn("b").SetType(TypeRestriction.LimitToInt);
            addNode.FirstValueOut().ExpectedType(ExpectedType.Int);
            
            var setCurrentIndexVar = VariablesHelpers.SetVariable(unitExporter, indexVar);
            setCurrentIndexVar.ValueIn(Variable_SetNode.IdInputValue).ConnectToSource(addNode.FirstValueOut());
            
            var sequence = unitExporter.CreateNode(new Flow_SequenceNode());
            whileNode.FlowOut(Flow_WhileNode.IdLoopBody).ConnectToFlowDestination(sequence.FlowIn(Flow_SequenceNode.IdFlowIn));

            loopBodyOut = sequence.FlowOut("0");
            sequence.FlowOut("1").ConnectToFlowDestination(setCurrentIndexVar.FlowIn(Variable_SetNode.IdFlowIn));
            
            var ascendingIndexCondition = unitExporter.CreateNode(new Math_LtNode());
            ascendingIndexCondition.ValueIn("a").ConnectToSource(indexVarValue);
            endIndex[1] = ascendingIndexCondition.ValueIn("b");
            
            var descendingIndexCondition = unitExporter.CreateNode(new Math_GtNode());
            descendingIndexCondition.ValueIn("a").ConnectToSource(indexVarValue);
            endIndex[2] = descendingIndexCondition.ValueIn("b");
            
            var conditionSelect = unitExporter.CreateNode(new Math_SelectNode());
            conditionSelect.ValueIn("a").ConnectToSource(ascendingIndexCondition.FirstValueOut());
            conditionSelect.ValueIn("b").ConnectToSource(descendingIndexCondition.FirstValueOut());
            conditionSelect.ValueIn("condition").ConnectToSource(ascendingCondition.FirstValueOut());
            
            whileNode.ValueIn(Flow_WhileNode.IdCondition).ConnectToSource(conditionSelect.FirstValueOut());

        }
    }
}