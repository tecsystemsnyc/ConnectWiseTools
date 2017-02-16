using EstimatingLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingUtilitiesLibrary
{
    #region Table Definitions
    public class BidInfoTable : TableBase
    {
        public static new string TableName = "TECBidInfo";
        public static new Type ModelType = typeof(TECBid);

        public static TableField DBVersion = new TableField("DBVersion", "TEXT", null);
        public static TableField BidName = new TableField("BidName", "TEXT", ModelType.GetProperty("Name"));
        public static TableField BidInfoID = new TableField("BidInfoID", "TEXT", ModelType.GetProperty("InfoGuid"));
        public static TableField BidNumber = new TableField("BidNumber", "TEXT", ModelType.GetProperty("BidNumber"));
        public static TableField DueDate = new TableField("DueDate", "TEXT", ModelType.GetProperty("DueDateString"));
        public static TableField Salesperson = new TableField("Salesperson", "TEXT", ModelType.GetProperty("Salesperson"));
        public static TableField Estimator = new TableField("Estimator", "TEXT", ModelType.GetProperty("Estimator"));

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            BidInfoID
            };

    }
    public class TemplatesInfoTable : TableBase
    {
        public static new string TableName = "TECTemplatesInfo";
        public static new Type ModelType = typeof(TECTemplates);

        public static TableField TemplatesInfoID = new TableField("TemplatesInfoID", "TEXT", ModelType.GetProperty("InfoGuid"));
        public static TableField DBVersion = new TableField("DBVersion", "TEXT", null);

        public static new List<TableField> PrimaryKey = new List<TableField>()
        {
            TemplatesInfoID
        };
    }
    public class LaborConstantsTable : TableBase
    {
        public static new string TableName = "TECLaborConst";
        public static new Type ModelType = typeof(TECLabor);

        public static TableField PMCoef = new TableField("PMCoef", "REAL", ModelType.GetProperty("PMCoef"));
        public static TableField PMRate = new TableField("PMRate", "REAL", ModelType.GetProperty("PMRate"));

        public static TableField ENGCoef = new TableField("ENGCoef", "REAL", ModelType.GetProperty("ENGCoef"));
        public static TableField ENGRate = new TableField("ENGRate", "REAL", ModelType.GetProperty("ENGRate"));

        public static TableField CommCoef = new TableField("CommCoef", "REAL", ModelType.GetProperty("CommCoef"));
        public static TableField CommRate = new TableField("CommRate", "REAL", ModelType.GetProperty("CommRate"));

        public static TableField SoftCoef = new TableField("SoftCoef", "REAL", ModelType.GetProperty("SoftCoef"));
        public static TableField SoftRate = new TableField("SoftRate", "REAL", ModelType.GetProperty("SoftRate"));

        public static TableField GraphCoef = new TableField("GraphCoef", "REAL", ModelType.GetProperty("GraphCoef"));
        public static TableField GraphRate = new TableField("GraphRate", "REAL", ModelType.GetProperty("GraphRate"));
    }
    public class SubcontractorConstantsTable : TableBase
    {
        public static new string TableName = "TECSubcontractorConst";
        public static new Type ModelType = typeof(TECLabor);

        public static TableField ElectricalRate = new TableField("ElectricalRate", "REAL", ModelType.GetProperty("ElectricalRate"));
        public static TableField ElectricalSuperRate = new TableField("ElectricalSuperRate", "REAL", ModelType.GetProperty("ElectricalSuperRate"));
    }
    public class UserAdjustmentsTable : TableBase
    {
        public static new string TableName = "TECUserAdjustments";
        public static new Type ModelType = typeof(TECLabor);

        public static TableField PMExtraHours = new TableField("PMExtraHours", "REAL", null);
        public static TableField ENGExtraHours = new TableField("ENGExtraHours", "REAL", null);
        public static TableField CommExtraHours = new TableField("CommExtraHours", "REAL", null);
        public static TableField SoftExtraHours = new TableField("SoftExtraHours", "REAL", null);
        public static TableField GraphExtraHours = new TableField("GraphExtraHours", "REAL", null);

    }
    public class CostAdditionsTable : TableBase
    {
        public static new string TableName = "TECCostAdditions";
        public static new Type ModelType = null;

        public static TableField Name = new TableField("Name", "TEXT", null);
        public static TableField Cost = new TableField("Cost", "REAL", null);

        public static new List<TableField> PrimaryKey = new List<TableField>()
        {
            Name
        };
    }
    public class NoteTable : TableBase
    {
        public static new string TableName = "TECNote";
        public static new Type ModelType = typeof(TECNote);


        public static TableField NoteID = new TableField("NoteID", "TEXT", ModelType.GetProperty("Guid"));
        public static TableField NoteText = new TableField("NoteText", "TEXT", ModelType.GetProperty("Text"));

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            NoteID
            };
    }
    public class ExclusionTable : TableBase
    {
        public static new string TableName = "TECExclusion";
        public static new Type ModelType = typeof(TECExclusion);

        public static TableField ExclusionID = new TableField("ExclusionID", "TEXT", ModelType.GetProperty("Guid"));
        public static TableField ExclusionText = new TableField("ExclusionText", "TEXT", ModelType.GetProperty("Text"));

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            ExclusionID
            };

    }
    public class BidScopeBranchTable : RelationTableBase
    {
        public static new string TableName = "TECBidTECScopeBranch";
        public static new Type ModelType = typeof(TECBid);
        public static new Type RelationType = typeof(TECScopeBranch);

        public static TableField BidID = new TableField("BidID", "TEXT", ModelType.GetProperty("InfoGuid"));
        public static TableField ScopeBranchID = new TableField("ScopeBranchID", "TEXT", RelationType.GetProperty("Guid"));

        public static new List<TableField> PrimaryKey = new List<TableField>()
        {
            BidID,
            ScopeBranchID
        };
    }
    public class ScopeBranchTable : TableBase
    {
        public static new string TableName = "TECScopeBranch";
        public static new Type ModelType = typeof(TECScopeBranch);

        public static TableField ScopeBranchID = new TableField("ScopeBranchID", "TEXT", ModelType.GetProperty("Guid"));
        public static TableField Name = new TableField("Name", "TEXT", ModelType.GetProperty("Name"));
        public static TableField Description = new TableField("Description", "TEXT", ModelType.GetProperty("Description"));

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            ScopeBranchID
            };

    }
    public class SystemTable : TableBase
    {
        public static new string TableName = "TECSystem";
        public static new Type ModelType = typeof(TECSystem);

        public static TableField SystemID = new TableField("SystemID", "TEXT", ModelType.GetProperty("Guid"));
        public static TableField Name = new TableField("Name", "TEXT", ModelType.GetProperty("Name"));
        public static TableField Description = new TableField("Description", "TEXT", ModelType.GetProperty("Description"));
        public static TableField Quantity = new TableField("Quantity", "INTEGER", ModelType.GetProperty("Quantity"));
        public static TableField BudgetPrice = new TableField("BudgetPrice", "REAL", ModelType.GetProperty("BudgetPrice"));

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            SystemID
            };

    }
    public class EquipmentTable : TableBase
    {
        public static new string TableName = "TECEquipment";
        public static new Type ModelType = typeof(TECEquipment);

        public static TableField EquipmentID = new TableField("EquipmentID", "TEXT", ModelType.GetProperty("Guid"));
        public static TableField Name = new TableField("Name", "TEXT", ModelType.GetProperty("Name"));
        public static TableField Description = new TableField("Description", "TEXT", ModelType.GetProperty("Description"));
        public static TableField Quantity = new TableField("Quantity", "INTEGER", ModelType.GetProperty("Quantity"));
        public static TableField BudgetPrice = new TableField("BudgetPrice", "REAL", ModelType.GetProperty("BudgetPrice"));

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            EquipmentID
            };

    }
    public class SubScopeTable : TableBase
    {
        public static new string TableName = "TECSubScope";
        public static new Type ModelType = typeof(TECSubScope);

        public static TableField SubScopeID = new TableField("SubScopeID", "TEXT", ModelType.GetProperty("Guid"));
        public static TableField Name = new TableField("Name", "TEXT", ModelType.GetProperty("Name"));
        public static TableField Description = new TableField("Description", "TEXT", ModelType.GetProperty("Description"));
        public static TableField Quantity = new TableField("Quantity", "INTEGER", ModelType.GetProperty("Quantity"));
        public static TableField Length = new TableField("Length", "REAL", ModelType.GetProperty("Length"));

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            SubScopeID
            };

    }
    public class DeviceTable : TableBase
    {
        public static new string TableName = "TECDevice";
        public static new Type ModelType = typeof(TECSubScope);

        public static TableField DeviceID = new TableField("DeviceID", "TEXT", ModelType.GetProperty("Guid"));
        public static TableField Name = new TableField("Name", "TEXT", ModelType.GetProperty("Name"));
        public static TableField Description = new TableField("Description", "TEXT", ModelType.GetProperty("Description"));
        public static TableField Cost = new TableField("Cost", "REAL", ModelType.GetProperty("Cost"));

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            DeviceID
            };

    }
    public class PointTable : TableBase
    {
        public static new string TableName = "TECPoint";
        public static new Type ModelType = typeof(TECPoint);

        public static TableField PointID = new TableField("PointID", "TEXT", ModelType.GetProperty("Guid"));
        public static TableField Name = new TableField("Name", "TEXT", ModelType.GetProperty("Name"));
        public static TableField Description = new TableField("Description", "TEXT", ModelType.GetProperty("Description"));
        public static TableField Quantity = new TableField("Quantity", "INTEGER", ModelType.GetProperty("Quantity"));
        public static TableField Type = new TableField("Type", "TEXT", ModelType.GetProperty("Type"));

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            PointID
            };

    }
    public class TagTable : TableBase
    {
        public static new string TableName = "TECTag";
        public static new Type ModelType = typeof(TECTag);

        public static TableField TagID = new TableField("TagID", "TEXT", ModelType.GetProperty("Guid"));
        public static TableField TagString = new TableField("TagString", "TEXT", ModelType.GetProperty("Text"));

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            TagID
            };

    }
    public class ManufacturerTable : TableBase
    {
        public static new string TableName = "TECManufacturer";
        public static new Type ModelType = typeof(TECManufacturer);

        public static TableField ManufacturerID = new TableField("ManufacturerID", "TEXT", ModelType.GetProperty("Guid"));
        public static TableField Name = new TableField("Name", "TEXT", ModelType.GetProperty("Name"));
        public static TableField Multiplier = new TableField("Multiplier", "REAL", ModelType.GetProperty("Multiplier"));

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            ManufacturerID
            };

    }
    public class DrawingTable : TableBase
    {
        public static new string TableName = "TECDrawing";
        public static new Type ModelType = typeof(TECDrawing);

        public static TableField DrawingID = new TableField("DrawingID", "TEXT", ModelType.GetProperty("Guid"));
        public static TableField Name = new TableField("Name", "TEXT", ModelType.GetProperty("Name"));
        public static TableField Description = new TableField("Description", "TEXT", ModelType.GetProperty("Description"));

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            DrawingID
            };

    }
    public class PageTable : TableBase
    {
        public static new string TableName = "TECPage";
        public static new Type ModelType = typeof(TECPage);

        public static TableField PageID = new TableField("PageID", "TEXT", ModelType.GetProperty("Guid"));
        public static TableField Image = new TableField("Image", "BLOB", null);
        public static TableField PageNum = new TableField("PageNum", "INTEGER", ModelType.GetProperty("PageNum"));

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            PageID
            };

    }
    public class LocationTable : TableBase
    {
        public static new string TableName = "TECLocation";
        public static new Type ModelType = typeof(TECLocation);

        public static TableField LocationID = new TableField("LocationID", "TEXT", ModelType.GetProperty("Guid"));
        public static TableField Name = new TableField("Name", "TEXT", ModelType.GetProperty("Name"));

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            LocationID
            };

    }
    public class VisualScopeTable : TableBase
    {
        public static new string TableName = "TECVisualScope";
        public static new Type ModelType = typeof(TECVisualScope);

        public static TableField VisualScopeID = new TableField("VisualScopeID", "TEXT", ModelType.GetProperty("Guid"));
        public static TableField XPos = new TableField("XPos", "REAL", ModelType.GetProperty("XPos"));
        public static TableField YPos = new TableField("YPos", "REAL", ModelType.GetProperty("YPos"));

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            VisualScopeID
            };

    }
    public class ProposalScopeTable : TableBase
    {
        public static new string TableName = "TECProposalScope";
        public static new Type ModelType = typeof(TECProposalScope);

        public static TableField ProposalScopeID = new TableField("ProposalScopeID", "TEXT", ModelType.GetProperty("Guid"));
        public static TableField IsProposed = new TableField("IsProposed", "INTEGER", ModelType.GetProperty("IsProposed"));

        public static new List<TableField> PrimaryKey = new List<TableField>()
        {
            ProposalScopeID
        };
    }
    public class ConnectionTypeTable : TableBase
    {
        public static new string TableName = "TECConnectionType";
        public static new Type ModelType = typeof(TECConnectionType);

        public static TableField ConnectionTypeID = new TableField("ConnectionTypeID", "TEXT", ModelType.GetProperty("Guid"));
        public static TableField Name = new TableField("Name", "TEXT", ModelType.GetProperty("Name"));
        public static TableField Cost = new TableField("Cost", "REAL", ModelType.GetProperty("Cost"));
        public static TableField Labor = new TableField("Labor", "REAL", ModelType.GetProperty("Labor"));

        public static new List<TableField> PrimaryKey = new List<TableField>()
        {
            ConnectionTypeID
        };
    }
    public class ConduitTypeTable : TableBase
    {
        public static new string TableName = "TECConduitType";
        public static new Type ModelType = typeof(TECConduitType);

        public static TableField ConduitTypeID = new TableField("ConduitTypeID", "TEXT", ModelType.GetProperty("Guid"));
        public static TableField Name = new TableField("Name", "TEXT", ModelType.GetProperty("Name"));
        public static TableField Cost = new TableField("Cost", "REAL", ModelType.GetProperty("Cost"));
        public static TableField Labor = new TableField("Labor", "REAL", ModelType.GetProperty("Labor"));

        public static new List<TableField> PrimaryKey = new List<TableField>()
        {
            ConduitTypeID
        };
    }
    public class AssociatedCostTable : TableBase
    {
        public static new string TableName = "TECAssociatedCost";
        public static new Type ModelType = typeof(TECAssociatedCost);

        public static TableField AssociatedCostID = new TableField("AssociatedCostID", "TEXT", ModelType.GetProperty("Guid"));
        public static TableField Name = new TableField("Name", "TEXT", ModelType.GetProperty("Name"));
        public static TableField Cost = new TableField("Cost", "REAL", ModelType.GetProperty("Cost"));

        public static new List<TableField> PrimaryKey = new List<TableField>()
        {
            AssociatedCostID
        };
    }
    public class ProposalScopeScopeBranchTable : RelationTableBase
    {
        public static new string TableName = "TECProposalScopeTECScopeBranch";
        public static new Type ModelType = typeof(TECProposalScope);
        public static new Type RelationType = typeof(TECScopeBranch);

        public static TableField ProposalScopeID = new TableField("ProposalScopeID", "TEXT", ModelType.GetProperty("Guid"));
        public static TableField ScopeBranchID = new TableField("ScopeBranchID", "TEXT", RelationType.GetProperty("Guid"));

        public static new List<TableField> PrimaryKey = new List<TableField>()
        {
            ScopeBranchID
        };
    }
    public class ConnectionTable : TableBase
    {
        public static new string TableName = "TECConnection";
        public static new Type ModelType = typeof(TECConnection);

        public static TableField ConnectionID = new TableField("ConnectionID", "TEXT", ModelType.GetProperty("Guid"));
        public static TableField Length = new TableField("Length", "REAL", ModelType.GetProperty("Length"));

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            ConnectionID
            };
    }
    public class ControllerTable : TableBase
    {
        public static new string TableName = "TECController";
        public static new Type ModelType = typeof(TECController);

        public static TableField ControllerID = new TableField("ControllerID", "TEXT", ModelType.GetProperty("Guid"));
        public static TableField Name = new TableField("Name", "TEXT", ModelType.GetProperty("Name"));
        public static TableField Description = new TableField("Description", "TEXT", ModelType.GetProperty("Description"));
        public static TableField Cost = new TableField("Cost", "REAL", ModelType.GetProperty("Cost"));

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            ControllerID
            };
    }
    public class ControllerIOTypeTable : RelationTableBase
    {
        public static new string TableName = "TECControllerTECIO";
        public static new Type ModelType = typeof(TECController);
        public static new Type RelationType = typeof(TECIO);

        public static TableField ControllerID = new TableField("ControllerID", "TEXT", ModelType.GetProperty("Guid"));
        public static TableField IOType = new TableField("IOType", "TEXT", RelationType.GetProperty("Type"));
        public static TableField Quantity = new TableField("Quantity", "INTEGER", ModelType.GetProperty("Quantity"));

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            ControllerID,
            IOType
            };
    }
    public class ControllerConnectionTable : RelationTableBase
    {
        public static new string TableName = "TECControllerTECConection";
        public static new Type ModelType = typeof(TECController);
        public static new Type RelationType = typeof(TECConnection);

        public static TableField ControllerID = new TableField("ControllerID", "TEXT", ModelType.GetProperty("Guid"));
        public static TableField ConnectionID = new TableField("ConnectionID", "TEXT", RelationType.GetProperty("Guid"));

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            ControllerID,
            ConnectionID
            };
    }
    public class ScopeConnectionTable : RelationTableBase
    {
        public static new string TableName = "TECScopeTECConnection";
        public static new Type ModelType = typeof(TECScope);
        public static new Type RelationType = typeof(TECConnection);

        public static TableField ScopeID = new TableField("ScopeID", "TEXT", ModelType.GetProperty("Guid"));
        public static TableField ConnectionID = new TableField("ConnectionID", "TEXT", RelationType.GetProperty("Guid"));

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            ScopeID,
            ConnectionID
            };
    }
    public class ScopeBranchHierarchyTable : RelationTableBase
    {
        public static new string TableName = "TECScopeBranchHierarchy";
        public static new Type ModelType = typeof(TECScopeBranch);
        public static new Type RelationType = typeof(TECScopeBranch);

        public static TableField ParentID = new TableField("ParentID", "TEXT", ModelType.GetProperty("Guid"));
        public static TableField ChildID = new TableField("ChildID", "TEXT", RelationType.GetProperty("Guid"));

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            ParentID,
            ChildID
            };

    }
    public class SystemIndexTable : RelationTableBase
    {
        public static new string TableName = "TECSystemIndex";
        public static new Type ModelType = typeof(TECSystem);
        public static new Type RelationType = null;

        public static TableField SystemID = new TableField("SystemID", "TEXT", ModelType.GetProperty("Guid"));
        public static TableField Index = new TableField("ScopeIndex", "INTEGER", null);

        public static new List<TableField> PrimaryKey = new List<TableField>()
        {
            SystemID
        };
    }
    public class SystemEquipmentTable : RelationTableBase
    {
        public static new string TableName = "TECSystemTECEquipment";
        public static new Type ModelType = typeof(TECSystem);
        public static new Type RelationType = typeof(TECEquipment);

        public static TableField SystemID = new TableField("SystemID", "TEXT", ModelType.GetProperty("Guid"));
        public static TableField EquipmentID = new TableField("EquipmentID", "TEXT", RelationType.GetProperty("Guid"));
        public static TableField ScopeIndex = new TableField("ScopeIndex", "INTEGER", null);

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            SystemID,
            EquipmentID
            };

    }
    public class EquipmentSubScopeTable : RelationTableBase
    {
        public static new string TableName = "TECEquipmentTECSubScope";
        public static new Type ModelType = typeof(TECEquipment);
        public static new Type RelationType = typeof(TECSubScope);

        public static TableField EquipmentID = new TableField("EquipmentID", "TEXT", ModelType.GetProperty("Guid"));
        public static TableField SubScopeID = new TableField("SubScopeID", "TEXT", RelationType.GetProperty("Guid"));
        public static TableField ScopeIndex = new TableField("ScopeIndex", "INTEGER", null);

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            EquipmentID,
            SubScopeID
            };

    }
    public class SubScopeDeviceTable : RelationTableBase
    {
        public static new string TableName = "TECSubScopeTECDevice";
        public static new Type ModelType = typeof(TECSubScope);
        public static new Type RelationType = typeof(TECDevice);

        public static TableField SubScopeID = new TableField("SubScopeID", "TEXT", ModelType.GetProperty("Guid"));
        public static TableField DeviceID = new TableField("DeviceID", "TEXT", RelationType.GetProperty("Guid"));
        public static TableField Quantity = new TableField("Quantity", "INTEGER", null);
        public static TableField ScopeIndex = new TableField("ScopeIndex", "INTEGER", null);

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            SubScopeID,
            DeviceID
            };

    }
    public class SubScopePointTable : RelationTableBase
    {
        public static new string TableName = "TECSubScopeTECPoint";
        public static new Type ModelType = typeof(TECSubScope);
        public static new Type RelationType = typeof(TECPoint);

        public static TableField SubScopeID = new TableField("SubScopeID", "TEXT", ModelType.GetProperty("Guid"));
        public static TableField PointID = new TableField("PointID", "TEXT", RelationType.GetProperty("Guid"));
        public static TableField ScopeIndex = new TableField("ScopeIndex", "INTEGER", null);

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            SubScopeID,
            PointID
            };

    }
    public class ScopeTagTable : RelationTableBase
    {
        public static new string TableName = "TECScopeTECTag";
        public static new Type ModelType = typeof(TECScope);
        public static new Type RelationType = typeof(TECTag);

        public static TableField ScopeID = new TableField("ScopeID", "TEXT", ModelType.GetProperty("Guid"));
        public static TableField TagID = new TableField("TagID", "TEXT", RelationType.GetProperty("Guid"));

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            ScopeID,
            TagID
            };
    }
    public class DeviceManufacturerTable : RelationTableBase
    {
        public static new string TableName = "TECDeviceTECManufacturer";
        public static new Type ModelType = typeof(TECDevice);
        public static new Type RelationType = typeof(TECManufacturer);

        public static TableField DeviceID = new TableField("DeviceID", "TEXT", ModelType.GetProperty("Guid"));
        public static TableField ManufacturerID = new TableField("ManufacturerID", "TEXT", RelationType.GetProperty("Guid"));

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            DeviceID
            };

    }
    public class DeviceConnectionTypeTable : RelationTableBase
    {
        public static new string TableName = "TECDeviceTECConnectionType";
        public static new Type ModelType = typeof(TECDevice);
        public static new Type RelationType = typeof(TECConnectionType);

        public static TableField DeviceID = new TableField("DeviceID", "TEXT", ModelType.GetProperty("Guid"));
        public static TableField TypeID = new TableField("ConnectionTypeID", "TEXT", RelationType.GetProperty("Guid"));

        public static new List<TableField> PrimaryKey = new List<TableField>()
        {

        };
    }
    public class DrawingPageTable : RelationTableBase
    {
        public static new string TableName = "TECDrawingTECPage";
        public static new Type ModelType = typeof(TECDrawing);
        public static new Type RelationType = typeof(TECPage);

        public static TableField DrawingID = new TableField("DrawingID", "TEXT", ModelType.GetProperty("Guid"));
        public static TableField PageID = new TableField("PageID", "TEXT", RelationType.GetProperty("Guid"));

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            DrawingID,
            PageID
            };

    }
    public class PageVisualScopeTable : RelationTableBase
    {
        public static new string TableName = "TECPageTECVisualScope";
        public static new Type ModelType = typeof(TECPage);
        public static new Type RelationType = typeof(TECVisualScope);

        public static TableField PageID = new TableField("PageID", "TEXT", ModelType.GetProperty("Guid"));
        public static TableField VisualScopeID = new TableField("VisualScopeID", "TEXT", RelationType.GetProperty("Guid"));

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            PageID,
            VisualScopeID
            };

    }
    public class VisualScopeScopeTable : RelationTableBase
    {
        public static new string TableName = "TECVisualScopeTECScope";
        public static new Type ModelType = typeof(TECVisualScope);
        public static new Type RelationType = typeof(TECScope);

        public static TableField VisualScopeID = new TableField("VisualScopeID", "TEXT", ModelType.GetProperty("Guid"));
        public static TableField ScopeID = new TableField("ScopeID", "TEXT", RelationType.GetProperty("Guid"));

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            VisualScopeID,
            ScopeID
            };

    }
    public class LocationScopeTable : RelationTableBase
    {
        public static new string TableName = "TECLocationTECScope";
        public static new Type ModelType = typeof(TECScope);
        public static new Type RelationType = typeof(TECLocation);

        public static TableField LocationID = new TableField("LocationID", "TEXT", RelationType.GetProperty("Guid"));
        public static TableField ScopeID = new TableField("ScopeID", "TEXT", ModelType.GetProperty("Guid"));

        public static new List<TableField> PrimaryKey = new List<TableField>() {
            LocationID,
            ScopeID
            };

    }
    public class ScopeAssociatedCostTable : RelationTableBase
    {
        public static new string TableName = "TECScopeTECAssociatedCost";
        public static new Type ModelType = typeof(TECLocation);
        public static new Type RelationType = typeof(TECScope);

        public static TableField ScopeID = new TableField("ScopeID", "TEXT", ModelType.GetProperty("Guid"));
        public static TableField AssociatedCostID = new TableField("AssociatedCostID", "TEXT", RelationType.GetProperty("Guid"));
        public static TableField Quantity = new TableField("Quantity", "INTEGER", null);

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

    public static class AllTables
    {
        public static List<object> Tables = new List<object>()
        {
            new BidInfoTable(),
            new TemplatesInfoTable(),
            new LaborConstantsTable(),
            new SubcontractorConstantsTable(),
            new UserAdjustmentsTable(),
            new CostAdditionsTable(),
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
            new DrawingTable(),
            new PageTable(),
            new LocationTable(),
            new VisualScopeTable(),
            new ProposalScopeTable(),
            new ConnectionTypeTable(),
            new ConduitTypeTable(),
            new AssociatedCostTable(),
            new ProposalScopeScopeBranchTable(),
            new ConnectionTable(),
            new ControllerTable(),
            new ControllerIOTypeTable(),
            new ControllerConnectionTable(),
            new ScopeConnectionTable(),
            new ScopeBranchHierarchyTable(),
            new SystemIndexTable(),
            new SystemEquipmentTable(),
            new EquipmentSubScopeTable(),
            new SubScopeDeviceTable(),
            new SubScopePointTable(),
            new ScopeTagTable(),
            new DeviceManufacturerTable(),
            new DeviceConnectionTypeTable(),
            new DrawingPageTable(),
            new PageVisualScopeTable(),
            new VisualScopeScopeTable(),
            new LocationScopeTable(),
            new ScopeAssociatedCostTable()
        };
    }

    public class TableBase
    {
        public static string TableName;
        public static Type ModelType;

        public static List<TableField> PrimaryKey;

    }

    public class RelationTableBase : TableBase
    {
        public static Type RelationType;
    }

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
    
    #endregion
}
