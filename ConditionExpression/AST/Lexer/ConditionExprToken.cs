using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConditionExpression
{ 
    public struct ConditionExprToken
    {
        public ConditionExprTokenType Type;
        public string Value;
        public int Position;

        public ConditionExprToken(ConditionExprTokenType type, string value, int position)
        {
            Type = type;
            Value = value;
            Position = position;
        }

        public override string ToString()
        {
            if (this.Type == ConditionExprTokenType.Custom)
            {
                return Value;
            }
            else if (this.Type == ConditionExprTokenType.Illegality)
            {
                return $"【{Type}:{Value}】";
            }
            else
            {
                return this.Type.ToString();
            }
        }

    }
}
