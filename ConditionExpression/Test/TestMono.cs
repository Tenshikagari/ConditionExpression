using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ConditionExpression
{

    public class AdvancedConditionTest : MonoBehaviour
    {
        void Start()
        {
            ttt(); 
        }

        [ContextMenu("TTT")]
        void ttt()
        {
            var testCases = new Dictionary<string, string>
        {
            {"简单 AND", "条件1 and 条件2"},
            {"简单 OR", "条件1 or 条件2"},
            {"NOT 操作", "not 条件1"},
            {"复杂嵌套", "((A and B) or (C and not D)) and E"},
            {"多重 NOT", "not not 条件1"},
            {"多重 NOT2", "((not A or B) or not (C and not D)) and not E"},
            //{"错误 括号不匹配1", " (not A or B)   )"},
            //{"错误 括号不匹配2", " ((not A or B) "},
            //{"错误 括号不匹配3", " ( not () A or B) "} ,
            //{"错误 1目错误 错误", " A not B "},
            //{"错误 2目错误 错误", " A and "},
            //{"错误 token错误 错误", " A And B "},
        };
             
            foreach (KeyValuePair<string, string> testCase in testCases)
            {

                StringBuilder sb = new();

                sb.Append($"测试: {testCase.Key} - 表达式: {testCase.Value}");
                sb.AppendLine();
                var list =  ConditionExprLexer.Tokenize(testCase.Value, (content) =>{ return true; });
                //Lexer.LogTokeList(list);
 
                //continue;
                try
                {
                    var conditionEvaluator = new ConditionExpression(testCase.Value, 
                        (content) => { return true; }, 
                        (custom) => {   return true; }
                        );
  

                    sb.Append($"解析成功,表达式的值为{conditionEvaluator.Evaluate()}");  
                    Debug.Log(sb);
                    conditionEvaluator.LogAstCompile();
                    conditionEvaluator.LogEvaluate();
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"解析失败: {e.Message}");
                }

            }
        }




        [ContextMenu("RRR")]
        // 完整的测试用例
        void ttt2()
        {
            // 类型#额外参数
            var config = "(power#>100 and openSeverDay#>10000)  and not task#1 ";
            var conditionEvaluator = new ConditionExpression(config,checkTokenIsLegal,executeCustom );
            conditionEvaluator.Evaluate();
            conditionEvaluator.LogAstCompile();
            conditionEvaluator.LogEvaluate();
        }

        bool checkTokenIsLegal(string token)
        {
            return token.Contains("#");  // 简单示例，所有包含#的token都合法。 
        }

        bool executeCustom(string customData)
        {
            var stringList = customData.Split('#', StringSplitOptions.RemoveEmptyEntries);
            var type = stringList[0];
            var parm = stringList[1];

            switch (type)
            {
                case "power":
                    {
                        string op = parm[0].ToString();
                        var curPower = 80; // 假设当前战力
                        var targetPower =  int.Parse(parm.Substring(1)); 
                        if (op == ">")
                        {
                            return curPower > targetPower;
                        }
                        else if (op == "<")
                        {
                            return curPower < targetPower;
                        }
                        else if (op == "=")
                        {
                            return curPower == targetPower; 
                        } 
                        return false;
                    }
                case "openSeverDay":
                {
                        var op = parm[0].ToString();
                        var curOpenSeverDay = 10; // 假设当前开服天数
                        var targetOpenSeverDay = int.Parse(parm.Substring(1));
                        if (op == ">")
                        {
                            return curOpenSeverDay > targetOpenSeverDay;
                        }
                        else if (op == "<")
                        {
                            return curOpenSeverDay < targetOpenSeverDay;
                        }
                        else if (op == "=")
                        {
                            return curOpenSeverDay == targetOpenSeverDay;
                        }
                        return false;
                    }
                case "task":
                    {
                        int taskId = int.Parse(parm);
                        return true; // 假设任务完成
                    } 
                default:
                    break;
            }

            return false;

        }
    }
}