using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace UnityGLTF.Interactivity.Export
{
    /// <summary>
    /// Don't use this interface directly, use IUnitExporter or IUnitExporterProvider instead.
    /// </summary>
    public interface IUnitTypeExporter
    {
        System.Type unitType { get; }
    }

    public interface IUnitExporterProvider : IUnitTypeExporter
    {
        public IUnitExporter GetExporter(IUnit unit);
    }

    public interface IUnitExporter : IUnitTypeExporter
    {
        void InitializeInteractivityNodes(UnitExporter unitExporter);
    }
    
    public static class UnitExporterRegistry
    {

        private static Dictionary<System.Type, IUnitTypeExporter> _exportRegistry =
            new Dictionary<System.Type, IUnitTypeExporter>();

        // TODO: find a better way to register export converters, a more static way aproach would be better. No need for instances her
        public static void RegisterExporter(IUnitTypeExporter nodeConvert)
        {
            if (_exportRegistry.ContainsKey(nodeConvert.unitType))
            {
                Debug.LogError("ExportNodeConvert already registered for unitType: " + nodeConvert.unitType.ToString());
                return;
            }

            _exportRegistry.Add(nodeConvert.unitType, nodeConvert);
        }

        public static bool HasUnitExporter(IUnit unit)
        {
            var directlyExported = _exportRegistry.ContainsKey(unit.GetType());
            if (unit is GetMember || unit is SetMember || unit is InvokeMember || unit is Expose || unit is CreateStruct)
                directlyExported = false;
            
            var invokeExported = unit is InvokeMember invokeMember &&
                                 InvokeUnitExport.HasInvokeConvert(invokeMember.member?.declaringType, invokeMember.member?.name);
            var setMemberExported = unit is SetMember setMember &&
                                    SetMemberUnitExport.HasMemberConvert(setMember.member?.declaringType, setMember.member?.name);
            var getMemberExported = unit is GetMember getMember &&
                                    GetMemberUnitExport.HasMemberConvert(getMember.member?.declaringType, getMember.member?.name);

            var createStructExported = unit is CreateStruct createStruct &&
                                    CreateStructsUnitExport.HasConvert(createStruct.type);

            var exposeExported = unit is Expose expose && ExposeUnitExport.HasConvert(expose.type);
            return createStructExported || directlyExported || invokeExported || setMemberExported || getMemberExported || exposeExported;
        }

        public static IUnitExporter GetUnitExporter(IUnit unit)
        {
            var unitType = unit.GetType();
            if (unitType == typeof(Literal))
            {
                // Only contains a value.
                return null;
            }

            if (!_exportRegistry.ContainsKey(unitType))
            {
                Debug.LogWarning("No ExportNodeConvert found for unitType: " + unitType.ToString());
                return null;
            }

            var converter = _exportRegistry[unitType];
            if (converter == null)
                return null;

            if (converter is IUnitExporterProvider dynamic)
                return dynamic.GetExporter(unit);

            return converter as IUnitExporter;
        }

        public static UnitExporter CreateUnitExporter(GltfInteractivityExportContext exportContext, IUnit unit)
        {
            var converter = GetUnitExporter(unit);
            if (converter == null)
            {
                return null;
            }

            return new UnitExporter(exportContext, converter, unit);
        }
    }
}