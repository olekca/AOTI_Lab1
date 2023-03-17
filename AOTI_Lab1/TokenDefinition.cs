using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AOTI_Lab1
{
    public class TokenDefinition
    {
        private Regex _regex;
        private readonly TokenType _returnsToken;

        public TokenDefinition(TokenType returnsToken, string regexPattern)
        {
            _regex = new Regex(regexPattern, RegexOptions.IgnoreCase);
            _returnsToken = returnsToken;
        }

        public TokenMatch Match(string inputString)
        {
            var match = _regex.Match(inputString);
            if (match.Success)
            {
                string remainingText = string.Empty;
                if (match.Length != inputString.Length)
                    remainingText = inputString.Substring(match.Length);

                return new TokenMatch()
                {
                    IsMatch = true,
                    RemainingText = remainingText,
                    TokenType = _returnsToken,
                    Value = match.Value
                };
            }
            else
            {
                return new TokenMatch() { IsMatch = false };
            }

        }
    }

    public class OutputToken
    {
        public TokenType TokenType { get; set; }
        public string Value { get; set; }
        public OutputToken(TokenType tokenType)
        {
            TokenType = tokenType;
            Value = string.Empty;
        }

        public OutputToken(TokenType tokenType, string value)
        {
            TokenType = tokenType;
            Value = value;
        }

        public OutputToken Clone()
        {
            return new OutputToken(TokenType, Value);
        }
    }
}
