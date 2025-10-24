using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ConditionExpression 
{
    public class ConditionExpression
    {
        private ConditionExprASTNode _ast;
        private string _expression;

        Func<string, bool> customExpression;
        Func<string, bool> customTokenRecognizer = null;

        public ConditionExpression(string expression,Func<string, bool> customTokenRecognizer, Func<string, bool> customExpression)
        {
            this.customTokenRecognizer = customTokenRecognizer;
            this.customExpression = customExpression;
            var tokens = ConditionExprLexer.Tokenize(expression, customTokenRecognizer);
            var parser = new ConditionExprParser(tokens, customExpression);
            _ast = parser.Parse();
            _expression = expression;
        } 

        public bool Evaluate()
        {
            return _ast.Evaluate();
        }
         
        public string AstCompileInfo()
        {
            var logStr = _ast?.ToString();
            var treeStr = ConditionExpressionUtil.getASTNodeTreeLog(_ast);
            return ($"原表达式:{this._expression} \n 【括号打印】:\n {logStr} \n  【树模式】:\n{treeStr}");
        }

        public void LogAstCompile()
        { 
            ConditionExpressionUtil.Log(AstCompileInfo());
        }
         
        public string EvaluateInfo()
        {
            var treeStr = ConditionExpressionUtil.getASTNodeTreeLog(_ast, true);
            return ($"原表达式:{this._expression} \n " +
                $"执行结果:{Evaluate()}" +
                $"【展开每步执行结果】:\n{treeStr}");
        }

        public void LogEvaluate()
        {
            ConditionExpressionUtil.Log(EvaluateInfo());
        }
    }
}
