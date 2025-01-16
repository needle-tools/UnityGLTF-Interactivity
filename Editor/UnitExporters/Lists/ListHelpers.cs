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

        public static void AddItem(UnitExporter unitExporter, GltfInteractivityExportContext.VariableBasedList list, ValueInput indexInput, ValueInput valueInput,
            ControlInput flowIn, ControlOutput flowOut)
        {
            var setIndex = VariablesHelpers.SetVariable(unitExporter, list.CurrentIndexVarId);
            var setValue = VariablesHelpers.SetVariable(unitExporter, list.ValueToSetVarId);

            setIndex.FlowIn(Variable_SetNode.IdFlowIn).MapToControlInput(flowIn);
            setIndex.FlowOut(Variable_SetNode.IdFlowOut)
                .ConnectToFlowDestination(setValue.FlowIn(Variable_SetNode.IdFlowIn));
            setValue.FlowOut(Variable_SetNode.IdFlowOut).MapToControlOutput(flowOut);

            setIndex.ValueIn(Variable_SetNode.IdInputValue).MapToInputPort(indexInput);
            setValue.ValueIn(Variable_SetNode.IdInputValue).MapToInputPort(valueInput);
        }

        // public static void GetItem(UnitExporter unitExporter, GltfInteractivityExportContext.VariableBasedList list, ValueInput indexInput, ValueOutput valueOutput,
        //     ControlInput flowIn, ControlOutput flowOut)
        // {
        //     var setIndex = VariablesHelpers.SetVariable(unitExporter, list.CurrentIndexVarId);
        //     
        //     setIndex.FlowIn(Variable_SetNode.IdFlowIn).MapToControlInput(flowIn);
        //     setIndex.FlowOut(Variable_SetNode.IdFlowOut).MapToControlOutput(flowOut);
        //     setIndex.ValueIn(Variable_SetNode.IdInputValue).MapToInputPort(indexInput);
        //
        //     list.getValueNodeSocket.MapToPort(valueOutput);
        // }
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
        
        
        public static void CreateListNodes(UnitExporter unitExporter, GltfInteractivityExportContext.VariableBasedList list)
        {
            
            VariablesHelpers.GetVariable(unitExporter, list.CountVarId, out var listCountValueOut);
            list.getCountNodeSocket = listCountValueOut;
            VariablesHelpers.GetVariable(unitExporter, list.CurrentIndexVarId, out var currentIndexValueOut);
            VariablesHelpers.GetVariable(unitExporter, list.ValueToSetVarId, out var valueToSetValueOut);

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
    }
}