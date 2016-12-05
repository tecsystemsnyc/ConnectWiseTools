﻿using System;
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
        public static TableField BidNumber = new TableField("BidNumber", "TEXT");
        public static TableField DueDate = new TableField("DueDate", "TEXT");
        public static TableField SalesPerson = new TableField("Salesperson", "TEXT");
        public static TableField Estimator = new TableField("Estimator", "TEXT");

        public static TableField PMCoef = new TableField("PMCoef", "REAL");
        public static TableField ENGCoef = new TableField("ENGCoef", "REAL");
        public static TableField CommCoef = new TableField("CommCoef", "REAL");
        public static TableField SoftCoef = new TableField("SoftCoef", "REAL");
        public static TableField GraphCoef = new TableField("GraphCoef", "REAL");

        public static TableField ElectricalRate = new TableField("ElectricalRate", "REAL");

    }
    public class NoteTable : TableBase
    {
        public static new string TableName = "TECNote";

        public static TableField NoteID = new TableField("NoteID", "TEXT");
        public static TableField NoteText = new TableField("NoteText", "TEXT");
    }
    public class ExclusionTable : TableBase
    {
        public static new string TableName = "TECExclusion";

        public static TableField ExclusionID = new TableField("ExclusionID", "TEXT");
        public static TableField ExclusionText = new TableField("ExclusionText", "TEXT");

    }
    public class ScopeBranchTable : TableBase
    {
        public static new string TableName = "TECScopeBranch";

        public static TableField ScopeBranchID = new TableField("ScopeBranchID", "TEXT");
        public static TableField Name = new TableField("Name", "TEXT");
        public static TableField Description = new TableField("Description", "TEXT");

    }
    public class SystemTable : TableBase
    {
        public static new string TableName = "TECSystem";

        public static TableField SystemID = new TableField("SystemID", "TEXT");
        public static TableField Name = new TableField("Name", "TEXT");
        public static TableField Description = new TableField("Description", "TEXT");
        public static TableField Quantity = new TableField("Quantity", "INTEGER");
        public static TableField BudgetPrice = new TableField("BudgetPrice", "REAL");

    }
    public class EquipmentTable : TableBase
    {
        public static new string TableName = "TECEquipment";

        public static TableField EquipmentID = new TableField("EquipmentID", "TEXT");
        public static TableField Name = new TableField("Name", "TEXT");
        public static TableField Description = new TableField("Description", "TEXT");
        public static TableField Quantity = new TableField("Quantity", "INTEGER");
        public static TableField BudgetPrice = new TableField("BudgetPrice", "REAL");

    }
    public class SubScopeTable : TableBase
    {
        public static new string TableName = "TECSubScope";

        public static TableField SubScopeID = new TableField("SubScopeID", "TEXT");
        public static TableField Name = new TableField("Name", "TEXT");
        public static TableField Description = new TableField("Description", "TEXT");
        public static TableField Quantity = new TableField("Quantity", "INTEGER");

    }
    public class DeviceTable : TableBase
    {
        public static new string TableName = "TECDevice";

        public static TableField DeviceID = new TableField("DeviceID", "TEXT");
        public static TableField Name = new TableField("Name", "TEXT");
        public static TableField Description = new TableField("Description", "TEXT");
        public static TableField Cost = new TableField("Cost", "REAL");
        public static TableField Wire = new TableField("Wire", "TEXT");

    }
    public class PointTable : TableBase
    {
        public static new string TableName = "TECPoint";

        public static TableField PointID = new TableField("PointID", "TEXT");
        public static TableField Name = new TableField("Name", "TEXT");
        public static TableField Description = new TableField("Description", "TEXT");
        public static TableField Quantity = new TableField("Quantity", "INTEGER");
        public static TableField Type = new TableField("Type", "TEXT");

    }
    public class TagTable : TableBase
    {
        public static new string TableName = "TECTag";

        public static TableField TagID = new TableField("TagID", "TEXT");
        public static TableField TagString = new TableField("TagString", "TEXT");

    }
    public class ManufacturerTable : TableBase
    {
        public static new string TableName = "TECManufacturer";

        public static TableField ManufacturerID = new TableField("ManufacturerID", "TEXT");
        public static TableField Name = new TableField("Name", "TEXT");
        public static TableField Multiplier = new TableField("Multiplier", "REAL");

    }
    public class DrawingTable : TableBase
    {
        public static new string TableName = "TECDrawing";

        public static TableField DrawingID = new TableField("DrawingID", "TEXT");
        public static TableField Name = new TableField("Name", "TEXT");
        public static TableField Description = new TableField("Description", "TEXT");

    }
    public class PageTable : TableBase
    {
        public static new string TableName = "TECPage";

        public static TableField PageID = new TableField("PageID", "TEXT");
        public static TableField Image = new TableField("Image", "TEXT");
        public static TableField PageNum = new TableField("PageNum", "INTEGER");

    }
    public class LocationTable : TableBase
    {
        public static new string TableName = "TECLocation";

        public static TableField LocationID = new TableField("LocationID", "TEXT");
        public static TableField Name = new TableField("Name", "TEXT");

    }
    public class VisualScopeTable : TableBase
    {
        public static new string TableName = "TECVisualScope";

        public static TableField VisualScopeID = new TableField("VisualScopeID", "TEXT");
        public static TableField XPos = new TableField("XPos", "REAL");
        public static TableField YPos = new TableField("YPos", "REAL");

    }

    public class ScopeBranchHierarchyTable : TableBase
    {
        public static new string TableName = "TECScopeBranchHierarchy";

        public static TableField ParentID = new TableField("ParentID", "TEXT");
        public static TableField ChildID = new TableField("ChildID", "TEXT");

    }
    public class SystemEquipmentTable : TableBase
    {
        public static new string TableName = "TECSystemTECEquipment";

        public static TableField SystemID = new TableField("SystemID", "TEXT");
        public static TableField EquipmentID = new TableField("EquipmentID", "TEXT");

    }
    public class EquipmentSubScopeTable : TableBase
    {
        public static new string TableName = "TECEquipmentTECSubScope";

        public static TableField EquipmentID = new TableField("EquipmentID", "TEXT");
        public static TableField SubScopeID = new TableField("SubScopeID", "TEXT");

    }
    public class SubScopeDeviceTable : TableBase
    {
        public static new string TableName = "TECSubScopeTECDevice";

        public static TableField SubScopeID = new TableField("SubScopeID", "TEXT");
        public static TableField DeviceID = new TableField("DeviceID", "TEXT");
        public static TableField Quantity = new TableField("Quantity", "INTEGER");

    }
    public class SubScopePointTable : TableBase
    {
        public static new string TableName = "TECSubScopeTECPoint";

        public static TableField SubScopeID = new TableField("SubScopeID", "TEXT");
        public static TableField PointID = new TableField("PointID", "TEXT");

    }
    public class ScopeTagTable : TableBase
    {
        public static new string TableName = "TECScopeTECTag";

        public static TableField ScopeID = new TableField("ScopeID", "TEXT");
        public static TableField TagID = new TableField("TagID", "TEXT");

    }
    public class DeviceManufacturerTable : TableBase
    {
        public static new string TableName = "TECDeviceTECManufacturer";

        public static TableField DeviceID = new TableField("DeviceID", "TEXT");
        public static TableField ManufacturerID = new TableField("ManufacturerID", "TEXT");

    }
    public class DrawingPageTable : TableBase
    {
        public static new string TableName = "TECDrawingTECPage";

        public static TableField DrawingID = new TableField("DrawingID", "TEXT");
        public static TableField PageID = new TableField("PageID", "TEXT");

    }
    public class PageVisualScopeTable : TableBase
    {
        public static new string TableName = "TECPageTECVisualScope";

        public static TableField PageID = new TableField("PageID", "TEXT");
        public static TableField VisualScopeID = new TableField("VisualScopeID", "TEXT");

    }
    public class VisualScopeScopeTable : TableBase
    {
        public static new string TableName = "TECVisualScopeTECScope";

        public static TableField VisualScopeID = new TableField("VisualScopeID", "TEXT");
        public static TableField ScopeID = new TableField("ScopeID", "TEXT");

    }
    public class LocationScopeTable : TableBase
    {
        public static new string TableName = "TECLocationTECScope";

        public static TableField LocationID = new TableField("LocationID", "TEXT");
        public static TableField ScopeID = new TableField("ScopeID", "TEXT");

    }

    public static class AllTables
    {
        public static List<object> Tables = new List<object>() {
            new BidInfoTable(),
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
            new ScopeBranchHierarchyTable(),
            new SystemEquipmentTable(),
            new EquipmentSubScopeTable(),
            new SubScopeDeviceTable(),
            new SubScopePointTable(),
            new ScopeTagTable(),
            new DeviceManufacturerTable(),
            new DrawingPageTable(),
            new PageVisualScopeTable(),
            new VisualScopeScopeTable(),
            new LocationScopeTable()
            };
    }

    public class TableBase
    {
        public static string TableName;
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