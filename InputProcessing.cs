using System;
using System.Collections.Generic;
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
            // TODO: implement tokenization
            return this.Tokens;
        }
    }
}
