using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityGLTF.Interactivity.Schema;

namespace UnityGLTF.Interactivity.Export
{
    public class GetMemberUnitExport : IUnitExporterProvider
    {
        public Type unitType { get => typeof(GetMember); }
        
        private static Dictionary<string, IUnitExporter> _memberExportRegister = new Dictionary<string, IUnitExporter>();
        
        [InitializeOnLoadMethod]
        private static void Register()
        {
            UnitExporterRegistry.RegisterExporter(new GetMemberUnitExport());
        }
        
        public static void RegisterMemberExporter(Type declaringType, string memberName, IUnitExporter unitExporter)
        {
            // HACK should match behaviour of InvokeNode.RegisterInvokeConvert
            _memberExportRegister[$"{declaringType.FullName}.{memberName}"] = unitExporter;
        }
        
        public static bool HasMemberConvert(Type declaringType, string memberName)
        {
            if (string.IsNullOrEmpty(memberName)) return false;
            if (_memberExportRegister.ContainsKey($"{declaringType.FullName}.{memberName}"))
                return true;
            
            return GetMemberGenericStaticValueExporter.CanBeExported(declaringType, memberName);
        }

        public IUnitExporter GetExporter(IUnit unit)
        {
            var getMember = unit as GetMember;
            var key = $"{getMember.member.declaringType.FullName}.{getMember.member.name}";
            if (_memberExportRegister.TryGetValue(key, out var exportNodeConvert))
            {
                return exportNodeConvert;
            }

            if (getMember.member.info.IsStatic())
            {
                // Fallback to export static values as Variables, e.g.: Mathf.Rad2Deg
                return new GetMemberGenericStaticValueExporter();
            }
            
            return null;
        }
    }
    
    public class GetMemberGenericStaticValueExporter: IUnitExporter
    {
        public Type unitType { get; }

        public static bool CanBeExported(Type declaringType, string memberName)
        {
            var field = declaringType.GetField(memberName);
            if (field != null)
                return field.GetValue(null) != null;
            else
            {
                var property = declaringType.GetProperty(memberName);
                if (property == null)
                    return false;

                if (!property.IsStatic())
                    return false;
                
                return property.GetValue(null) != null;
            }
        }
        
        public void InitializeInteractivityNodes(UnitExporter unitExporter)
        {
            var unit = unitExporter.unit as GetMember;

            var varName = unit.member.name + "_" + unit.member.declaringType.Name;
            
            object value = null;
            object rawValue = null;
            var field = unit.member.declaringType.GetField(unit.member.name);
            
            if (field != null && field.IsStatic())
                rawValue = field.GetValue(null);
            else
            {
                var property = unit.member.declaringType.GetProperty(unit.member.name);
                if (property == null)
                    return;
                
                if (!property.IsStatic())
                    return;
                
                rawValue = property.GetValue(null);
            }

            if (rawValue == null)
            {
                unitExporter.IsTranslatable = false;
                return;
            }
            
            if (rawValue is Double d)
                value = (float)d;
            else
            if (rawValue is long l)
                value = (int)l;
            else
            if (rawValue is byte b)
                value = (int)b;
            else
                value = rawValue;
            
            var gltfTypeIndex = GltfTypes.TypeIndex(value.GetType());
            if (gltfTypeIndex == -1)
            {
                Debug.LogError("Unsupported type to get static value: " + value.GetType()+ " from " + unit.member.declaringType);
                UnitExportLogging.AddErrorLog(unit, "Unsupported type: "+value.GetType().ToString());
                unitExporter.IsTranslatable = false;
                // Unsupported type
                return;
            }
            var node = unitExporter.CreateNode(new Variable_GetNode());

            var variableIndex = unitExporter.exportContext.AddVariableWithIdIfNeeded(varName, value, VariableKind.Scene, gltfTypeIndex);
            node.OutValueSocket[Variable_GetNode.IdOutputValue].expectedType = ExpectedType.GtlfType(gltfTypeIndex);
            
            node.ConfigurationData["variable"].Value = variableIndex;
            
            unitExporter.MapValueOutportToSocketName(unit.value, Variable_GetNode.IdOutputValue, node); 
            
        }
    }
}