using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LPR381Project.Tokens;

namespace LPR381Project.InputProcessing
{
    /// <summary>
    /// The <see cref="Lexer"/> is used to process the input files content and convert it into <see cref="Token"/> instances.
    /// </summary>
    internal class Lexer
    {
        private List<Token> Tokens;

        public Lexer()
        {
            this.Tokens = new List<Token>();
        }

        /// <summary>
        /// Retrieves the list of tokens.
        /// </summary>
        /// <returns>
        /// Return <see cref="List"/> of <see cref="Token"/>.
        /// </returns>
        public List<Token> GetTokens()
        {
            var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var inputFile = Path.Combine(desktopPath, "input.txt");

            // read file
            var inputFileContents = File.ReadAllLines(inputFile);

            // Check if file is empty
            if (inputFileContents.Length == 0)
            {
                Console.Error.WriteLine("Error: Input file is empty.");
                Environment.Exit(1);
            }

            // seperate objective from the rest of the input
            string objStr = inputFileContents[0].ToLower();
            string[] objArr = [.. objStr.Split(' ')];

            // Sign restrictions
            string restStr = inputFileContents[^1].ToLower();
            string[] restArr = [.. restStr.Split(' ')];

            // Constraints
            string[] conArr = inputFileContents
                .Skip(1)
                .Take(inputFileContents.Length - 2)
                .ToArray();

            // check for objective kind
            Token? objKind = Token.BuildTokenProblemKind(objArr[0]);

            // check if objective kind is valid
            if (objKind == null)
            {
                Console.Error.WriteLine("Error: Invalid token.");
                Environment.Exit(1);
            }

            this.Tokens.Add(objKind);
            // separate objective kind
            objArr = objArr.Skip(1).ToArray();

            foreach (var num in objArr)
            {
                Token? res = Token.BuildTokenNumber(num);

                if (res == null)
                {
                    Console.Error.WriteLine(
                        "Error: Invalid format, make sure that each number has one or less opperators"
                    );
                    Environment.Exit(1);
                }
                else
                {
                    this.Tokens.Add(res);
                }
            }
            // end of obj function
            this.Tokens.Add(Token.BuildTokenNewLine());

            // constraints
            foreach (var con in conArr)
            {
                var cleanCon = con.Split(' ');

                foreach (var item in cleanCon)
                {
                    Token? numRes = Token.BuildTokenNumber(item);

                    if (numRes != null)
                    {
                        this.Tokens.Add(numRes);

                        continue;
                    }

                    Token? resSign = Token.BuildTokenSign(item);

                    if (resSign == null)
                    {
                        Console.Error.WriteLine("test");
                        Console.Error.WriteLine($"Error: invalid input {item}");
                        Environment.Exit(1);
                    }
                    else
                    {
                        this.Tokens.Add(resSign);
                    }
                }

                this.Tokens.Add(Token.BuildTokenNewLine());
            }

            // decision variable sign restrictions
            foreach (var item in restArr)
            {
                Token? res = Token.BuildTokenRestriction(item);

                if (res == null)
                {
                    Console.Error.WriteLine("Error: invalid restriction " + item);
                }
                else
                {
                    this.Tokens.Add(res);
                }
            }

            return this.Tokens;
        }
    }
}
