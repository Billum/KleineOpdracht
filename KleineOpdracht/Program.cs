using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SolverFoundation;
using Microsoft.SolverFoundation.Common;
using Microsoft.SolverFoundation.Services;


namespace KleineOpdracht
{
    static class Program
    {
        public delegate void LoopSum(SumTermBuilder sum);

        public static Term makeSum(int capacity, LoopSum sumDelegate)
        {
            var sum = new SumTermBuilder(capacity);
            sumDelegate(sum);
            return sum.ToTerm();
        }

        public static Term sums(Decision[] d, int year = -1)
        {
            if (year == -1)
                year = d.Length;
            return makeSum(year, sum =>
            {
                for (int i = 0; i < year; i++)
                    sum.Add(d[i]);
            });
        }

        static void Main(string[] args)
        {
            var context = SolverContext.GetContext();
            var model = context.CreateModel();

            var x = new Decision[6];                // projecten
            var y = new Decision[4];                // leningen
            var z = new Decision[4];                // geld uitzetten
            var w = new Decision[4];                // schuld aan derden

            model.AddDecision(x[0] = new Decision(Domain.RealNonnegative, "Invest_Proj___1"));
            model.AddDecision(x[1] = new Decision(Domain.RealNonnegative, "Invest_Proj___2"));
            model.AddDecision(x[2] = new Decision(Domain.RealNonnegative, "Invest_Proj___3"));
            model.AddDecision(x[3] = new Decision(Domain.RealNonnegative, "Invest_Proj___4"));
            model.AddDecision(x[4] = new Decision(Domain.RealNonnegative, "Invest_Proj___5"));
            model.AddDecision(x[5] = new Decision(Domain.RealNonnegative, "Invest_Proj___6"));

            model.AddDecision(y[0] = new Decision(Domain.RealNonnegative, "Lening________1"));
            model.AddDecision(y[1] = new Decision(Domain.RealNonnegative, "Lening________2"));
            model.AddDecision(y[2] = new Decision(Domain.RealNonnegative, "Lening________3"));
            model.AddDecision(y[3] = new Decision(Domain.RealNonnegative, "Lening________4"));

            model.AddDecision(z[0] = new Decision(Domain.RealNonnegative, "Geld_uitz_____1"));
            model.AddDecision(z[1] = new Decision(Domain.RealNonnegative, "Geld_uitz_____2"));
            model.AddDecision(z[2] = new Decision(Domain.RealNonnegative, "Geld_uitz_____3"));
            model.AddDecision(z[3] = new Decision(Domain.RealNonnegative, "Geld_uitz_____4"));

            model.AddDecision(w[0] = new Decision(Domain.RealNonnegative, "Schuld_afb____1"));
            model.AddDecision(w[1] = new Decision(Domain.RealNonnegative, "Schuld_afb____2"));
            model.AddDecision(w[2] = new Decision(Domain.RealNonnegative, "Schuld_afb____3"));
            model.AddDecision(w[3] = new Decision(Domain.RealNonnegative, "Schuld_afb____4"));

            var a0 = -50 * x[0] - 100 * x[1] - 60 * x[2] - 50 * x[3] - 170 * x[4] - 16 * x[5];
            var a1 = -80 * x[0] - 50 * x[1] - 60 * x[2] - 100 * x[3] - 40 * x[4] - 25 * x[5];
            var a2 = 20 * x[0] - 20 * x[1] - 60 * x[2] - 150 * x[3] + 50 * x[4] - 40 * x[5];
            var a3 = 150 * x[0] + 210 * x[1] + 220 * x[2] + 350 * x[3] + 200 * x[4] + 100 * x[5];

            var a2_2 = -20 * x[1] - 60 * x[2] - 150 * x[3] - 40 * x[5]; // alleen investeringen jaar 3

            model.AddConstraints(null,
                x[0] >= 0, x[1] >= 0, x[2] >= 0,
                x[3] >= 0, x[4] >= 0, x[5] >= 0,
                x[0] <= 1, x[1] <= 1, x[2] <= 1,
                x[3] <= 1, x[4] <= 1, x[5] <= 1,
                y[0] >= 0, y[1] >= 0, y[2] >= 0, y[3] >= 0,
                z[0] >= 0, z[1] >= 0, z[2] >= 0, z[3] >= 0,
                w[0] >= 0, w[1] >= 0, w[2] >= 0, w[3] >= 0
            );

            var restKapitaal_0 = 300 + a0 + y[0] - w[0] - z[0];
            var restKapitaal_1 = 100 + restKapitaal_0 + a1
                                 + y[1] - y[0] * 1.12
                                 - w[1] - 0.11 * (10 - w[0])
                                 - z[1] + 1.08 * z[0];

            model.AddConstraints(null,
                -a0 <= 300
                        + y[0]
                        - w[0]
                        - z[0],
                -a1 <= 100 + restKapitaal_0
                        + y[1] - y[0] * 1.12
                        - w[1] - 0.11 * (10 - sums(w, 1))
                        - z[1] + 1.08 * z[0],
                -a2 <= 200 + restKapitaal_1
                        + y[2] - y[1] * 1.12
                        - w[2] - 0.11 * (10 - sums(w, 2))
                        - z[2] + 1.08 * z[1]
            );

            model.AddConstraints(null,
                y[0] <= 50,
                y[1] <= 50 - 0.2 * a0,
                y[2] <= 50 - 0.2 * (a0 + a1),
                y[3] <= 50 - 0.2 * (a0 + a1 + a2_2)
            );

            model.AddConstraints(null,
                w[0] <= 10,
                w[1] <= 10 + 0.11 * (10 - sums(w, 1)),
                w[2] <= 10 + 0.11 * (10 - sums(w, 2)),
                w[3] <= 10 + 0.11 * (10 - sums(w, 3)),
                //sums(w, 4) >= 10,                           // als schuld moet worden afbetaald
                sums(w, 4) <= 10 * Math.Pow(1.11, 3)
            );

            model.AddGoal("Goal", GoalKind.Maximize,
                a3                                          // inkomsten/uitkering van jaar 4  
                - 1.12 * y[2]// + y[3]                        // lening jaar 3 aflossen (+ lening in jaar 4)
                + 1.08 * z[2]                               // rente uitgezet geld van jaar 3
                - w[3] - 0.11 * (10 - sums(w, 3))           // schuld aflossen van jaar 4 - rente overig schuld aan derden
            );

            Solution solution = context.Solve(new SimplexDirective());
            Console.WriteLine(solution.GetReport());

            // check the solution by simulating
            //Simulate(d2d(x), d2d(y), d2d(z), d2d(w));
        }

        public static double[] d2d(Decision[] d)
        {
            var result = new double[d.Length];
            for (int i = 0; i < d.Length; i++)
                result[i] = d[i].ToDouble();
            return result;
        }

        public static void print(double[] d)
        {
            foreach (var n in d)
                Console.WriteLine(double.IsNaN(n) ? "" : n.ToString());
        }

        public static void Simulate(
            double[] x, //projecten
            double[] y, //lening
            double[] z, //uitzetten
            double[] w  //schuld
            )
        {
            var results = new List<double>();

            var geld1 = 300.0;
            geld1 += y[0];
            geld1 -= z[0];
            geld1 -= w[0];
            var winst1 = -50 * x[0] - 100 * x[1] - 60 * x[2] - 50 * x[3] - 170 * x[4] - 16 * x[5];
            winst1 -= 1.12 * y[0];
            winst1 += 1.08 * z[0];

            var geld2 = 100.0;
            geld2 += winst1;
            geld2 += y[1];
            geld2 -= 1.08 * z[0] + z[1];
            geld2 -= w[1];
            var winst2 = -80 * x[0] - 50 * x[1] - 60 * x[2] - 100 * x[3] - 40 * x[4] - 25 * x[5];
            winst2 -= 1.12 * y[1];
            winst2 += 1.08 * z[1];

            var geld3 = 200.0;
            geld3 += winst2;
            geld3 += y[2];
            geld3 -= 1.08 * z[1] + z[2];
            geld3 -= w[2];
            var winst3 = 20 * x[0] - 20 * x[1] - 60 * x[2] - 150 * x[3] + 50 * x[4] - 40 * x[5];
            winst3 -= 1.12 * y[2];
            winst3 += 1.08 * z[2];

            var geld4 = 150 * x[0] + 210 * x[1] + 220 * x[2] + 350 * x[3] + 200 * x[4] + 100 * x[5];
            geld4 += y[3];
            geld4 -= 1.08 * z[2] + z[3];
            geld4 -= w[3];

            print(new double[]
            {
                geld1, winst1,
                double.NaN,
                geld2, winst2,
                double.NaN,
                geld3, winst3,
                double.NaN,
                geld4
            });
        }
    }
}