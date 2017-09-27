using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary.Utilities
{
    public class CostBatch
    {
        private Dictionary<CostType, CostObject> typeDictionary;

        public CostBatch()
        {
            typeDictionary = new Dictionary<CostType, CostObject>();
        }
        public CostBatch(double cost, double labor, CostType type) : this()
        {
            typeDictionary.Add(type, new CostObject(cost, labor));
        }
        public CostBatch(TECCost cost) : this()
        {
            typeDictionary.Add(cost.Type, new CostObject(cost));
        }
        public CostBatch(List<TECCost> costs) : this()
        {
            foreach(TECCost cost in costs)
            {
                AddCost(cost);
            }
        }

        public List<TECCost> AllCosts
        {
            get
            {
                List<TECCost> costs = new List<TECCost>();
                foreach(KeyValuePair<CostType, CostObject> pair in typeDictionary)
                {
                    TECCost cost = new TECCost(pair.Key);
                    cost.Cost = pair.Value.Cost;
                    cost.Labor = pair.Value.Labor;
                    costs.Add(cost);
                }
                return costs;
            }
        }

        public static CostBatch operator +(CostBatch left, CostBatch right)
        {
            CostBatch newCostBatch = new CostBatch();
            newCostBatch.typeDictionary = left.typeDictionary;
            foreach(KeyValuePair<CostType, CostObject> type in right.typeDictionary)
            {
                if (newCostBatch.typeDictionary.ContainsKey(type.Key))
                {
                    newCostBatch.typeDictionary[type.Key].Cost += right.typeDictionary[type.Key].Cost;
                    newCostBatch.typeDictionary[type.Key].Labor += right.typeDictionary[type.Key].Labor;
                }
                else
                {
                    newCostBatch.typeDictionary.Add(type.Key, type.Value);
                }
            }
            return newCostBatch;
        }
        public static CostBatch operator -(CostBatch costBatch)
        {
            CostBatch negativeCostBatch = new CostBatch();
            foreach(KeyValuePair<CostType, CostObject> type in costBatch.typeDictionary)
            {
                negativeCostBatch.typeDictionary[type.Key] = (-type.Value);
            }
            return negativeCostBatch;
        }
        public static CostBatch operator -(CostBatch left, CostBatch right)
        {
            CostBatch newCostBatch = new CostBatch();
            newCostBatch.typeDictionary = left.typeDictionary;
            foreach (KeyValuePair<CostType, CostObject> type in right.typeDictionary)
            {
                if (newCostBatch.typeDictionary.ContainsKey(type.Key))
                {
                    newCostBatch.typeDictionary[type.Key].Cost -= right.typeDictionary[type.Key].Cost;
                    newCostBatch.typeDictionary[type.Key].Labor -= right.typeDictionary[type.Key].Labor;
                }
                else
                {
                    newCostBatch.typeDictionary.Add(type.Key, type.Value);
                }
            }
            return newCostBatch;
        }
        public static CostBatch operator *(CostBatch left, double right)
        {
            CostBatch newCostBatch = new CostBatch();
            foreach(KeyValuePair<CostType, CostObject> type in left.typeDictionary)
            {
                newCostBatch.typeDictionary[type.Key] = (type.Value * right);
            }
            return newCostBatch;
        }

        public double GetCost(CostType type)
        {
            if (typeDictionary.ContainsKey(type))
            {
                CostObject obj = typeDictionary[type];
                return obj.Cost;
            }
            else
            {
                return 0;
            }
        }
        public double GetLabor(CostType type)
        {
            if (typeDictionary.ContainsKey(type))
            {
                CostObject obj = typeDictionary[type];
                return obj.Labor;
            }
            else
            {
                return 0;
            }
        }

        public void AddCost(TECCost cost)
        {
            if (typeDictionary.ContainsKey(cost.Type))
            {
                typeDictionary[cost.Type].Cost += cost.Cost;
                typeDictionary[cost.Type].Labor += cost.Labor;
            }
            else
            {
                typeDictionary.Add(cost.Type, new CostObject(cost));
            }
        }
        public void RemoveCost(TECCost cost)
        {
            if (typeDictionary.ContainsKey(cost.Type))
            {
                typeDictionary[cost.Type].Cost -= cost.Cost;
                typeDictionary[cost.Type].Labor -= cost.Labor;
            }
            else
            {
                typeDictionary.Add(cost.Type, (new CostObject(cost) * -1));
            }
        }

        private class CostObject
        {
            public double Cost, Labor;

            public CostObject(double cost, double labor)
            {
                Cost = cost;
                Labor = labor;
            }

            public CostObject(TECCost cost)
            {
                Cost = cost.Cost;
                Labor = cost.Labor;
            }

            public static CostObject operator +(CostObject left, CostObject right)
            {
                return new CostObject((left.Cost + right.Cost), (left.Labor + right.Labor));
            }

            public static CostObject operator -(CostObject costObject)
            {
                return new CostObject(-costObject.Cost, -costObject.Labor);
            }
            public static CostObject operator -(CostObject left, CostObject right)
            {
                return new CostObject((left.Cost - right.Cost), (left.Labor - right.Labor));
            }

            public static CostObject operator *(CostObject left, double right)
            {
                return new CostObject(left.Cost * right, left.Labor * right);
            }
        }
    }
}
