using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ConditionExpression 
{
    class ConditionExpressionUtil
    {

        static bool withEvaluate = false;

        public static string getASTNodeTreeLog(ConditionExprASTNode node,bool withEvaluate = false)
        {
            ConditionExpressionUtil.withEvaluate = withEvaluate;
            List<string> lines = new List<string>();
            BuildTreeLines(node, lines, "", "");
            return string.Join("\n", lines); 
        }
         

        private static void BuildTreeLines(ConditionExprASTNode node, List<string> lines, string prefix, string childrenPrefix)
        {
            // 当前节点显示
            string nodeText = node switch
            {
                ConditionExprBinaryNode binary => $"[{binary.AstType}]",
                ConditionExprUnaryNode unary => $"[{unary}]",
                ConditionExprCustomNode custom => $"\"{custom.Content}\"",
                _ => "[错误节点]"
            };

            if (!ConditionExpressionUtil.withEvaluate)
            {
                lines.Add(prefix + nodeText);
            }
            else
            {
                lines.Add(prefix + nodeText + (!node.Evaluate() ? "【x】" :"【√】"));
            }
            
            var children = GetChildren(node);
            for (int i = 0; i < children.Count; i++)
            {
                bool isLast = i == children.Count - 1;
                string newPrefix = childrenPrefix + (isLast ? "└── " : "├── ");
                string newChildrenPrefix = childrenPrefix + (isLast ? "    " : "│   ");
                BuildTreeLines(children[i], lines, newPrefix, newChildrenPrefix);
            }
        }

        private static List<ConditionExprASTNode> GetChildren(ConditionExprASTNode node) => node switch
        {
            ConditionExprBinaryNode binary => new List<ConditionExprASTNode> { binary.Left, binary.Right },
            ConditionExprUnaryNode unary => new List<ConditionExprASTNode> { unary.Operand },
            _ => new List<ConditionExprASTNode>()
        };



        static public void Error(string error )
        {
            UnityEngine.Debug.LogError(error);
        }

        static public void Log(string log)
        {
            UnityEngine.Debug.Log(log);
        }
    }
}
