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

        public static double GetMaterialCost(TECBid bid)
        {
            double cost = 0;
            foreach (TECSystem system in bid.Systems)
            {
                cost += system.MaterialCost;
            }
            return cost;
        }

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
        public static double GetTECSubtotal(TECBid bid)
        {
            double outCost = 0;
            outCost += GetTECCost(bid);

            outCost += outCost * bid.Parameters.Profit / 100;

            return outCost;
        }

        public static double GetSubcontractorCost(TECBid bid)
        {
            double outCost = 0;
            outCost += bid.Labor.SubcontractorSubTotal;
            outCost += bid.ElectricalMaterialCost;
            outCost += outCost * bid.Parameters.SubcontractorEscalation / 100;

            return outCost;
        }
        public static double GetSubcontractorSubtotal(TECBid bid)
        {
            double outCost = 0;
            outCost += GetSubcontractorCost(bid);
            outCost += outCost * bid.Parameters.SubcontractorMarkup / 100;

            return outCost;
        }

        public static double GetTax(TECBid bid)
        {
            double outTax = 0;

            if (!bid.Parameters.IsTaxExempt)
            {
                outTax += .0875 * bid.MaterialCost;
            }

            return outTax;
        }

        public static double GetTotalPrice(TECBid bid)
        {
            double outPrice = 0;

            outPrice += GetTECSubtotal(bid);
            outPrice += GetSubcontractorSubtotal(bid);

            return outPrice;
        }

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

        public static double GetElectricalMaterialCost(TECBid bid)
        {
            double cost = 0;

            foreach (TECConnection conn in bid.Connections)
            {
                var length = conn.Length;
                foreach(TECConnectionType type in conn.ConnectionTypes)
                {
                    cost += length * type.Cost;
                    foreach(TECAssociatedCost associatedCost in type.AssociatedCosts)
                    { cost += associatedCost.Cost; }
                }
            }

            foreach (TECSystem system in bid.Systems)
            {
                foreach (TECEquipment equipment in system.Equipment)
                {
                    foreach (TECSubScope sub in equipment.SubScope)
                    {
                        if (sub.Connection == null)
                        {
                            foreach(TECConnectionType type in sub.ConnectionTypes)
                            { cost += sub.Length * type.Cost; }
                        }
                    }
                }
            } 
            return cost; 
        }

        public static double GetElectricalLaborCost(TECBid bid)
        {
            double labor = 0;

            foreach (TECConnection conn in bid.Connections)
            {
                var length = conn.Length;
                foreach (TECConnectionType type in conn.ConnectionTypes)
                {
                    labor += length * type.Labor;
                    foreach (TECAssociatedCost associatedCost in type.AssociatedCosts)
                    { labor += associatedCost.Cost; }
                }
            }

            foreach (TECSystem system in bid.Systems)
            {
                foreach (TECEquipment equipment in system.Equipment)
                {
                    foreach (TECSubScope sub in equipment.SubScope)
                    {
                        if (sub.Connection == null)
                        {
                            foreach (TECConnectionType type in sub.ConnectionTypes)
                            { labor += sub.Length * type.Labor; }
                        }
                    }
                }
            }
            labor *= bid.Labor.ElectricalRate;
            labor += bid.Labor.SubcontractorSubTotal;
            return labor;
        }
    }
}
