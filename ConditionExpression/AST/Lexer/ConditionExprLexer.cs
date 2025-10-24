using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConditionExpression
{
    public static class ConditionExprLexer
    {
        public static List<ConditionExprToken> Tokenize(string input, Func<string, bool> customTokenRecognizer = null)
        {
            var position = 0;
            var tokens = new List<ConditionExprToken>();
            var tempSB = new StringBuilder();
            while (position < input.Length)
            {
                var currentChar = input[position];

                if (char.IsWhiteSpace(currentChar))
                {
                    position++;
                    continue;
                }

                // 单个字符的关键字
                if (currentChar == ConditionExprKeyWord.LeftParenthesis)
                {
                    tokens.Add(new ConditionExprToken(ConditionExprTokenType.LeftParenthesis, ConditionExprKeyWord.LeftParenthesis.ToString(), position));
                    position++;
                    continue;
                }
                else if (currentChar == ConditionExprKeyWord.RightParenthesis)
                {
                    tokens.Add(new ConditionExprToken(ConditionExprTokenType.RightParenthesis, ConditionExprKeyWord.RightParenthesis.ToString(), position));
                    position++;
                    continue;
                }

                //成词的关键字 
                tempSB.Clear();
                var startPosition = position;
                while (position < input.Length && !char.IsWhiteSpace(input[position])
                       && input[position] != ConditionExprKeyWord.LeftParenthesis && input[position] != ConditionExprKeyWord.RightParenthesis )
                {
                    tempSB.Append(input[position]);
                    position++;
                }

                var word = tempSB.ToString();
                ConditionExprTokenType tokenType;
                if (word == ConditionExprKeyWord.Not)
                {
                    tokenType = ConditionExprTokenType.Not;
                }
                else if (word == ConditionExprKeyWord.And)
                {
                    tokenType = ConditionExprTokenType.And;
                }
                else if (word == ConditionExprKeyWord.Or)
                {
                    tokenType = ConditionExprTokenType.Or;
                }
                else if (word == ConditionExprKeyWord.Not)
                {
                    tokenType = ConditionExprTokenType.Not;
                }
                else if (customTokenRecognizer != null && customTokenRecognizer(word))
                {
                    tokenType = ConditionExprTokenType.Custom;
                }
                else
                {
                    tokenType = ConditionExprTokenType.Illegality;
                    ConditionExpressionUtil.Error($" 位置{startPosition}的{word}  是非法 token");
                }
                tokens.Add(new ConditionExprToken(tokenType, word, startPosition));
            } 
            tokens.Add(new ConditionExprToken(ConditionExprTokenType.EOF, "", position));
            return tokens;
        }

        static public void LogTokeList(List<ConditionExprToken> Tokenize)
        {
            StringBuilder sb = new();
            foreach (var token in Tokenize)
            {
                sb.Append(token.ToString());
                sb.Append(" ");
            }
            ConditionExpressionUtil.Log(sb.ToString());
        }
    }
}
