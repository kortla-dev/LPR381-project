using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LPR381Project.Tokens;

namespace LPR381Project.Common
{
    /// <summary>
    /// This enum represents supported constraint types.
    /// <para>NOTE: Do not use this enum directly. Retrieve enum variants with the <see cref="Sign"/> class instead.</para>
    /// <para>This enum is used for specifying various types of constraints, including:</para>
    ///
    /// <list type="bullet">
    /// <item><description>Lesser: Represents a lesser than condition</description></item>
    /// <item><description>LesserEq: Represents a lesser than or equal to condition</description></item>
    /// <item><description>Greater: Represents a greater than condition</description></item>
    /// <item><description>GreaterEq: Represents a greater than or equal to condition</description></item>
    /// <item><description>Equal: Represents an equal to condition</description></item>
    /// <item><description>Urs: Represents a custom or special condition</description></item>
    /// </list>
    /// </summary>
    internal enum ConstraintEnum
    {
        LesserEq,
        GreaterEq,
        Equal,
        Bin,
        Int,
        NonNegative,
    }

    /// <summary>
    /// This class is used to get enum variants from <see cref="ConstraintEnum"/>
    /// </summary>
    internal class Constraint
    {
        public static ConstraintEnum LesserEq => ConstraintEnum.LesserEq;
        public static ConstraintEnum GreaterEq => ConstraintEnum.GreaterEq;
        public static ConstraintEnum Equal => ConstraintEnum.Equal;
        public static ConstraintEnum Bin => ConstraintEnum.Bin;
        public static ConstraintEnum Int => ConstraintEnum.Int;
        public static ConstraintEnum NonNegative => ConstraintEnum.NonNegative;
    }

    internal enum ProblemKindEnum
    {
        Min,
        Max,
    }

    internal class ProblemKind
    {
        public static ProblemKindEnum Min => ProblemKindEnum.Min;
        public static ProblemKindEnum Max => ProblemKindEnum.Max;
    }

    /// <summary>
    /// Class representing a tableau
    /// </summary>
    internal class Tableau
    {
        public ProblemKindEnum kind { get; set; }

        // Dynamically sized matrix
        private List<List<double>> table;
        private Dictionary<string, ConstraintEnum> constraints;
        public int ConCount { get; set; }

        public Tableau(List<Token> tokens)
        {
            List<List<Token>> problemTokens = new();
            this.ConCount = 1;

            int subListPtr = 0;
            problemTokens.Add(new List<Token>());
            foreach (Token token in tokens)
            {
                if (token.Kind != TokenKind.NewLine)
                {
                    problemTokens[subListPtr].Add(token);
                }
                else
                {
                    problemTokens.Add(new List<Token>());
                    subListPtr++;
                }
            }

            this.kind = Tableau.GetObjectiveKind(problemTokens[0][0]);
            this.table = new List<List<double>>();

            int ptr = 0;
            this.table.Add(new List<double>());
            this.constraints = new Dictionary<string, ConstraintEnum>();
            foreach (Token token in problemTokens[0])
            {
                if (ptr == 0)
                {
                    ptr++;
                    continue;
                }

                this.constraints.Add($"x{ptr}", GetRestriction(problemTokens[^1][ptr - 1]));
                this.table[0].Add(double.Parse(token.Value));
                ptr++;
            }
            this.table[0].Add(0.0); // adding rhs value

            // skip index 0 and n-1
            for (int i = 1; i < problemTokens.Count - 1; i++)
            {
                // this.table.Add(new List<double>());
                List<double> nums = new();
                ConstraintEnum sign = Constraint.Equal; // default
                foreach (Token token in problemTokens[i])
                {
                    if (token.Kind != TokenKind.Number)
                    {
                        //this.constraints.Add(ConCount.ToString(), Tableau.GetConstraint(token));
                        //this.ConCount++;
                        sign = Tableau.GetConstraint(token);
                        continue;
                    }

                    nums.Add(double.Parse(token.Value));
                }

                this.AddConstraint(nums, sign);
            }
        }

        private static ProblemKindEnum GetObjectiveKind(Token token)
        {
            return (token.Kind == TokenKind.Min) ? ProblemKindEnum.Min : ProblemKindEnum.Max;
        }

        private static ConstraintEnum GetRestriction(Token token)
        {
            return token.Kind switch
            {
                TokenKindEnum.Bin => Constraint.Bin,
                TokenKindEnum.Int => Constraint.Int,
                _ => Constraint.NonNegative,
            };
        }

        private static ConstraintEnum GetConstraint(Token token)
        {
            return token.Kind switch
            {
                TokenKindEnum.LessEq => Constraint.LesserEq,
                TokenKindEnum.GreaterEq => Constraint.GreaterEq,
                _ => Constraint.Equal,
            };
        }

        /// <summary>
        /// Add objective function to the tableau.
        /// </summary>
        /// <param name="nums">Numbers representign the objective function</param>
        public void AddObjective(List<double> nums)
        {
            // HACK: might not be the best way to do this
            if (this.table.Count != 0)
            {
                Console.Error.WriteLine(
                    "Error: Tableau already has elements cannot add another objective function"
                );
                Environment.Exit(1);
            }

            this.table.Add(nums);
        }

        /// <summary>
        /// Add constraint to the tableau.
        ///
        /// NOTE: this should only be used when creating the tableau
        /// </summary>
        /// <param name="nums">Numbers representing the constraint</param>
        /// <param name="sign">Sign of the constraint</param>
        public void AddConstraint(List<double> nums, ConstraintEnum sign)
        {
            // TODO: discuss if constraints should be stored as "con n" or just "n"

            // Check for "normal" amount of constraints
            if (this.constraints.Count > 100)
            {
                // lets be reasonable
                Console.Error.WriteLine("Error: Cannot add more than 100 constraints.");
                Environment.Exit(1);
            }

            this.constraints.Add(this.ConCount.ToString(), sign);

            // adds new constraint column to other rows
            int tableSize = this.table.Count;
            for (int i = 0; i < tableSize; i++)
            {
                // insert before rhs value
                this.table[i].Insert(this.table[i].Count - 1, 0.0);
            }

            // make constraint len match other rows
            int matchLen = this.table[0].Count;
            // number or other constraints (therefore -1 for how many to account for)
            int diff = matchLen - nums.Count;
            for (int i = 0; i < diff - 1; i++)
            {
                nums.Insert(nums.Count - 1, 0.0);
            }
            
            // if e constraint
            if (this.constraints[this.ConCount.ToString()] == ConstraintEnum.GreaterEq)
            {
                for(int i=0; i<nums.Count; i++)
                {
                    nums[i] *= -1;
                }
            }
            this.ConCount++;

            nums.Insert(nums.Count - 1, 1.0);

            this.table.Add(nums);
        }

        // TBD
        public void AddNewConstraint(List<double> nums, ConstraintEnum sign)
        {
            // Check for "normal" amount of constraints
            if (this.constraints.Count > 100)
            {
                // lets be reasonable
                Console.Error.WriteLine("Error: Cannot add more than 100 constraints.");
                Environment.Exit(1);
            }

            this.constraints.Add(this.ConCount.ToString(), sign);
            this.ConCount++;

            this.table.Add(nums);
        }

        // HACK: is adding the restrictions one at a time the best?
        /// <summary>
        /// Add sign restriction for decision variables
        /// </summary>
        /// <param name="variable"></param>
        /// <param name="restriction"></param>
        public void AddRestriction(string variable, ConstraintEnum restriction)
        {
            if (this.constraints.ContainsKey(variable))
            {
                Console.Error.WriteLine($"Error: Restriction already set for {variable}.");
            }
            this.constraints.Add(variable, restriction);
        }
    }
}
