using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;

namespace UnityGLTF.Interactivity.Export
{
    public class SetMemberUnitExport : IUnitExporterProvider
    {
        public Type unitType { get => typeof(SetMember); }
        
        private static Dictionary<string, IUnitExporter> _memberExportRegister = new Dictionary<string, IUnitExporter>();
        
        [InitializeOnLoadMethod]
        private static void Register()
        {
            UnitExporterRegistry.RegisterExporter(new SetMemberUnitExport());
        }
        
        public static void RegisterMemberExporter(Type declaringType, string memberName, IUnitExporter unitExporter)
        {
            // HACK should match behaviour of InvokeNode.RegisterInvokeConvert
            _memberExportRegister[$"{declaringType.FullName}.{memberName}"] = unitExporter;
        }
        
        public static bool HasMemberConvert(Type declaringType, string memberName)
        {
            if (string.IsNullOrEmpty(memberName)) return false;
            return _memberExportRegister.ContainsKey($"{declaringType.FullName}.{memberName}");
        }

        public IUnitExporter GetExporter(IUnit unit)
        {
            var setMember = unit as SetMember;
            var key = $"{setMember.member.declaringType.FullName}.{setMember.member.name}";
            if (_memberExportRegister.TryGetValue(key, out var exportNodeConvert))
            {
                return exportNodeConvert;
            }
            
            return null;
        }
    }
}