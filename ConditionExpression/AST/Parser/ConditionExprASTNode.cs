using System;
using UnityEngine.Rendering;

namespace ConditionExpression
{
    public abstract class ConditionExprASTNode
    { 
        public ConditionExprAstTypeEnum AstType;
        public abstract bool Evaluate(); 
    }

    public class ConditionExprUnaryNode : ConditionExprASTNode
    {
        public ConditionExprASTNode Operand { get; }

        public ConditionExprUnaryNode(ConditionExprASTNode operand, ConditionExprAstTypeEnum astType)
        { 
            Operand = operand;
            this.AstType = astType;
        }

        public override bool Evaluate()
        {
            if (AstType == ConditionExprAstTypeEnum.Not)
            {
                return !Operand.Evaluate();
            } 
            return false;
        } 
        public override string ToString() => $"({AstType} {Operand})";
         
    }


    public class ConditionExprBinaryNode : ConditionExprASTNode
    {
        public ConditionExprASTNode Left { get; }
        public ConditionExprASTNode Right { get; } 

        public ConditionExprBinaryNode(ConditionExprASTNode left, ConditionExprASTNode right, ConditionExprAstTypeEnum astType)
        {
            Left = left;
            Right = right;
            this.AstType = astType; 
        } 
        public override bool Evaluate()
        { 
            return AstType switch
            {
                ConditionExprAstTypeEnum.And => Left.Evaluate() && Right.Evaluate(),
                ConditionExprAstTypeEnum.Or => Left.Evaluate() || Right.Evaluate(),
                _ => throw new InvalidOperationException($"未知操作符: {AstType}")
            };
        }

        public override string ToString() => $"({Left}  {AstType}  {Right})";
    }


    // 自定义内容节点
    public class ConditionExprCustomNode : ConditionExprASTNode
    {
        public string Content { get; }

        Func<string, bool> customExpression;

        public ConditionExprCustomNode(string content, Func<string, bool> customExpression)
        {
            Content = content;
            this.customExpression = customExpression;
        }

        public override bool Evaluate()
        {
            return customExpression(Content);
        }

        public override string ToString() => Content;
    }
}