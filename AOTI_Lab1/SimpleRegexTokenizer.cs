
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
namespace AOTI_Lab1
{
    public class SimpleRegexTokenizer
    {
        private List<TokenDefinition> _tokenDefinitions;

        public SimpleRegexTokenizer()
        {
            _tokenDefinitions = new List<TokenDefinition>();

            _tokenDefinitions.Add(new TokenDefinition(TokenType.For, "^for"));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.OpenParenthesis, "^\\("));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.CloseParenthesis, "^\\)"));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.OpenBrace, "^{"));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.CloseBrace, "^}"));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.Int, "^int"));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.Float, "^float"));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.Double, "^double"));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.Var, "^[a-zA-Z][a-zA-Z0-9]*"));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.Equals, "^="));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.IntNumber, "^[0-9]+"));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.FloatNumber, "^[+-]?([1-9]*[.])?[0-9]+"));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.ArrayElement, "^[a-zA-Z][a-zA-Z0-9]*\\[[1-9][0-9]*\\]"));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.Semicolon, "^;"));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.LessOrEven, "^>="));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.GreaterOrEven, "^<="));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.LessThan, "^<"));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.GreaterThan, "^>"));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.Plus, "^\\+"));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.Minus, "^-"));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.Multiply, "^\\*"));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.Divide, "^/"));
        }


        public IEnumerable<OutputToken> Tokenize(string text)
        {
            var tokens = new List<OutputToken>();

            string remainingText = text;

            while (!string.IsNullOrWhiteSpace(remainingText))
            {
                var match = FindMatch(remainingText);
                if (match.IsMatch)
                {
                    tokens.Add(new OutputToken(match.TokenType, match.Value));
                    remainingText = match.RemainingText;
                }
                else
                {
                    if (IsWhitespace(remainingText))
                    {
                        remainingText = remainingText.Substring(1);
                    }
                    else
                    {
                        var invalidTokenMatch = CreateInvalidTokenMatch(remainingText);
                        tokens.Add(new OutputToken(invalidTokenMatch.TokenType, invalidTokenMatch.Value));
                        remainingText = invalidTokenMatch.RemainingText;
                    }
                }
            }

            tokens.Add(new OutputToken(TokenType.SequenceTerminator, string.Empty));

            return tokens;
        }

        private TokenMatch FindMatch(string text)
        {
            foreach (var tokenDefinition in _tokenDefinitions)
            {
                var match = tokenDefinition.Match(text);
                if (match.IsMatch)
                    return match;
            }

            return new TokenMatch() { IsMatch = false };
        }

        private bool IsWhitespace(string text) 
        {
            return Regex.IsMatch(text, "^\\s+");
        }

        private TokenMatch CreateInvalidTokenMatch(string text)
        {
            var match = Regex.Match(text, "(^\\S+\\s)|^\\S+");
            if (match.Success)
            {
                return new TokenMatch()
                {
                    IsMatch = true,
                    RemainingText = text.Substring(match.Length),
                    TokenType = TokenType.Invalid,
                    Value = match.Value.Trim()
                };
            }

            throw new Exception("Failed to generate invalid token");
        }
    }
}
