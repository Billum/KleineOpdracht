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
        static void Main(string[] args)
        {
            SolverContext context = SolverContext.GetContext();
            Model model = context.CreateModel();

            Decision[,] x = new Decision[7,5];
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

            model.AddConstraints("Lening", z[1] <= maxlening,
                                                    z[2] <= maxlening,
                                                    z[3] <= maxlening,
                                                    z[4] <= maxlening);
            model.AddConstraint("Schuld aan derden", w[1] + w[2] + w[3] + w[4] <= 10);
        }
    }
}