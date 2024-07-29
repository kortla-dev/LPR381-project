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
    internal enum SignEnum
    {
        Lesser,
        LesserEq,
        Greater,
        GreaterEq,
        Equal,
        Urs,
    }

    /// <summary>
    /// This class is used to get enum variants from <see cref="SignEnum"/>
    /// </summary>
    internal class Sign
    {
        public static SignEnum Lesser => SignEnum.Lesser;
        public static SignEnum LesserEq => SignEnum.LesserEq;
        public static SignEnum Greater => SignEnum.Greater;
        public static SignEnum GreaterEq => SignEnum.GreaterEq;
        public static SignEnum Equal => SignEnum.Equal;
        public static SignEnum Urs => SignEnum.Urs;
    }

    /// <summary>
    /// Class representing a tableau
    /// </summary>
    internal class Tableau
    {
        // Dynamically sized matrix
        private List<List<double>> table;
        private Dictionary<string, double> constraints;

        public Tableau()
        {
            this.table = new List<List<double>>();
            this.constraints = new Dictionary<string, double>();
        }

        /// <summary>
        /// Add constraint to the tableau
        /// </summary>
        /// <param name="nums">Numbers representing the constraint</param>
        /// <param name="sign">Sign of the constraint</param>
        void add_con(List<double> nums, SignEnum sign) { }
    }
}
