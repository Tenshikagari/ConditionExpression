using System;
using System.Collections.Generic;
using UnityEngine;

namespace ConditionExpression
{
    public class ConditionExprParser
    {
        private readonly List<ConditionExprToken> _tokens;
        private int _current;
        Func<string, bool> customExpression;

        public ConditionExprParser(List<ConditionExprToken> tokens, Func<string, bool> customExpression)
        {
            _tokens = tokens;
            _current = 0;
            this.customExpression = customExpression;
        }
         
 
        public ConditionExprASTNode Parse()
        {
            var result = SubExpr(0);  
            if (!IsAtEnd())
            {
                throw new ParseException($"意外的token: {CurrentToken().Type} 在位置 {CurrentToken().Position}");
            } 
            return result;
        }

        private ConditionExprASTNode Primaryexp()
        {
            if (Match(ConditionExprTokenType.LeftParenthesis))
            {
                var expr = SubExpr(0);  
                Consume(ConditionExprTokenType.RightParenthesis, "括号没闭合，期望 ')'");
                return expr;
            }
            if (Match(ConditionExprTokenType.Custom))
            {
                return new ConditionExprCustomNode(Previous().Value, customExpression);
            }


            throw new ParseException($"期望表达式，得到: {CurrentToken().Type} 在位置 {CurrentToken().Position}");
        }

        private ConditionExprASTNode SubExpr(int limit)
        {
            ConditionExprASTNode left;  
            if (IsUnaryOperator(CurrentToken().Type))
            {
                var opType = GetUnoprType(CurrentToken().Type);
                Next(); 
                var operand = SubExpr(limit);
                left = new ConditionExprUnaryNode(operand, opType);
            }
            else
            { 
                left = Primaryexp();
            }

            while (!IsAtEnd() && IsBinaryOperator(CurrentToken().Type))
            {
                var opToken = CurrentToken();
                var opPriority = GetBinaryPriority(opToken.Type);
 
                if (opPriority <= limit)
                    break;

                Next(); 
                var right = SubExpr(opPriority);
                left = new ConditionExprBinaryNode(left, right, GetBinoprType(opToken.Type));
            }

            return left;
        }
         

        private bool IsAtEnd() => _current >= _tokens.Count || CurrentToken().Type == ConditionExprTokenType.EOF;
        private ConditionExprToken CurrentToken() => _tokens[_current];
        private ConditionExprToken Previous() => _tokens[_current - 1];

        private bool Match(ConditionExprTokenType type)
        {
            if (!IsAtEnd() && CurrentToken().Type == type)
            {
                _current++;
                return true;
            }
            return false;
        }

        private void Consume(ConditionExprTokenType type, string message)
        {
            if (!Match(type))
                throw new ParseException($"{message}，得到: {CurrentToken().Type} 在位置 {CurrentToken().Position}");
        }

        private void Next()
        {
            if (!IsAtEnd()) _current++;
        }

 
        private int GetBinaryPriority(ConditionExprTokenType type)
        {
            return ConditionExprPraserDef.BinaryPriority.TryGetValue(type, out int priority) ? priority : 0;
        }

        private bool IsBinaryOperator(ConditionExprTokenType type) => ConditionExprPraserDef.BinaryPriority.ContainsKey(type);
        private bool IsUnaryOperator(ConditionExprTokenType type) => type == ConditionExprTokenType.Not;

        private ConditionExprAstTypeEnum GetBinoprType(ConditionExprTokenType tokenType)
        {
            return tokenType switch
            {
                ConditionExprTokenType.And => ConditionExprAstTypeEnum.And,
                ConditionExprTokenType.Or => ConditionExprAstTypeEnum.Or,
                _ => ConditionExprAstTypeEnum.NoBinOpr
            };
        }

        private ConditionExprAstTypeEnum GetUnoprType(ConditionExprTokenType tokenType)
        {
            return tokenType switch
            {
                ConditionExprTokenType.Not => ConditionExprAstTypeEnum.Not,
                _ => ConditionExprAstTypeEnum.NoUnopr
            };
        }
    }

    public class ParseException : Exception
    {
        public ParseException(string message) : base(message) { }
    }
}