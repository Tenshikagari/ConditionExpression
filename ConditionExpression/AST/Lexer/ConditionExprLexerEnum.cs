using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ConditionExpression
{ 
    public enum ConditionExprTokenType
    {
        LeftParenthesis,  // (
        RightParenthesis, // )
        And,              // and
        Or,               // or  
        Not,              // not
        Custom,           // 自定义内容
        EOF,               // 结束
        Illegality,               // 识别不到
    }

    public static class ConditionExprKeyWord
    {
        public const char LeftParenthesis = '(';  
        public const char RightParenthesis = ')';  
        public const string And= "and";               
        public const string Or= "or";               
        public const string Not= "not";                        
    } 
}