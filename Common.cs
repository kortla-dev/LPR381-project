using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        Lesser,
        LesserEq,
        Greater,
        GreaterEq,
        Equal,
        Urs,
        Bin,
        Int,
    }

    /// <summary>
    /// This class is used to get enum variants from <see cref="ConstraintEnum"/>
    /// </summary>
    internal class Constraint
    {
        public static ConstraintEnum Lesser => ConstraintEnum.Lesser;
        public static ConstraintEnum LesserEq => ConstraintEnum.LesserEq;
        public static ConstraintEnum Greater => ConstraintEnum.Greater;
        public static ConstraintEnum GreaterEq => ConstraintEnum.GreaterEq;
        public static ConstraintEnum Equal => ConstraintEnum.Equal;
        public static ConstraintEnum Urs => ConstraintEnum.Urs;
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

        public Tableau(ProblemKindEnum kind, List<List<double>> table)
        {
            this.kind = kind ;
            this.table = new List<List<double>>();
            this.constraints = new Dictionary<string, ConstraintEnum>();
        }

        /// <summary>
        /// Add objective function to the tableau.
        /// </summary>
        /// <param name="nums">Numbers representign the objective function</param>
        void AddObjective(List<double> nums)
        {
            // HACK: might not be the best way to do this
            if (this.table.Count != 0)
            {
                Console.Error.WriteLine("Error: Tableau already has elements cannot add another objective function");
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
        void AddConstraint(List<double> nums, ConstraintEnum sign)
        {
            // TODO: discuss if constraints should be stored as "con n" or just "n"
            
            // Check for "normal" amount of constraints
            if (this.constraints.Count > 100)
            {
                // lets be reasonable
                Console.Error.WriteLine("Error: Cannot add more than 100 constraints.");
                Environment.Exit(1);
            }

            string conNum = this.constraints.Count.ToString();
            this.constraints.Add(conNum, sign);
        }
        
        // TBD
        void AddNewConstraint(List<double> nums) {}

       // HACK: is adding the restrictions one at a time the best? 
        /// <summary>
        /// Add sign restriction for decision variables
        /// </summary>
        /// <param name="variable"></param>
        /// <param name="restriction"></param>
        void AddRestriction(string variable, ConstraintEnum restriction)
        {
            if (this.constraints.ContainsKey(variable))
            {
                Console.Error.WriteLine($"Error: Restriction already set for {variable}.");
            } 
            this.constraints.Add(variable, restriction);
        }
    }
}
