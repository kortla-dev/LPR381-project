using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LPR381Project.Common;
using LPR381Project.InputProcessing;
using LPR381Project.SensitivityAnalysis.Menus;
using LPR381Project.SensitivityAnalysis.Preliminaries;
using LPR381Project.Simplex.Primal;

namespace LPR381Project
{
    internal class Program
    {
        static void Main()
        {
            double[,] mat =
            {
                { 1, 0, 1 },
                { 2, -2, -1 },
                { 3, 0, 0 },
            };

            // TODO: use lexer to tokenize and validate input file contents then pass to tableau ctor
            var lexer = new Lexer();
            var tokens = lexer.GetTokens();

            var table = new Tableau(tokens);
            double[][] initial = table.table.Select(innerList => innerList.ToArray()).ToArray();

            var newTable = new List<List<double>>();

            var running = true;

            while (running)
            {
                Console.WriteLine(
                    "1. Primal Simplex\n"
                        + "2. Branch and Bound Simplex\n"
                        + "3. Cutting Plane Algorithm\n"
                        + "4. Branch and Bound Knapsack\n"
                        + "0. Exit\n"
                );
                Console.Write("Enter a number: ");

                // NOTE: input can be null but will be handled by the default case
                string userInput = Console.ReadLine();

                Console.WriteLine();

                // TODO: call respective algorithm and pass tableau
                switch (userInput)
                {
                    case "0":
                        Environment.Exit(0);
                        break;

                    case "1":
                        var algo = new PrimalSimplex(table);
                        algo.Solve();
                        break;

                    case "2":
                        Console.WriteLine("branch and bound simplex");
                        break;

                    case "3":
                        Console.WriteLine("cutting plane algorithm");
                        break;

                    case "4":
                        Console.WriteLine("branch and bound knapsack");
                        break;

                    default:
                        // clears console and inform user of invalid input
                        Console.Clear();
                        Console.WriteLine("Invalid input please try again");
                        continue;
                }

                // exit loop after processing valid input
                running = false;
            }

            running = true;

            while (running)
            {
                Console.Write("Proceed to Sensitivity Analysis(y/n): ");
                var keyInfo = Console.ReadKey();

                switch (keyInfo.Key)
                {
                    case ConsoleKey.Y:
                        running = false;
                        break;

                    case ConsoleKey.N:
                        Environment.Exit(0);
                        break;

                    default:
                        Console.Clear();
                        Console.WriteLine("Invalid input please try again");
                        break;
                }
            }

            Console.Clear(); // clear console before prompting user
            // TODO: prompt user with available actions

            running = true;

            var prelims = new Prelims(table, initial);

            while (running)
            {
                Console.WriteLine("1. Shadow Prices\n" + "2. Change RHS value\n" + "0. Exit\n");

                Console.WriteLine(prelims.NewTable());

                Console.Write("Enter a number: ");

                string input = Console.ReadLine();
                Console.WriteLine();

                switch (input)
                {
                    case "0":
                        Environment.Exit(0);
                        break;

                    case "1":
                        Menus.ShadowPrices(prelims);
                        break;

                    case "2":
                        Menus.ChangeRhs(prelims);
                        Console.WriteLine(prelims.NewTable());

                        Console.ReadKey();
                        Console.Clear();
                        break;

                    default:
                        // clears console and inform user of invalid input
                        Console.Clear();
                        Console.WriteLine("Invalid input please try again");
                        continue;
                }
            }
        }
    }
}
