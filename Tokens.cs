using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPR381Project.Tokens
{
    internal enum TokenKindEnum
    {
        ProblemKind,
        Number,
        Constraint,
    }

    internal class TokenKind
    {
        public static TokenKindEnum ProblemKind => TokenKindEnum.ProblemKind;
        public static TokenKindEnum Number => TokenKindEnum.Number;
        public static TokenKindEnum Constraint => TokenKindEnum.Constraint;
    }
    internal class Token
    {
        public TokenKindEnum Kind { get; set; }
        public string Value { get; set; }

        public Token(TokenKindEnum kind, string value)
        {
            this.Kind = kind;
            this.Value = value;
        }
    }
}
