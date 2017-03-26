using EstimatingLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    public static class EstimateCalculator
    {
        /// <summary>
        /// Returns TEC material costs of devices and their associated costs
        /// </summary>
        public static double GetMaterialCost(TECBid bid)
        {
            double cost = 0;
            foreach (TECSystem system in bid.Systems)
            {
                cost += system.MaterialCost;
            }
            return cost;
        }
        /// <summary>
        /// Returns tax from the TEC materials cost at 8.75% if tax is not exempt
        /// </summary>
        public static double GetTax(TECBid bid)
        {
            double outTax = 0;

            if (!bid.Parameters.IsTaxExempt)
            {
                outTax += .0875 * bid.MaterialCost;
            }

            return outTax;
        }

        /// <summary>
        /// Returns cost of all TEC material and labor with escalation, overhead, and tax
        /// </summary>
        public static double GetTECCost(TECBid bid)
        {
            double outCost = 0;
            outCost += bid.Labor.TECSubTotal;
            outCost += bid.MaterialCost;
            outCost += outCost * bid.Parameters.Escalation / 100;
            outCost += outCost * bid.Parameters.Overhead / 100;
            outCost += bid.Tax;

            return outCost;
        }
        /// <summary>
        /// Returns TEC Cost plus profit
        /// </summary>
        public static double GetTECSubtotal(TECBid bid)
        {
            double outCost = 0;
            outCost += GetTECCost(bid);
            outCost += outCost * bid.Parameters.Profit / 100;

            return outCost;
        }

        /// <summary>
        /// Returns the electrical material cost of all wire, conduit, and their associated costs 
        /// </summary>
        public static double GetElectricalMaterialCost(TECBid bid)
        {
            double cost = 0;

            foreach(TECMiscWiring wiring in bid.MiscWiring)
            {
                cost += wiring.Cost * wiring.Quantity;
            }

            foreach (TECController controller in bid.Controllers)
            {
                foreach(TECConnection connection in controller.ChildrenConnections)
                {
                    var length = connection.Length;

                    if (connection is TECNetworkConnection)
                    {
                        TECConnectionType type = (connection as TECNetworkConnection).ConnectionType;
                        cost += length * type.Cost;
                        foreach (TECAssociatedCost associatedCost in type.AssociatedCosts)
                        { cost += associatedCost.Cost; }
                    }
                    else if (connection is TECSubScopeConnection)
                    {
                        foreach (TECConnectionType type in (connection as TECSubScopeConnection).ConnectionTypes)
                        {
                            cost += length * type.Cost;
                            foreach (TECAssociatedCost associatedCost in type.AssociatedCosts)
                            {
                                cost += associatedCost.Cost;
                            }
                        }
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
            }
            return cost;
        }
        /// <summary>
        /// Returns the electrical labor cost of all wire, conduit, and their associated costs 
        /// </summary>
        public static double GetElectricalLaborCost(TECBid bid)
        {
            double laborHours = 0;

            foreach (TECController controller in bid.Controllers)
            {
                foreach (TECConnection connection in controller.ChildrenConnections)
                {
                    var length = connection.Length;
                    if (connection.ConduitType != null)
                    { laborHours += connection.Length * connection.ConduitType.Labor; }

                    if (connection is TECNetworkConnection)
                    {
                        TECConnectionType type = (connection as TECNetworkConnection).ConnectionType;
                        laborHours += length * type.Labor;
                        foreach (TECAssociatedCost associatedCost in type.AssociatedCosts)
                        { laborHours += associatedCost.Labor; }
                    }
                    else if (connection is TECSubScopeConnection)
                    {
                        foreach (TECConnectionType type in (connection as TECSubScopeConnection).ConnectionTypes)
                        {
                            laborHours += length * type.Labor;
                            foreach (TECAssociatedCost associatedCost in type.AssociatedCosts)
                            {
                                laborHours += associatedCost.Labor;
                            }
                        }
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }

            }
            
            double laborCost =  laborHours * bid.Labor.ElectricalEffectiveRate;
            laborCost += bid.Labor.SubcontractorSubTotal;
            return laborCost;
        }
        /// <summary>
        /// Returns the electrical material and labor costs with escalation 
        /// </summary>
        public static double GetSubcontractorCost(TECBid bid)
        {
            double outCost = 0;
            outCost += bid.SubcontractorLaborCost;
            outCost += bid.ElectricalMaterialCost;
            outCost += outCost * bid.Parameters.SubcontractorEscalation / 100;

            return outCost;
        }
        /// <summary>
        /// Returns the electrical total with markup 
        /// </summary>
        public static double GetSubcontractorSubtotal(TECBid bid)
        {
            double outCost = 0;
            outCost += GetSubcontractorCost(bid);
            outCost += outCost * bid.Parameters.SubcontractorMarkup / 100;
            return outCost;
        }

        /// <summary>
        /// Returns the final sell price 
        /// </summary>
        public static double GetTotalPrice(TECBid bid)
        {
            double outPrice = 0;

            outPrice += GetTECSubtotal(bid);
            outPrice += GetSubcontractorSubtotal(bid);

            return outPrice;
        }
        #region Budgeting
        /// <summary>
        /// Returns the budget price based on the user-assigned values in systems
        /// </summary>
        public static double GetBudgetPrice(TECBid bid)
        {
            double price = 0;
            foreach (TECSystem system in bid.Systems)
            {
                if (system.TotalBudgetPrice >= 0)
                {
                    price += system.TotalBudgetPrice;
                }
            }
            return price;
        }
        #endregion
        #region Metrics
        /// <summary>
        /// Returns the final price per point 
        /// </summary>
        public static double GetPricePerPoint(TECBid bid)
        {
            return (GetTotalPrice(bid) / bid.TotalPointNumber);
        }
        /// <summary>
        /// Returns the Margin based on sell price and cost 
        /// </summary>
        public static double GetMargin(TECBid bid)
        {
            double margin;
            double totalPrice = GetTotalPrice(bid);
            double tecCost = GetTECCost(bid);
            double subCost = GetSubcontractorCost(bid);
            margin = (totalPrice - tecCost - subCost) / totalPrice;
            return margin;
        }
        #endregion


    }
}
