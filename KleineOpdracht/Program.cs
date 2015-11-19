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

            Decision project1 = new Decision(Domain.RealNonnegative, "Project 1");
            Decision project2 = new Decision(Domain.RealNonnegative, "Project 2");
            Decision project3 = new Decision(Domain.RealNonnegative, "Project 3");
            Decision project4 = new Decision(Domain.RealNonnegative, "Project 4");
            Decision project5 = new Decision(Domain.RealNonnegative, "Project 5");
            Decision project6 = new Decision(Domain.RealNonnegative, "Project 6");
            Decision jaar1 = new Decision(Domain.IntegerNonnegative, "Jaar 1");
            Decision jaar2 = new Decision(Domain.IntegerNonnegative, "Jaar 2");
            Decision jaar3 = new Decision(Domain.IntegerNonnegative, "Jaar 3");
            Decision jaar4 = new Decision(Domain.IntegerNonnegative, "Jaar 4");

            Decision[,] x = new Decision[7,5];
            x[1, 1] = new Decision(Domain.RealNonnegative, "(1,1)");
            x[1, 2] = new Decision(Domain.RealNonnegative, "(1,2)");
            x[1, 3] = new Decision(Domain.RealNonnegative, "(1,3)");
            x[1, 4] = new Decision(Domain.RealNonnegative, "(1,4)");
            x[2, 1] = new Decision(Domain.RealNonnegative, "(2,1)");
            x[2, 2] = new Decision(Domain.RealNonnegative, "(2,2)");
            x[2, 3] = new Decision(Domain.RealNonnegative, "(2,3)");
            x[2, 4] = new Decision(Domain.RealNonnegative, "(2,4)");
            x[3, 1] = new Decision(Domain.RealNonnegative, "(3,1)");
            x[3, 2] = new Decision(Domain.RealNonnegative, "(3,2)");
            x[3, 3] = new Decision(Domain.RealNonnegative, "(3,3)");
            x[3, 4] = new Decision(Domain.RealNonnegative, "(3,4)");
            x[4, 1] = new Decision(Domain.RealNonnegative, "(4,1)");
            x[4, 2] = new Decision(Domain.RealNonnegative, "(4,2)");
            x[4, 3] = new Decision(Domain.RealNonnegative, "(4,3)");
            x[4, 4] = new Decision(Domain.RealNonnegative, "(4,4)");
            x[5, 1] = new Decision(Domain.RealNonnegative, "(5,1)");
            x[5, 2] = new Decision(Domain.RealNonnegative, "(5,2)");
            x[5, 3] = new Decision(Domain.RealNonnegative, "(5,3)");
            x[5, 4] = new Decision(Domain.RealNonnegative, "(5,4)");
            x[6, 1] = new Decision(Domain.RealNonnegative, "(6,1)");
            x[6, 2] = new Decision(Domain.RealNonnegative, "(6,2)");
            x[6, 3] = new Decision(Domain.RealNonnegative, "(6,3)");
            x[6, 4] = new Decision(Domain.RealNonnegative, "(6,4)");
            Decision y = new Decision(Domain.RealNonnegative, "Geld uitzetten");
            Decision z = new Decision(Domain.RealNonnegative, "Lening");
            Decision w = new Decision(Domain.RealNonnegative, "Schuld aan derden");


            model.AddDecisions(y,z,w);
        }
    }
}
