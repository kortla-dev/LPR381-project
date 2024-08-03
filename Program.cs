using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LPR381Project.Common;
using LPR381Project.InputProcessing;

namespace LPR381Project
{
    internal class Program
    {
        static void Main()
        {
            // TODO: use lexer to tokenize and validate input file contents then pass to tableau ctor
            var lexer = new Lexer();
            var tokens = lexer.GetTokens();
            
            var running = true;
            
            while (running)
            {
                Console.WriteLine("1. Primal Simplex\n" +
                                  "2. Revised Primal Simplex\n" +
                                  "3. Branch and Bound Simplex\n" +
                                  "4. Cutting Plane Algorithm\n" +
                                  "5. Branch and Bound Knapsack\n" +
                                  "0. Exit\n");
                Console.Write("Enter a number: ");
                
                // NOTE: input can be null but will be handled by the default case
                string userInput = Console.ReadLine();
                
                // TODO: call respective algorithm and pass tableau
                switch (userInput)
                {
                    case "0":
                        Environment.Exit(0);
                        break;
                    
                    case "1":
                        Console.WriteLine("primal simplex");
                        break;
                    
                    case "2":
                        Console.WriteLine("revised simplex");
                        break;
                    
                    case "3":
                        Console.WriteLine("branch and bound simplex");
                        break;
                    
                    case "4":
                        Console.WriteLine("cutting plane algorithm");
                        break;
                    
                    case "5":
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
            
            Console.Clear(); // clear console before prompting user
            // TODO: prompt user if they would like to do sensitivity analysis
        }
    }
}
