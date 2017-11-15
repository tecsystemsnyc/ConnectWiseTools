using EstimatingLibrary;
using EstimatingLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace EstimatingUtilitiesLibrary.Database
{
    #region Table Definitions
    #region Object Tables
    internal class MetadataTable : TableBase
    {
        public static string TableName = "Metadata";
        public static Type HelperType = typeof(HelperProperties);

        public static TableField Version = new TableField("Version", "TEXT", HelperType.GetProperty("DBVersion"));

        private List<TableField> primaryKeys = new List<TableField>() {
            Version
        };
        private List<TableField> fields = new List<TableField>() {
            Version
        };

        public override List<TableField> PrimaryKeys
        {
            get { return primaryKeys; }
        }
        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return new List<Type>(); } }
        public override List<string> PropertyNames { get { return new List<string>(); } }
        public override List<TableField> Fields { get { return fields; } }
    }

    internal class BidInfoTable : TableBase
    {
        #region static
        public static string TableName = "BidInfo";
        public static Type ObjectType = typeof(TECBid);

        public static TableField Name = new TableField("Name", "TEXT", ObjectType.GetProperty("Name"));
        public static TableField ID = new TableField("ID", "TEXT", ObjectType.GetProperty("Guid"));
        public static TableField Number = new TableField("Number", "TEXT", ObjectType.GetProperty("BidNumber"));
        public static TableField DueDate = new TableField("DueDate", "TEXT", ObjectType.GetProperty("DueDate"));
        public static TableField Salesperson = new TableField("Salesperson", "TEXT", ObjectType.GetProperty("Salesperson"));
        public static TableField Estimator = new TableField("Estimator", "TEXT", ObjectType.GetProperty("Estimator"));
        #endregion

        private List<TableField> fields = new List<TableField>()
        {
            Name,
            ID,
            Number,
            DueDate,
            Salesperson,
            Estimator
        };
        private List<TableField> primaryKeys = new List<TableField>()
        {
            ID
        };
        private List<Type> types = new List<Type>() {
            ObjectType
        };
        private List<string> propertyNames = new List<string>();

        public override string NameString { get { return TableName; } }
        public override List<TableField> Fields { get { return fields; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }

    }
    internal class TemplatesInfoTable : TableBase
    {
        public static string TableName = "TemplatesInfo";
        public static Type ObjectType = typeof(TECTemplates);

        public static TableField ID = new TableField("ID", "TEXT", ObjectType.GetProperty("Guid"));

        private List<TableField> primaryKeys = new List<TableField>()
        {
            ID
        };
        private List<TableField> fields = new List<TableField>()
        {
            ID
        };
        private List<Type> types = new List<Type>()
        {
            ObjectType
        };
        private List<string> propertyNames = new List<string>();

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    internal class ParametersTable : TableBase
    {
        public static string TableName = "Parameters";
        public static Type ParameterType = typeof(TECParameters);

        public static TableField ID = new TableField("ID", "TEXT", ParameterType.GetProperty("Guid"));
        public static TableField Label = new TableField("Label", "TEXT", ParameterType.GetProperty("Label"));

        public static TableField Escalation = new TableField("Escalation", "REAL", ParameterType.GetProperty("Escalation"));
        public static TableField Overhead = new TableField("Overhead", "REAL", ParameterType.GetProperty("Overhead"));
        public static TableField Profit = new TableField("Profit", "REAL", ParameterType.GetProperty("Profit"));
        public static TableField SubcontractorMarkup = new TableField("SubcontractorMarkup", "REAL", ParameterType.GetProperty("SubcontractorMarkup"));
        public static TableField SubcontractorEscalation = new TableField("SubcontractorEscalation", "REAL", ParameterType.GetProperty("SubcontractorEscalation"));
        public static TableField Warranty = new TableField("Warranty", "REAL", ParameterType.GetProperty("Warranty"));
        public static TableField Shipping = new TableField("Shipping", "REAL", ParameterType.GetProperty("Shipping"));
        public static TableField Tax = new TableField("Tax", "REAL", ParameterType.GetProperty("Tax"));
        public static TableField SubcontractorWarranty = new TableField("SubcontractorWarranty", "REAL", ParameterType.GetProperty("SubcontractorWarranty"));
        public static TableField SubcontractorShipping = new TableField("SubcontractorShipping", "REAL", ParameterType.GetProperty("SubcontractorShipping"));
        public static TableField BondRate = new TableField("BondRate", "REAL", ParameterType.GetProperty("BondRate"));

        public static TableField IsTaxExempt = new TableField("IsTaxExempt", "INTEGER", ParameterType.GetProperty("IsTaxExempt"));
        public static TableField RequiresBond = new TableField("RequiresBond", "INTEGER", ParameterType.GetProperty("RequiresBond"));
        public static TableField RequiresWrapUp = new TableField("RequiresWrapUp", "INTEGER", ParameterType.GetProperty("RequiresWrapUp"));

        #region Labor
        public static TableField DesiredConfidence = new TableField("DesiredConfidence", "TEXT", ParameterType.GetProperty("DesiredConfidence"), defaultValue: "NinetyFive");

        public static TableField PMCoefStdError = new TableField("PMCoefStdError", "REAL", ParameterType.GetProperty("PMCoefStdError"));
        public static TableField PMCoef = new TableField("PMCoef", "REAL", ParameterType.GetProperty("PMCoef"));
        public static TableField PMRate = new TableField("PMRate", "REAL", ParameterType.GetProperty("PMRate"));

        public static TableField ENGCoefStdError = new TableField("ENGCoefStdError", "REAL", ParameterType.GetProperty("ENGCoefStdError"));
        public static TableField ENGCoef = new TableField("ENGCoef", "REAL", ParameterType.GetProperty("ENGCoef"));
        public static TableField ENGRate = new TableField("ENGRate", "REAL", ParameterType.GetProperty("ENGRate"));

        public static TableField CommCoefStdError = new TableField("CommCoefStdError", "REAL", ParameterType.GetProperty("CommCoefStdError"));
        public static TableField CommCoef = new TableField("CommCoef", "REAL", ParameterType.GetProperty("CommCoef"));
        public static TableField CommRate = new TableField("CommRate", "REAL", ParameterType.GetProperty("CommRate"));

        public static TableField SoftCoefStdError = new TableField("SoftCoefStdError", "REAL", ParameterType.GetProperty("SoftCoefStdError"));
        public static TableField SoftCoef = new TableField("SoftCoef", "REAL", ParameterType.GetProperty("SoftCoef"));
        public static TableField SoftRate = new TableField("SoftRate", "REAL", ParameterType.GetProperty("SoftRate"));

        public static TableField GraphCoefStdError = new TableField("GraphCoefStdError", "REAL", ParameterType.GetProperty("GraphCoefStdError"));
        public static TableField GraphCoef = new TableField("GraphCoef", "REAL", ParameterType.GetProperty("GraphCoef"));
        public static TableField GraphRate = new TableField("GraphRate", "REAL", ParameterType.GetProperty("GraphRate"));
        #endregion

        #region SubContractor
        public static TableField ElectricalRate = new TableField("ElectricalRate", "REAL", ParameterType.GetProperty("ElectricalRate"));
        public static TableField ElectricalSuperRate = new TableField("ElectricalSuperRate", "REAL", ParameterType.GetProperty("ElectricalSuperRate"));
        public static TableField ElectricalNonUnionRate = new TableField("ElectricalNonUnionRate", "REAL", ParameterType.GetProperty("ElectricalNonUnionRate"));
        public static TableField ElectricalSuperNonUnionRate = new TableField("ElectricalSuperNonUnionRate", "REAL", ParameterType.GetProperty("ElectricalSuperNonUnionRate"));
        public static TableField ElectricalSuperRatio = new TableField("ElectricalSuperRatio", "REAL", ParameterType.GetProperty("ElectricalSuperRatio"));

        public static TableField ElectricalIsOnOvertime = new TableField("ElectricalIsOnOvertime", "INTEGER", ParameterType.GetProperty("ElectricalIsOnOvertime"));
        public static TableField ElectricalIsUnion = new TableField("ElectricalIsUnion", "INTEGER", ParameterType.GetProperty("ElectricalIsUnion"));
        #endregion

        private List<TableField> primaryKeys = new List<TableField>()
        {
            ID
        };
        private List<Type> types = new List<Type>()
        {
            ParameterType
        };
        private List<TableField> fields = new List<TableField>()
        {
            Label,
            ID,
            Escalation,
            Overhead,
            Profit,
            SubcontractorMarkup,
            SubcontractorEscalation,
            Warranty,
            Shipping,
            Tax,
            SubcontractorWarranty,
            SubcontractorShipping,
            BondRate,
            IsTaxExempt,
            RequiresBond,
            RequiresWrapUp,
            DesiredConfidence,
            PMCoef,
            PMCoefStdError,
            PMRate,
            ENGCoef,
            ENGCoefStdError,
            ENGRate,
            CommCoef,
            CommCoefStdError,
            CommRate,
            SoftCoef,
            SoftCoefStdError,
            SoftRate,
            GraphCoef,
            GraphCoefStdError,
            GraphRate,
            ElectricalRate,
            ElectricalSuperRate,
            ElectricalNonUnionRate,
            ElectricalSuperNonUnionRate,
            ElectricalSuperRatio,
            ElectricalIsOnOvertime,
            ElectricalIsUnion

        };
        private List<string> propertyNames = new List<string>()
        {
            "Parameters"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    internal class ExtraLaborTable : TableBase
    {
        public static string TableName = "ExtraLabor";
        public static Type ObjectType =  typeof(TECExtraLabor);

        public static TableField ID = new TableField("BidID", "TEXT", ObjectType.GetProperty("Guid"));

        public static TableField PMExtraHours = new TableField("PMExtraHours", "REAL", ObjectType.GetProperty("PMExtraHours"));
        public static TableField ENGExtraHours = new TableField("ENGExtraHours", "REAL", ObjectType.GetProperty("ENGExtraHours"));
        public static TableField CommExtraHours = new TableField("CommExtraHours", "REAL", ObjectType.GetProperty("CommExtraHours"));
        public static TableField SoftExtraHours = new TableField("SoftExtraHours", "REAL", ObjectType.GetProperty("SoftExtraHours"));
        public static TableField GraphExtraHours = new TableField("GraphExtraHours", "REAL", ObjectType.GetProperty("GraphExtraHours"));

        private List<TableField> primaryKeys = new List<TableField>()
        {
            ID
        };

        private List<Type> types = new List<Type>()
        {
            ObjectType
        };

        private List<TableField> fields = new List<TableField>()
        {
            ID,
            PMExtraHours,
            ENGExtraHours,
            CommExtraHours,
            SoftExtraHours,
            GraphExtraHours
        };
        private List<string> propertyNames = new List<string>()
        {
            "ExtraLabor"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }

    }
    internal class NoteTable : TableBase
    {
        public static string TableName = "Note";
        public static Type ObjectType = typeof(TECLabeled);
        
        public static TableField ID = new TableField("ID", "TEXT", ObjectType.GetProperty("Guid"));
        public static TableField NoteText = new TableField("Label", "TEXT", ObjectType.GetProperty("Label"));

        private List<TableField> primaryKeys = new List<TableField>() {
            ID
            };

        private List<Type> types = new List<Type>()
        {
            ObjectType
        };
        private List<TableField> fields = new List<TableField>()
        {
            ID,
            NoteText
        };
        private List<string> propertyNames = new List<string>()
        {
            "Notes"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    internal class ExclusionTable : TableBase
    {
        public static string TableName = "Exclusion";
        public static Type ObjectType = typeof(TECLabeled);

        public static TableField ID = new TableField("ID", "TEXT", ObjectType.GetProperty("Guid"));
        public static TableField ExclusionText = new TableField("Label", "TEXT", ObjectType.GetProperty("Label"));

        private List<TableField> primaryKeys = new List<TableField>() {
            ID
            };

        private List<Type> types = new List<Type>()
        {
            ObjectType
        };
        private List<TableField> fields = new List<TableField>()
        {
            ID,
            ExclusionText
        };
        private List<string> propertyNames = new List<string>()
        {
            "Exclusions"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }

    }
    internal class ScopeBranchTable : TableBase
    {
        public static string TableName = "ScopeBranch";
        public static Type ObjectType = typeof(TECScopeBranch);

        public static TableField ID = new TableField("ID", "TEXT", ObjectType.GetProperty("Guid"));
        public static TableField Label = new TableField("Label", "TEXT", ObjectType.GetProperty("Label"));

        private List<TableField> primaryKeys = new List<TableField>() {
            ID
            };
        private List<Type> types = new List<Type>()
        {
            ObjectType
        };
        private List<TableField> fields = new List<TableField>()
        {
            ID,
            Label
        };
        private List<string> propertyNames = new List<string>()
        {
            "Branches",
            "ScopeBranches",
            "ScopeTree"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }

    }
    internal class SystemTable : TableBase
    {
        public static string TableName = "System";
        public static Type ObjectType = typeof(TECSystem);

        public static TableField ID = new TableField("ID", "TEXT", ObjectType.GetProperty("Guid"));
        public static TableField Name = new TableField("Name", "TEXT", ObjectType.GetProperty("Name"));
        public static TableField Description = new TableField("Description", "TEXT", ObjectType.GetProperty("Description"));
        public static TableField ProposeEquipment = new TableField("ProposeEquipment", "INTEGER", ObjectType.GetProperty("ProposeEquipment"));

        private List<TableField> primaryKeys = new List<TableField>() {
            ID
            };
        private List<Type> types = new List<Type>()
        {
            ObjectType
        };
        private List<TableField> fields = new List<TableField>()
        {
            ID,
            Name,
            Description,
            ProposeEquipment

        };
        private List<string> propertyNames = new List<string>()
        {
            "Systems",
            "Instances",
            "SystemTemplates"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }

    }
    internal class EquipmentTable : TableBase
    {
        public static string TableName = "Equipment";
        public static Type ObjectType = typeof(TECEquipment);

        public static TableField ID = new TableField("ID", "TEXT", ObjectType.GetProperty("Guid"));
        public static TableField Name = new TableField("Name", "TEXT", ObjectType.GetProperty("Name"));
        public static TableField Description = new TableField("Description", "TEXT", ObjectType.GetProperty("Description"));

        private List<TableField> primaryKeys = new List<TableField>() {
            ID
            };
        private List<Type> types = new List<Type>()
        {
            ObjectType
        };
        private List<TableField> fields = new List<TableField>()
        {
            ID,
            Name,
            Description
        };
        private List<string> propertyNames = new List<string>()
        {
            "Equipment",
            "EquipmentTemplates"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }

    }
    internal class SubScopeTable : TableBase
    {
        public static string TableName = "SubScope";
        public static Type ObjectType = typeof(TECSubScope);

        public static TableField ID = new TableField("ID", "TEXT", ObjectType.GetProperty("Guid"));
        public static TableField Name = new TableField("Name", "TEXT", ObjectType.GetProperty("Name"));
        public static TableField Description = new TableField("Description", "TEXT", ObjectType.GetProperty("Description"));

        private List<TableField> primaryKeys = new List<TableField>() {
            ID
            };
        private List<Type> types = new List<Type>()
        {
            ObjectType
        };
        private List<TableField> fields = new List<TableField>()
        {
            ID,
            Name,
            Description
        };
        private List<string> propertyNames = new List<string>()
        {
            "SubScope",
            "SubScopeTemplates"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }

    }
    internal class DeviceTable : TableBase
    {
        public static string TableName = "Device";
        public static Type ObjectType = typeof(TECDevice);

        public static TableField ID = new TableField("ID", "TEXT", ObjectType.GetProperty("Guid"));
        public static TableField Name = new TableField("Name", "TEXT", ObjectType.GetProperty("Name"));
        public static TableField Description = new TableField("Description", "TEXT", ObjectType.GetProperty("Description"));
        public static TableField Price = new TableField("Price", "REAL", ObjectType.GetProperty("Price"));

        private List<TableField> primaryKeys = new List<TableField>() {
            ID
            };
        private List<Type> types = new List<Type>()
        {
            ObjectType
        };
        private List<TableField> fields = new List<TableField>()
        {
            ID,
            Name,
            Description,
            Price
        };
        private List<string> propertyNames = new List<string>()
        {
            "Devices"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }

    }
    internal class PointTable : TableBase
    {
        public static string TableName = "Point";
        public static Type ObjectType = typeof(TECPoint);

        public static TableField ID = new TableField("ID", "TEXT", ObjectType.GetProperty("Guid"));
        public static TableField Name = new TableField("Name", "TEXT", ObjectType.GetProperty("Label"));
        public static TableField Quantity = new TableField("Quantity", "INTEGER", ObjectType.GetProperty("Quantity"));
        public static TableField Type = new TableField("Type", "TEXT", ObjectType.GetProperty("Type"));

        private List<TableField> primaryKeys = new List<TableField>() {
            ID
            };
        private List<Type> types = new List<Type>()
        {
            ObjectType
        };
        private List<TableField> fields = new List<TableField>()
        {
            ID,
            Name,
            Quantity,
            Type
        };
        private List<string> propertyNames = new List<string>()
        {
            "Points"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }

    }
    internal class TagTable : TableBase
    {
        public static string TableName = "Tag";
        public static Type ObjectType = typeof(TECLabeled);

        public static TableField ID = new TableField("ID", "TEXT", ObjectType.GetProperty("Guid"));
        public static TableField TagString = new TableField("Label", "TEXT", ObjectType.GetProperty("Label"));

        private List<TableField> primaryKeys = new List<TableField>()
        { ID };
        private List<Type> types = new List<Type>()
        {
            ObjectType
        };
        private List<TableField> fields = new List<TableField>()
        {
            ID,
            TagString
        };
        private List<string> propertyNames = new List<string>()
        {
            "Tags"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }

    }
    internal class ManufacturerTable : TableBase
    {
        public static string TableName = "Manufacturer";
        public static Type ObjectType = typeof(TECManufacturer);

        public static TableField ID = new TableField("ID", "TEXT", ObjectType.GetProperty("Guid"));
        public static TableField Name = new TableField("Name", "TEXT", ObjectType.GetProperty("Label"));
        public static TableField Multiplier = new TableField("Multiplier", "REAL", ObjectType.GetProperty("Multiplier"));

        private List<TableField> primaryKeys = new List<TableField>() {
            ID
            };
        private List<Type> types = new List<Type>()
        {
            ObjectType
        };
        private List<TableField> fields = new List<TableField>()
        {
            ID,
            Name,
            Multiplier
        };
        private List<string> propertyNames = new List<string>()
        {
            "Manufacturers"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }

    }
    internal class LocationTable : TableBase
    {
        public static string TableName = "Location";
        public static Type ObjectType = typeof(TECLabeled);

        public static TableField ID = new TableField("ID", "TEXT", ObjectType.GetProperty("Guid"));
        public static TableField Name = new TableField("Label", "TEXT", ObjectType.GetProperty("Label"));

        private List<TableField> primaryKeys = new List<TableField>() {
            ID
            };
        private List<Type> types = new List<Type>()
        {
            ObjectType
        };
        private List<TableField> fields = new List<TableField>()
        {
            ID,
            Name
        };
        private List<string> propertyNames = new List<string>()
        {
            "Locations"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }

    }
    internal class ConnectionTypeTable : TableBase
    {
        public static string TableName = "ConnectionType";
        public static Type ObjectType = typeof(TECElectricalMaterial);

        public static TableField ID = new TableField("ID", "TEXT", ObjectType.GetProperty("Guid"));
        public static TableField Name = new TableField("Name", "TEXT", ObjectType.GetProperty("Name"));
        public static TableField Cost = new TableField("Cost", "REAL", ObjectType.GetProperty("Cost"));
        public static TableField Labor = new TableField("Labor", "REAL", ObjectType.GetProperty("Labor"));

        private List<TableField> primaryKeys = new List<TableField>()
        {
            ID
        };
        private List<Type> types = new List<Type>()
        {
            ObjectType
        };
        private List<TableField> fields = new List<TableField>()
        {
            ID,
            Name,
            Cost,
            Labor
        };
        private List<string> propertyNames = new List<string>()
        {
            "ConnectionTypes"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    internal class ConduitTypeTable : TableBase
    {
        public static string TableName = "ConduitType";
        public static Type ObjectType = typeof(TECElectricalMaterial);

        public static TableField ID = new TableField("ID", "TEXT", ObjectType.GetProperty("Guid"));
        public static TableField Name = new TableField("Name", "TEXT", ObjectType.GetProperty("Name"));
        public static TableField Cost = new TableField("Cost", "REAL", ObjectType.GetProperty("Cost"));
        public static TableField Labor = new TableField("Labor", "REAL", ObjectType.GetProperty("Labor"));

        private List<TableField> primaryKeys = new List<TableField>()
        {
            ID
        };
        private List<Type> types = new List<Type>()
        {
            ObjectType
        };
        private List<TableField> fields = new List<TableField>()
        {
            ID,
            Name,
            Cost,
            Labor
        };
        private List<string> propertyNames = new List<string>()
        {
            "ConduitTypes"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    internal class AssociatedCostTable : TableBase
    {
        public static string TableName = "AssociatedCost";
        public static Type ObjectType = typeof(TECCost);

        public static TableField ID = new TableField("ID", "TEXT", ObjectType.GetProperty("Guid"));
        public static TableField Name = new TableField("Name", "TEXT", ObjectType.GetProperty("Name"));
        public static TableField Cost = new TableField("Cost", "REAL", ObjectType.GetProperty("Cost"));
        public static TableField Labor = new TableField("Labor", "REAL", ObjectType.GetProperty("Labor"));
        public static TableField Type = new TableField("Type", "TEXT", ObjectType.GetProperty("Type"));

        private List<TableField> primaryKeys = new List<TableField>()
        {
            ID
        };
        private List<Type> types = new List<Type>()
        {
            ObjectType
        };
        private List<TableField> fields = new List<TableField>()
        {
            ID,
            Name,
            Cost,
            Labor,
            Type
        };
        private List<string> propertyNames = new List<string>()
        {
            "AssociatedCosts"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    internal class SubScopeConnectionTable : TableBase
    {
        public static string TableName = "SubScopeConnection";
        public static Type ObjectType = typeof(TECSubScopeConnection);

        public static TableField ID = new TableField("ID", "TEXT", ObjectType.GetProperty("Guid"));
        public static TableField Length = new TableField("Length", "REAL", ObjectType.GetProperty("Length"));
        public static TableField ConduitLength = new TableField("ConduitLength", "REAL", ObjectType.GetProperty("ConduitLength"));

        private List<TableField> primaryKeys = new List<TableField>() {
            ID
            };
        private List<Type> types = new List<Type>()
        {
            ObjectType
        };
        private List<TableField> fields = new List<TableField>()
        {
            ID,
            Length,
            ConduitLength
        };
        private List<string> propertyNames = new List<string>()
        {
            "ChildrenConnections"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    internal class NetworkConnectionTable : TableBase
    {
        public static string TableName = "NetworkConnection";
        public static Type ObjectType = typeof(TECNetworkConnection);

        public static TableField ID = new TableField("ID", "TEXT", ObjectType.GetProperty("Guid"));
        public static TableField Length = new TableField("Length", "REAL", ObjectType.GetProperty("Length"));
        public static TableField ConduitLength = new TableField("ConduitLength", "REAL", ObjectType.GetProperty("ConduitLength"));
        public static TableField IOType = new TableField("IOType", "TEXT", ObjectType.GetProperty("IOType"));

        private List<TableField> primaryKeys = new List<TableField>() {
            ID
            };
        private List<Type> types = new List<Type>()
        {
            ObjectType
        };
        private List<TableField> fields = new List<TableField>()
        {
            ID,
            Length,
            ConduitLength,
            IOType
        };
        private List<string> propertyNames = new List<string>()
        {
            "ChildrenConnections"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    internal class ControllerTable : TableBase
    {
        public static string TableName = "Controller";
        public static Type ObjectType = typeof(TECController);

        public static TableField ID = new TableField("ID", "TEXT", ObjectType.GetProperty("Guid"));
        public static TableField Name = new TableField("Name", "TEXT", ObjectType.GetProperty("Name"));
        public static TableField Description = new TableField("Description", "TEXT", ObjectType.GetProperty("Description"));

        private List<TableField> primaryKeys = new List<TableField>() {
            ID
            };
        private List<Type> types = new List<Type>()
        {
            ObjectType
        };
        private List<TableField> fields = new List<TableField>()
        {
            ID,
            Name,
            Description
        };
        private List<string> propertyNames = new List<string>()
        {
            "Controllers",
            "ControllerTemplates"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    internal class MiscTable : TableBase
    {
        public static string TableName = "Misc";
        public static Type ObjectType = typeof(TECMisc);

        public static TableField ID = new TableField("ID", "TEXT", ObjectType.GetProperty("Guid"));
        public static TableField Name = new TableField("Name", "TEXT", ObjectType.GetProperty("Name"));
        public static TableField Cost = new TableField("Cost", "REAL", ObjectType.GetProperty("Cost"));
        public static TableField Labor = new TableField("Labor", "REAL", ObjectType.GetProperty("Labor"));
        public static TableField Quantity = new TableField("Quantity", "INTEGER", ObjectType.GetProperty("Quantity"));
        public static TableField Type = new TableField("Type", "TEXT", ObjectType.GetProperty("Type"));

        private List<TableField> primaryKeys = new List<TableField>()
        {
            ID
        };

        private List<Type> types = new List<Type>()
        {
            ObjectType
        };
        private List<TableField> fields = new List<TableField>()
        {
            ID,
            Name,
            Cost,
            Labor,
            Quantity,
            Type
        };
        private List<string> propertyNames = new List<string>()
        {
            "MiscCosts",
            "MiscCostTemplates"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    internal class PanelTypeTable : TableBase
    {
        public static string TableName = "PanelType";
        public static Type ObjectType = typeof(TECPanelType);

        public static TableField ID = new TableField("ID", "TEXT", ObjectType.GetProperty("Guid"));
        public static TableField Name = new TableField("Name", "TEXT", ObjectType.GetProperty("Name"));
        public static TableField Price = new TableField("Price", "REAL", ObjectType.GetProperty("Price"));
        public static TableField Labor = new TableField("Labor", "REAL", ObjectType.GetProperty("Labor"));

        private List<TableField> primaryKeys = new List<TableField>()
        {
            ID
        };

        private List<Type> types = new List<Type>()
        {
            ObjectType
        };
        private List<TableField> fields = new List<TableField>()
        {
            ID,
            Name,
            Price,
            Labor
        };
        private List<string> propertyNames = new List<string>()
        {
            "PanelTypes"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    internal class PanelTable : TableBase
    {
        public static string TableName = "Panel";
        public static Type ObjectType = typeof(TECPanel);

        public static TableField ID = new TableField("ID", "TEXT", ObjectType.GetProperty("Guid"));
        public static TableField Name = new TableField("Name", "TEXT", ObjectType.GetProperty("Name"));
        public static TableField Description = new TableField("Description", "TEXT", ObjectType.GetProperty("Description"));

        private List<TableField> primaryKeys = new List<TableField>()
        {
            ID
        };

        private List<Type> types = new List<Type>()
        {
            ObjectType
        };
        private List<TableField> fields = new List<TableField>()
        {
            ID,
            Name,
            Description
        };
        private List<string> propertyNames = new List<string>()
        {
            "Panels",
            "PanelTemplates"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    internal class IOModuleTable : TableBase
    {
        public static string TableName = "IOModule";
        public static Type IOModuleType = typeof(TECIOModule);

        public static TableField ID = new TableField("ID", "TEXT", IOModuleType.GetProperty("Guid"));
        public static TableField Name = new TableField("Name", "TEXT", IOModuleType.GetProperty("Name"));
        public static TableField Description = new TableField("Description", "TEXT", IOModuleType.GetProperty("Description"));
        public static TableField Price = new TableField("Price", "REAL", IOModuleType.GetProperty("Price"));

        private List<TableField> primaryKeys = new List<TableField>()
        {
            ID
        };

        private List<Type> types = new List<Type>()
        {
            IOModuleType
        };
        private List<TableField> fields = new List<TableField>()
        {
            ID,
            Name,
            Description,
            Price
        };
        private List<string> propertyNames = new List<string>()
        {
            "IOModules"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }

    }
    internal class IOTable : TableBase
    {
        public static string TableName = "IO";
        public static Type IOObjectType = typeof(TECIO);

        public static TableField ID = new TableField("ID", "TEXT", IOObjectType.GetProperty("Guid"));
        public static TableField IOType = new TableField("Type", "TEXT", IOObjectType.GetProperty("Type"));
        public static TableField Quantity = new TableField("Quantity", "INTEGER", IOObjectType.GetProperty("Quantity"));

        private List<TableField> primaryKeys = new List<TableField>() {
            ID
        };
        private List<Type> types = new List<Type>()
        {
            IOObjectType
        };
        private List<TableField> fields = new List<TableField>()
        {
            ID,
            IOType,
            Quantity
        };
        private List<string> propertyNames = new List<string>()
        {
            "IO"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    internal class ControllerTypeTable : TableBase
    {
        public static string TableName = "ControllerType";
        public static Type ObjectType = typeof(TECControllerType);

        public static TableField ID = new TableField("ID", "TEXT", ObjectType.GetProperty("Guid"));
        public static TableField Name = new TableField("Name", "TEXT", ObjectType.GetProperty("Name"));
        public static TableField Description = new TableField("Description", "TEXT", ObjectType.GetProperty("Description"));
        public static TableField Price = new TableField("Price", "REAL", ObjectType.GetProperty("Price"));
        public static TableField Labor = new TableField("Labor", "REAL", ObjectType.GetProperty("Labor"));
        public static TableField Type = new TableField("Type", "TEXT", ObjectType.GetProperty("Type"));
        
        private List<TableField> primaryKeys = new List<TableField>() {
            ID
            };
        private List<Type> types = new List<Type>()
        {
            ObjectType
        };
        private List<TableField> fields = new List<TableField>()
        {
            ID,
            Name,
            Description,
            Price,
            Labor,
            Type
        };
        private List<string> propertyNames = new List<string>()
        {
            "ControllerTypes"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    internal class ValveTable : TableBase
    {
        public static string TableName = "Valve";
        public static Type ObjectType = typeof(TECValve);

        public static TableField ID = new TableField("ID", "TEXT", ObjectType.GetProperty("Guid"));
        public static TableField Name = new TableField("Name", "TEXT", ObjectType.GetProperty("Name"));
        public static TableField Description = new TableField("Description", "TEXT", ObjectType.GetProperty("Description"));
        public static TableField Price = new TableField("Price", "REAL", ObjectType.GetProperty("Price"));
        public static TableField Labor = new TableField("Labor", "REAL", ObjectType.GetProperty("Labor"));
        public static TableField Type = new TableField("Type", "TEXT", ObjectType.GetProperty("Type"));
        public static TableField Cv = new TableField("Cv", "Real", ObjectType.GetProperty("Cv"));
        public static TableField Size = new TableField("Size", "Real", ObjectType.GetProperty("Size"));
        public static TableField Style = new TableField("Style", "TEXT", ObjectType.GetProperty("Style"));
        
        private List<TableField> primaryKeys = new List<TableField>() {
            ID
        };
        private List<Type> types = new List<Type>()
        {
            ObjectType
        };
        private List<TableField> fields = new List<TableField>()
        {
            ID,
            Name,
            Description,
            Price,
            Labor,
            Type,
            Cv,
            Size,
            Style
        };
        private List<string> propertyNames = new List<string>()
        {
            "Valves"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    #endregion

    #region Relationship Tables
    internal class BidScopeBranchTable : TableBase
    {
        public static string TableName = "BidScopeBranch";
        public static Type ObjectType = typeof(TECBid);
        public static Type ReferenceType = typeof(TECScopeBranch);

        public static TableField BidID = new TableField("BidID", "TEXT", ObjectType.GetProperty("Guid"));
        public static TableField ScopeBranchID = new TableField("ScopeBranchID", "TEXT", ReferenceType.GetProperty("Guid"));

        private List<TableField> primaryKeys = new List<TableField>()
        {
            BidID,
            ScopeBranchID
        };

        private List<Type> types = new List<Type>()
        {
            ObjectType,
            ReferenceType
        };
        private List<TableField> fields = new List<TableField>()
        {
            BidID,
            ScopeBranchID
        };
        private List<string> propertyNames = new List<string>()
        {
            "ScopeTree"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    internal class BidMiscTable : TableBase
    {
        public static string TableName = "BidMisc";
        public static Type BidType = typeof(TECBid);
        public static Type CostType = typeof(TECMisc);

        public static TableField BidID = new TableField("BidID", "TEXT", BidType.GetProperty("Guid"));
        public static TableField MiscID = new TableField("MiscID", "TEXT", CostType.GetProperty("Guid"));

        private List<TableField> primaryKeys = new List<TableField>()
        {
            BidID,
            MiscID
        };

        private List<Type> types = new List<Type>()
        {
            BidType,
            CostType
        };
        private List<TableField> fields = new List<TableField>()
        {
            BidID,
            MiscID
        };
        private List<string> propertyNames = new List<string>()
        {
            "MiscCosts"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    internal class HardwareManufacturerTable : TableBase
    {
        public static string TableName = "HardwareManufacturer";
        public static Type ObjectType = typeof(TECHardware);
        public static Type ReferenceType = typeof(TECManufacturer);

        public static TableField HardwareID = new TableField("HardwareID", "TEXT", ObjectType.GetProperty("Guid"));
        public static TableField ManufacturerID = new TableField("ManufacturerID", "TEXT", ReferenceType.GetProperty("Guid"));

        private List<TableField> primaryKeys = new List<TableField>() {
            HardwareID,
            ManufacturerID
            };
        private List<Type> types = new List<Type>()
        {
            ObjectType,
            ReferenceType
        };
        private List<TableField> fields = new List<TableField>()
        {
            HardwareID,
            ManufacturerID
        };
        private List<string> propertyNames = new List<string>()
        {
            "Manufacturer"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    internal class ControllerTypeIOTable : TableBase
    {
        public static string TableName = "ControllerTypeIO";
        public static Type ObjectType = typeof(TECControllerType);
        public static Type ReferenceType = typeof(TECIO);

        public static TableField TypeID = new TableField("TypeID", "TEXT", ObjectType.GetProperty("Guid"));
        public static TableField IOID = new TableField("IOID", "TEXT", ReferenceType.GetProperty("Guid"));

        private List<TableField> primaryKeys = new List<TableField>() {
            TypeID,
            IOID
            };
        private List<Type> types = new List<Type>()
        {
            ObjectType,
            ReferenceType
        };
        private List<TableField> fields = new List<TableField>()
        {
            TypeID,
            IOID
        };
        private List<string> propertyNames = new List<string>()
        {
            "IO"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    internal class ControllerTypeIOModuleTable : TableBase
    {
        public static string TableName = "ControllerTypeIOModule";
        public static Type ObjectType = typeof(TECControllerType);
        public static Type ReferenceType = typeof(TECIOModule);

        public static TableField TypeID = new TableField("TypeID", "TEXT", ObjectType.GetProperty("Guid"));
        public static TableField ModuleID = new TableField("ModuleID", "TEXT", ReferenceType.GetProperty("Guid"));

        private List<TableField> primaryKeys = new List<TableField>() {
            TypeID,
            ModuleID
            };
        private List<Type> types = new List<Type>()
        {
            ObjectType,
            ReferenceType
        };
        private List<TableField> fields = new List<TableField>()
        {
            TypeID,
            ModuleID
        };
        private List<string> propertyNames = new List<string>()
        {
            "IOModules"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    internal class ControllerIOModuleTable : TableBase
    {
        public static string TableName = "ControllerIOModule";
        public static Type ObjectType = typeof(TECController);
        public static Type ReferenceType = typeof(TECIOModule);

        public static TableField ControllerID = new TableField("ControllerID", "TEXT", ObjectType.GetProperty("Guid"));
        public static TableField ModuleID = new TableField("ModuleID", "TEXT", ReferenceType.GetProperty("Guid"));

        private List<TableField> primaryKeys = new List<TableField>() {
            ControllerID,
            ModuleID
            };
        private List<Type> types = new List<Type>()
        {
            ObjectType,
            ReferenceType
        };
        private List<TableField> fields = new List<TableField>()
        {
            ControllerID,
            ModuleID
        };
        private List<string> propertyNames = new List<string>()
        {
            "IOModules"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    internal class IOModuleIOTable : TableBase
    {
        public static string TableName = "IOModuleIO";
        public static Type ObjectType = typeof(TECIOModule);
        public static Type ReferenceType = typeof(TECIO);

        public static TableField ModuleID = new TableField("IOModuleID", "TEXT", ObjectType.GetProperty("Guid"));
        public static TableField IOID = new TableField("IOID", "TEXT", ReferenceType.GetProperty("Guid"));

        private List<TableField> primaryKeys = new List<TableField>() {
            ModuleID,
            IOID
        };
        private List<Type> types = new List<Type>()
        {
            ObjectType,
            ReferenceType
        };
        private List<TableField> fields = new List<TableField>()
        {
            ModuleID,
            IOID
        };
        private List<string> propertyNames = new List<string>()
        {
            "IO"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    internal class ControllerConnectionTable : TableBase
    {
        public static string TableName = "ControllerConnection";
        public static Type ObjectType = typeof(TECController);
        public static Type ReferenceType = typeof(TECConnection);

        public static TableField ControllerID = new TableField("ControllerID", "TEXT", ObjectType.GetProperty("Guid"));
        public static TableField ConnectionID = new TableField("ConnectionID", "TEXT", ReferenceType.GetProperty("Guid"));

        private List<TableField> primaryKeys = new List<TableField>() {
            ControllerID,
            ConnectionID
            };
        private List<Type> types = new List<Type>()
        {
            ObjectType,
            ReferenceType
        };
        private List<TableField> fields = new List<TableField>()
        {
            ControllerID,
            ConnectionID
        };
        private List<string> propertyNames = new List<string>()
        {
            "ChildrenConnections"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    internal class ScopeBranchHierarchyTable : TableBase
    {
        public static string TableName = "ScopeBranchHierarchy";
        public static Type ObjectType = typeof(TECScopeBranch);
        public static Type ReferenceType = typeof(TECScopeBranch);

        public static TableField ParentID = new TableField("ParentID", "TEXT", ObjectType.GetProperty("Guid"));
        public static TableField ChildID = new TableField("ChildID", "TEXT", ReferenceType.GetProperty("Guid"));

        private List<TableField> primaryKeys = new List<TableField>() {
            ParentID,
            ChildID
            };
        private List<Type> types = new List<Type>()
        {
            ObjectType,
            ReferenceType
        };
        private List<TableField> fields = new List<TableField>()
        {
            ParentID,
            ChildID
        };
        private List<string> propertyNames = new List<string>()
        {
            "Branches"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }

    }
    internal class BidSystemTable : TableBase
    {
        public static string TableName = "BidSystem";
        public static Type BidType = typeof(TECBid);
        public static Type SystemType = typeof(TECSystem);

        public static Type Helpers = typeof(HelperProperties);

        public static TableField BidID = new TableField("BidID", "TEXT", BidType.GetProperty("Guid"));
        public static TableField SystemID = new TableField("SystemID", "TEXT", SystemType.GetProperty("Guid"));
        public static TableField Index = new TableField("ScopeIndex", "INTEGER", Helpers.GetProperty("Index"), "Systems");

        private List<TableField> primaryKeys = new List<TableField>()
        {
            BidID,
            SystemID
        };
        private List<Type> types = new List<Type>()
        {
            BidType,
            SystemType
        };
        private List<TableField> fields = new List<TableField>()
        {
            BidID,
            SystemID,
            Index
        };
        private List<string> propertyNames = new List<string>()
        {
            "Systems"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    internal class SystemEquipmentTable : TableBase
    {
        public static string TableName = "SystemEquipment";
        public static Type ObjectType = typeof(TECSystem);
        public static Type ReferenceType = typeof(TECEquipment);

        public static Type HelperType = typeof(HelperProperties);

        public static TableField SystemID = new TableField("SystemID", "TEXT", ObjectType.GetProperty("Guid"));
        public static TableField EquipmentID = new TableField("EquipmentID", "TEXT", ReferenceType.GetProperty("Guid"));
        public static TableField ScopeIndex = new TableField("ScopeIndex", "INTEGER", HelperType.GetProperty("Index"), "Equipment");

        private List<TableField> primaryKeys = new List<TableField>() {
            SystemID,
            EquipmentID
            };
        private List<Type> types = new List<Type>()
        {
            ObjectType,
            ReferenceType
        };
        private List<TableField> fields = new List<TableField>()
        {
            SystemID,
            EquipmentID,
            ScopeIndex
        };
        private List<string> propertyNames = new List<string>()
        {
            "Equipment"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }

    }
    internal class EquipmentSubScopeTable : TableBase
    {
        public static string TableName = "EquipmentSubScope";
        public static Type ObjectType = typeof(TECEquipment);
        public static Type ReferenceType = typeof(TECSubScope);

        public static Type HelperType = typeof(HelperProperties);

        public static TableField EquipmentID = new TableField("EquipmentID", "TEXT", ObjectType.GetProperty("Guid"));
        public static TableField SubScopeID = new TableField("SubScopeID", "TEXT", ReferenceType.GetProperty("Guid"));
        public static TableField ScopeIndex = new TableField("ScopeIndex", "INTEGER", HelperType.GetProperty("Index"), "SubScope");

        private List<TableField> primaryKeys = new List<TableField>() {
            EquipmentID,
            SubScopeID
            };
        private List<Type> types = new List<Type>()
        {
            ObjectType,
            ReferenceType
        };
        private List<TableField> fields = new List<TableField>()
        {
            EquipmentID,
            SubScopeID,
            ScopeIndex
        };
        private List<string> propertyNames = new List<string>()
        {
            "SubScope"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }

    }
    internal class SubScopeDeviceTable : TableBase
    {
        public static string TableName = "SubScopeDevice";
        public static Type ObjectType = typeof(TECSubScope);
        public static Type ReferenceType = typeof(IEndDevice);

        public static Type HelperType = typeof(HelperProperties);

        public static TableField SubScopeID = new TableField("SubScopeID", "TEXT", ObjectType.GetProperty("Guid"));
        public static TableField DeviceID = new TableField("DeviceID", "TEXT", ReferenceType.GetProperty("Guid"));
        public static TableField Quantity = new TableField("Quantity", "INTEGER", HelperType.GetProperty("Quantity"), "Devices");
        public static TableField ScopeIndex = new TableField("ScopeIndex", "INTEGER", HelperType.GetProperty("Index"), "Devices");

        private List<TableField> primaryKeys = new List<TableField>() {
            SubScopeID,
            DeviceID
            };
        private List<Type> types = new List<Type>()
        {
            ObjectType,
            ReferenceType
        };
        private List<TableField> fields = new List<TableField>()
        {
            SubScopeID,
            DeviceID,
            Quantity,
            ScopeIndex
        };
        private List<string> propertyNames = new List<string>()
        {
            "Devices"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }

    }
    internal class SubScopePointTable : TableBase
    {
        public static string TableName = "SubScopePoint";
        public static Type ObjectType = typeof(TECSubScope);
        public static Type ReferenceType = typeof(TECPoint);

        public static TableField SubScopeID = new TableField("SubScopeID", "TEXT", ObjectType.GetProperty("Guid"));
        public static TableField PointID = new TableField("PointID", "TEXT", ReferenceType.GetProperty("Guid"));

        private List<TableField> primaryKeys = new List<TableField>() {
            SubScopeID,
            PointID
            };
        private List<Type> types = new List<Type>()
        {
            ObjectType,
            ReferenceType
        };
        private List<TableField> fields = new List<TableField>()
        {
            SubScopeID,
            PointID
        };
        private List<string> propertyNames = new List<string>()
        {
            "Points"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }

    }
    internal class ScopeTagTable : TableBase
    {
        public static string TableName = "ScopeTag";
        public static Type ObjectType = typeof(TECScope);
        public static Type ReferenceType = typeof(TECLabeled);

        public static TableField ScopeID = new TableField("ScopeID", "TEXT", ObjectType.GetProperty("Guid"));
        public static TableField TagID = new TableField("TagID", "TEXT", ReferenceType.GetProperty("Guid"));

        private List<TableField> primaryKeys = new List<TableField>() {
            ScopeID,
            TagID
            };
        private List<Type> types = new List<Type>()
        {
            ObjectType,
            ReferenceType
        };
        private List<TableField> fields = new List<TableField>()
        {
            ScopeID,
            TagID
        };
        private List<string> propertyNames = new List<string>()
        {
            "Tags"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    internal class DeviceConnectionTypeTable : TableBase
    {
        public static string TableName = "DeviceConnectionType";
        public static Type ObjectType = typeof(TECDevice);
        public static Type ReferenceType = typeof(TECElectricalMaterial);

        public static Type HelperType = typeof(HelperProperties);

        public static TableField DeviceID = new TableField("DeviceID", "TEXT", ObjectType.GetProperty("Guid"));
        public static TableField TypeID = new TableField("ConnectionTypeID", "TEXT", ReferenceType.GetProperty("Guid"));
        public static TableField Quantity = new TableField("Quantity", "INTEGER", HelperType.GetProperty("Quantity"), "ConnectionTypes");

        private List<TableField> primaryKeys = new List<TableField>()
        {
            DeviceID,
            TypeID
        };
        private List<Type> types = new List<Type>()
        {
            ObjectType,
            ReferenceType
        };
        private List<TableField> fields = new List<TableField>()
        {
            DeviceID,
            TypeID,
            Quantity
        };
        private List<string> propertyNames = new List<string>()
        {
            "ConnectionTypes"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    internal class LocatedLocationTable : TableBase
    {
        public static string TableName = "LocatedLocation";
        public static Type ObjectType = typeof(TECLocated);
        public static Type ReferenceType = typeof(TECLabeled);

        public static TableField ScopeID = new TableField("ScopeID", "TEXT", ObjectType.GetProperty("Guid"));
        public static TableField LocationID = new TableField("LocationID", "TEXT", ReferenceType.GetProperty("Guid"));

        private List<TableField> primaryKeys = new List<TableField>() {
            ScopeID,
            LocationID
            };
        private List<Type> types = new List<Type>()
        {
            ObjectType,
            ReferenceType
        };
        private List<TableField> fields = new List<TableField>()
        {
            ScopeID,
            LocationID
        };
        private List<string> propertyNames = new List<string>()
        {
            "Location"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    internal class ScopeAssociatedCostTable : TableBase
    {
        public static string TableName = "ScopeAssociatedCost";
        public static Type ObjectType = typeof(TECScope);
        public static Type ReferenceType = typeof(TECCost);

        public static Type HelperType = typeof(HelperProperties);

        public static TableField ScopeID = new TableField("ScopeID", "TEXT", ObjectType.GetProperty("Guid"));
        public static TableField AssociatedCostID = new TableField("AssociatedCostID", "TEXT", ReferenceType.GetProperty("Guid"));
        public static TableField Quantity = new TableField("Quantity", "INTEGER", HelperType.GetProperty("Quantity"), "AssociatedCosts");

        private List<TableField> primaryKeys = new List<TableField>()
        {
            ScopeID,
            AssociatedCostID
        };
        private List<Type> types = new List<Type>()
        {
            ObjectType,
            ReferenceType
        };
        private List<TableField> fields = new List<TableField>()
        {
            ScopeID,
            AssociatedCostID,
            Quantity
        };
        private List<string> propertyNames = new List<string>()
        {
            "AssociatedCosts"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    internal class ElectricalMaterialRatedCostTable : TableBase
    {
        public static string TableName = "ElectricalMaterialRatedCost";
        public static Type ObjectType = typeof(TECElectricalMaterial);
        public static Type ReferenceType = typeof(TECCost);

        public static Type HelperType = typeof(HelperProperties);

        public static TableField ComponentID = new TableField("ComponentID", "TEXT", ObjectType.GetProperty("Guid"));
        public static TableField CostID = new TableField("CostID", "TEXT", ReferenceType.GetProperty("Guid"));
        public static TableField Quantity = new TableField("Quantity", "INTEGER", HelperType.GetProperty("Quantity"), "RatedCosts");

        private List<TableField> primaryKeys = new List<TableField>()
        {
            ComponentID,
            CostID
        };
        private List<Type> types = new List<Type>()
        {
            ObjectType,
            ReferenceType
        };
        private List<TableField> fields = new List<TableField>()
        {
            ComponentID,
            CostID,
            Quantity
        };
        private List<string> propertyNames = new List<string>()
        {
            "RatedCosts"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    internal class ControllerControllerTypeTable : TableBase
    {
        public static string TableName = "ControllerControllerType";
        public static Type ControllerType = typeof(TECController);
        public static Type TypeType = typeof(TECControllerType);

        public static TableField ControllerID = new TableField("ControllerID", "TEXT", ControllerType.GetProperty("Guid"));
        public static TableField TypeID = new TableField("TypeID", "TEXT", TypeType.GetProperty("Guid"));

        private List<TableField> primaryKeys = new List<TableField>() {
            ControllerID,
            TypeID
            };
        private List<Type> types = new List<Type>()
        {
            ControllerType,
            TypeType
        };
        private List<TableField> fields = new List<TableField>()
        {
            ControllerID,
            TypeID
        };
        private List<string> propertyNames = new List<string>()
        {
            "Type"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    internal class ConnectionConduitTypeTable : TableBase
    {
        public static string TableName = "ConnectionConduitType";
        public static Type ObjectType = typeof(TECConnection);
        public static Type ReferenceType = typeof(TECElectricalMaterial);

        public static TableField ConnectionID = new TableField("ConnectionID", "TEXT", ObjectType.GetProperty("Guid"));
        public static TableField TypeID = new TableField("ConduitTypeID", "TEXT", ReferenceType.GetProperty("Guid"));

        private List<TableField> primaryKeys = new List<TableField>()
        {
            ConnectionID,
            TypeID
        };
        private List<Type> types = new List<Type>()
        {
            ObjectType,
            ReferenceType
        };
        private List<TableField> fields = new List<TableField>()
        {
            ConnectionID,
            TypeID
        };
        private List<string> propertyNames = new List<string>()
        {
            "ConduitType"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    internal class NetworkConnectionConnectionTypeTable : TableBase
    {
        public static string TableName = "NetworkConnectionConnectionType";
        public static Type ObjectType = typeof(TECNetworkConnection);
        public static Type ReferenceType = typeof(TECElectricalMaterial);

        public static TableField ConnectionID = new TableField("ConnectionID", "TEXT", ObjectType.GetProperty("Guid"));
        public static TableField TypeID = new TableField("ConnectionTypeID", "TEXT", ReferenceType.GetProperty("Guid"));

        private List<TableField> primaryKeys = new List<TableField>()
        {
            ConnectionID,
            TypeID
        };
        private List<Type> types = new List<Type>()
        {
            ObjectType,
            ReferenceType
        };
        private List<TableField> fields = new List<TableField>()
        {
            ConnectionID,
            TypeID
        };
        private List<string> propertyNames = new List<string>()
        {
            "ConnectionTypes"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    internal class NetworkConnectionChildrenTable : TableBase
    {
        public static string TableName = "NetworkConnectionChildren";
        public static Type ConnectionType = typeof(TECNetworkConnection);
        public static Type ChildType = typeof(INetworkConnectable);

        public static TableField ConnectionID = new TableField("ConnectionID", "TEXT", ConnectionType.GetProperty("Guid"));
        public static TableField ChildID = new TableField("ChildID", "TEXT", ChildType.GetProperty("Guid"));
       
        private List<TableField> primaryKeys = new List<TableField>() {
            ConnectionID,
            ChildID
            };
        private List<Type> types = new List<Type>()
        {
            ConnectionType,
            ChildType
        };
        private List<TableField> fields = new List<TableField>()
        {
            ConnectionID,
            ChildID
        };
        private List<string> propertyNames = new List<string>()
        {
            "Children"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    internal class SubScopeConnectionChildrenTable : TableBase
    {
        public static string TableName = "SubScopeConnectionChild";
        public static Type ConnectionType = typeof(TECSubScopeConnection);
        public static Type ChildType = typeof(TECSubScope);

        public static TableField ConnectionID = new TableField("ConnectionID", "TEXT", ConnectionType.GetProperty("Guid"));
        public static TableField ChildID = new TableField("ScopeID", "TEXT", ChildType.GetProperty("Guid"));
        
        private List<TableField> primaryKeys = new List<TableField>() {
            ConnectionID,
            ChildID
            };
        private List<Type> types = new List<Type>()
        {
            ConnectionType,
            ChildType
        };
        private List<TableField> fields = new List<TableField>()
        {
            ConnectionID,
            ChildID
        };
        private List<string> propertyNames = new List<string>()
        {
            "SubScope"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    internal class PanelPanelTypeTable : TableBase
    {
        public static string TableName = "PanelPanelType";
        public static Type ObjectType = typeof(TECPanel);
        public static Type ReferenceType = typeof(TECPanelType);

        public static TableField PanelID = new TableField("PanelID", "TEXT", ObjectType.GetProperty("Guid"));
        public static TableField PanelTypeID = new TableField("PanelTypeID", "TEXT", ReferenceType.GetProperty("Guid"));

        private List<TableField> primaryKeys = new List<TableField>()
        {
            PanelID,
            PanelTypeID
        };
        private List<Type> types = new List<Type>()
        {
            ObjectType,
            ReferenceType
        };
        private List<TableField> fields = new List<TableField>()
        {
            PanelID,
            PanelTypeID
        };
        private List<string> propertyNames = new List<string>()
        {
            "Type"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    internal class PanelControllerTable : TableBase
    {
        public static string TableName = "PanelController";
        public static Type ObjectType = typeof(TECPanel);
        public static Type ReferenceType = typeof(TECController);

        public static TableField PanelID = new TableField("PanelID", "TEXT", ObjectType.GetProperty("Guid"));
        public static TableField ControllerID = new TableField("ControllerID", "TEXT", ReferenceType.GetProperty("Guid"));

        private List<TableField> primaryKeys = new List<TableField>() {
            PanelID,
            ControllerID
            };
        private List<Type> types = new List<Type>()
        {
            ObjectType,
            ReferenceType
        };
        private List<TableField> fields = new List<TableField>()
        {
            PanelID,
            ControllerID
        };
        private List<string> propertyNames = new List<string>()
        {
            "Controllers"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    internal class SystemControllerTable : TableBase
    {
        public static string TableName = "SystemController";
        public static Type SystemType = typeof(TECSystem);
        public static Type ControllerType = typeof(TECController);

        public static TableField SystemID = new TableField("SystemID", "TEXT", SystemType.GetProperty("Guid"));
        public static TableField ControllerID = new TableField("ControllerID", "TEXT", ControllerType.GetProperty("Guid"));

        private List<TableField> primaryKeys = new List<TableField>() {
            SystemID,
            ControllerID
            };
        private List<Type> types = new List<Type>()
        {
            SystemType,
            ControllerType
        };
        private List<TableField> fields = new List<TableField>()
        {
            SystemID,
            ControllerID
        };
        private List<string> propertyNames = new List<string>()
        {
            "Controllers"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    internal class SystemPanelTable : TableBase
    {
        public static string TableName = "SystemPanel";
        public static Type SystemType = typeof(TECSystem);
        public static Type PanelType = typeof(TECPanel);

        public static TableField SystemID = new TableField("SystemID", "TEXT", SystemType.GetProperty("Guid"));
        public static TableField PanelID = new TableField("PanelID", "TEXT", PanelType.GetProperty("Guid"));

        private List<TableField> primaryKeys = new List<TableField>() {
            SystemID,
            PanelID
            };
        private List<Type> types = new List<Type>()
        {
            SystemType,
            PanelType
        };
        private List<TableField> fields = new List<TableField>()
        {
            SystemID,
            PanelID
        };
        private List<string> propertyNames = new List<string>()
        {
            "Panels"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
   
    }
    internal class SystemScopeBranchTable : TableBase
    {
        public static string TableName = "SystemScopeBranch";
        public static Type SystemType = typeof(TECSystem);
        public static Type ScopeBranchType = typeof(TECScopeBranch);

        public static TableField SystemID = new TableField("SystemID", "TEXT", SystemType.GetProperty("Guid"));
        public static TableField BranchID = new TableField("BranchID", "TEXT", ScopeBranchType.GetProperty("Guid"));

        private List<TableField> primaryKeys = new List<TableField>() {
            SystemID,
            BranchID
            };
        private List<Type> types = new List<Type>()
        {
            SystemType,
            ScopeBranchType
        };
        private List<TableField> fields = new List<TableField>()
        {
            SystemID,
            BranchID
        };
        private List<string> propertyNames = new List<string>()
        {
            "ScopeBranches"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    internal class SystemHierarchyTable : TableBase
    {
        public static string TableName = "SystemHierarchy";
        public static Type SystemType = typeof(TECSystem);

        public static TableField ParentID = new TableField("ParentID", "TEXT", SystemType.GetProperty("Guid"));
        public static TableField ChildID = new TableField("ChildID", "TEXT", SystemType.GetProperty("Guid"));

        private List<TableField> primaryKeys = new List<TableField>() {
            ParentID,
            ChildID
            };
        private List<Type> types = new List<Type>()
        {
            SystemType,
            SystemType
        };
        private List<TableField> fields = new List<TableField>()
        {
            ParentID,
            ChildID
        };
        private List<string> propertyNames = new List<string>()
        {
            "Instances"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    internal class SystemMiscTable : TableBase
    {
        public static string TableName = "SystemMisc";
        public static Type SystemType = typeof(TECSystem);
        public static Type MiscType = typeof(TECMisc);

        public static TableField SystemID = new TableField("SystemID", "TEXT", SystemType.GetProperty("Guid"));
        public static TableField MiscID = new TableField("MiscID", "TEXT", MiscType.GetProperty("Guid"));
        
        private List<TableField> primaryKeys = new List<TableField>() {
            SystemID,
            MiscID
            };
        private List<Type> types = new List<Type>()
        {
            SystemType,
            MiscType
        };
        private List<TableField> fields = new List<TableField>()
        {
            SystemID,
            MiscID
        };
        private List<string> propertyNames = new List<string>()
        {
            "MiscCosts"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    internal class ValveActuatorTable : TableBase
    {
        public static string TableName = "ValveActuator";
        public static Type ValveType = typeof(TECValve);
        public static Type ActuatorType = typeof(TECDevice);

        public static TableField ValveID = new TableField("SystemID", "TEXT", ValveType.GetProperty("Guid"));
        public static TableField ActuatorID = new TableField("MIscID", "TEXT", ActuatorType.GetProperty("Guid"));

        private List<TableField> primaryKeys = new List<TableField>() {
            ValveID,
            ActuatorID
        };
        private List<Type> types = new List<Type>()
        {
            ValveType,
            ActuatorType
        };
        private List<TableField> fields = new List<TableField>()
        {
            ValveID,
            ActuatorID
        };
        private List<string> propertyNames = new List<string>()
        {
            "Actuator"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }

    internal class TypicalInstanceTable : TableBase
    {
        public static string TableName = "TypicalInstance";
        public static Type ScopeType = typeof(TECObject);

        public static TableField TypicalID = new TableField("TypicalID", "TEXT", ScopeType.GetProperty("Guid"));
        public static TableField InstanceID = new TableField("InstanceID", "TEXT", ScopeType.GetProperty("Guid"));

        private List<TableField> primaryKeys = new List<TableField>() {
            TypicalID,
            InstanceID
            };
        private List<Type> types = new List<Type>()
        {
            ScopeType,
            ScopeType
        };
        private List<TableField> fields = new List<TableField>()
        {
            TypicalID,
            InstanceID
        };
        private List<string> propertyNames = new List<string>()
        {
            "TypicalInstanceDictionary"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
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
            new LocatedLocationTable(),
            new ControllerTable(),
            new AssociatedCostTable(),
            new ElectricalMaterialRatedCostTable(),
            new ControllerConnectionTable(),
            new ControllerTypeIOTable(),
            new IOModuleIOTable(),
            new ControllerIOModuleTable(),
            new ControllerTypeIOModuleTable(),
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
            new NetworkConnectionChildrenTable(),
            new NetworkConnectionConnectionTypeTable(),
            };
    }

    internal static class AllTemplateTables
    {
        public static List<TableBase> Tables = new List<TableBase>()
        {
            new MetadataTable(),
            new TemplatesInfoTable(),
            new ParametersTable(),
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
            new NetworkConnectionTable(),
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
            new IOModuleIOTable(),
            new ControllerIOModuleTable(),
            new ControllerTypeIOModuleTable(),
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
            new NetworkConnectionChildrenTable(),
            new NetworkConnectionConnectionTypeTable(),
            new ScopeBranchHierarchyTable()
        };
    }

    internal static class AllTables
    {
        public static List<TableBase> Tables = new List<TableBase>()
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
            new IOModuleIOTable(),
            new ControllerIOModuleTable(),
            new ControllerTypeIOModuleTable(),
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
            new LocatedLocationTable(),
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
            new NetworkConnectionChildrenTable(),
            new NetworkConnectionConnectionTypeTable(),
            new BidMiscTable()
        };
    }

    internal abstract class TableBase
    {
        public abstract string NameString { get; }
        public abstract List<Type> Types { get; }
        public abstract List<string> PropertyNames { get; }

        public abstract List<TableField> PrimaryKeys { get; }
        public abstract List<TableField> Fields { get; }

    }
    
    internal class TableField
    {
        private string _defaultValue;

        public string Name { get; }
        public string FieldType { get; }
        public PropertyInfo Property { get; }
        public string HelperContext { get; }
        public string DefaultValue
        {
            get
            {
                if (_defaultValue == null)
                {
                    throw new NotImplementedException(string.Format("TableField {0} doesn't have a default value.", Name));
                }
                else
                {
                    return _defaultValue;
                }
            }
        }

        public TableField(string name, string fieldType, PropertyInfo property, string helperContext = "", string defaultValue = null)
        {
            Name = name;
            FieldType = fieldType;
            Property = property;
            HelperContext = helperContext;
            _defaultValue = defaultValue;
        }
    }

    internal class HelperProperties
    {
        public bool Index { get; set; }
        public bool Quantity { get; set; }

        public HelperProperties()
        {
            Index = true;
            Quantity = true;
        }

    }
    
    #endregion
}
