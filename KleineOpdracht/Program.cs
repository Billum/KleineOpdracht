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

        static void Main(string[] args)
        {
            SolverContext context = SolverContext.GetContext();
            Model model = context.CreateModel();
       
            Decision[,] x = new Decision[7, 5];
            Decision[] y = new Decision[5];
            Decision[] z = new Decision[5];
            Decision[] w = new Decision[5];

            model.AddDecision(x[1, 1] = new Decision(Domain.RealNonnegative, "x(1,1)"));
            model.AddDecision(x[1, 2] = new Decision(Domain.RealNonnegative, "x(1,2)"));
            model.AddDecision(x[1, 3] = new Decision(Domain.RealNonnegative, "x(1,3)"));
            model.AddDecision(x[1, 4] = new Decision(Domain.RealNonnegative, "x(1,4)"));
            model.AddDecision(x[2, 1] = new Decision(Domain.RealNonnegative, "x(2,1)"));
            model.AddDecision(x[2, 2] = new Decision(Domain.RealNonnegative, "x(2,2)"));
            model.AddDecision(x[2, 3] = new Decision(Domain.RealNonnegative, "x(2,3)"));
            model.AddDecision(x[2, 4] = new Decision(Domain.RealNonnegative, "x(2,4)"));
            model.AddDecision(x[3, 1] = new Decision(Domain.RealNonnegative, "x(3,1)"));
            model.AddDecision(x[3, 2] = new Decision(Domain.RealNonnegative, "x(3,2)"));
            model.AddDecision(x[3, 3] = new Decision(Domain.RealNonnegative, "x(3,3)"));
            model.AddDecision(x[3, 4] = new Decision(Domain.RealNonnegative, "x(3,4)"));
            model.AddDecision(x[4, 1] = new Decision(Domain.RealNonnegative, "x(4,1)"));
            model.AddDecision(x[4, 2] = new Decision(Domain.RealNonnegative, "x(4,2)"));
            model.AddDecision(x[4, 3] = new Decision(Domain.RealNonnegative, "x(4,3)"));
            model.AddDecision(x[4, 4] = new Decision(Domain.RealNonnegative, "x(4,4)"));
            model.AddDecision(x[5, 1] = new Decision(Domain.RealNonnegative, "x(5,1)"));
            model.AddDecision(x[5, 2] = new Decision(Domain.RealNonnegative, "x(5,2)"));
            model.AddDecision(x[5, 3] = new Decision(Domain.RealNonnegative, "x(5,3)"));
            model.AddDecision(x[5, 4] = new Decision(Domain.RealNonnegative, "x(5,4)"));
            model.AddDecision(x[6, 1] = new Decision(Domain.RealNonnegative, "x(6,1)"));
            model.AddDecision(x[6, 2] = new Decision(Domain.RealNonnegative, "x(6,2)"));
            model.AddDecision(x[6, 3] = new Decision(Domain.RealNonnegative, "x(6,3)"));
            model.AddDecision(x[6, 4] = new Decision(Domain.RealNonnegative, "x(6,4)"));

            model.AddDecision(y[1] = new Decision(Domain.RealNonnegative, "y(1)"));
            model.AddDecision(y[2] = new Decision(Domain.RealNonnegative, "y(2)"));
            model.AddDecision(y[3] = new Decision(Domain.RealNonnegative, "y(3)"));
            model.AddDecision(y[4] = new Decision(Domain.RealNonnegative, "y(4)"));

            model.AddDecision(z[1] = new Decision(Domain.RealNonnegative, "z(1)"));
            model.AddDecision(z[2] = new Decision(Domain.RealNonnegative, "z(2)"));
            model.AddDecision(z[3] = new Decision(Domain.RealNonnegative, "z(3)"));
            model.AddDecision(z[4] = new Decision(Domain.RealNonnegative, "z(4)"));

            model.AddDecision(w[1] = new Decision(Domain.RealNonnegative, "w(1)"));
            model.AddDecision(w[2] = new Decision(Domain.RealNonnegative, "w(2)"));
            model.AddDecision(w[3] = new Decision(Domain.RealNonnegative, "w(3)"));
            model.AddDecision(w[4] = new Decision(Domain.RealNonnegative, "w(4)"));

            var maxlening = 50;

<<<<<<< HEAD
            model.AddConstraints("Tabel", x[1, 1] == -50,  x[1, 2] == -80,  x[1, 3] == 20,   x[1, 4] == 150,
                                          x[2, 1] == -100, x[2, 2] == -50,  x[2, 3] == -20,  x[2, 4] == 210,
                                          x[3, 1] == -60,  x[3, 2] == -60,  x[3, 3] == -60,  x[3, 4] == 220,
                                          x[4, 1] == -50,  x[4, 2] == -100, x[4, 3] == -150, x[4, 4] == 350,
                                          x[5, 1] == -170, x[5, 2] == -40,  x[5, 3] == 50,   x[5, 4] == 200,
                                          x[6, 1] == -16,  x[6, 2] == -25,  x[6, 3] == -40,  x[6, 4] == 100);
            model.AddConstraints("Lening", z[1] <= maxlening,
                                           z[2] <= maxlening,
                                           z[3] <= maxlening,
                                           z[4] <= maxlening);
            model.AddConstraint("Schuld aan derden", w[1] + w[2] + w[3] + w[4] <= 10);
=======
            model.AddConstraints("Lening",
                z[1] <= maxlening,
                z[2] <= maxlening,
                z[3] <= maxlening,
                z[4] <= maxlening);
            
            model.AddConstraint("Schuld aan derden",
                w[1] + w[2] + w[3] + w[4] <= 10);

            Action<SumTermBuilder,int> sumYears = (sum, year) =>
            {
                for (int i = 1; i < 7; i++)
                    for (int j = 1; j <= year; j++)
                        sum.Add(x[i, j]);
            };

            model.AddConstraint("test",
                makeSum(6, sum =>
                    {
                        for (int i = 1; i < 4; i++)
                            sumYears(sum, i);
                        for (int i = 1; i < 7; i++)
                            sum.Add(x[i, 1]);
                    }));
>>>>>>> 4ccc90f94100ae8f5e0451eb5fb846b8fbf0ad3f
        }
    }
}