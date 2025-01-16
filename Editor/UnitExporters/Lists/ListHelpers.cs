using System;
using System.CodeDom;
using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityGLTF.Interactivity;
using UnityGLTF.Interactivity.Export;
using UnityGLTF.Interactivity.Schema;

namespace Editor.UnitExporters.Lists
{
    public static class ListHelpers
    {

        public static void GetListCount(GltfInteractivityExportContext.VariableBasedList list, GltfInteractivityUnitExporterNode.ValueInputSocketData toInputSocket)
        {
            toInputSocket.ConnectToSource(list.getCountNodeSocket);
        }
        
        public static void GetListCount(GltfInteractivityExportContext.VariableBasedList list, ValueOutput mapToSocket)
        {
            list.getCountNodeSocket.MapToPort(mapToSocket);
        }

        public static GltfInteractivityUnitExporterNode.ValueOutputSocketData GetListCountSocket(GltfInteractivityExportContext.VariableBasedList list)
        {
            return list.getCountNodeSocket;
        }

        public static void ClearList(UnitExporter unitExporter, GltfInteractivityExportContext.VariableBasedList list, ControlInput flowIn, ControlOutput flowOut)
        {
            VariablesHelpers.SetVariableStaticValue(unitExporter, list.CountVarId, 0, flowIn, flowOut);
        }

        public static void AddItem(UnitExporter unitExporter, GltfInteractivityExportContext.VariableBasedList list,
            out GltfInteractivityUnitExporterNode.ValueInputSocketData valueInputSocket,
            ControlInput flowIn, ControlOutput flowOut)
        {
            var addCount = unitExporter.CreateNode(new Math_AddNode());
            addCount.ValueIn("a").ConnectToSource(GetListCountSocket(list));
            addCount.ValueIn("b").SetValue(1);
            addCount.FirstValueOut().ExpectedType(ExpectedType.Int);
            
            SetItem(unitExporter, list, out var indexInput, out valueInputSocket, flowIn, flowOut);
            indexInput.SetValue(addCount.FirstValueOut());
        }

        public static void AddItem(UnitExporter unitExporter, GltfInteractivityExportContext.VariableBasedList list, ValueInput valueInput,
            ControlInput flowIn, ControlOutput flowOut)
        {
            AddItem(unitExporter, list, out var valueInputSocket, flowIn, flowOut);
            valueInputSocket.MapToInputPort(valueInput);
        }

        public static void SetItem(UnitExporter unitExporter, GltfInteractivityExportContext.VariableBasedList list,
            out GltfInteractivityUnitExporterNode.ValueInputSocketData indexInput,
            out GltfInteractivityUnitExporterNode.ValueInputSocketData valueInput,
            ControlInput flowIn, ControlOutput flowOut)
        {
            if (list.setValueFlowIn == null)
                CreateSetItemListNodes(unitExporter, list);
            
            var sequence = unitExporter.CreateNode(new Flow_SequenceNode());
            var setIndex = VariablesHelpers.SetVariable(unitExporter, list.CurrentIndexVarId);
            var setValue = VariablesHelpers.SetVariable(unitExporter, list.ValueToSetVarId);

            sequence.FlowIn(Flow_SequenceNode.IdFlowIn).MapToControlInput(flowIn);
            sequence.FlowOut("0").ConnectToFlowDestination(setIndex.FlowIn(Variable_SetNode.IdFlowIn));
            sequence.FlowOut("1").MapToControlOutput(flowOut);
            
            setIndex.FlowOut(Variable_SetNode.IdFlowOut)
                .ConnectToFlowDestination(setValue.FlowIn(Variable_SetNode.IdFlowIn));
            
            setValue.FlowOut(Variable_SetNode.IdFlowOut).ConnectToFlowDestination(list.setValueFlowIn);

            indexInput = setIndex.ValueIn(Variable_SetNode.IdInputValue);
            valueInput = setValue.ValueIn(Variable_SetNode.IdInputValue);
        }

        
        public static void SetItem(UnitExporter unitExporter, GltfInteractivityExportContext.VariableBasedList list, ValueInput indexInput, ValueInput valueInput,
            ControlInput flowIn, ControlOutput flowOut)
        {
            SetItem(unitExporter, list, out var indexInputSocket, out var valueInputSocket, flowIn, flowOut);
            indexInputSocket.MapToInputPort(indexInput);
            valueInputSocket.MapToInputPort(valueInput);
        }
        
        public static void GetItem(UnitExporter unitExporter, GltfInteractivityExportContext.VariableBasedList list,
            GltfInteractivityUnitExporterNode.ValueOutputSocketData indexInput, out GltfInteractivityUnitExporterNode.ValueOutputSocketData valueOutput)
        {
            GetItem(unitExporter, list, out var indexInputSocket, out valueOutput);
            foreach (var indexSockets in indexInputSocket)
                indexSockets.ConnectToSource(indexInput);
        }
        
        public static void GetItem(UnitExporter unitExporter, GltfInteractivityExportContext.VariableBasedList list,
            ValueInput indexInput, out GltfInteractivityUnitExporterNode.ValueOutputSocketData valueOutput)
        {
            GetItem(unitExporter, list, out var indexInputSocket, out valueOutput);
            foreach (var indexSockets in indexInputSocket)
                indexSockets.MapToInputPort(indexInput);
        }

        public static void GetItem(UnitExporter unitExporter, GltfInteractivityExportContext.VariableBasedList list, out GltfInteractivityUnitExporterNode.ValueInputSocketData[] indexInput, out GltfInteractivityUnitExporterNode.ValueOutputSocketData valueOutput)
        {
            indexInput = null;
            // Get Values
            GltfInteractivityUnitExporterNode prevSelectNode = null;
            indexInput = new GltfInteractivityUnitExporterNode.ValueInputSocketData[list.Capacity];
            int index = 0;
            int[] indices = new int[list.Capacity];
            for (int i = list.StartIndex; i <= list.EndIndex; i++)
            {
                indices[index] = index;
                VariablesHelpers.GetVariable(unitExporter, i, out var valueOut);
                var eq = unitExporter.CreateNode(new Math_EqNode());
                indexInput[index] = eq.ValueIn("a");
                eq.ValueIn("b").SetValue(index);
                eq.FirstValueOut().ExpectedType(ExpectedType.Int);

                var sel = unitExporter.CreateNode(new Math_SelectNode());
                sel.ValueIn(Math_SelectNode.IdCondition).ConnectToSource(eq.FirstValueOut());
                sel.ValueIn(Math_SelectNode.IdValueA).ConnectToSource(valueOut);
                if (prevSelectNode != null)
                    sel.ValueIn(Math_SelectNode.IdValueB).ConnectToSource(prevSelectNode.FirstValueOut());
                else
                {
                    // ?? This would be an outofrange error, for now we use the same value
                    sel.ValueIn(Math_SelectNode.IdValueB).ConnectToSource(valueOut);
                }

                prevSelectNode = sel;
                index++;
            }

            valueOutput = prevSelectNode.FirstValueOut();
        }

        public static GltfInteractivityExportContext.VariableBasedList FindListByConnections(GltfInteractivityExportContext context, IUnit unit)
        {
            var l = context.GetListByCreator(unit);
            if (l != null)
                return l;

            foreach (var input in unit.valueInputs)
            {
                
                if ((input.type.IsInterface && input.type == typeof(IEnumerable)) || input.type.GetInterfaces().Contains(typeof(IEnumerable)))
                {
                    foreach (var c in input.validConnections)
                    {
                        var varList = FindListByConnections(context, c.source.unit);
                        if (varList != null)
                            return varList;
                    }
                }
            }

            return null;
        }

        private static void CreateSetItemListNodes(UnitExporter unitExporter,
            GltfInteractivityExportContext.VariableBasedList list)
        {
            VariablesHelpers.GetVariable(unitExporter, list.CurrentIndexVarId, out var currentIndexValueOut);
            VariablesHelpers.GetVariable(unitExporter, list.ValueToSetVarId, out var valueToSetValueOut);
            // Set Values
            int[] indices = new int[list.Capacity];
            for (int i = 0; i < list.Capacity; i++)
                indices[i] = i;
            
            var flowSwitch = unitExporter.CreateNode(new Flow_SwitchNode());
            list.setValueFlowIn = flowSwitch.FlowIn(Flow_SwitchNode.IdFlowIn);
            flowSwitch.ValueIn(Flow_SwitchNode.IdSelection).ConnectToSource(currentIndexValueOut);
            flowSwitch.ConfigurationData["cases"] = new GltfInteractivityNode.ConfigData
                { Id = "cases", Value = indices };

            for (int i = 0; i < indices.Length; i++)
            {
                flowSwitch.FlowSocketConnectionData.Add(i.ToString(), new GltfInteractivityNode.FlowSocketData {Id = i.ToString()});
                
                VariablesHelpers.SetVariable(unitExporter, list.StartIndex + i, valueToSetValueOut, flowSwitch.FlowOut(i.ToString()), null);
            }
        }
        
        public static void CreateListNodes(UnitExporter unitExporter, GltfInteractivityExportContext.VariableBasedList list)
        {
            VariablesHelpers.GetVariable(unitExporter, list.CountVarId, out var listCountValueOut);
            list.getCountNodeSocket = listCountValueOut;

            // Get Values
            // GltfInteractivityUnitExporterNode prevSelectNode = null;
            // int index = 0;
            // int[] indices = new int[list.Capacity];
            // for (int i = list.StartIndex; i <= list.EndIndex; i++)
            // {
            //     indices[index] = index;
            //     VariablesHelpers.GetVariable(unitExporter, i, out var valueOut);
            //     var eq = unitExporter.CreateNode(new Math_EqNode());
            //     eq.ValueIn("a").ConnectToSource(currentIndexValueOut).SetType(TypeRestriction.LimitToInt);
            //     eq.ValueIn("b").SetValue(index);
            //     eq.FirstValueOut().ExpectedType(ExpectedType.Int);
            //
            //     var sel = unitExporter.CreateNode(new Math_SelectNode());
            //     sel.ValueIn(Math_SelectNode.IdCondition).ConnectToSource(eq.FirstValueOut());
            //     sel.ValueIn(Math_SelectNode.IdValueA).ConnectToSource(valueOut);
            //     if (prevSelectNode != null)
            //         sel.ValueIn(Math_SelectNode.IdValueB).ConnectToSource(prevSelectNode.FirstValueOut());
            //     else
            //     {
            //         // ?? This would be an outofrange error, for now we use the same value
            //         sel.ValueIn(Math_SelectNode.IdValueB).ConnectToSource(valueOut);
            //     }
            //
            //     prevSelectNode = sel;
            //     index++;
            // }
            //
            // list.getValueNodeSocket = prevSelectNode.FirstValueOut();
            

        }
    }
}