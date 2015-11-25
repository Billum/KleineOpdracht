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

            var x = new Decision[6];
            var y = new Decision[4];
            var z = new Decision[4];
            var w = new Decision[4];

            model.AddDecision(x[0] = new Decision(Domain.RealNonnegative, "Project_____1"));
            model.AddDecision(x[1] = new Decision(Domain.RealNonnegative, "Project_____2"));
            model.AddDecision(x[2] = new Decision(Domain.RealNonnegative, "Project_____3"));
            model.AddDecision(x[3] = new Decision(Domain.RealNonnegative, "Project_____4"));
            model.AddDecision(x[4] = new Decision(Domain.RealNonnegative, "Project_____5"));
            model.AddDecision(x[5] = new Decision(Domain.RealNonnegative, "Project_____6"));

            model.AddDecision(y[0] = new Decision(Domain.RealNonnegative, "Lening______1"));
            model.AddDecision(y[1] = new Decision(Domain.RealNonnegative, "Lening______2"));
            model.AddDecision(y[2] = new Decision(Domain.RealNonnegative, "Lening______3"));
            model.AddDecision(y[3] = new Decision(Domain.RealNonnegative, "Lening______4"));

            model.AddDecision(z[0] = new Decision(Domain.RealNonnegative, "Geld_uitz___1"));
            model.AddDecision(z[1] = new Decision(Domain.RealNonnegative, "Geld_uitz___2"));
            model.AddDecision(z[2] = new Decision(Domain.RealNonnegative, "Geld_uitz___3"));
            model.AddDecision(z[3] = new Decision(Domain.RealNonnegative, "Geld_uitz___4"));

            model.AddDecision(w[0] = new Decision(Domain.RealNonnegative, "Schuld______1"));
            model.AddDecision(w[1] = new Decision(Domain.RealNonnegative, "Schuld______2"));
            model.AddDecision(w[2] = new Decision(Domain.RealNonnegative, "Schuld______3"));
            model.AddDecision(w[3] = new Decision(Domain.RealNonnegative, "Schuld______4"));

            var a0 = -50 * x[0] - 100 * x[1] - 60 * x[2] - 50 * x[3] - 170 * x[4] - 16 * x[5];
            var a1 = -80 * x[0] - 50 * x[1] - 60 * x[2] - 100 * x[3] - 40 * x[4] - 25 * x[5];
            var a2 = 20 * x[0] - 20 * x[1] - 60 * x[2] - 150 * x[3] + 50 * x[4] - 40 * x[5];
            var a3 = 150 * x[0] + 210 * x[1] + 220 * x[2] + 350 * x[3] + 200 * x[4] + 100 * x[5];

            model.AddConstraints(null,
                x[0] >= 0, x[1] >= 0, x[2] >= 0,
                x[3] >= 0, x[4] >= 0, x[5] >= 0,
                x[0] <= 1, x[1] <= 1, x[2] <= 1,
                x[3] <= 1, x[4] <= 1, x[5] <= 1,
                y[0] >= 0, y[1] >= 0, y[2] >= 0, y[3] >= 0,
                z[0] >= 0, z[1] >= 0, z[2] >= 0, z[3] >= 0
                //w[0] >= 0, w[1] >= 0, w[2] >= 0, w[3] >= 0
                );

            model.AddConstraints(null,
                -a0 <= 300 + y[0],// - w[0],
                -a1 <= 100 + (300 + a0) + y[1] - y[0] * 1.12,// - w[1] - 0.11 * (10 - sums(w, 1)),
                -a2 <= 200 + (300 + a0) + (100 + a1) + y[2] - y[1] * 1.12// - w[2] - 0.11 * (10 - sums(w, 2))
                );

            model.AddConstraints(null,
                y[0] <= 50,
                y[1] <= 50 - 0.2 * a0,
                y[2] <= 50 - 0.2 * (a0 + a1),
                y[3] <= 50 - 0.2 * (a0 + a1 + a2)
                );

            //model.AddConstraints(null,
            //w[0] + w[1] + w[2] + w[3] <= 10
            //);

            model.AddGoal("Goal", GoalKind.Maximize,
                a3                                  // inkomsten jaar 4  
                - 1.12 * y[2] + y[3]                // lening jaar 3 aflossen + lening jaar
                                                    //- w[3] - 0.11 * (10 - sums(w,3))    // schuld aan derden aflossen - rente schuld aan derden
                );

            Solution solution = context.Solve(new SimplexDirective());
            Console.WriteLine(solution.GetReport());

            // check the solution by simulating
            Simulate(x[0].ToDouble(),
                x[1].ToDouble(),
                x[2].ToDouble(),
                x[3].ToDouble(),
                x[4].ToDouble(),
                x[5].ToDouble(),
                1.12 * y[2].ToDouble() - y[3].ToDouble());
        }
        public static void Simulate(
            double x0,
            double x1,
            double x2,
            double x3,
            double x4,
            double x5,
            double sub = 0.0
            )
        {
            var geld1 = 300;
            Console.WriteLine(geld1);
            var kosten1 = -50 * x0 - 100 * x1 - 60 * x2 - 50 * x3 - 170 * x4 - 16 * x5;
            Console.WriteLine(kosten1);
            var geld2 = 100 + (geld1 + kosten1);
            Console.WriteLine(geld2);
            var kosten2 = -80 * x0 - 50 * x1 - 60 * x2 - 100 * x3 - 40 * x4 - 25 * x5;
            Console.WriteLine(kosten2);
            var geld3 = 200 + (geld2 + kosten2);
            Console.WriteLine(geld3);
            var kosten3 = 20 * x0 - 20 * x1 - 60 * x2 - 150 * x3 + 50 * x4 - 40 * x5;
            Console.WriteLine(kosten3);
            var geld4 = 150 * x0 + 210 * x1 + 220 * x2 + 350 * x3 + 200 * x4 + 100 * x5;
            Console.WriteLine(geld4);
            Console.WriteLine(geld4 - sub);
        }
    }
}