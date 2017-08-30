using EstimatingLibrary;
using EstimatingLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingUtilitiesLibrary.Database
{
    #region Table Definitions
    #region Object Tables
    internal class MetadataTable : TableBase
    {
        public static new string TableName = "Metadata";
        public static Type HelperType = typeof(HelperProperties);

        public static TableField Version = new TableField("Version", "TEXT", HelperType.GetProperty("DBVersion"));

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            Version
        };
    }

    internal class BidInfoTable : TableBase
    {
        public static new string TableName = "BidInfo";
        public static FlavoredType ObjectType = new FlavoredType(typeof(TECBid), 0);

        public static Type HelperType = typeof(HelperProperties);

        public static TableField Name = new TableField("Name", "TEXT", ObjectType.Type.GetProperty("Name"));
        public static TableField ID = new TableField("ID", "TEXT", ObjectType.Type.GetProperty("Guid"));
        public static TableField Number = new TableField("Number", "TEXT", ObjectType.Type.GetProperty("BidNumber"));
        public static TableField DueDate = new TableField("DueDate", "TEXT", ObjectType.Type.GetProperty("DueDateString"));
        public static TableField Salesperson = new TableField("Salesperson", "TEXT", ObjectType.Type.GetProperty("Salesperson"));
        public static TableField Estimator = new TableField("Estimator", "TEXT", ObjectType.Type.GetProperty("Estimator"));

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            ID
        };

        public static new List<FlavoredType> Types = new List<FlavoredType>() {
            ObjectType
        };

    }
    internal class TemplatesInfoTable : TableBase
    {
        public static new string TableName = "TemplatesInfo";
        public static FlavoredType ObjectType = new FlavoredType(typeof(TECTemplates), 0);

        public static Type HelperType = typeof(HelperProperties);

        public static TableField ID = new TableField("ID", "TEXT", ObjectType.Type.GetProperty("Guid"));
        public static TableField DBVersion = new TableField("DBVersion", "TEXT", HelperType.GetProperty("DBVersion"));

        public static new List<TableField> PrimaryKey = new List<TableField>()
        {
            ID
        };

        public static new List<FlavoredType> Types = new List<FlavoredType>()
        {
            ObjectType
        };
    }
    internal class ParametersTable : TableBase
    {
        public static new string TableName = "Parameters";
        public static FlavoredType ParameterType = new FlavoredType(typeof(TECParameters), 0);

        public static TableField ID = new TableField("ID", "TEXT", ParameterType.Type.GetProperty("Guid"));

        public static TableField Escalation = new TableField("Escalation", "REAL", ParameterType.Type.GetProperty("Escalation"));
        public static TableField Overhead = new TableField("Overhead", "REAL", ParameterType.Type.GetProperty("Overhead"));
        public static TableField Profit = new TableField("Profit", "REAL", ParameterType.Type.GetProperty("Profit"));
        public static TableField SubcontractorMarkup = new TableField("SubcontractorMarkup", "REAL", ParameterType.Type.GetProperty("SubcontractorMarkup"));
        public static TableField SubcontractorEscalation = new TableField("SubcontractorEscalation", "REAL", ParameterType.Type.GetProperty("SubcontractorEscalation"));

        public static TableField IsTaxExempt = new TableField("IsTaxExempt", "INTEGER", ParameterType.Type.GetProperty("IsTaxExempt"));
        public static TableField RequiresBond = new TableField("RequiresBond", "INTEGER", ParameterType.Type.GetProperty("RequiresBond"));
        public static TableField RequiresWrapUp = new TableField("RequiresWrapUp", "INTEGER", ParameterType.Type.GetProperty("RequiresWrapUp"));

        #region Labor
        public static TableField PMCoef = new TableField("PMCoef", "REAL", ParameterType.Type.GetProperty("PMCoef"));
        public static TableField PMRate = new TableField("PMRate", "REAL", ParameterType.Type.GetProperty("PMRate"));

        public static TableField ENGCoef = new TableField("ENGCoef", "REAL", ParameterType.Type.GetProperty("ENGCoef"));
        public static TableField ENGRate = new TableField("ENGRate", "REAL", ParameterType.Type.GetProperty("ENGRate"));

        public static TableField CommCoef = new TableField("CommCoef", "REAL", ParameterType.Type.GetProperty("CommCoef"));
        public static TableField CommRate = new TableField("CommRate", "REAL", ParameterType.Type.GetProperty("CommRate"));

        public static TableField SoftCoef = new TableField("SoftCoef", "REAL", ParameterType.Type.GetProperty("SoftCoef"));
        public static TableField SoftRate = new TableField("SoftRate", "REAL", ParameterType.Type.GetProperty("SoftRate"));

        public static TableField GraphCoef = new TableField("GraphCoef", "REAL", ParameterType.Type.GetProperty("GraphCoef"));
        public static TableField GraphRate = new TableField("GraphRate", "REAL", ParameterType.Type.GetProperty("GraphRate"));
        #endregion

        #region SubContractor
        public static TableField ElectricalRate = new TableField("ElectricalRate", "REAL", ParameterType.Type.GetProperty("ElectricalRate"));
        public static TableField ElectricalSuperRate = new TableField("ElectricalSuperRate", "REAL", ParameterType.Type.GetProperty("ElectricalSuperRate"));
        public static TableField ElectricalNonUnionRate = new TableField("ElectricalNonUnionRate", "REAL", ParameterType.Type.GetProperty("ElectricalNonUnionRate"));
        public static TableField ElectricalSuperNonUnionRate = new TableField("ElectricalSuperNonUnionRate", "REAL", ParameterType.Type.GetProperty("ElectricalSuperNonUnionRate"));
        public static TableField ElectricalSuperRatio = new TableField("ElectricalSuperRatio", "REAL", ParameterType.Type.GetProperty("ElectricalSuperRatio"));

        public static TableField ElectricalIsOnOvertime = new TableField("ElectricalIsOnOvertime", "INTEGER", ParameterType.Type.GetProperty("ElectricalIsOnOvertime"));
        public static TableField ElectricalIsUnion = new TableField("ElectricalIsUnion", "INTEGER", ParameterType.Type.GetProperty("ElectricalIsUnion"));
        #endregion

        public static new List<TableField> PrimaryKey = new List<TableField>()
        {
            ID
        };

        public static new List<FlavoredType> Types = new List<FlavoredType>()
        {
            ParameterType
        };
    }
    internal class ExtraLaborTable : TableBase
    {
        public static new string TableName = "ExtraLabor";
        public static FlavoredType ObjectType = new FlavoredType( typeof(TECExtraLabor), 0);

        public static TableField ID = new TableField("BidID", "TEXT", ObjectType.Type.GetProperty("Guid"));

        public static TableField PMExtraHours = new TableField("PMExtraHours", "REAL", ObjectType.Type.GetProperty("PMExtraHours"));
        public static TableField ENGExtraHours = new TableField("ENGExtraHours", "REAL", ObjectType.Type.GetProperty("ENGExtraHours"));
        public static TableField CommExtraHours = new TableField("CommExtraHours", "REAL", ObjectType.Type.GetProperty("CommExtraHours"));
        public static TableField SoftExtraHours = new TableField("SoftExtraHours", "REAL", ObjectType.Type.GetProperty("SoftExtraHours"));
        public static TableField GraphExtraHours = new TableField("GraphExtraHours", "REAL", ObjectType.Type.GetProperty("GraphExtraHours"));

        public static new List<TableField> PrimaryKey = new List<TableField>()
        {
            ID
        };

        public static new List<FlavoredType> Types = new List<FlavoredType>()
        {
            ObjectType
        };

    }
    internal class NoteTable : TableBase
    {
        public static new string TableName = "Note";
        public static FlavoredType ObjectType = new FlavoredType(typeof(TECLabeled), Flavor.Note);


        public static TableField ID = new TableField("ID", "TEXT", ObjectType.Type.GetProperty("Guid"));
        public static TableField NoteText = new TableField("Label", "TEXT", ObjectType.Type.GetProperty("Label"));

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            ID
            };

        public static new List<FlavoredType> Types = new List<FlavoredType>()
        {
            ObjectType
        };
    }
    internal class ExclusionTable : TableBase
    {
        public static new string TableName = "Exclusion";
        public static FlavoredType ObjectType = new FlavoredType(typeof(TECLabeled), Flavor.Exclusion);
        public static Flavor ObjectFlavor = Flavor.Exclusion;

        public static TableField ID = new TableField("ID", "TEXT", ObjectType.Type.GetProperty("Guid"));
        public static TableField ExclusionText = new TableField("Label", "TEXT", ObjectType.Type.GetProperty("Label"));

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            ID
            };

        public static new List<FlavoredType> Types = new List<FlavoredType>()
        {
            ObjectType
        };

    }
    internal class ScopeBranchTable : TableBase
    {
        public static new string TableName = "ScopeBranch";
        public static FlavoredType ObjectType = new FlavoredType(typeof(TECScopeBranch), 0);

        public static TableField ID = new TableField("ID", "TEXT", ObjectType.Type.GetProperty("Guid"));
        public static TableField Label = new TableField("Label", "TEXT", ObjectType.Type.GetProperty("Label"));

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            ID
            };
        public static new List<FlavoredType> Types = new List<FlavoredType>()
        {
            ObjectType
        };

    }
    internal class SystemTable : TableBase
    {
        public static new string TableName = "System";
        public static FlavoredType ObjectType = new FlavoredType(typeof(TECSystem), 0);

        public static TableField ID = new TableField("ID", "TEXT", ObjectType.Type.GetProperty("Guid"));
        public static TableField Name = new TableField("Name", "TEXT", ObjectType.Type.GetProperty("Name"));
        public static TableField Description = new TableField("Description", "TEXT", ObjectType.Type.GetProperty("Description"));
        public static TableField ProposeEquipment = new TableField("ProposeEquipment", "INTEGER", ObjectType.Type.GetProperty("ProposeEquipment"));

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            ID
            };
        public static new List<FlavoredType> Types = new List<FlavoredType>()
        {
            ObjectType
        };

    }
    internal class EquipmentTable : TableBase
    {
        public static new string TableName = "Equipment";
        public static FlavoredType ObjectType = new FlavoredType(typeof(TECEquipment), 0);

        public static TableField ID = new TableField("ID", "TEXT", ObjectType.Type.GetProperty("Guid"));
        public static TableField Name = new TableField("Name", "TEXT", ObjectType.Type.GetProperty("Name"));
        public static TableField Description = new TableField("Description", "TEXT", ObjectType.Type.GetProperty("Description"));

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            ID
            };
        public static new List<FlavoredType> Types = new List<FlavoredType>()
        {
            ObjectType
        };

    }
    internal class SubScopeTable : TableBase
    {
        public static new string TableName = "SubScope";
        public static FlavoredType ObjectType = new FlavoredType(typeof(TECSubScope), 0);

        public static TableField ID = new TableField("ID", "TEXT", ObjectType.Type.GetProperty("Guid"));
        public static TableField Name = new TableField("Name", "TEXT", ObjectType.Type.GetProperty("Name"));
        public static TableField Description = new TableField("Description", "TEXT", ObjectType.Type.GetProperty("Description"));

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            ID
            };
        public static new List<FlavoredType> Types = new List<FlavoredType>()
        {
            ObjectType
        };

    }
    internal class DeviceTable : CatalogTableBase
    {
        public static new string TableName = "Device";
        public static FlavoredType ObjectType = new FlavoredType(typeof(TECDevice), 0);

        public static TableField ID = new TableField("ID", "TEXT", ObjectType.Type.GetProperty("Guid"));
        public static TableField Name = new TableField("Name", "TEXT", ObjectType.Type.GetProperty("Name"));
        public static TableField Description = new TableField("Description", "TEXT", ObjectType.Type.GetProperty("Description"));
        public static TableField Cost = new TableField("Cost", "REAL", ObjectType.Type.GetProperty("Cost"));

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            ID
            };
        public static new List<FlavoredType> Types = new List<FlavoredType>()
        {
            ObjectType
        };

    }
    internal class PointTable : TableBase
    {
        public static new string TableName = "Point";
        public static FlavoredType ObjectType = new FlavoredType(typeof(TECPoint), 0);

        public static TableField ID = new TableField("ID", "TEXT", ObjectType.Type.GetProperty("Guid"));
        public static TableField Name = new TableField("Name", "TEXT", ObjectType.Type.GetProperty("Label"));
        public static TableField Quantity = new TableField("Quantity", "INTEGER", ObjectType.Type.GetProperty("Quantity"));
        public static TableField Type = new TableField("Type", "TEXT", ObjectType.Type.GetProperty("Type"));

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            ID
            };
        public static new List<FlavoredType> Types = new List<FlavoredType>()
        {
            ObjectType
        };

    }
    internal class TagTable : CatalogTableBase
    {
        public static new string TableName = "Tag";
        public static FlavoredType ObjectType = new FlavoredType(typeof(TECLabeled), Flavor.Tag);
        public static Flavor ObjectFlavor = Flavor.Tag;

        public static TableField ID = new TableField("ID", "TEXT", ObjectType.Type.GetProperty("Guid"));
        public static TableField TagString = new TableField("Label", "TEXT", ObjectType.Type.GetProperty("Label"));

        public static new List<TableField> PrimaryKey = new List<TableField>()
        { ID };
        public static new List<FlavoredType> Types = new List<FlavoredType>()
        {
            ObjectType
        };

    }
    internal class ManufacturerTable : CatalogTableBase
    {
        public static new string TableName = "Manufacturer";
        public static FlavoredType ObjectType = new FlavoredType(typeof(TECManufacturer), 0);

        public static TableField ID = new TableField("ID", "TEXT", ObjectType.Type.GetProperty("Guid"));
        public static TableField Name = new TableField("Name", "TEXT", ObjectType.Type.GetProperty("Label"));
        public static TableField Multiplier = new TableField("Multiplier", "REAL", ObjectType.Type.GetProperty("Multiplier"));

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            ID
            };
        public static new List<FlavoredType> Types = new List<FlavoredType>()
        {
            ObjectType
        };

    }
    internal class LocationTable : CatalogTableBase
    {
        public static new string TableName = "Location";
        public static FlavoredType ObjectType = new FlavoredType(typeof(TECLabeled), Flavor.Location);
        public static Flavor ObjectFlavor = Flavor.Location;

        public static TableField ID = new TableField("ID", "TEXT", ObjectType.Type.GetProperty("Guid"));
        public static TableField Name = new TableField("Label", "TEXT", ObjectType.Type.GetProperty("Label"));

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            ID
            };
        public static new List<FlavoredType> Types = new List<FlavoredType>()
        {
            ObjectType
        };

    }
    internal class ConnectionTypeTable : CatalogTableBase
    {
        public static new string TableName = "Connectiontype";
        public static FlavoredType ObjectType = new FlavoredType(typeof(TECElectricalMaterial), Flavor.Wire);
        public static Flavor ObjectFlavor = Flavor.Wire;

        public static TableField ID = new TableField("ID", "TEXT", ObjectType.Type.GetProperty("Guid"));
        public static TableField Name = new TableField("Name", "TEXT", ObjectType.Type.GetProperty("Name"));
        public static TableField Cost = new TableField("Cost", "REAL", ObjectType.Type.GetProperty("Cost"));
        public static TableField Labor = new TableField("Labor", "REAL", ObjectType.Type.GetProperty("Labor"));

        public static new List<TableField> PrimaryKey = new List<TableField>()
        {
            ID
        };
        public static new List<FlavoredType> Types = new List<FlavoredType>()
        {
            ObjectType
        };
    }
    internal class ConduitTypeTable : CatalogTableBase
    {
        public static new string TableName = "ConduitType";
        public static FlavoredType ObjectType = new FlavoredType(typeof(TECElectricalMaterial), Flavor.Conduit);
        public static Flavor ObejctFlavor = Flavor.Conduit;

        public static TableField ID = new TableField("ID", "TEXT", ObjectType.Type.GetProperty("Guid"));
        public static TableField Name = new TableField("Name", "TEXT", ObjectType.Type.GetProperty("Name"));
        public static TableField Cost = new TableField("Cost", "REAL", ObjectType.Type.GetProperty("Cost"));
        public static TableField Labor = new TableField("Labor", "REAL", ObjectType.Type.GetProperty("Labor"));

        public static new List<TableField> PrimaryKey = new List<TableField>()
        {
            ID
        };
        public static new List<FlavoredType> Types = new List<FlavoredType>()
        {
            ObjectType
        };
    }
    internal class AssociatedCostTable : CatalogTableBase
    {
        public static new string TableName = "AssociatedCost";
        public static FlavoredType ObjectType = new FlavoredType(typeof(TECCost), 0);

        public static TableField ID = new TableField("ID", "TEXT", ObjectType.Type.GetProperty("Guid"));
        public static TableField Name = new TableField("Name", "TEXT", ObjectType.Type.GetProperty("Name"));
        public static TableField Cost = new TableField("Cost", "REAL", ObjectType.Type.GetProperty("Cost"));
        public static TableField Labor = new TableField("Labor", "REAL", ObjectType.Type.GetProperty("Labor"));
        public static TableField Type = new TableField("Type", "TEXT", ObjectType.Type.GetProperty("Type"));

        public static new List<TableField> PrimaryKey = new List<TableField>()
        {
            ID
        };
        public static new List<FlavoredType> Types = new List<FlavoredType>()
        {
            ObjectType
        };
    }
    internal class SubScopeConnectionTable : TableBase
    {
        public static new string TableName = "SubScopeConnection";
        public static FlavoredType ObjectType = new FlavoredType(typeof(TECSubScopeConnection), 0);

        public static TableField ID = new TableField("ID", "TEXT", ObjectType.Type.GetProperty("Guid"));
        public static TableField Length = new TableField("Length", "REAL", ObjectType.Type.GetProperty("Length"));
        public static TableField ConduitLength = new TableField("ConduitLength", "REAL", ObjectType.Type.GetProperty("ConduitLength"));

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            ID
            };
        public static new List<FlavoredType> Types = new List<FlavoredType>()
        {
            ObjectType
        };
    }
    internal class NetworkConnectionTable : TableBase
    {
        public static new string TableName = "NetworkConnection";
        public static FlavoredType ObjectType = new FlavoredType(typeof(TECNetworkConnection), 0);

        public static TableField ID = new TableField("ID", "TEXT", ObjectType.Type.GetProperty("Guid"));
        public static TableField Length = new TableField("Length", "REAL", ObjectType.Type.GetProperty("Length"));
        public static TableField ConduitLength = new TableField("ConduitLength", "REAL", ObjectType.Type.GetProperty("ConduitLength"));
        public static TableField IOType = new TableField("IOType", "TEXT", ObjectType.Type.GetProperty("IOType"));

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            ID
            };
        public static new List<FlavoredType> Types = new List<FlavoredType>()
        {
            ObjectType
        };
    }
    internal class ControllerTable : TableBase
    {
        public static new string TableName = "Controller";
        public static FlavoredType ObjectType = new FlavoredType(typeof(TECController), 0);

        public static TableField ID = new TableField("ID", "TEXT", ObjectType.Type.GetProperty("Guid"));
        public static TableField Name = new TableField("Name", "TEXT", ObjectType.Type.GetProperty("Name"));
        public static TableField Description = new TableField("Description", "TEXT", ObjectType.Type.GetProperty("Description"));
        public static TableField Type = new TableField("Type", "TEXT", ObjectType.Type.GetProperty("NetworkType"));

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            ID
            };
        public static new List<FlavoredType> Types = new List<FlavoredType>()
        {
            ObjectType
        };
    }
    internal class MiscTable : TableBase
    {
        public static new string TableName = "Misc";
        public static FlavoredType ObjectType = new FlavoredType(typeof(TECMisc), 0);

        public static TableField ID = new TableField("ID", "TEXT", ObjectType.Type.GetProperty("Guid"));
        public static TableField Name = new TableField("Name", "TEXT", ObjectType.Type.GetProperty("Name"));
        public static TableField Cost = new TableField("Cost", "REAL", ObjectType.Type.GetProperty("Cost"));
        public static TableField Labor = new TableField("Labor", "REAL", ObjectType.Type.GetProperty("Labor"));
        public static TableField Quantity = new TableField("Quantity", "INTEGER", ObjectType.Type.GetProperty("Quantity"));
        public static TableField Type = new TableField("Type", "TEXT", ObjectType.Type.GetProperty("Type"));

        public static new List<TableField> PrimaryKey = new List<TableField>()
        {
            ID
        };

        public static new List<FlavoredType> Types = new List<FlavoredType>()
        {
            ObjectType
        };
    }
    internal class PanelTypeTable : CatalogTableBase
    {
        public static new string TableName = "PanelType";
        public static FlavoredType ObjectType = new FlavoredType(typeof(TECPanelType), 0);

        public static TableField ID = new TableField("ID", "TEXT", ObjectType.Type.GetProperty("Guid"));
        public static TableField Name = new TableField("Name", "TEXT", ObjectType.Type.GetProperty("Name"));
        public static TableField Cost = new TableField("Cost", "REAL", ObjectType.Type.GetProperty("Cost"));
        public static TableField Labor = new TableField("Labor", "REAL", ObjectType.Type.GetProperty("Labor"));

        public static new List<TableField> PrimaryKey = new List<TableField>()
        {
            ID
        };

        public static new List<FlavoredType> Types = new List<FlavoredType>()
        {
            ObjectType
        };
    }
    internal class PanelTable : TableBase
    {
        public static new string TableName = "Panel";
        public static FlavoredType ObjectType = new FlavoredType(typeof(TECPanel), 0);

        public static TableField ID = new TableField("ID", "TEXT", ObjectType.Type.GetProperty("Guid"));
        public static TableField Name = new TableField("Name", "TEXT", ObjectType.Type.GetProperty("Name"));
        public static TableField Description = new TableField("Description", "TEXT", ObjectType.Type.GetProperty("Description"));

        public static new List<TableField> PrimaryKey = new List<TableField>()
        {
            ID
        };

        public static new List<FlavoredType> Types = new List<FlavoredType>()
        {
            ObjectType
        };
    }
    internal class IOModuleTable : CatalogTableBase
    {
        public static new string TableName = "IOModule";
        public static FlavoredType IOModuleType = new FlavoredType(typeof(TECIOModule), 0);

        public static TableField ID = new TableField("ID", "TEXT", IOModuleType.Type.GetProperty("Guid"));
        public static TableField Name = new TableField("Name", "TEXT", IOModuleType.Type.GetProperty("Name"));
        public static TableField Description = new TableField("Description", "TEXT", IOModuleType.Type.GetProperty("Description"));
        public static TableField Cost = new TableField("Cost", "REAL", IOModuleType.Type.GetProperty("Cost"));
        public static TableField IOPerModule = new TableField("IOPerModule", "REAL", IOModuleType.Type.GetProperty("IOPerModule"));

        public static new List<TableField> PrimaryKey = new List<TableField>()
        {
            ID
        };

        public static new List<FlavoredType> Types = new List<FlavoredType>()
        {
            IOModuleType
        };

    }
    internal class IOTable : TableBase
    {
        public static new string TableName = "IO";
        public static FlavoredType IOObjectType = new FlavoredType(typeof(TECIO), 0);

        public static TableField ID = new TableField("ID", "TEXT", IOObjectType.Type.GetProperty("Guid"));
        public static TableField IOType = new TableField("IOType", "TEXT", IOObjectType.Type.GetProperty("Type"));
        public static TableField Quantity = new TableField("Quantity", "INTEGER", IOObjectType.Type.GetProperty("Quantity"));

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            ID
        };
        public static new List<FlavoredType> Types = new List<FlavoredType>()
        {
            IOObjectType
        };
    }
    internal class ControllerTypeTable : CatalogTableBase
    {
        public static new string TableName = "ControllerType";
        public static FlavoredType ObjectType = new FlavoredType(typeof(TECControllerType), 0);

        public static TableField ID = new TableField("ID", "TEXT", ObjectType.Type.GetProperty("Guid"));
        public static TableField Name = new TableField("Name", "TEXT", ObjectType.Type.GetProperty("Name"));
        public static TableField Description = new TableField("Description", "TEXT", ObjectType.Type.GetProperty("Description"));
        public static TableField Cost = new TableField("Cost", "REAL", ObjectType.Type.GetProperty("Cost"));
        public static TableField Labor = new TableField("Labor", "REAL", ObjectType.Type.GetProperty("Labor"));
        public static TableField Type = new TableField("Type", "TEXT", ObjectType.Type.GetProperty("Type"));
        

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            ID
            };
        public static new List<FlavoredType> Types = new List<FlavoredType>()
        {
            ObjectType
        };
    }
    internal class ValveTable : CatalogTableBase
    {
        public static new string TableName = "Valve";
        public static FlavoredType ObjectType = new FlavoredType(typeof(TECValve), 0);

        public static TableField ID = new TableField("ID", "TEXT", ObjectType.Type.GetProperty("Guid"));
        public static TableField Name = new TableField("Name", "TEXT", ObjectType.Type.GetProperty("Name"));
        public static TableField Description = new TableField("Description", "TEXT", ObjectType.Type.GetProperty("Description"));
        public static TableField Cost = new TableField("Cost", "REAL", ObjectType.Type.GetProperty("Cost"));
        public static TableField Labor = new TableField("Labor", "REAL", ObjectType.Type.GetProperty("Labor"));
        public static TableField Type = new TableField("Type", "TEXT", ObjectType.Type.GetProperty("Type"));
        public static TableField Cv = new TableField("Cv", "Real", ObjectType.Type.GetProperty("Cv"));
        public static TableField Size = new TableField("Size", "Real", ObjectType.Type.GetProperty("Size"));
        public static TableField Style = new TableField("Style", "TEXT", ObjectType.Type.GetProperty("Style"));
        
        public static new List<TableField> PrimaryKey = new List<TableField>() {
            ID
        };
        public static new List<FlavoredType> Types = new List<FlavoredType>()
        {
            ObjectType
        };
    }
    #endregion

    #region Relationship Tables
    internal class BidScopeBranchTable : TableBase
    {
        public static new string TableName = "BidScopeBranch";
        public static FlavoredType ObjectType = new FlavoredType(typeof(TECScopeBranch), 0);
        public static FlavoredType ReferenceType = new FlavoredType(typeof(TECBid), 0);

        public static TableField BidID = new TableField("BidID", "TEXT", ReferenceType.Type.GetProperty("Guid"));
        public static TableField ScopeBranchID = new TableField("ScopeBranchID", "TEXT", ObjectType.Type.GetProperty("Guid"));

        public static new List<TableField> PrimaryKey = new List<TableField>()
        {
            BidID,
            ScopeBranchID
        };

        public static new List<FlavoredType> Types = new List<FlavoredType>()
        {
            ObjectType,
            ReferenceType
        };
    }
    internal class BidMiscTable : TableBase
    {
        public static new string TableName = "BidMisc";
        public static FlavoredType BidType = new FlavoredType(typeof(TECBid), 0);
        public static FlavoredType CostType = new FlavoredType(typeof(TECMisc), 0);

        public static TableField BidID = new TableField("BidID", "TEXT", BidType.Type.GetProperty("Guid"));
        public static TableField MiscID = new TableField("MiscID", "TEXT", CostType.Type.GetProperty("Guid"));

        public static new List<TableField> PrimaryKey = new List<TableField>()
        {
            BidID,
            MiscID
        };

        public static new List<FlavoredType> Types = new List<FlavoredType>()
        {
            BidType,
            CostType
        };
    }
    internal class ControllerTypeIOTable : TableBase
    {
        public static new string TableName = "ControllerTypeIO";
        public static FlavoredType ObjectType = new FlavoredType(typeof(TECControllerType), 0);
        public static FlavoredType ReferenceType = new FlavoredType(typeof(TECIO), 0);

        public static TableField TypeID = new TableField("TypeID", "TEXT", ObjectType.Type.GetProperty("Guid"));
        public static TableField IOID = new TableField("IOID", "TEXT", ReferenceType.Type.GetProperty("Guid"));

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            TypeID,
            IOID
            };
        public static new List<FlavoredType> Types = new List<FlavoredType>()
        {
            ObjectType,
            ReferenceType
        };
    }
    internal class HardwareManufacturerTable : TableBase
    {
        public static new string TableName = "HardwareManufacturer";
        public static FlavoredType ObjectType = new FlavoredType(typeof(TECHardware), 0);
        public static FlavoredType ReferenceType = new FlavoredType(typeof(TECManufacturer), 0);

        public static TableField HardwareID = new TableField("HardwareID", "TEXT", ObjectType.Type.GetProperty("Guid"));
        public static TableField ManufacturerID = new TableField("ManufacturerID", "TEXT", ReferenceType.Type.GetProperty("Guid"));

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            HardwareID,
            ManufacturerID
            };
        public static new List<FlavoredType> Types = new List<FlavoredType>()
        {
            ObjectType,
            ReferenceType
        };
    }
    internal class IOIOModuleTable : TableBase
    {
        public static new string TableName = "IOModuleIO";
        public static FlavoredType ObjectType = new FlavoredType(typeof(TECIOModule), 0);
        public static FlavoredType ReferenceType = new FlavoredType(typeof(TECIO), 0);

        public static TableField ModuleID = new TableField("IOModuleID", "TEXT", ObjectType.Type.GetProperty("Guid"));
        public static TableField IOID = new TableField("IOID", "TEXT", ReferenceType.Type.GetProperty("Guid"));

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            ModuleID,
            IOID
        };
        public static new List<FlavoredType> Types = new List<FlavoredType>()
        {
            ObjectType,
            ReferenceType
        };
    }
    internal class ControllerConnectionTable : TableBase
    {
        public static new string TableName = "ControllerConnection";
        public static FlavoredType ObjectType = new FlavoredType(typeof(TECController), 0);
        public static FlavoredType ReferenceType = new FlavoredType(typeof(TECConnection), 0);

        public static TableField ControllerID = new TableField("ControllerID", "TEXT", ObjectType.Type.GetProperty("Guid"));
        public static TableField ConnectionID = new TableField("ConnectionID", "TEXT", ReferenceType.Type.GetProperty("Guid"));

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            ControllerID,
            ConnectionID
            };
        public static new List<FlavoredType> Types = new List<FlavoredType>()
        {
            ObjectType,
            ReferenceType
        };
    }
    internal class ScopeBranchHierarchyTable : TableBase
    {
        public static new string TableName = "ScopeBranchHierarchy";
        public static FlavoredType ObjectType = new FlavoredType(typeof(TECScopeBranch), 0);
        public static FlavoredType ReferenceType = new FlavoredType(typeof(TECScopeBranch), 0);

        public static TableField ParentID = new TableField("ParentID", "TEXT", ObjectType.Type.GetProperty("Guid"));
        public static TableField ChildID = new TableField("ChildID", "TEXT", ReferenceType.Type.GetProperty("Guid"));

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            ParentID,
            ChildID
            };
        public static new List<FlavoredType> Types = new List<FlavoredType>()
        {
            ObjectType,
            ReferenceType
        };

    }
    internal class BidSystemTable : IndexedRelationTableBase
    {
        public static new string TableName = "BidSystem";
        public static FlavoredType SystemType = new FlavoredType(typeof(TECSystem), 0);
        public static FlavoredType BidType = new FlavoredType(typeof(TECBid), 0);

        public static Type Helpers = typeof(HelperProperties);

        public static TableField SystemID = new TableField("SystemID", "TEXT", SystemType.Type.GetProperty("Guid"));
        public static TableField BidID = new TableField("BidID", "TEXT", BidType.Type.GetProperty("Guid"));
        public static TableField Index = new TableField("ScopeIndex", "INTEGER", Helpers.GetProperty("Index"));

        public static new List<TableField> PrimaryKey = new List<TableField>()
        {
            SystemID,
            BidID
        };
        public static new List<FlavoredType> Types = new List<FlavoredType>()
        {
            BidType,
            SystemType
        };
    }
    internal class SystemEquipmentTable : IndexedRelationTableBase
    {
        public static new string TableName = "SystemEquipment";
        public static FlavoredType ObjectType = new FlavoredType(typeof(TECSystem), 0);
        public static FlavoredType ReferenceType = new FlavoredType(typeof(TECEquipment), 0);

        public static Type HelperType = typeof(HelperProperties);

        public static TableField SystemID = new TableField("SystemID", "TEXT", ObjectType.Type.GetProperty("Guid"));
        public static TableField EquipmentID = new TableField("EquipmentID", "TEXT", ReferenceType.Type.GetProperty("Guid"));
        public static TableField ScopeIndex = new TableField("ScopeIndex", "INTEGER", HelperType.GetProperty("Index"));

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            SystemID,
            EquipmentID
            };
        public static new List<FlavoredType> Types = new List<FlavoredType>()
        {
            ObjectType,
            ReferenceType
        };

    }
    internal class EquipmentSubScopeTable : IndexedRelationTableBase
    {
        public static new string TableName = "EquipmentSubScope";
        public static FlavoredType ObjectType = new FlavoredType(typeof(TECEquipment), 0);
        public static FlavoredType ReferenceType = new FlavoredType(typeof(TECSubScope), 0);

        public static Type HelperType = typeof(HelperProperties);

        public static TableField EquipmentID = new TableField("EquipmentID", "TEXT", ObjectType.Type.GetProperty("Guid"));
        public static TableField SubScopeID = new TableField("SubScopeID", "TEXT", ReferenceType.Type.GetProperty("Guid"));
        public static TableField ScopeIndex = new TableField("ScopeIndex", "INTEGER", HelperType.GetProperty("Index"));

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            EquipmentID,
            SubScopeID
            };
        public static new List<FlavoredType> Types = new List<FlavoredType>()
        {
            ObjectType,
            ReferenceType
        };

    }
    internal class SubScopeDeviceTable : IndexedRelationTableBase
    {
        public static new string TableName = "SubScopeDevice";
        public static FlavoredType ObjectType = new FlavoredType(typeof(TECSubScope), 0);
        public static FlavoredType ReferenceType = new FlavoredType(typeof(ITECConnectable), 0);

        public static Type HelperType = typeof(HelperProperties);

        public static TableField SubScopeID = new TableField("SubScopeID", "TEXT", ObjectType.Type.GetProperty("Guid"));
        public static TableField DeviceID = new TableField("DeviceID", "TEXT", ReferenceType.Type.GetProperty("Guid"));
        public static TableField Quantity = new TableField("Quantity", "INTEGER", HelperType.GetProperty("Quantity"));
        public static TableField ScopeIndex = new TableField("ScopeIndex", "INTEGER", HelperType.GetProperty("Index"));

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            SubScopeID,
            DeviceID
            };
        public static new List<FlavoredType> Types = new List<FlavoredType>()
        {
            ObjectType,
            ReferenceType
        };

    }
    internal class SubScopePointTable : IndexedRelationTableBase
    {
        public static new string TableName = "SubScopePoint";
        public static FlavoredType ObjectType = new FlavoredType(typeof(TECSubScope), 0);
        public static FlavoredType ReferenceType = new FlavoredType(typeof(TECPoint), 0);

        public static Type HelperType = typeof(HelperProperties);

        public static TableField SubScopeID = new TableField("SubScopeID", "TEXT", ObjectType.Type.GetProperty("Guid"));
        public static TableField PointID = new TableField("PointID", "TEXT", ReferenceType.Type.GetProperty("Guid"));

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            SubScopeID,
            PointID
            };
        public static new List<FlavoredType> Types = new List<FlavoredType>()
        {
            ObjectType,
            ReferenceType
        };

    }
    internal class ScopeTagTable : TableBase
    {
        public static new string TableName = "ScopeTag";
        public static FlavoredType ObjectType = new FlavoredType(typeof(TECScope), 0);
        public static FlavoredType ReferenceType = new FlavoredType(typeof(TECLabeled), Flavor.Tag);

        public static TableField ScopeID = new TableField("ScopeID", "TEXT", ObjectType.Type.GetProperty("Guid"));
        public static TableField TagID = new TableField("TagID", "TEXT", ReferenceType.Type.GetProperty("Guid"));

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            ScopeID,
            TagID
            };
        public static new List<FlavoredType> Types = new List<FlavoredType>()
        {
            ObjectType,
            ReferenceType
        };
    }
    internal class DeviceConnectionTypeTable : IndexedRelationTableBase
    {
        public static new string TableName = "DeviceConnectionType";
        public static FlavoredType ObjectType = new FlavoredType(typeof(TECDevice), 0);
        public static FlavoredType ReferenceType = new FlavoredType(typeof(TECElectricalMaterial), Flavor.Wire);
        public static Flavor ReferenceFlavor = Flavor.Wire;

        public static Type HelperType = typeof(HelperProperties);

        public static TableField DeviceID = new TableField("DeviceID", "TEXT", ObjectType.Type.GetProperty("Guid"));
        public static TableField TypeID = new TableField("ConnectionTypeID", "TEXT", ReferenceType.Type.GetProperty("Guid"));
        public static TableField Quantity = new TableField("Quantity", "INTEGER", HelperType.GetProperty("Quantity"));

        public static new List<TableField> PrimaryKey = new List<TableField>()
        {
            DeviceID,
            TypeID
        };
        public static new List<FlavoredType> Types = new List<FlavoredType>()
        {
            ObjectType,
            ReferenceType
        };
    }
    internal class LoactedLocationTable : TableBase
    {
        public static new string TableName = "LocatedLocation";
        public static FlavoredType ObjectType = new FlavoredType(typeof(TECLocated), 0);
        public static FlavoredType ReferenceType = new FlavoredType(typeof(TECLabeled), Flavor.Location);

        public static TableField ScopeID = new TableField("ScopeID", "TEXT", ObjectType.Type.GetProperty("Guid"));
        public static TableField LocationID = new TableField("LocationID", "TEXT", ReferenceType.Type.GetProperty("Guid"));

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            LocationID,
            ScopeID
            };
        public static new List<FlavoredType> Types = new List<FlavoredType>()
        {
            ObjectType,
            ReferenceType
        };
    }
    internal class ScopeAssociatedCostTable : IndexedRelationTableBase
    {
        public static new string TableName = "ScopeAssociatedCost";
        public static FlavoredType ObjectType = new FlavoredType(typeof(TECScope), 0);
        public static FlavoredType ReferenceType = new FlavoredType(typeof(TECCost), 0);

        public static Type HelperType = typeof(HelperProperties);

        public static TableField ScopeID = new TableField("ScopeID", "TEXT", ObjectType.Type.GetProperty("Guid"));
        public static TableField AssociatedCostID = new TableField("AssociatedCostID", "TEXT", ReferenceType.Type.GetProperty("Guid"));
        public static TableField Quantity = new TableField("Quantity", "INTEGER", HelperType.GetProperty("Quantity"));

        public static new List<TableField> PrimaryKey = new List<TableField>()
        {
            ScopeID,
            AssociatedCostID
        };
        public static new List<FlavoredType> Types = new List<FlavoredType>()
        {
            ObjectType,
            ReferenceType
        };
    }
    internal class ElectricalMaterialRatedCostTable : IndexedRelationTableBase
    {
        public static new string TableName = "ElectricalMaterialRatedCost";
        public static FlavoredType ObjectType = new FlavoredType(typeof(TECElectricalMaterial), 0);
        public static FlavoredType ReferenceType = new FlavoredType(typeof(TECCost), 0);

        public static Type HelperType = typeof(HelperProperties);

        public static TableField ComponentID = new TableField("ComponentID", "TEXT", ObjectType.Type.GetProperty("Guid"));
        public static TableField CostID = new TableField("CostID", "TEXT", ReferenceType.Type.GetProperty("Guid"));
        public static TableField Quantity = new TableField("Quantity", "INTEGER", HelperType.GetProperty("Quantity"));

        public static new List<TableField> PrimaryKey = new List<TableField>()
        {
            ComponentID,
            CostID
        };
        public static new List<FlavoredType> Types = new List<FlavoredType>()
        {
            ObjectType,
            ReferenceType
        };
    }
    internal class ControllerControllerTypeTable : TableBase
    {
        public static new string TableName = "ControllerControllerType";
        public static FlavoredType ControllerType = new FlavoredType(typeof(TECController), 0);
        public static FlavoredType TypeType = new FlavoredType(typeof(TECControllerType), 0);

        public static TableField ControllerID = new TableField("ControllerID", "TEXT", ControllerType.Type.GetProperty("Guid"));
        public static TableField TypeID = new TableField("TypeID", "TEXT", TypeType.Type.GetProperty("Guid"));

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            ControllerID,
            TypeID
            };
        public static new List<FlavoredType> Types = new List<FlavoredType>()
        {
            ControllerType,
            TypeType
        };
    }
    internal class ConnectionConduitTypeTable : TableBase
    {
        public static new string TableName = "ConnectionConduitType";
        public static FlavoredType ObjectType = new FlavoredType(typeof(TECConnection), 0);
        public static FlavoredType ReferenceType = new FlavoredType(typeof(TECElectricalMaterial), Flavor.Conduit);
        public static Flavor ReferenceFlavor = Flavor.Conduit;

        public static TableField ConnectionID = new TableField("ConnectionID", "TEXT", ObjectType.Type.GetProperty("Guid"));
        public static TableField TypeID = new TableField("ConduitTypeID", "TEXT", ReferenceType.Type.GetProperty("Guid"));

        public static new List<TableField> PrimaryKey = new List<TableField>()
        {
            ConnectionID
        };
        public static new List<FlavoredType> Types = new List<FlavoredType>()
        {
            ObjectType,
            ReferenceType
        };
    }
    internal class NetworkConnectionConnectionTypeTable : TableBase
    {
        public static new string TableName = "NetworkConnectionConnectionType";
        public static FlavoredType ObjectType = new FlavoredType(typeof(TECNetworkConnection), 0);
        public static FlavoredType ReferenceType = new FlavoredType(typeof(TECElectricalMaterial), Flavor.Wire);

        public static TableField ConnectionID = new TableField("ConnectionID", "TEXT", ObjectType.Type.GetProperty("Guid"));
        public static TableField TypeID = new TableField("ConnectionTypeID", "TEXT", ReferenceType.Type.GetProperty("Guid"));

        public static new List<TableField> PrimaryKey = new List<TableField>()
        {
            ConnectionID
        };
        public static new List<FlavoredType> Types = new List<FlavoredType>()
        {
            ObjectType,
            ReferenceType
        };
    }
    internal class NetworkConnectionControllerTable : TableBase
    {
        public static new string TableName = "NetworkConnectionController";
        public static FlavoredType ConnectionType = new FlavoredType(typeof(TECNetworkConnection), 0);
        public static FlavoredType ControllerType = new FlavoredType(typeof(TECController), 0);

        public static TableField ConnectionID = new TableField("ConnectionID", "TEXT", ConnectionType.Type.GetProperty("Guid"));
        public static TableField ControllerID = new TableField("ControllerID", "TEXT", ControllerType.Type.GetProperty("Guid"));
       
        public static new List<TableField> PrimaryKey = new List<TableField>() {
            ConnectionID,
            ControllerID
            };
        public static new List<FlavoredType> Types = new List<FlavoredType>()
        {
            ConnectionType,
            ControllerType
        };
    }
    internal class SubScopeConnectionChildrenTable : TableBase
    {
        public static new string TableName = "SubScopeConnectionChild";
        public static FlavoredType ConnectionType = new FlavoredType(typeof(TECSubScopeConnection), 0);
        public static FlavoredType ChildType = new FlavoredType(typeof(TECSubScope), 0);

        public static TableField ConnectionID = new TableField("ConnectionID", "TEXT", ConnectionType.Type.GetProperty("Guid"));
        public static TableField ChildID = new TableField("ScopeID", "TEXT", ChildType.Type.GetProperty("Guid"));


        public static new List<TableField> PrimaryKey = new List<TableField>() {
            ConnectionID,
            ChildID
            };
        public static new List<FlavoredType> Types = new List<FlavoredType>()
        {
            ConnectionType,
            ChildType
        };
    }
    internal class PanelPanelTypeTable : TableBase
    {
        public static new string TableName = "PanelPanelType";
        public static FlavoredType ObjectType = new FlavoredType(typeof(TECPanel), 0);
        public static FlavoredType ReferenceType = new FlavoredType(typeof(TECPanelType), 0);

        public static TableField PanelID = new TableField("PanelID", "TEXT", ObjectType.Type.GetProperty("Guid"));
        public static TableField PanelTypeID = new TableField("PanelTypeID", "TEXT", ReferenceType.Type.GetProperty("Guid"));

        public static new List<TableField> PrimaryKey = new List<TableField>()
        {
            PanelID,
            PanelTypeID
        };
        public static new List<FlavoredType> Types = new List<FlavoredType>()
        {
            ObjectType,
            ReferenceType
        };
    }
    internal class PanelControllerTable : TableBase
    {
        public static new string TableName = "PanelController";
        public static FlavoredType ObjectType = new FlavoredType(typeof(TECPanel), 0);
        public static FlavoredType ReferenceType = new FlavoredType(typeof(TECController), 0);

        public static TableField PanelID = new TableField("PanelID", "TEXT", ObjectType.Type.GetProperty("Guid"));
        public static TableField ControllerID = new TableField("ControllerID", "TEXT", ReferenceType.Type.GetProperty("Guid"));

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            PanelID,
            ControllerID
            };
        public static new List<FlavoredType> Types = new List<FlavoredType>()
        {
            ObjectType,
            ReferenceType
        };
    }
    internal class SystemControllerTable : TableBase
    {
        public static new string TableName = "SystemController";
        public static FlavoredType SystemType = new FlavoredType(typeof(TECSystem), 0);
        public static FlavoredType ControllerType = new FlavoredType(typeof(TECController), 0);

        public static TableField SystemID = new TableField("SystemID", "TEXT", SystemType.Type.GetProperty("Guid"));
        public static TableField ControllerID = new TableField("ControllerID", "TEXT", ControllerType.Type.GetProperty("Guid"));

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            SystemID,
            ControllerID
            };
        public static new List<FlavoredType> Types = new List<FlavoredType>()
        {
            SystemType,
            ControllerType
        };
    }
    internal class SystemPanelTable : TableBase
    {
        public static new string TableName = "SystemPanel";
        public static FlavoredType SystemType = new FlavoredType(typeof(TECSystem), 0);
        public static FlavoredType PanelType = new FlavoredType(typeof(TECPanel), 0);

        public static TableField SystemID = new TableField("SystemID", "TEXT", SystemType.Type.GetProperty("Guid"));
        public static TableField PanelID = new TableField("PanelID", "TEXT", PanelType.Type.GetProperty("Guid"));

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            SystemID,
            PanelID
            };
        public static new List<FlavoredType> Types = new List<FlavoredType>()
        {
            SystemType,
            PanelType
        };
    }
    internal class SystemScopeBranchTable : TableBase
    {
        public static new string TableName = "SystemScopeBranch";
        public static FlavoredType SystemType = new FlavoredType(typeof(TECSystem), 0);
        public static FlavoredType ScopeBranchType = new FlavoredType(typeof(TECScopeBranch), 0);

        public static TableField SystemID = new TableField("SystemID", "TEXT", SystemType.Type.GetProperty("Guid"));
        public static TableField BranchID = new TableField("BranchID", "TEXT", ScopeBranchType.Type.GetProperty("Guid"));

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            SystemID,
            BranchID
            };
        public static new List<FlavoredType> Types = new List<FlavoredType>()
        {
            SystemType,
            ScopeBranchType
        };
    }
    internal class SystemHierarchyTable : TableBase
    {
        public static new string TableName = "SystemHierarchy";
        public static FlavoredType SystemType = new FlavoredType(typeof(TECSystem), 0);

        public static TableField ParentID = new TableField("ParentID", "TEXT", SystemType.Type.GetProperty("Guid"));
        public static TableField ChildID = new TableField("ChildID", "TEXT", SystemType.Type.GetProperty("Guid"));

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            ParentID,
            ChildID
            };
        public static new List<FlavoredType> Types = new List<FlavoredType>()
        {
            SystemType,
            SystemType
        };
    }
    internal class SystemMiscTable : TableBase
    {
        public static new string TableName = "SystemMisc";
        public static FlavoredType SystemType = new FlavoredType(typeof(TECSystem), 0);
        public static FlavoredType MiscType = new FlavoredType(typeof(TECMisc), 0);

        public static TableField SystemID = new TableField("SystemID", "TEXT", SystemType.Type.GetProperty("Guid"));
        public static TableField MiscID = new TableField("MIscID", "TEXT", MiscType.Type.GetProperty("Guid"));
        
        public static new List<TableField> PrimaryKey = new List<TableField>() {
            SystemID,
            MiscID
            };
        public static new List<FlavoredType> Types = new List<FlavoredType>()
        {
            SystemType,
            MiscType
        };
    }
    internal class ValveActuatorTable : TableBase
    {
        public static new string TableName = "ValveActuator";
        public static FlavoredType ValveType = new FlavoredType(typeof(TECValve), 0);
        public static FlavoredType ActuatorType = new FlavoredType(typeof(TECDevice), 0);

        public static TableField ValveID = new TableField("SystemID", "TEXT", ValveType.Type.GetProperty("Guid"));
        public static TableField ActuatorID = new TableField("MIscID", "TEXT", ActuatorType.Type.GetProperty("Guid"));

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            ValveID,
            ActuatorID
        };
        public static new List<FlavoredType> Types = new List<FlavoredType>()
        {
            ValveType,
            ActuatorType
        };
    }

    internal class TypicalInstanceTable : TableBase
    {
        public static new string TableName = "TypicalInstance";
        public static FlavoredType ScopeType = new FlavoredType(typeof(TECScope), 0);

        public static TableField TypicalID = new TableField("TypicalID", "TEXT", ScopeType.Type.GetProperty("Guid"));
        public static TableField InstanceID = new TableField("InstanceID", "TEXT", ScopeType.Type.GetProperty("Guid"));

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            TypicalID,
            InstanceID
            };
        public static new List<FlavoredType> Types = new List<FlavoredType>()
        {
            ScopeType,
            ScopeType
        };
    }
    #endregion

    internal static class AllBidTables
    {
        public static List<TableBase> Tables = new List<TableBase>() {
            new MetadataTable(),
            new BidInfoTable(),
            new ParametersTable(),
            new ExtraLaborTable(),
            new NoteTable(),
            new ExclusionTable(),
            new ScopeBranchTable(),
            new SystemTable(),
            new EquipmentTable(),
            new SubScopeTable(),
            new DeviceTable(),
            new PointTable(),
            new TagTable(),
            new ManufacturerTable(),
            new LocationTable(),
            new MiscTable(),
            new PanelTable(),
            new PanelTypeTable(),
            new SubScopeConnectionTable(),
            new NetworkConnectionTable(),
            new IOTable(),
            new IOModuleTable(),
            new ControllerTypeTable(),
            new ValveTable(),

            new ConnectionTypeTable(),
            new ConduitTypeTable(),
            new ScopeBranchHierarchyTable(),
            new BidSystemTable(),
            new SystemEquipmentTable(),
            new EquipmentSubScopeTable(),
            new SubScopeDeviceTable(),
            new SubScopePointTable(),
            new ScopeTagTable(),
            new HardwareManufacturerTable(),
            new LoactedLocationTable(),
            new ControllerTable(),
            new AssociatedCostTable(),
            new ElectricalMaterialRatedCostTable(),
            new ControllerConnectionTable(),
            new ControllerTypeIOTable(),
            new IOIOModuleTable(),
            new BidScopeBranchTable(),
            new DeviceConnectionTypeTable(),
            new ScopeAssociatedCostTable(),
            new ControllerControllerTypeTable(),
            new ConnectionConduitTypeTable(),
            new SystemControllerTable(),
            new SystemPanelTable(),
            new SystemScopeBranchTable(),
            new SystemHierarchyTable(),
            new SystemMiscTable(),
            new TypicalInstanceTable(),
            new BidMiscTable(),
            new PanelPanelTypeTable(),
            new PanelControllerTable(),
            new SubScopeConnectionChildrenTable(),
            new NetworkConnectionControllerTable(),
            new NetworkConnectionConnectionTypeTable(),
            };
    }

    internal static class AllTemplateTables
    {
        public static List<TableBase> Tables = new List<TableBase>()
        {
            new MetadataTable(),
            new TemplatesInfoTable(),
            new ExtraLaborTable(),
            new SystemTable(),
            new EquipmentTable(),
            new SubScopeTable(),
            new DeviceTable(),
            new PointTable(),
            new TagTable(),
            new ManufacturerTable(),
            new PanelTypeTable(),
            new PanelTable(),
            new MiscTable(),
            new SubScopeConnectionTable(),
            new IOModuleTable(),
            new IOTable(),
            new ScopeBranchTable(),
            new ControllerTypeTable(),
            new ValveTable(),

            new ConnectionTypeTable(),
            new ConduitTypeTable(),
            new AssociatedCostTable(),
            new SystemEquipmentTable(),
            new ControllerConnectionTable(),
            new EquipmentSubScopeTable(),
            new SubScopeDeviceTable(),
            new SubScopePointTable(),
            new ScopeTagTable(),
            new ControllerTable(),
            new ControllerTypeIOTable(),
            new IOIOModuleTable(),
            new HardwareManufacturerTable(),
            new DeviceConnectionTypeTable(),
            new ScopeAssociatedCostTable(),
            new ElectricalMaterialRatedCostTable(),
            new ControllerControllerTypeTable(),
            new ConnectionConduitTypeTable(),
            new PanelPanelTypeTable(),
            new SystemControllerTable(),
            new SystemPanelTable(),
            new SystemHierarchyTable(),
            new SystemScopeBranchTable(),
            new SystemMiscTable(),
            new PanelControllerTable(),
            new SubScopeConnectionChildrenTable(),
            new ScopeBranchHierarchyTable()
        };
    }

    internal static class AllTables
    {
        public static List<object> Tables = new List<object>()
        {
            new MetadataTable(),
            new BidInfoTable(),
            new TemplatesInfoTable(),
            new ParametersTable(),
            new ExtraLaborTable(),
            new NoteTable(),
            new ExclusionTable(),
            new BidScopeBranchTable(),
            new ScopeBranchTable(),
            new SystemTable(),
            new EquipmentTable(),
            new SubScopeTable(),
            new DeviceTable(),
            new PointTable(),
            new TagTable(),
            new ManufacturerTable(),
            new LocationTable(),
            new MiscTable(),
            new PanelTypeTable(),
            new PanelTable(),
            new SubScopeConnectionTable(),
            new NetworkConnectionTable(),
            new IOModuleTable(),
            new IOTable(),
            new ControllerTypeTable(),
            new ValveTable(),

            new ConnectionTypeTable(),
            new ConduitTypeTable(),
            new AssociatedCostTable(),
            new ControllerTable(),
            new ControllerTypeIOTable(),
            new IOIOModuleTable(),
            new ControllerConnectionTable(),
            new ScopeBranchHierarchyTable(),
            new BidSystemTable(),
            new SystemEquipmentTable(),
            new EquipmentSubScopeTable(),
            new SubScopeDeviceTable(),
            new SubScopePointTable(),
            new ScopeTagTable(),
            new HardwareManufacturerTable(),
            new DeviceConnectionTypeTable(),
            new LoactedLocationTable(),
            new ScopeAssociatedCostTable(),
            new ElectricalMaterialRatedCostTable(),
            new ControllerControllerTypeTable(),
            new ConnectionConduitTypeTable(),
            new PanelPanelTypeTable(),
            new PanelControllerTable(),
            new SystemControllerTable(),
            new SystemPanelTable(),
            new SystemHierarchyTable(),
            new SystemScopeBranchTable(),
            new SystemMiscTable(),
            new TypicalInstanceTable(),
            new SubScopeConnectionChildrenTable(),
            new NetworkConnectionControllerTable(),
            new NetworkConnectionConnectionTypeTable(),
            new BidMiscTable()
        };
    }

    internal class TableBase
    {
        public static string TableName;
        public static List<FlavoredType> Types;

        public static List<TableField> PrimaryKey;
    }

    internal class IndexedRelationTableBase : TableBase { }

    internal class CatalogTableBase : TableBase { }

    internal class TableField
    {
        public string Name;
        public string FieldType;
        public PropertyInfo Property;

        public TableField(string name, string fieldType, PropertyInfo property)
        {
            Name = name;
            FieldType = fieldType;
            Property = property;
        }
    }

    internal class HelperProperties
    {
        public bool Index { get; set; }
        public bool Quantity { get; set; }
        public bool DBVersion { get; set; }

        public HelperProperties()
        {
            Index = true;
            Quantity = true;
            DBVersion = true;
        }

    }

    internal class TableInfo
    {
        //Returns Tuple<TableName, List<AllTableFields>, List<PrimaryKeyTableFields>, List<RelevantTypesInTable>, isRelationTable>
        public string Name { get; set; }
        public List<TableField> Fields { get; set; }
        public List<TableField> PrimaryFields { get; set; }
        public List<FlavoredType> Types { get; set; }
        public bool IsRelationTable { get; set; }
        public bool IsCatalogTable { get; set; }

        public TableInfo(TableBase table)
        {
            string tableName = "";
            List<TableField> primaryKey = new List<TableField>();
            List<TableField> fields = new List<TableField>();
            List<FlavoredType> types = new List<FlavoredType>();
            var tableType = table.GetType();
            

            bool isCatalogTable = false;
            if (tableType.BaseType == typeof(CatalogTableBase))
            {
                isCatalogTable = true;
            }

            foreach (var p in tableType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static))
            {
                if (p.Name == "TableName")
                {
                    var v = p.GetValue(null);
                    tableName += (string)v;
                }
                else if (p.Name == "PrimaryKey")
                {
                    var v = p.GetValue(null) as List<TableField>;
                    foreach (TableField field in v)
                    { primaryKey.Add(field); }
                }
                else if (p.Name == "Types")
                {
                    var v = p.GetValue(null) as List<FlavoredType>;
                    foreach (FlavoredType type in v)
                    { types.Add(type); }
                }
                else if (p.FieldType.Name == "TableField")
                {
                    var v = p.GetValue(null) as TableField;
                    fields.Add(v);
                }

            }

            bool isIndexedRelationTable = false;
            if (tableType.BaseType == typeof(IndexedRelationTableBase) || types.Count > 1)
            {
                isIndexedRelationTable = true;
            }

            Name = tableName;
            Fields = fields;
            PrimaryFields = primaryKey;
            Types = types;
            IsRelationTable = isIndexedRelationTable;
            IsCatalogTable = isCatalogTable;
        }

    }

    internal struct FlavoredType
    {
        public Type Type { get; private set; }
        public Flavor Flavor { get; private set; }

        public FlavoredType(Type type, Flavor flavor)
        {
            Type = type;
            Flavor = flavor;
        }

        public static bool operator ==(FlavoredType left, FlavoredType right)
        {
            return ((left.Type == right.Type) && (left.Flavor == right.Flavor));
        }
        public static bool operator !=(FlavoredType left, FlavoredType right)
        {
            return !(left == right);
        }
    }
    #endregion
}
