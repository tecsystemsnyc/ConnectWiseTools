using EstimatingLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    static public class CostTestingUtilities
    {
        public const double DELTA = 1.0 / 1000.0;

        public class Total
        {
            public double Cost;
            public double Labor;

            public Total()
            {
                Cost = 0;
                Labor = 0;
            }

            public static Total operator +(Total left, Total right)
            {
                Total total = new Total();
                total.Cost = left.Cost + right.Cost;
                total.Labor = left.Labor + right.Labor;
                return total;
            }

            public static Total operator -(Total left, Total right)
            {
                Total total = new Total();
                total.Cost = left.Cost - right.Cost;
                total.Labor = left.Labor - right.Labor;
                return total;
            }

            public static Total operator *(Total left, double right)
            {
                Total total = new Total();
                total.Cost = left.Cost * right;
                total.Labor = left.Labor * right;
                return total;
            }
        }

        #region Calculation Methods

        static public Total CalculateTotal(TECHardware hardware, CostType type)
        {
            Total total = new Total();
            if (type == hardware.Type)
            {
                total.Cost = hardware.Cost;
                total.Labor = hardware.Labor;
            }
            total += CalculateTotal(hardware as TECScope, type);
            return total;
        }

        static public Total CalculateTotal(TECCost cost, CostType type)
        {
            int qty = 1;
            if (cost is TECMisc)
            {
                qty = (cost as TECMisc).Quantity;
            }
            if (cost.Type == type)
            {
                Total total = new Total();
                total.Cost = cost.Cost * qty;
                total.Labor = cost.Labor * qty;
                return total;
            }
            else
            {
                return new Total();
            }
        }

        static public Total CalculateTotal(TECScope scope, CostType type)
        {
            Total total = new Total();
            foreach (TECCost cost in scope.AssociatedCosts)
            {
                total += CalculateTotal(cost, type);
            }
            return total;
        }

        static public Total CalculateTotal(TECSubScope subScope, CostType type)
        {
            Total total = new Total();
            foreach (TECDevice device in subScope.Devices)
            {
                total += CalculateTotal(device, type);
            }
            //foreach(TECPoint point in subScope.Points)
            //{
            //    total += calculateTotal(point, type);
            //}
            total += CalculateTotal(subScope as TECScope, type);
            return total;
        }

        static public Total CalculateTotal(TECEquipment equipment, CostType type)
        {
            Total total = new Total();
            foreach (TECSubScope subScope in equipment.SubScope)
            {
                total += CalculateTotal(subScope, type);
            }
            total += CalculateTotal(equipment as TECScope, type);
            return total;
        }

        static public Total CalculateTotal(TECController controller, CostType type)
        {
            Total total = new Total();
            total += CalculateTotal(controller as TECScope, type);
            total += CalculateTotal(controller.Type as TECHardware, type);
            foreach (TECConnection connection in controller.ChildrenConnections)
            {
                total += CalculateTotal(connection, type);
            }
            return total;
        }

        static public Total CalculateTotal(TECPanel panel, CostType type)
        {
            Total total = new Total();
            total += CalculateTotal(panel as TECScope, type);
            total += CalculateTotal(panel.Type as TECCost, type);
            return total;
        }

        static public Total CalculateTotalInstanceSystem(TECSystem instance, TECSystem typical, CostType type)
        {
            Total total = new Total();
            foreach (TECEquipment equipment in instance.Equipment)
            {
                Total equipSubTotal = CalculateTotal(equipment, type);
                total += equipSubTotal;
            }
            foreach (TECMisc misc in typical.MiscCosts)
            {
                Total miscSubTotal = CalculateTotal(misc, type);
                total += miscSubTotal;
            }
            foreach (TECController controller in instance.Controllers)
            {
                Total controllerSubTotal = CalculateTotal(controller, type);
                total += controllerSubTotal;
            }
            foreach (TECPanel panel in instance.Panels)
            {
                Total panelSubTotal = CalculateTotal(panel, type);
                total += panelSubTotal;
            }
            Total systemScopeSubTotal = CalculateTotal(instance as TECScope, type);
            total += systemScopeSubTotal;
            return total;
        }

        static public Total CalculateTotal(TECConnection connection, CostType type)
        {
            Total total = new Total();
            if (connection is TECSubScopeConnection)
            {
                foreach (TECElectricalMaterial conType in (connection as TECSubScopeConnection).ConnectionTypes)
                {
                    total += CalculateTotal(conType, type) * connection.Length;
                    total += CalculateTotal(conType as TECScope, type);
                    foreach (TECCost cost in conType.RatedCosts)
                    {
                        total += CalculateTotal(cost, type) * connection.Length;
                    }
                }
            }
            else if (connection is TECNetworkConnection)
            {
                total += CalculateTotal((connection as TECNetworkConnection).ConnectionType, type) * connection.Length;
                total += CalculateTotal((connection as TECNetworkConnection).ConnectionType as TECScope, type);
                foreach (TECCost cost in (connection as TECNetworkConnection).ConnectionType.RatedCosts)
                {
                    total += CalculateTotal(cost, type) * connection.Length;
                }
            }
            if (connection.ConduitType != null)
            {
                total += CalculateTotal(connection.ConduitType, type) * connection.ConduitLength;
                total += CalculateTotal(connection.ConduitType as TECScope, type);
                foreach (TECCost cost in connection.ConduitType.RatedCosts)
                {
                    total += CalculateTotal(cost, type) * connection.ConduitLength;
                }
            }
            return total;
        }

        #endregion
    }
}
