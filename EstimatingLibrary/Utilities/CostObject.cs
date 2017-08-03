using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary.Utilities
{
    public class CostObject
    {
        public double Cost, Labor;
        public CostType Type;

        public CostObject(double cost, double labor, CostType type = CostType.None)
        {
            Cost = cost;
            Labor = labor;
            Type = type;
        }

        public static CostObject operator +(CostObject left, CostObject right)
        {
            if (left.Type == right.Type)
            {
                return new CostObject((left.Cost + right.Cost), (left.Labor + right.Labor), left.Type);
            }
            else throw new ArithmeticException("Cost types don't match.");
        }

        public static CostObject operator -(CostObject left, CostObject right)
        {
            if (left.Type == right.Type)
            {
                return new CostObject((left.Cost - right.Cost), (left.Labor - right.Labor), left.Type);
            }
            else throw new ArithmeticException("Cost types don't match.");
        }
    }
}
