using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3dprinter.Components.Coords
{
    public class CoordsCalc
    {
        public Queue<Coords> GetCircle()
        {
            Queue <Coords> queue= new Queue<Coords>();
            double[] x = { 0, 0, 2 * Math.Sqrt(3) };
            double[] y = { 3, 7, 5 };
            double[] z = new double[3];
            double[] pc = { 1, 6, 3 };
            double r = 4;
            Coords iC = new Coords(0,0,0);

            for (int i = 0; i <= 1000; i++)
            {
                double t = i / 10.0;
                pc[0] = Math.Sin(t) + Math.Sqrt(3);
                pc[1] = Math.Cos(t) + 5;
                pc[2] = 4 * Math.Sin(t / 20.0);
    
                for(int j=0 ; j <= 2; j++)
                {
                        z[j] = pc[2] + Math.Sqrt(Math.Pow(r, 2) - Math.Pow(y[j] - pc[1], 2) - Math.Pow(x[j] - pc[0], 2));
                }

                int xT = Convert.ToInt32(z[0] * 100);
                int yT = Convert.ToInt32(z[1] * 100);
                int zT = Convert.ToInt32(z[2] * 100);

                Coords coord = new Coords(xT - iC.X, yT - iC.Y, zT - iC.Z);
                queue.Enqueue(coord);
                iC = coord;
            }
            return queue;
        }
    }

}
