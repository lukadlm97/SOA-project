using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.SOA.CoreClient.Helpers
{
    public static class TickResurseGenerator
    {
        public static double[] CreateArray(int count)
        {
            double[] array = new double[count];

            for (int i = 0; i < count; i++)
                array[i] = GetRandomNumber(2.5620, 2.5640);

            return array;
        }

        public static double GetRandomNumber(double minimum, double maximum)
        {
            Random random = new Random();
            return random.NextDouble() * (maximum - minimum) + minimum;
        }
    }
}
