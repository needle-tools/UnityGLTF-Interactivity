using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityGLTF.Interactivity.Schema;

namespace UnityGLTF.Interactivity.Export
{
    public class Mathf_FloorToIntUnitExport : IUnitExporter
    {
        [InitializeOnLoadMethod]
        private static void Register()
        {
            InvokeUnitExport.RegisterInvokeExporter(typeof(Mathf), nameof(Mathf.FloorToInt),
                new Mathf_FloorToIntUnitExport());
        }

        public Type unitType { get => typeof(InvokeMember); }

        public void InitializeInteractivityNodes(UnitExporter unitExporter)
        {
            var unit = unitExporter.unit as InvokeMember;
            
            var floorNode = unitExporter.CreateNode(new GltfInt_Floor());
            
            var floatToIntNode = unitExporter.CreateNode(new Type_FloatToIntNode());
   
            unitExporter.MapInputPortToSocketName(unit.valueInputs[0], GltfInt_Floor.IdInputA, floorNode);
            unitExporter.MapInputPortToSocketName(GltfInt_Floor.IdValueResult, floorNode, Type_FloatToIntNode.IdInputA, floatToIntNode);

            unitExporter.MapValueOutportToSocketName(unit.result, Type_FloatToIntNode.IdValueResult, floatToIntNode);
            unitExporter.ByPassFlow(unit.enter, unit.exit);
         }
        
    }
}