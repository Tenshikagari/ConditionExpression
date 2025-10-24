using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConditionExpression 
{
    public enum ConditionExprAstTypeEnum
    {
        None,
        And,Or,NoBinOpr,
        Not,NoUnopr, 
        Custom,
    }
     
    public class ConditionExprPraserDef
    {
        public static readonly Dictionary<ConditionExprTokenType, int> BinaryPriority = new Dictionary<ConditionExprTokenType, int>
        {
            { ConditionExprTokenType.Or, 1 },
            { ConditionExprTokenType.And, 2 }
        }; 
        public const int UnaryPriority = 3; 
    } 

}
