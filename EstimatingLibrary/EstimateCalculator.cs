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
            outCost += GetTECLaborCost(bid);
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

        #region Labor
        /// <summary>
        /// Returns PM labor hours based on points
        /// </summary>
        public static double GetPMPointHours(TECBid bid)
        {
            double hours = bid.TotalPointNumber * bid.Labor.PMCoef;
           
            return hours;
        }
        /// <summary>
        /// Returns total PM labor hours
        /// </summary>
        public static double GetPMTotalHours(TECBid bid)
        {
            double hours = GetPMPointHours(bid);
            hours += bid.Labor.PMExtraHours;
            
            return hours;
        }
        /// <summary>
        /// Returns PM labor cost
        /// </summary>
        public static double GetPMLaborCost(TECBid bid)
        {
            double cost = GetPMTotalHours(bid) * bid.Labor.PMRate;

            return cost;
        }

        /// <summary>
        /// Returns ENG labor hours based on points
        /// </summary>
        public static double GetENGPointHours(TECBid bid)
        {
            double hours = bid.TotalPointNumber * bid.Labor.ENGCoef;

            return hours;
        }
        /// <summary>
        /// Returns total ENG labor hours
        /// </summary>
        public static double GetENGTotalHours(TECBid bid)
        {
            double hours = GetENGPointHours(bid);
            hours += bid.Labor.ENGExtraHours;

            return hours;
        }
        /// <summary>
        /// Returns ENG labor cost
        /// </summary>
        public static double GetENGLaborCost(TECBid bid)
        {
            double cost = GetENGTotalHours(bid) * bid.Labor.ENGRate;

            return cost;
        }

        /// <summary>
        /// Returns PM labor hours based on points
        /// </summary>
        public static double GetCommPointHours(TECBid bid)
        {
            double hours = bid.TotalPointNumber * bid.Labor.CommCoef;

            return hours;
        }
        /// <summary>
        /// Returns total PM labor hours
        /// </summary>
        public static double GetCommTotalHours(TECBid bid)
        {
            double hours = GetCommPointHours(bid);
            hours += bid.Labor.CommExtraHours;

            return hours;
        }
        /// <summary>
        /// Returns PM labor cost
        /// </summary>
        public static double GetCommLaborCost(TECBid bid)
        {
            double cost = GetCommTotalHours(bid) * bid.Labor.CommRate;

            return cost;
        }

        /// <summary>
        /// Returns Soft labor hours based on points
        /// </summary>
        public static double GetSoftPointHours(TECBid bid)
        {
            double hours = bid.TotalPointNumber * bid.Labor.SoftCoef;

            return hours;
        }
        /// <summary>
        /// Returns total Soft labor hours
        /// </summary>
        public static double GetSoftTotalHours(TECBid bid)
        {
            double hours = GetSoftPointHours(bid);
            hours += bid.Labor.SoftExtraHours;

            return hours;
        }
        /// <summary>
        /// Returns Soft labor cost
        /// </summary>
        public static double GetSoftLaborCost(TECBid bid)
        {
            double cost = GetSoftTotalHours(bid) * bid.Labor.SoftRate;

            return cost;
        }

        /// <summary>
        /// Returns Graph labor hours based on points
        /// </summary>
        public static double GetGraphPointHours(TECBid bid)
        {
            double hours = bid.TotalPointNumber * bid.Labor.GraphCoef;

            return hours;
        }
        /// <summary>
        /// Returns total Graph labor hours
        /// </summary>
        public static double GetGraphTotalHours(TECBid bid)
        {
            double hours = GetGraphPointHours(bid);
            hours += bid.Labor.GraphExtraHours;

            return hours;
        }
        /// <summary>
        /// Returns Graph labor cost
        /// </summary>
        public static double GetGraphLaborCost(TECBid bid)
        {
            double cost = GetGraphTotalHours(bid) * bid.Labor.GraphRate;

            return cost;
        }

        /// <summary>
        /// Returns all TEC labor hours
        /// </summary>
        public static double GetTECLaborHours(TECBid bid)
        {
            double outLabor = 0;
            outLabor += GetPMTotalHours(bid);
            outLabor += GetENGTotalHours(bid);
            outLabor += GetCommTotalHours(bid);
            outLabor += GetSoftTotalHours(bid);
            outLabor += GetGraphTotalHours(bid);
            return outLabor;
        }
        /// <summary>
        /// Returns all TEC labor cost
        /// </summary>
        public static double GetTECLaborCost(TECBid bid)
        {
            double outCost = 0;
            outCost += GetPMLaborCost(bid);
            outCost += GetENGLaborCost(bid);
            outCost += GetCommLaborCost(bid);
            outCost += GetSoftLaborCost(bid);
            outCost += GetGraphLaborCost(bid);
            return outCost;
        }

        /// <summary>
        /// Returns the Journeyman electrical labor hours
        /// </summary>
        public static double GetElectricalLaborHours(TECBid bid)
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
                            { laborHours += associatedCost.Labor; }
                        }
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }

            }

            return laborHours;
        }
        /// <summary>
        /// Returns the Journeyman electrical labor cost
        /// </summary>
        public static double GetElectricalLaborCost(TECBid bid)
        {
            double laborHours = GetElectricalLaborHours(bid) * bid.Labor.ElectricalEffectiveRate;

            return laborHours;
        }
        /// <summary>
        /// Returns the electrical super labor hours
        /// </summary>
        public static double GetElectricalSuperLaborHours(TECBid bid)
        {
            double laborHours = GetElectricalLaborHours(bid);
            
            return laborHours / 7;
        }
        /// <summary>
        /// Returns the electrical super labor cost
        /// </summary>
        public static double GetElectricalSuperLaborCost(TECBid bid)
        {
            double laborHours = GetElectricalLaborHours(bid);

            return laborHours / 7;
        }
        /// <summary>
        /// Returns the electrical labor hours of all wire, conduit, and their associated costs 
        /// </summary>
        public static double GetTotalElectricalLaborHours(TECBid bid)
        {
            double laborCost = GetElectricalLaborHours(bid) + GetElectricalSuperLaborHours(bid);
            return laborCost;
        }
        /// <summary>
        /// Returns the electrical labor cost of all wire, conduit, and their associated costs 
        /// </summary>
        public static double GetTotalElectricalLaborCost(TECBid bid)
        {
            double laborCost = GetElectricalLaborCost(bid) + GetElectricalSuperLaborCost(bid);
            return laborCost;
        }

        /// <summary>
        /// Returns the subcontractor labor hours
        /// </summary>
        public static double GetSubcontractorLaborHours(TECBid bid)
        {
            double laborHours = GetTotalElectricalLaborHours(bid);
            return laborHours;
        }
        /// <summary>
        /// Returns the subcontractor labor cost
        /// </summary>
        public static double GetSubcontractorLaborCost(TECBid bid)
        {
            double laborHours = GetTotalElectricalLaborCost(bid);
            return laborHours;
        }
        #endregion
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
