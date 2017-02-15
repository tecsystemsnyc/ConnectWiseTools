using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingUtilitiesLibrary
{
    #region Table String Definitions
    public class BidInfoTable : TableBase
    {
        public static new string TableName = "TECBidInfo";

        public static TableField DBVersion = new TableField("DBVersion", "TEXT");
        public static TableField BidName = new TableField("BidName", "TEXT");
        public static TableField BidInfoID = new TableField("BidInfoID", "TEXT");
        public static TableField BidNumber = new TableField("BidNumber", "TEXT");
        public static TableField DueDate = new TableField("DueDate", "TEXT");
        public static TableField Salesperson = new TableField("Salesperson", "TEXT");
        public static TableField Estimator = new TableField("Estimator", "TEXT");

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            BidInfoID
            };

    }

    public class TemplatesInfoTable : TableBase
    {
        public static new string TableName = "TECTemplatesInfo";

        public static TableField TemplatesInfoID = new TableField("TemplatesInfoID", "TEXT");
        public static TableField DBVersion = new TableField("DBVersion", "TEXT");

        public static new List<TableField> PrimaryKey = new List<TableField>()
        {
            TemplatesInfoID
        };
    }

    public class LaborConstantsTable : TableBase
    {
        public static new string TableName = "TECLaborConst";

        public static TableField PMCoef = new TableField("PMCoef", "REAL");
        public static TableField PMRate = new TableField("PMRate", "REAL");

        public static TableField ENGCoef = new TableField("ENGCoef", "REAL");
        public static TableField ENGRate = new TableField("ENGRate", "REAL");

        public static TableField CommCoef = new TableField("CommCoef", "REAL");
        public static TableField CommRate = new TableField("CommRate", "REAL");

        public static TableField SoftCoef = new TableField("SoftCoef", "REAL");
        public static TableField SoftRate = new TableField("SoftRate", "REAL");

        public static TableField GraphCoef = new TableField("GraphCoef", "REAL");
        public static TableField GraphRate = new TableField("GraphRate", "REAL");
    }

    public class SubcontractorConstantsTable : TableBase
    {
        public static new string TableName = "TECSubcontractorConst";

        public static TableField ElectricalRate = new TableField("ElectricalRate", "REAL");
        public static TableField ElectricalSuperRate = new TableField("ElectricalSuperRate", "REAL");
    }

    public class UserAdjustmentsTable : TableBase
    {
        public static new string TableName = "TECUserAdjustments";

        public static TableField PMExtraHours = new TableField("PMExtraHours", "REAL");
        public static TableField ENGExtraHours = new TableField("ENGExtraHours", "REAL");
        public static TableField CommExtraHours = new TableField("CommExtraHours", "REAL");
        public static TableField SoftExtraHours = new TableField("SoftExtraHours", "REAL");
        public static TableField GraphExtraHours = new TableField("GraphExtraHours", "REAL");

    }

    public class CostAdditionsTable : TableBase
    {
        public static new string TableName = "TECCostAdditions";

        public static TableField Name = new TableField("Name", "TEXT");
        public static TableField Cost = new TableField("Cost", "REAL");

        public static new List<TableField> PrimaryKey = new List<TableField>()
        {
            Name
        };
    }

    public class NoteTable : TableBase
    {
        public static new string TableName = "TECNote";

        public static TableField NoteID = new TableField("NoteID", "TEXT");
        public static TableField NoteText = new TableField("NoteText", "TEXT");

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            NoteID
            };
    }
    public class ExclusionTable : TableBase
    {
        public static new string TableName = "TECExclusion";

        public static TableField ExclusionID = new TableField("ExclusionID", "TEXT");
        public static TableField ExclusionText = new TableField("ExclusionText", "TEXT");

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            ExclusionID
            };

    }

    public class BidScopeBranchTable : TableBase
    {
        public static new string TableName = "TECBidTECScopeBranch";

        public static TableField BidID = new TableField("BidID", "TEXT");
        public static TableField ScopeBranchID = new TableField("ScopeBranchID", "TEXT");

        public static new List<TableField> PrimaryKey = new List<TableField>()
        {
            BidID,
            ScopeBranchID
        };
    }

    public class ScopeBranchTable : TableBase
    {
        public static new string TableName = "TECScopeBranch";

        public static TableField ScopeBranchID = new TableField("ScopeBranchID", "TEXT");
        public static TableField Name = new TableField("Name", "TEXT");
        public static TableField Description = new TableField("Description", "TEXT");

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            ScopeBranchID
            };

    }
    public class SystemTable : TableBase
    {
        public static new string TableName = "TECSystem";

        public static TableField SystemID = new TableField("SystemID", "TEXT");
        public static TableField Name = new TableField("Name", "TEXT");
        public static TableField Description = new TableField("Description", "TEXT");
        public static TableField Quantity = new TableField("Quantity", "INTEGER");
        public static TableField BudgetPrice = new TableField("BudgetPrice", "REAL");

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            SystemID
            };

    }
    public class EquipmentTable : TableBase
    {
        public static new string TableName = "TECEquipment";

        public static TableField EquipmentID = new TableField("EquipmentID", "TEXT");
        public static TableField Name = new TableField("Name", "TEXT");
        public static TableField Description = new TableField("Description", "TEXT");
        public static TableField Quantity = new TableField("Quantity", "INTEGER");
        public static TableField BudgetPrice = new TableField("BudgetPrice", "REAL");

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            EquipmentID
            };

    }
    public class SubScopeTable : TableBase
    {
        public static new string TableName = "TECSubScope";

        public static TableField SubScopeID = new TableField("SubScopeID", "TEXT");
        public static TableField Name = new TableField("Name", "TEXT");
        public static TableField Description = new TableField("Description", "TEXT");
        public static TableField Quantity = new TableField("Quantity", "INTEGER");
        public static TableField Length = new TableField("Length", "REAL");

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            SubScopeID
            };

    }
    public class DeviceTable : TableBase
    {
        public static new string TableName = "TECDevice";

        public static TableField DeviceID = new TableField("DeviceID", "TEXT");
        public static TableField Name = new TableField("Name", "TEXT");
        public static TableField Description = new TableField("Description", "TEXT");
        public static TableField Cost = new TableField("Cost", "REAL");
        public static TableField ConnectionType = new TableField("ConnectionType", "TEXT");

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            DeviceID
            };

    }
    public class PointTable : TableBase
    {
        public static new string TableName = "TECPoint";

        public static TableField PointID = new TableField("PointID", "TEXT");
        public static TableField Name = new TableField("Name", "TEXT");
        public static TableField Description = new TableField("Description", "TEXT");
        public static TableField Quantity = new TableField("Quantity", "INTEGER");
        public static TableField Type = new TableField("Type", "TEXT");

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            PointID
            };

    }
    public class TagTable : TableBase
    {
        public static new string TableName = "TECTag";

        public static TableField TagID = new TableField("TagID", "TEXT");
        public static TableField TagString = new TableField("TagString", "TEXT");

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            TagID
            };

    }
    public class ManufacturerTable : TableBase
    {
        public static new string TableName = "TECManufacturer";

        public static TableField ManufacturerID = new TableField("ManufacturerID", "TEXT");
        public static TableField Name = new TableField("Name", "TEXT");
        public static TableField Multiplier = new TableField("Multiplier", "REAL");

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            ManufacturerID
            };

    }
    public class DrawingTable : TableBase
    {
        public static new string TableName = "TECDrawing";

        public static TableField DrawingID = new TableField("DrawingID", "TEXT");
        public static TableField Name = new TableField("Name", "TEXT");
        public static TableField Description = new TableField("Description", "TEXT");

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            DrawingID
            };

    }
    public class PageTable : TableBase
    {
        public static new string TableName = "TECPage";

        public static TableField PageID = new TableField("PageID", "TEXT");
        public static TableField Image = new TableField("Image", "BLOB");
        public static TableField PageNum = new TableField("PageNum", "INTEGER");

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            PageID
            };

    }
    public class LocationTable : TableBase
    {
        public static new string TableName = "TECLocation";

        public static TableField LocationID = new TableField("LocationID", "TEXT");
        public static TableField Name = new TableField("Name", "TEXT");

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            LocationID
            };

    }
    public class VisualScopeTable : TableBase
    {
        public static new string TableName = "TECVisualScope";

        public static TableField VisualScopeID = new TableField("VisualScopeID", "TEXT");
        public static TableField XPos = new TableField("XPos", "REAL");
        public static TableField YPos = new TableField("YPos", "REAL");

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            VisualScopeID
            };

    }
    public class ProposalScopeTable : TableBase
    {
        public static new string TableName = "TECProposalScope";

        public static TableField ProposalScopeID = new TableField("ProposalScopeID", "TEXT");
        public static TableField IsProposed = new TableField("IsProposed", "INTEGER");

        public static new List<TableField> PrimaryKey = new List<TableField>()
        {
            ProposalScopeID
        };
    }
    public class ConnectionTypeTable : TableBase
    {
        public static new string TableName = "TECConnectionType";

        public static TableField ConnectionTypeID = new TableField("ConnectionTypeID", "TEXT");
        public static TableField Name = new TableField("Name", "TEXT");
        public static TableField Cost = new TableField("Cost", "REAL");
        public static TableField Labor = new TableField("Labor", "REAL");

        public static new List<TableField> PrimaryKey = new List<TableField>()
        {
            ConnectionTypeID
        };
    }
    public class ConduitTypeTable : TableBase
    {
        public static new string TableName = "TECConduitType";

        public static TableField ConduitTypeID = new TableField("ConduitTypeID", "TEXT");
        public static TableField Name = new TableField("Name", "TEXT");
        public static TableField Cost = new TableField("Cost", "REAL");
        public static TableField Labor = new TableField("Labor", "REAL");

        public static new List<TableField> PrimaryKey = new List<TableField>()
        {
            ConduitTypeID
        };
    }
    public class AssociatedCostTable : TableBase
    {
        public static new string TableName = "TECAssociatedCost";

        public static TableField AssociatedCostID = new TableField("AssociatedCostID", "TEXT");
        public static TableField Name = new TableField("Name", "TEXT");
        public static TableField Cost = new TableField("Cost", "REAL");

        public static new List<TableField> PrimaryKey = new List<TableField>()
        {
            AssociatedCostID
        };
    }

    public class ProposalScopeScopeBranchTable : TableBase
    {
        public static new string TableName = "TECProposalScopeTECScopeBranch";

        public static TableField ProposalScopeID = new TableField("ProposalScopeID", "TEXT");
        public static TableField ScopeBranchID = new TableField("ScopeBranchID", "TEXT");

        public static new List<TableField> PrimaryKey = new List<TableField>()
        {
            ScopeBranchID
        };
    }

    public class ConnectionTable : TableBase
    {
        public static new string TableName = "TECConnection";

        public static TableField ConnectionID = new TableField("ConnectionID", "TEXT");
        public static TableField Length = new TableField("Length", "REAL");

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            ConnectionID
            };
    }
    public class ControllerTable : TableBase
    {
        public static new string TableName = "TECController";

        public static TableField ControllerID = new TableField("ControllerID", "TEXT");
        public static TableField Name = new TableField("Name", "TEXT");
        public static TableField Description = new TableField("Description", "TEXT");
        public static TableField Cost = new TableField("Cost", "REAL");

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            ControllerID
            };
    }
    public class ControllerIOTypeTable : TableBase
    {
        public static new string TableName = "TECControllerTECIO";

        public static TableField ControllerID = new TableField("ControllerID", "TEXT");
        public static TableField IOType = new TableField("IOType", "TEXT");
        public static TableField Quantity = new TableField("Quantity", "INTEGER");

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            ControllerID,
            IOType
            };
    }
    public class ControllerConnectionTable : TableBase
    {
        public static new string TableName = "TECControllerTECConection";

        public static TableField ControllerID = new TableField("ControllerID", "TEXT");
        public static TableField ConnectionID = new TableField("ConnectionID", "TEXT");

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            ControllerID,
            ConnectionID
            };
    }
    public class ScopeConnectionTable : TableBase
    {
        public static new string TableName = "TECScopeTECConnection";

        public static TableField ScopeID = new TableField("ScopeID", "TEXT");
        public static TableField ConnectionID = new TableField("ConnectionID", "TEXT");

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            ScopeID,
            ConnectionID
            };
    }
    public class ScopeBranchHierarchyTable : TableBase
    {
        public static new string TableName = "TECScopeBranchHierarchy";

        public static TableField ParentID = new TableField("ParentID", "TEXT");
        public static TableField ChildID = new TableField("ChildID", "TEXT");

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            ParentID,
            ChildID
            };

    }
    public class SystemIndexTable : TableBase
    {
        public static new string TableName = "TECSystemIndex";

        public static TableField SystemID = new TableField("SystemID", "TEXT");
        public static TableField Index = new TableField("ScopeIndex", "INTEGER");

        public static new List<TableField> PrimaryKey = new List<TableField>()
        {
            SystemID
        };
    }
    public class SystemEquipmentTable : TableBase
    {
        public static new string TableName = "TECSystemTECEquipment";

        public static TableField SystemID = new TableField("SystemID", "TEXT");
        public static TableField EquipmentID = new TableField("EquipmentID", "TEXT");
        public static TableField ScopeIndex = new TableField("ScopeIndex", "INTEGER");

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            SystemID,
            EquipmentID
            };

    }
    public class EquipmentSubScopeTable : TableBase
    {
        public static new string TableName = "TECEquipmentTECSubScope";

        public static TableField EquipmentID = new TableField("EquipmentID", "TEXT");
        public static TableField SubScopeID = new TableField("SubScopeID", "TEXT");
        public static TableField ScopeIndex = new TableField("ScopeIndex", "INTEGER");

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            EquipmentID,
            SubScopeID
            };

    }
    public class SubScopeDeviceTable : TableBase
    {
        public static new string TableName = "TECSubScopeTECDevice";

        public static TableField SubScopeID = new TableField("SubScopeID", "TEXT");
        public static TableField DeviceID = new TableField("DeviceID", "TEXT");
        public static TableField Quantity = new TableField("Quantity", "INTEGER");
        public static TableField ScopeIndex = new TableField("ScopeIndex", "INTEGER");

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            SubScopeID,
            DeviceID
            };

    }
    public class SubScopePointTable : TableBase
    {
        public static new string TableName = "TECSubScopeTECPoint";

        public static TableField SubScopeID = new TableField("SubScopeID", "TEXT");
        public static TableField PointID = new TableField("PointID", "TEXT");
        public static TableField ScopeIndex = new TableField("ScopeIndex", "INTEGER");

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            SubScopeID,
            PointID
            };

    }
    public class ScopeTagTable : TableBase
    {
        public static new string TableName = "TECScopeTECTag";

        public static TableField ScopeID = new TableField("ScopeID", "TEXT");
        public static TableField TagID = new TableField("TagID", "TEXT");

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            ScopeID,
            TagID
            };
    }
    public class DeviceManufacturerTable : TableBase
    {
        public static new string TableName = "TECDeviceTECManufacturer";

        public static TableField DeviceID = new TableField("DeviceID", "TEXT");
        public static TableField ManufacturerID = new TableField("ManufacturerID", "TEXT");

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            DeviceID
            };

    }
    public class DeviceConnectionTypeTable : TableBase
    {
        public static new string TableName = "TECDeviceTECConnectionType";

        public static TableField DeviceID = new TableField("DeviceID", "TEXT");
        public static TableField TypeID = new TableField("ConnectionTypeID", "TEXT");

        public static new List<TableField> PrimaryKey = new List<TableField>()
        {

        };
    }
    public class DrawingPageTable : TableBase
    {
        public static new string TableName = "TECDrawingTECPage";

        public static TableField DrawingID = new TableField("DrawingID", "TEXT");
        public static TableField PageID = new TableField("PageID", "TEXT");

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            DrawingID,
            PageID
            };

    }
    public class PageVisualScopeTable : TableBase
    {
        public static new string TableName = "TECPageTECVisualScope";

        public static TableField PageID = new TableField("PageID", "TEXT");
        public static TableField VisualScopeID = new TableField("VisualScopeID", "TEXT");

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            PageID,
            VisualScopeID
            };

    }
    public class VisualScopeScopeTable : TableBase
    {
        public static new string TableName = "TECVisualScopeTECScope";

        public static TableField VisualScopeID = new TableField("VisualScopeID", "TEXT");
        public static TableField ScopeID = new TableField("ScopeID", "TEXT");

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            VisualScopeID,
            ScopeID
            };

    }
    public class LocationScopeTable : TableBase
    {
        public static new string TableName = "TECLocationTECScope";

        public static TableField LocationID = new TableField("LocationID", "TEXT");
        public static TableField ScopeID = new TableField("ScopeID", "TEXT");

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            LocationID,
            ScopeID
            };

    }
    public class ScopeAssociatedCostTable : TableBase
    {
        public static new string TableName = "TECScopeTECAssociatedCost";

        public static TableField ScopeID = new TableField("ScopeID", "TEXT");
        public static TableField AssociatedCostID = new TableField("AssociatedCostID", "TEXT");
        public static TableField Quantity = new TableField("Quantity", "INTEGER");

        public static new List<TableField> PrimaryKey = new List<TableField>()
        {
            ScopeID,
            AssociatedCostID
        };
    }

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
            new DrawingTable(),
            new PageTable(),
            new LocationTable(),
            new VisualScopeTable(),
            new ConnectionTypeTable(),
            new ConduitTypeTable(),
            new ScopeBranchHierarchyTable(),
            new SystemIndexTable(),
            new SystemEquipmentTable(),
            new EquipmentSubScopeTable(),
            new SubScopeDeviceTable(),
            new SubScopePointTable(),
            new ScopeTagTable(),
            new DeviceManufacturerTable(),
            new DrawingPageTable(),
            new PageVisualScopeTable(),
            new VisualScopeScopeTable(),
            new LocationScopeTable(),
            new ConnectionTable(),
            new ControllerTable(),
            new AssociatedCostTable(),
            new ControllerConnectionTable(),
            new ControllerIOTypeTable(),
            new ScopeConnectionTable(),
            new ProposalScopeTable(),
            new ProposalScopeScopeBranchTable(),
            new BidScopeBranchTable(),
            new DeviceConnectionTypeTable(),
            new ScopeAssociatedCostTable()
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
            new ConnectionTypeTable(),
            new ConduitTypeTable(),
            new AssociatedCostTable(),
            new SystemEquipmentTable(),
            new EquipmentSubScopeTable(),
            new SubScopeDeviceTable(),
            new SubScopePointTable(),
            new ScopeTagTable(),
            new ControllerTable(),
            new ControllerIOTypeTable(),
            new ScopeConnectionTable(),
            new DeviceManufacturerTable(),
            new SystemIndexTable(),
            new DeviceConnectionTypeTable(),
            new ScopeAssociatedCostTable()
        };
    }

    public class TableBase
    {
        public static string TableName;

        public static List<TableField> PrimaryKey;

    }

    public class TableField
    {
        public string Name;
        public string FieldType;

        public TableField(string name, string fieldType)
        {
            Name = name;
            FieldType = fieldType;
        }
    }
    
    #endregion
}
