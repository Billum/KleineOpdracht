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
    class Program
    {
        public delegate void LoopSum(SumTermBuilder sum);

        public static Term makeSum(int capacity, LoopSum sumDelegate)
        {
            var sum = new SumTermBuilder(capacity);
            sumDelegate(sum);
            return sum.ToTerm();
        }

        public static Term sumSigma(Decision[] d, int max)
        {
            return makeSum(max, sum =>
            {
                for (int i = 1; i <= max; i++)
                    sum.Add(d[i]);
            });
        }
        public static Term sumSigma(Decision[,] d, int max)
        {
            return makeSum(max, sum =>
            {
                for (int i = 1; i < d.GetLength(0) ; i++ )
                    for (int j = 1; j <= max; j++)
                        sum.Add(d[i,j]);
            });
        }

        static void Main(string[] args)
        {
            SolverContext context = SolverContext.GetContext();
            Model model = context.CreateModel();

            Decision[] x = new Decision[6]; // projecten
            Decision[] y = new Decision[3]; // lening
            Decision[] z = new Decision[3]; // geld uitzetten
            Decision[] w = new Decision[3]; // schuld aan derden

            model.AddDecision(x[0] = new Decision(Domain.RealNonnegative, "project___1"));
            model.AddDecision(x[1] = new Decision(Domain.RealNonnegative, "project___2"));
            model.AddDecision(x[2] = new Decision(Domain.RealNonnegative, "project___3"));
            model.AddDecision(x[3] = new Decision(Domain.RealNonnegative, "project___4"));
            model.AddDecision(x[4] = new Decision(Domain.RealNonnegative, "project___5"));
            model.AddDecision(x[5] = new Decision(Domain.RealNonnegative, "project___6"));

            model.AddDecision(y[0] = new Decision(Domain.RealNonnegative, "lening____1"));
            model.AddDecision(y[1] = new Decision(Domain.RealNonnegative, "lening____2"));
            model.AddDecision(y[2] = new Decision(Domain.RealNonnegative, "lening____3"));

            model.AddDecision(z[0] = new Decision(Domain.RealNonnegative, "geld_uitz_1"));
            model.AddDecision(z[1] = new Decision(Domain.RealNonnegative, "geld_uitz_2"));
            model.AddDecision(z[2] = new Decision(Domain.RealNonnegative, "geld_uitz_3"));

            model.AddDecision(w[0] = new Decision(Domain.RealNonnegative, "schuld____1"));
            model.AddDecision(w[1] = new Decision(Domain.RealNonnegative, "schuld____2"));
            model.AddDecision(w[2] = new Decision(Domain.RealNonnegative, "schuld____3"));

            var a0 = -50 * x[0] - 100 * x[1] - 60 * x[2] - 50 * x[3] - 170 * x[4] - 16 * x[5]; // jaar 1
            var a1 = -80 * x[0] - 50 * x[1] - 60 * x[2] - 100 * x[3] - 40 * x[4] - 25 * x[5];  // jaar 2
            var a2 = 20 * x[0] - 20 * x[1] - 60 * x[2] - 150 * x[3] + 50 * x[4] - 40 * x[5];   // jaar 3

            model.AddConstraints("projecten", 
                x[0] <= 1, x[1] <= 1, x[2] <= 1,
                x[3] <= 1, x[4] <= 1, x[5] <= 1);

            model.AddConstraints("leningen", 
                y[0] <= 50,
                y[1] <= 50 + 0.20 * (300 * a0 + y[0]),
                y[2] <= 50 + 0.20 * (400 * a1 + y[1] + z[1]));

            model.AddConstraints("geld_uitzetten",
                z[0] <= 300 + y[0] - (300 * a0 + w[0]),
                z[1] <= 100 + y[1] + 1.08 * z[0] - (100 * a1 + w[1]),
                z[2] <= 200 + y[2] + 1.08 * z[1] - (200 * a2 + w[2]));

            model.AddConstraints("schuld_aan_derden",
                w[0] + w[1] + w[2] <= 10,
                w[0] <= 300 + y[0]               - (300 * a0 + z[0]),
                w[1] <= 100 + y[1] + 1.08 * z[0] - (100 * a1 + (10 - w[0]) + z[1]),
                w[2] <= 200 + y[2] + 1.08 * z[1] - (200 * a2 + (10 - (w[0] + w[1])) + z[2]));

            model.AddConstraints("groter_dan_nul",
                x[0] >= 0, x[1] >= 0, x[2] >= 0, x[3] >= 0, x[4] >= 0, x[5] >= 0,
                y[0] >= 0, y[1] >= 0, y[2] >= 0,
                z[0] >= 0, z[1] >= 0, z[2] >= 0,
                w[0] >= 0, w[1] >= 0, w[2] >= 0);

            model.AddConstraints("uitgaven_uitkeringen",
                a0 <= 300,
                a1 <= 400 - a0,
                a2 <= 600 - (a0 + a1));

            model.AddGoal("winst", GoalKind.Maximize,
                150 * x[0] + 210 * x[1] + 220 * x[2] + 350 * x[3] + 200 * x[4] + 100 * x[5] 
                - (y[0] + y[1] + y[2]) * 1.12 
                - (z[0] + z[1] + z[2]) * 1.08 
                - ((w[0] + w[1] + w[2]) + (10 - (w[0] + w[1] + w[2])) * 0.11));

            Solution sol = context.Solve(new SimplexDirective());
            Console.WriteLine(sol.GetReport());
        }
    }
}