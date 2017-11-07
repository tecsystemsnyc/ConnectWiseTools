using EstimatingLibrary;

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
            public Total(double cost, double labor)
            {
                Cost = cost;
                Labor = labor;
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
            if (cost is TECMisc misc)
            {
                qty = misc.Quantity;
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
            total += CalculateTotal(panel.Type as TECHardware, type);
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
            else if (connection is TECNetworkConnection netConn)
            {
                
                foreach(TECElectricalMaterial connType in netConn.ConnectionTypes)
                {
                    total += CalculateTotal(connType, type) * connection.Length;
                    total += CalculateTotal(connType as TECScope, type);
                    foreach (TECCost cost in connType.RatedCosts)
                    {
                        total += CalculateTotal(cost, type) * connection.Length;
                    }
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

        static public Total CalculateTotal(TECSystem system, CostType type)
        {
            Total total = new Total();
            foreach(TECEquipment equip in system.Equipment)
            {
                Total subTotal = CalculateTotal(equip, type);
                total += subTotal;
            }
            foreach (TECController controller in system.Controllers)
            {
                Total subTotal = CalculateTotal(controller, type);
                total += subTotal;
            }
            foreach (TECPanel panel in system.Panels)
            {
                Total subTotal = CalculateTotal(panel, type);
                total += subTotal;
            }
            foreach (TECMisc misc in system.MiscCosts)
            {
                Total subTotal = CalculateTotal(misc, type);
                total += subTotal;
            }
            Total systemSubTotal = CalculateTotal(system as TECScope, type);
            total += systemSubTotal;
            return total;
        }
        #endregion
    }
}
