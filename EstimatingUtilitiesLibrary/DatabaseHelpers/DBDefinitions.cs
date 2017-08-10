using EstimatingLibrary;
using EstimatingLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingUtilitiesLibrary
{
    #region Table Definitions
    #region Object Tables
    public class BidInfoTable : TableBase
    {
        public static new string TableName = "TECBidInfo";
        public static FlavoredType ObjectType = new FlavoredType(typeof(TECBid), 0);

        public static Type HelperType = typeof(HelperProperties);

        public static TableField DBVersion = new TableField("DBVersion", "TEXT", HelperType.GetProperty("DBVersion"));
        public static TableField BidName = new TableField("BidName", "TEXT", ObjectType.Type.GetProperty("Name"));
        public static TableField BidID = new TableField("BidID", "TEXT", ObjectType.Type.GetProperty("Guid"));
        public static TableField BidNumber = new TableField("BidNumber", "TEXT", ObjectType.Type.GetProperty("BidNumber"));
        public static TableField DueDate = new TableField("DueDate", "TEXT", ObjectType.Type.GetProperty("DueDateString"));
        public static TableField Salesperson = new TableField("Salesperson", "TEXT", ObjectType.Type.GetProperty("Salesperson"));
        public static TableField Estimator = new TableField("Estimator", "TEXT", ObjectType.Type.GetProperty("Estimator"));

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            BidID
        };

        public static new List<FlavoredType> Types = new List<FlavoredType>() {
            ObjectType
        };

    }
    public class TemplatesInfoTable : TableBase
    {
        public static new string TableName = "TECTemplatesInfo";
        public static FlavoredType ObjectType = new FlavoredType(typeof(TECTemplates), 0);

        public static Type HelperType = typeof(HelperProperties);

        public static TableField TemplateID = new TableField("TemplateID", "TEXT", ObjectType.Type.GetProperty("Guid"));
        public static TableField DBVersion = new TableField("DBVersion", "TEXT", HelperType.GetProperty("DBVersion"));

        public static new List<TableField> PrimaryKey = new List<TableField>()
        {
            TemplateID
        };

        public static new List<FlavoredType> Types = new List<FlavoredType>()
        {
            ObjectType
        };
    }
    public class BidParametersTable : TableBase
    {
        public static new string TableName = "TECBidParameters";
        public static FlavoredType ParameterType = new FlavoredType(typeof(TECBidParameters), 0);

        public static TableField ParametersID = new TableField("ParametersID", "TEXT", ParameterType.Type.GetProperty("Guid"));

        public static TableField Escalation = new TableField("Escalation", "REAL", ParameterType.Type.GetProperty("Escalation"));
        public static TableField Overhead = new TableField("Overhead", "REAL", ParameterType.Type.GetProperty("Overhead"));
        public static TableField Profit = new TableField("Profit", "REAL", ParameterType.Type.GetProperty("Profit"));
        public static TableField SubcontractorMarkup = new TableField("SubcontractorMarkup", "REAL", ParameterType.Type.GetProperty("SubcontractorMarkup"));
        public static TableField SubcontractorEscalation = new TableField("SubcontractorEscalation", "REAL", ParameterType.Type.GetProperty("SubcontractorEscalation"));

        public static TableField IsTaxExempt = new TableField("IsTaxExempt", "INTEGER", ParameterType.Type.GetProperty("IsTaxExempt"));
        public static TableField RequiresBond = new TableField("RequiresBond", "INTEGER", ParameterType.Type.GetProperty("RequiresBond"));
        public static TableField RequiresWrapUp = new TableField("RequiresWrapUp", "INTEGER", ParameterType.Type.GetProperty("RequiresWrapUp"));

        public static new List<TableField> PrimaryKey = new List<TableField>()
        {
            ParametersID
        };

        public static new List<FlavoredType> Types = new List<FlavoredType>()
        {
            ParameterType
        };
    }
    public class LaborConstantsTable : TableBase
    {
        public static new string TableName = "TECLaborConst";
        public static FlavoredType LaborType = new FlavoredType(typeof(TECLabor), 0);

        public static TableField LaborID = new TableField("LaborID", "TEXT", LaborType.Type.GetProperty("Guid"));

        public static TableField PMCoef = new TableField("PMCoef", "REAL", LaborType.Type.GetProperty("PMCoef"));
        public static TableField PMRate = new TableField("PMRate", "REAL", LaborType.Type.GetProperty("PMRate"));

        public static TableField ENGCoef = new TableField("ENGCoef", "REAL", LaborType.Type.GetProperty("ENGCoef"));
        public static TableField ENGRate = new TableField("ENGRate", "REAL", LaborType.Type.GetProperty("ENGRate"));

        public static TableField CommCoef = new TableField("CommCoef", "REAL", LaborType.Type.GetProperty("CommCoef"));
        public static TableField CommRate = new TableField("CommRate", "REAL", LaborType.Type.GetProperty("CommRate"));

        public static TableField SoftCoef = new TableField("SoftCoef", "REAL", LaborType.Type.GetProperty("SoftCoef"));
        public static TableField SoftRate = new TableField("SoftRate", "REAL", LaborType.Type.GetProperty("SoftRate"));

        public static TableField GraphCoef = new TableField("GraphCoef", "REAL", LaborType.Type.GetProperty("GraphCoef"));
        public static TableField GraphRate = new TableField("GraphRate", "REAL", LaborType.Type.GetProperty("GraphRate"));

        public static new List<TableField> PrimaryKey = new List<TableField>()
        {
            LaborID
        };

        public static new List<FlavoredType> Types = new List<FlavoredType>()
        {
            LaborType
        };
    }
    public class SubcontractorConstantsTable : TableBase
    {
        public static new string TableName = "TECSubcontractorConst";
        public static FlavoredType LaborType = new FlavoredType(typeof(TECLabor), 0);

        public static TableField LaborID = new TableField("LaborID", "TEXT", LaborType.Type.GetProperty("Guid"));

        public static TableField ElectricalRate = new TableField("ElectricalRate", "REAL", LaborType.Type.GetProperty("ElectricalRate"));
        public static TableField ElectricalSuperRate = new TableField("ElectricalSuperRate", "REAL", LaborType.Type.GetProperty("ElectricalSuperRate"));
        public static TableField ElectricalNonUnionRate = new TableField("ElectricalNonUnionRate", "REAL", LaborType.Type.GetProperty("ElectricalNonUnionRate"));
        public static TableField ElectricalSuperNonUnionRate = new TableField("ElectricalSuperNonUnionRate", "REAL", LaborType.Type.GetProperty("ElectricalSuperNonUnionRate"));
        public static TableField ElectricalSuperRatio = new TableField("ElectricalSuperRatio", "REAL", LaborType.Type.GetProperty("ElectricalSuperRatio"));

        public static TableField ElectricalIsOnOvertime = new TableField("ElectricalIsOnOvertime", "INTEGER", LaborType.Type.GetProperty("ElectricalIsOnOvertime"));
        public static TableField ElectricalIsUnion = new TableField("ElectricalIsUnion", "INTEGER", LaborType.Type.GetProperty("ElectricalIsUnion"));

        public static new List<TableField> PrimaryKey = new List<TableField>()
        {
            LaborID
        };

        public static new List<FlavoredType> Types = new List<FlavoredType>()
        {
            LaborType
        };
    }
    public class UserAdjustmentsTable : TableBase
    {
        public static new string TableName = "TECUserAdjustments";
        public static FlavoredType BidType = new FlavoredType( typeof(TECBid), 0);
        public static FlavoredType LaborType = new FlavoredType( typeof(TECLabor), 0);

        public static TableField BidID = new TableField("BidID", "TEXT", BidType.Type.GetProperty("Guid"));

        public static TableField PMExtraHours = new TableField("PMExtraHours", "REAL", LaborType.Type.GetProperty("PMExtraHours"));
        public static TableField ENGExtraHours = new TableField("ENGExtraHours", "REAL", LaborType.Type.GetProperty("ENGExtraHours"));
        public static TableField CommExtraHours = new TableField("CommExtraHours", "REAL", LaborType.Type.GetProperty("CommExtraHours"));
        public static TableField SoftExtraHours = new TableField("SoftExtraHours", "REAL", LaborType.Type.GetProperty("SoftExtraHours"));
        public static TableField GraphExtraHours = new TableField("GraphExtraHours", "REAL", LaborType.Type.GetProperty("GraphExtraHours"));

        public static new List<TableField> PrimaryKey = new List<TableField>()
        {
            BidID
        };

        public static new List<FlavoredType> Types = new List<FlavoredType>()
        {
            BidType,
            LaborType
        };

    }
    public class NoteTable : TableBase
    {
        public static new string TableName = "TECNote";
        public static FlavoredType ObjectType = new FlavoredType(typeof(TECLabeled), Flavor.Note);


        public static TableField NoteID = new TableField("NoteID", "TEXT", ObjectType.Type.GetProperty("Guid"));
        public static TableField NoteText = new TableField("NoteText", "TEXT", ObjectType.Type.GetProperty("Label"));

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            NoteID
            };

        public static new List<FlavoredType> Types = new List<FlavoredType>()
        {
            ObjectType
        };
    }
    public class ExclusionTable : TableBase
    {
        public static new string TableName = "TECExclusion";
        public static FlavoredType ObjectType = new FlavoredType(typeof(TECLabeled), Flavor.Exclusion);
        public static Flavor ObjectFlavor = Flavor.Exclusion;

        public static TableField ExclusionID = new TableField("ExclusionID", "TEXT", ObjectType.Type.GetProperty("Guid"));
        public static TableField ExclusionText = new TableField("ExclusionText", "TEXT", ObjectType.Type.GetProperty("Label"));

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            ExclusionID
            };

        public static new List<FlavoredType> Types = new List<FlavoredType>()
        {
            ObjectType
        };

    }
    public class ScopeBranchTable : TableBase
    {
        public static new string TableName = "TECScopeBranch";
        public static FlavoredType ObjectType = new FlavoredType(typeof(TECScopeBranch), 0);

        public static TableField ScopeBranchID = new TableField("ScopeBranchID", "TEXT", ObjectType.Type.GetProperty("Guid"));
        public static TableField Label = new TableField("Label", "TEXT", ObjectType.Type.GetProperty("Label"));

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            ScopeBranchID
            };
        public static new List<FlavoredType> Types = new List<FlavoredType>()
        {
            ObjectType
        };

    }
    public class SystemTable : TableBase
    {
        public static new string TableName = "TECSystem";
        public static FlavoredType ObjectType = new FlavoredType(typeof(TECSystem), 0);

        public static TableField SystemID = new TableField("SystemID", "TEXT", ObjectType.Type.GetProperty("Guid"));
        public static TableField Name = new TableField("Name", "TEXT", ObjectType.Type.GetProperty("Name"));
        public static TableField Description = new TableField("Description", "TEXT", ObjectType.Type.GetProperty("Description"));
        public static TableField ProposeEquipment = new TableField("ProposeEquipment", "INTEGER", ObjectType.Type.GetProperty("ProposeEquipment"));

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            SystemID
            };
        public static new List<FlavoredType> Types = new List<FlavoredType>()
        {
            ObjectType
        };

    }
    public class EquipmentTable : TableBase
    {
        public static new string TableName = "TECEquipment";
        public static FlavoredType ObjectType = new FlavoredType(typeof(TECEquipment), 0);

        public static TableField EquipmentID = new TableField("EquipmentID", "TEXT", ObjectType.Type.GetProperty("Guid"));
        public static TableField Name = new TableField("Name", "TEXT", ObjectType.Type.GetProperty("Name"));
        public static TableField Description = new TableField("Description", "TEXT", ObjectType.Type.GetProperty("Description"));

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            EquipmentID
            };
        public static new List<FlavoredType> Types = new List<FlavoredType>()
        {
            ObjectType
        };

    }
    public class SubScopeTable : TableBase
    {
        public static new string TableName = "TECSubScope";
        public static FlavoredType ObjectType = new FlavoredType(typeof(TECSubScope), 0);

        public static TableField SubScopeID = new TableField("SubScopeID", "TEXT", ObjectType.Type.GetProperty("Guid"));
        public static TableField Name = new TableField("Name", "TEXT", ObjectType.Type.GetProperty("Name"));
        public static TableField Description = new TableField("Description", "TEXT", ObjectType.Type.GetProperty("Description"));

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            SubScopeID
            };
        public static new List<FlavoredType> Types = new List<FlavoredType>()
        {
            ObjectType
        };

    }
    public class DeviceTable : CatalogTableBase
    {
        public static new string TableName = "TECDevice";
        public static FlavoredType ObjectType = new FlavoredType(typeof(TECDevice), 0);

        public static TableField DeviceID = new TableField("DeviceID", "TEXT", ObjectType.Type.GetProperty("Guid"));
        public static TableField Name = new TableField("Name", "TEXT", ObjectType.Type.GetProperty("Name"));
        public static TableField Description = new TableField("Description", "TEXT", ObjectType.Type.GetProperty("Description"));
        public static TableField Cost = new TableField("Cost", "REAL", ObjectType.Type.GetProperty("Cost"));

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            DeviceID
            };
        public static new List<FlavoredType> Types = new List<FlavoredType>()
        {
            ObjectType
        };

    }
    public class PointTable : TableBase
    {
        public static new string TableName = "TECPoint";
        public static FlavoredType ObjectType = new FlavoredType(typeof(TECPoint), 0);

        public static TableField PointID = new TableField("PointID", "TEXT", ObjectType.Type.GetProperty("Guid"));
        public static TableField Name = new TableField("Name", "TEXT", ObjectType.Type.GetProperty("Label"));
        public static TableField Quantity = new TableField("Quantity", "INTEGER", ObjectType.Type.GetProperty("Quantity"));
        public static TableField Type = new TableField("Type", "TEXT", ObjectType.Type.GetProperty("Type"));

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            PointID
            };
        public static new List<FlavoredType> Types = new List<FlavoredType>()
        {
            ObjectType
        };

    }
    public class TagTable : CatalogTableBase
    {
        public static new string TableName = "TECTag";
        public static FlavoredType ObjectType = new FlavoredType(typeof(TECLabeled), Flavor.Tag);
        public static Flavor ObjectFlavor = Flavor.Tag;

        public static TableField TagID = new TableField("TagID", "TEXT", ObjectType.Type.GetProperty("Guid"));
        public static TableField TagString = new TableField("TagString", "TEXT", ObjectType.Type.GetProperty("Label"));

        public static new List<TableField> PrimaryKey = new List<TableField>()
        { TagID };
        public static new List<FlavoredType> Types = new List<FlavoredType>()
        {
            ObjectType
        };

    }
    public class ManufacturerTable : CatalogTableBase
    {
        public static new string TableName = "TECManufacturer";
        public static FlavoredType ObjectType = new FlavoredType(typeof(TECManufacturer), 0);

        public static TableField ManufacturerID = new TableField("ManufacturerID", "TEXT", ObjectType.Type.GetProperty("Guid"));
        public static TableField Name = new TableField("Name", "TEXT", ObjectType.Type.GetProperty("Label"));
        public static TableField Multiplier = new TableField("Multiplier", "REAL", ObjectType.Type.GetProperty("Multiplier"));

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            ManufacturerID
            };
        public static new List<FlavoredType> Types = new List<FlavoredType>()
        {
            ObjectType
        };

    }
    public class LocationTable : CatalogTableBase
    {
        public static new string TableName = "TECLocation";
        public static FlavoredType ObjectType = new FlavoredType(typeof(TECLabeled), Flavor.Location);
        public static Flavor ObjectFlavor = Flavor.Location;

        public static TableField LocationID = new TableField("LocationID", "TEXT", ObjectType.Type.GetProperty("Guid"));
        public static TableField Name = new TableField("Name", "TEXT", ObjectType.Type.GetProperty("Label"));

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            LocationID
            };
        public static new List<FlavoredType> Types = new List<FlavoredType>()
        {
            ObjectType
        };

    }
    public class ConnectionTypeTable : CatalogTableBase
    {
        public static new string TableName = "TECConnectiontype";
        public static FlavoredType ObjectType = new FlavoredType(typeof(TECElectricalMaterial).BaseType, Flavor.Wire);
        public static Flavor ObjectFlavor = Flavor.Wire;

        public static TableField ConnectionTypeID = new TableField("ConnectionTypeID", "TEXT", ObjectType.Type.GetProperty("Guid"));
        public static TableField Name = new TableField("Name", "TEXT", ObjectType.Type.GetProperty("Label"));
        public static TableField Cost = new TableField("Cost", "REAL", ObjectType.Type.GetProperty("Cost"));
        public static TableField Labor = new TableField("Labor", "REAL", ObjectType.Type.GetProperty("Labor"));

        public static new List<TableField> PrimaryKey = new List<TableField>()
        {
            ConnectionTypeID
        };
        public static new List<FlavoredType> Types = new List<FlavoredType>()
        {
            ObjectType
        };
    }
    public class ConduitTypeTable : CatalogTableBase
    {
        public static new string TableName = "TECConduitType";
        public static FlavoredType ObjectType = new FlavoredType(typeof(TECElectricalMaterial), Flavor.Conduit);
        public static Flavor ObejctFlavor = Flavor.Conduit;

        public static TableField ConduitTypeID = new TableField("ConduitTypeID", "TEXT", ObjectType.Type.GetProperty("Guid"));
        public static TableField Name = new TableField("Name", "TEXT", ObjectType.Type.GetProperty("Label"));
        public static TableField Cost = new TableField("Cost", "REAL", ObjectType.Type.GetProperty("Cost"));
        public static TableField Labor = new TableField("Labor", "REAL", ObjectType.Type.GetProperty("Labor"));

        public static new List<TableField> PrimaryKey = new List<TableField>()
        {
            ConduitTypeID
        };
        public static new List<FlavoredType> Types = new List<FlavoredType>()
        {
            ObjectType
        };
    }
    public class AssociatedCostTable : CatalogTableBase
    {
        public static new string TableName = "TECAssociatedCost";
        public static FlavoredType ObjectType = new FlavoredType(typeof(TECCost), 0);

        public static TableField AssociatedCostID = new TableField("AssociatedCostID", "TEXT", ObjectType.Type.GetProperty("Guid"));
        public static TableField Name = new TableField("Name", "TEXT", ObjectType.Type.GetProperty("Name"));
        public static TableField Cost = new TableField("Cost", "REAL", ObjectType.Type.GetProperty("Cost"));
        public static TableField Labor = new TableField("Labor", "REAL", ObjectType.Type.GetProperty("Labor"));
        public static TableField Type = new TableField("Type", "TEXT", ObjectType.Type.GetProperty("Type"));

        public static new List<TableField> PrimaryKey = new List<TableField>()
        {
            AssociatedCostID
        };
        public static new List<FlavoredType> Types = new List<FlavoredType>()
        {
            ObjectType
        };
    }
    public class SubScopeConnectionTable : TableBase
    {
        public static new string TableName = "TECSubScopeConnection";
        public static FlavoredType ObjectType = new FlavoredType(typeof(TECSubScopeConnection), 0);

        public static TableField ConnectionID = new TableField("ConnectionID", "TEXT", ObjectType.Type.GetProperty("Guid"));
        public static TableField Length = new TableField("Length", "REAL", ObjectType.Type.GetProperty("Length"));
        public static TableField ConduitLength = new TableField("ConduitLength", "REAL", ObjectType.Type.GetProperty("ConduitLength"));

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            ConnectionID
            };
        public static new List<FlavoredType> Types = new List<FlavoredType>()
        {
            ObjectType
        };
    }
    public class NetworkConnectionTable : TableBase
    {
        public static new string TableName = "TECNetworkConnection";
        public static FlavoredType ObjectType = new FlavoredType(typeof(TECNetworkConnection), 0);

        public static TableField ConnectionID = new TableField("ConnectionID", "TEXT", ObjectType.Type.GetProperty("Guid"));
        public static TableField Length = new TableField("Length", "REAL", ObjectType.Type.GetProperty("Length"));
        public static TableField ConduitLength = new TableField("ConduitLength", "REAL", ObjectType.Type.GetProperty("ConduitLength"));
        public static TableField IOType = new TableField("IOType", "TEXT", ObjectType.Type.GetProperty("IOType"));

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            ConnectionID
            };
        public static new List<FlavoredType> Types = new List<FlavoredType>()
        {
            ObjectType
        };
    }
    public class ControllerTable : TableBase
    {
        public static new string TableName = "TECController";
        public static FlavoredType ObjectType = new FlavoredType(typeof(TECController), 0);

        public static TableField ControllerID = new TableField("ControllerID", "TEXT", ObjectType.Type.GetProperty("Guid"));
        public static TableField Name = new TableField("Name", "TEXT", ObjectType.Type.GetProperty("Name"));
        public static TableField Description = new TableField("Description", "TEXT", ObjectType.Type.GetProperty("Description"));
        public static TableField Type = new TableField("Type", "TEXT", ObjectType.Type.GetProperty("NetworkType"));

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            ControllerID
            };
        public static new List<FlavoredType> Types = new List<FlavoredType>()
        {
            ObjectType
        };
    }
    public class MiscTable : TableBase
    {
        public static new string TableName = "TECMisc";
        public static FlavoredType ObjectType = new FlavoredType(typeof(TECMisc), 0);

        public static TableField MiscID = new TableField("MiscID", "TEXT", ObjectType.Type.GetProperty("Guid"));
        public static TableField Name = new TableField("Name", "TEXT", ObjectType.Type.GetProperty("Name"));
        public static TableField Cost = new TableField("Cost", "REAL", ObjectType.Type.GetProperty("Cost"));
        public static TableField Labor = new TableField("Labor", "REAL", ObjectType.Type.GetProperty("Labor"));
        public static TableField Quantity = new TableField("Quantity", "INTEGER", ObjectType.Type.GetProperty("Quantity"));
        public static TableField Type = new TableField("Type", "TEXT", ObjectType.Type.GetProperty("Type"));

        public static new List<TableField> PrimaryKey = new List<TableField>()
        {
            MiscID
        };

        public static new List<FlavoredType> Types = new List<FlavoredType>()
        {
            ObjectType
        };
    }
    public class PanelTypeTable : CatalogTableBase
    {
        public static new string TableName = "TECPanelType";
        public static FlavoredType ObjectType = new FlavoredType(typeof(TECPanelType), 0);

        public static TableField PanelTypeID = new TableField("PanelTypeID", "TEXT", ObjectType.Type.GetProperty("Guid"));
        public static TableField Name = new TableField("Name", "TEXT", ObjectType.Type.GetProperty("Name"));
        public static TableField Cost = new TableField("Cost", "REAL", ObjectType.Type.GetProperty("Cost"));
        public static TableField Labor = new TableField("Labor", "REAL", ObjectType.Type.GetProperty("Labor"));

        public static new List<TableField> PrimaryKey = new List<TableField>()
        {
            PanelTypeID
        };

        public static new List<FlavoredType> Types = new List<FlavoredType>()
        {
            ObjectType
        };
    }
    public class PanelTable : TableBase
    {
        public static new string TableName = "TECPanel";
        public static FlavoredType ObjectType = new FlavoredType(typeof(TECPanel), 0);

        public static TableField PanelID = new TableField("PanelID", "TEXT", ObjectType.Type.GetProperty("Guid"));
        public static TableField Name = new TableField("Name", "TEXT", ObjectType.Type.GetProperty("Name"));
        public static TableField Description = new TableField("Description", "TEXT", ObjectType.Type.GetProperty("Description"));
        public static TableField Quantity = new TableField("Quantity", "INTEGER", ObjectType.Type.GetProperty("Quantity"));

        public static new List<TableField> PrimaryKey = new List<TableField>()
        {
            PanelID
        };

        public static new List<FlavoredType> Types = new List<FlavoredType>()
        {
            ObjectType
        };
    }
    public class IOModuleTable : CatalogTableBase
    {
        public static new string TableName = "TECIOModule";
        public static FlavoredType IOModuleType = new FlavoredType(typeof(TECIOModule), 0);

        public static TableField IOModuleID = new TableField("IOModuleID", "TEXT", IOModuleType.Type.GetProperty("Guid"));
        public static TableField Name = new TableField("Name", "TEXT", IOModuleType.Type.GetProperty("Name"));
        public static TableField Description = new TableField("Description", "TEXT", IOModuleType.Type.GetProperty("Description"));
        public static TableField Cost = new TableField("Cost", "REAL", IOModuleType.Type.GetProperty("Cost"));
        public static TableField IOPerModule = new TableField("IOPerModule", "REAL", IOModuleType.Type.GetProperty("IOPerModule"));

        public static new List<TableField> PrimaryKey = new List<TableField>()
        {
            IOModuleID
        };

        public static new List<FlavoredType> Types = new List<FlavoredType>()
        {
            IOModuleType
        };

    }
    public class IOTable : TableBase
    {
        public static new string TableName = "TECIO";
        public static FlavoredType IOObjectType = new FlavoredType(typeof(TECIO), 0);

        public static TableField IOID = new TableField("IOID", "TEXT", IOObjectType.Type.GetProperty("Guid"));
        public static TableField IOType = new TableField("IOType", "TEXT", IOObjectType.Type.GetProperty("Type"));
        public static TableField Quantity = new TableField("Quantity", "INTEGER", IOObjectType.Type.GetProperty("Quantity"));

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            IOID
        };
        public static new List<FlavoredType> Types = new List<FlavoredType>()
        {
            IOObjectType
        };
    }
    public class ControllerTypeTable : TableBase
    {
        public static new string TableName = "TECControllerType";
        public static FlavoredType ObjectType = new FlavoredType(typeof(TECControllerType), 0);

        public static TableField TypeID = new TableField("ControllerID", "TEXT", ObjectType.Type.GetProperty("Guid"));
        public static TableField Name = new TableField("Name", "TEXT", ObjectType.Type.GetProperty("Name"));
        public static TableField Description = new TableField("Description", "TEXT", ObjectType.Type.GetProperty("Description"));
        public static TableField Cost = new TableField("Cost", "REAL", ObjectType.Type.GetProperty("Cost"));
        public static TableField Labor = new TableField("Labor", "REAL", ObjectType.Type.GetProperty("Labor"));
        public static TableField Type = new TableField("Type", "TEXT", ObjectType.Type.GetProperty("Type"));
        

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            TypeID
            };
        public static new List<FlavoredType> Types = new List<FlavoredType>()
        {
            ObjectType
        };
    }
    #endregion

    #region Relationship Tables
    public class BidLaborTable : TableBase
    {
        public static new string TableName = "TECBidTECLabor";
        public static FlavoredType BidType = new FlavoredType(typeof(TECBid), 0);
        public static FlavoredType LaborType = new FlavoredType(typeof(TECLabor), 0);

        public static TableField BidID = new TableField("BidID", "TEXT", BidType.Type.GetProperty("Guid"));
        public static TableField LaborID = new TableField("LaborID", "TEXT", LaborType.Type.GetProperty("Guid"));

        public static new List<TableField> PrimaryKey = new List<TableField>()
        {
            BidID,
            LaborID
        };

        public static new List<FlavoredType> Types = new List<FlavoredType>()
        {
            BidType,
            LaborType
        };
    }
    public class BidBidParametersTable : TableBase
    {
        public static new string TableName = "TECBidTECParameters";
        public static FlavoredType BidType = new FlavoredType(typeof(TECBid), 0);
        public static FlavoredType ParameterType = new FlavoredType(typeof(TECBidParameters), 0);

        public static TableField BidID = new TableField("BidID", "TEXT", BidType.Type.GetProperty("Guid"));
        public static TableField ParametersID = new TableField("ParametersID", "TEXT", ParameterType.Type.GetProperty("Guid"));

        public static new List<TableField> PrimaryKey = new List<TableField>()
        {
            BidID,
            ParametersID
        };

        public static new List<FlavoredType> Types = new List<FlavoredType>()
        {
            BidType,
            ParameterType
        };
    }
    public class BidScopeBranchTable : TableBase
    {
        public static new string TableName = "TECBidTECScopeBranch";
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
    public class BidMiscTable : TableBase
    {
        public static new string TableName = "TECBidTECMisc";
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
    public class ControllerTypeIOTable : TableBase
    {
        public static new string TableName = "TECControllerTypeTECIO";
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
    public class IOModuleManufacturerTable : TableBase
    {
        public static new string TableName = "TECIOModuleTECManufacturer";
        public static FlavoredType ObjectType = new FlavoredType(typeof(TECIOModule), 0);
        public static FlavoredType ReferenceType = new FlavoredType(typeof(TECManufacturer), 0);

        public static TableField IOModuleID = new TableField("IOModuleID", "TEXT", ObjectType.Type.GetProperty("Guid"));
        public static TableField ManufacturerID = new TableField("ManufacturerID", "TEXT", ReferenceType.Type.GetProperty("Guid"));

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            IOModuleID
            };
        public static new List<FlavoredType> Types = new List<FlavoredType>()
        {
            ObjectType,
            ReferenceType
        };
    }
    public class IOIOModuleTable : TableBase
    {
        public static new string TableName = "TECIOModuleTECIO";
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
    public class ControllerConnectionTable : TableBase
    {
        public static new string TableName = "TECControllerTECConection";
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
    public class ScopeBranchHierarchyTable : TableBase
    {
        public static new string TableName = "TECScopeBranchHierarchy";
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
    public class BidSystemTable : IndexedRelationTableBase
    {
        public static new string TableName = "TECBidTECSystem";
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
    public class SystemEquipmentTable : IndexedRelationTableBase
    {
        public static new string TableName = "TECSystemTECEquipment";
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
    public class EquipmentSubScopeTable : IndexedRelationTableBase
    {
        public static new string TableName = "TECEquipmentTECSubScope";
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
    public class SubScopeDeviceTable : IndexedRelationTableBase
    {
        public static new string TableName = "TECSubScopeTECDevice";
        public static FlavoredType ObjectType = new FlavoredType(typeof(TECSubScope), 0);
        public static FlavoredType ReferenceType = new FlavoredType(typeof(TECDevice), 0);

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
    public class SubScopePointTable : IndexedRelationTableBase
    {
        public static new string TableName = "TECSubScopeTECPoint";
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
    public class ScopeTagTable : TableBase
    {
        public static new string TableName = "TECScopeTECTag";
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
    public class DeviceManufacturerTable : TableBase
    {
        public static new string TableName = "TECDeviceTECManufacturer";
        public static FlavoredType ObjectType = new FlavoredType(typeof(TECDevice), 0);
        public static FlavoredType ReferenceType = new FlavoredType(typeof(TECManufacturer), 0);

        public static TableField DeviceID = new TableField("DeviceID", "TEXT", ObjectType.Type.GetProperty("Guid"));
        public static TableField ManufacturerID = new TableField("ManufacturerID", "TEXT", ReferenceType.Type.GetProperty("Guid"));

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            DeviceID
            };
        public static new List<FlavoredType> Types = new List<FlavoredType>()
        {
            ObjectType,
            ReferenceType
        };

    }
    public class DeviceConnectionTypeTable : IndexedRelationTableBase
    {
        public static new string TableName = "TECDeviceTECConnectionType";
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
    public class LocationScopeTable : TableBase
    {
        public static new string TableName = "TECLocationTECScope";
        public static FlavoredType ObjectType = new FlavoredType(typeof(TECScope), 0);
        public static FlavoredType ReferenceType = new FlavoredType(typeof(TECLabeled), 0);
        public static Flavor ReferenceFlavor = Flavor.Location;

        public static TableField LocationID = new TableField("LocationID", "TEXT", ReferenceType.Type.GetProperty("Guid"));
        public static TableField ScopeID = new TableField("ScopeID", "TEXT", ObjectType.Type.GetProperty("Guid"));

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
    public class ScopeAssociatedCostTable : IndexedRelationTableBase
    {
        public static new string TableName = "TECScopeTECAssociatedCost";
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
    public class ElectricalMaterialRatedCostTable : IndexedRelationTableBase
    {
        public static new string TableName = "ElectricalMaterialRatedCost";
        public static FlavoredType ObjectType = new FlavoredType(typeof(ElectricalMaterialComponent), 0);
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
    public class ControllerTypeManufacturerTable : TableBase
    {
        public static new string TableName = "TECControllerTECManufacturer";
        public static FlavoredType ObjectType = new FlavoredType(typeof(TECControllerType), 0);
        public static FlavoredType ReferenceType = new FlavoredType(typeof(TECManufacturer), 0);

        public static TableField ControllerID = new TableField("ControllerID", "TEXT", ObjectType.Type.GetProperty("Guid"));
        public static TableField ManufacturerID = new TableField("ManufacturerID", "TEXT", ReferenceType.Type.GetProperty("Guid"));

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            ControllerID
            };
        public static new List<FlavoredType> Types = new List<FlavoredType>()
        {
            ObjectType,
            ReferenceType
        };
    }
    public class ControllerControllerTypeTable : TableBase
    {
        public static new string TableName = "TECControllerTECControllerType";
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
    public class ConnectionConduitTypeTable : TableBase
    {
        public static new string TableName = "TECConnectionTECConduitType";
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
    public class NetworkConnectionConnectionTypeTable : TableBase
    {
        public static new string TableName = "TECNetworkConnectionTECConnectionType";
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
    public class NetworkConnectionControllerTable : TableBase
    {
        public static new string TableName = "TECNetworkConnectionTECController";
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
    public class SubScopeConnectionChildrenTable : TableBase
    {
        public static new string TableName = "TECSubScopeConnectionChild";
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
    public class PanelPanelTypeTable : TableBase
    {
        public static new string TableName = "TECPanelTECPanelType";
        public static FlavoredType ObjectType = new FlavoredType(typeof(TECPanel), 0);
        public static FlavoredType ReferenceType = new FlavoredType(typeof(TECPanelType), 0);

        public static TableField PanelID = new TableField("PanelID", "TEXT", ObjectType.Type.GetProperty("Guid"));
        public static TableField PanelTypeID = new TableField("PaneltypeID", "TEXT", ReferenceType.Type.GetProperty("Guid"));

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
    public class PanelControllerTable : TableBase
    {
        public static new string TableName = "TECPanelTECController";
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
    public class SystemControllerTable : TableBase
    {
        public static new string TableName = "TECSystemTECController";
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
    public class SystemPanelTable : TableBase
    {
        public static new string TableName = "TECSystemTECPanel";
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
    public class SystemScopeBranchTable : TableBase
    {
        public static new string TableName = "TECSystemTECScopeBranch";
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
    public class SystemHierarchyTable : TableBase
    {
        public static new string TableName = "TECSystemHierarchy";
        public static FlavoredType SystemType = new FlavoredType(typeof(TECSystem), 0);

        public static TableField ParentID = new TableField("SystemParentID", "TEXT", SystemType.Type.GetProperty("Guid"));
        public static TableField ChildID = new TableField("SystemChildID", "TEXT", SystemType.Type.GetProperty("Guid"));

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
    public class SystemMiscTable : TableBase
    {
        public static new string TableName = "TECSystemTECMisc";
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

    public class CharacteristicScopeInstanceScopeTable : TableBase
    {
        public static new string TableName = "CharacteristicScopeInstanceScope";
        public static FlavoredType ScopeType = new FlavoredType(typeof(TECScope), 0);

        public static TableField CharacteristicID = new TableField("CharacteristicID", "TEXT", ScopeType.Type.GetProperty("Guid"));
        public static TableField InstanceID = new TableField("InstanceID", "TEXT", ScopeType.Type.GetProperty("Guid"));

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            CharacteristicID,
            InstanceID
            };
        public static new List<FlavoredType> Types = new List<FlavoredType>()
        {
            ScopeType,
            ScopeType
        };
    }
    #endregion

    public static class AllBidTables
    {
        public static List<object> Tables = new List<object>() {
            new BidInfoTable(),
            new LaborConstantsTable(),
            new UserAdjustmentsTable(),
            new SubcontractorConstantsTable(),
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
            new BidParametersTable(),
            new MiscTable(),
            new PanelTable(),
            new PanelTypeTable(),
            new SubScopeConnectionTable(),
            new NetworkConnectionTable(),
            new IOTable(),
            new IOModuleTable(),

            new BidLaborTable(),
            new ConnectionTypeTable(),
            new ConduitTypeTable(),
            new ScopeBranchHierarchyTable(),
            new BidSystemTable(),
            new SystemEquipmentTable(),
            new EquipmentSubScopeTable(),
            new SubScopeDeviceTable(),
            new SubScopePointTable(),
            new ScopeTagTable(),
            new DeviceManufacturerTable(),
            new LocationScopeTable(),
            new ControllerTable(),
            new AssociatedCostTable(),
            new ElectricalMaterialRatedCostTable(),
            new ControllerConnectionTable(),
            new ControllerTypeIOTable(),
            new IOIOModuleTable(),
            new BidScopeBranchTable(),
            new DeviceConnectionTypeTable(),
            new ScopeAssociatedCostTable(),
            new ControllerTypeManufacturerTable(),
            new ControllerControllerTypeTable(),
            new ConnectionConduitTypeTable(),
            new SystemControllerTable(),
            new SystemPanelTable(),
            new SystemScopeBranchTable(),
            new SystemHierarchyTable(),
            new SystemMiscTable(),
            new CharacteristicScopeInstanceScopeTable(),
            new BidBidParametersTable(),
            new BidMiscTable(),
            new PanelPanelTypeTable(),
            new PanelControllerTable(),
            new SubScopeConnectionChildrenTable(),
            new NetworkConnectionControllerTable(),
            new NetworkConnectionConnectionTypeTable(),
            new IOModuleManufacturerTable()
            };
    }

    public static class AllTemplateTables
    {
        public static List<object> Tables = new List<object>()
        {
            new TemplatesInfoTable(),
            new LaborConstantsTable(),
            new SubcontractorConstantsTable(),
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
            new DeviceManufacturerTable(),
            new DeviceConnectionTypeTable(),
            new ScopeAssociatedCostTable(),
            new ElectricalMaterialRatedCostTable(),
            new ControllerTypeManufacturerTable(),
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
            new IOModuleManufacturerTable(),
            new ScopeBranchHierarchyTable()
        };
    }

    public static class AllTables
    {
        public static List<object> Tables = new List<object>()
        {
            new BidInfoTable(),
            new TemplatesInfoTable(),
            new LaborConstantsTable(),
            new SubcontractorConstantsTable(),
            new UserAdjustmentsTable(),
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
            new BidParametersTable(),
            new MiscTable(),
            new PanelTypeTable(),
            new PanelTable(),
            new SubScopeConnectionTable(),
            new NetworkConnectionTable(),
            new IOModuleTable(),
            new IOTable(),
            
            new BidLaborTable(),
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
            new DeviceManufacturerTable(),
            new DeviceConnectionTypeTable(),
            new LocationScopeTable(),
            new ScopeAssociatedCostTable(),
            new ElectricalMaterialRatedCostTable(),
            new ControllerTypeManufacturerTable(),
            new ControllerControllerTypeTable(),
            new ConnectionConduitTypeTable(),
            new BidBidParametersTable(),
            new PanelPanelTypeTable(),
            new PanelControllerTable(),
            new SystemControllerTable(),
            new SystemPanelTable(),
            new SystemHierarchyTable(),
            new SystemScopeBranchTable(),
            new SystemMiscTable(),
            new CharacteristicScopeInstanceScopeTable(),
            new SubScopeConnectionChildrenTable(),
            new NetworkConnectionControllerTable(),
            new NetworkConnectionConnectionTypeTable(),
            new IOModuleManufacturerTable(),
            new BidMiscTable()
        };
    }

    public class TableBase
    {
        public static string TableName;
        public static List<FlavoredType> Types;

        public static List<TableField> PrimaryKey;
    }

    public class IndexedRelationTableBase : TableBase { }

    public class CatalogTableBase : TableBase { }

    public class TableField
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

    public class HelperProperties
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

    public class TableInfo
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
            bool isIndexedRelationTable = false;
            if (tableType.BaseType == typeof(IndexedRelationTableBase))
            {
                isIndexedRelationTable = true;
            }

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

            Name = tableName;
            Fields = fields;
            PrimaryFields = primaryKey;
            Types = types;
            IsRelationTable = isIndexedRelationTable;
            IsCatalogTable = isCatalogTable;
        }

    }

    public struct FlavoredType
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
