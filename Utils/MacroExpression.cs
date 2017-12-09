//***************************************************************
// 文件名:		MacroExpression.cs
// 版权:		Copyright @ 
// 代码实现:	
// 修改人:		
// 描述:		MacroExpression
// 备注:		
//***************************************************************
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Com.Yst.Framework.Utils
{
    public class MacroExpression
    {
        private static Regex RegexArg = new Regex(@"\#\{(?<name>[a-zA-Z_0-9]+)}", RegexOptions.Compiled | RegexOptions.ECMAScript | RegexOptions.IgnoreCase);
        private Dictionary<string, string> _Arguments;

        public MacroExpression()
        {
            _Arguments = new Dictionary<string, string>();
        }

        public void AddMacro(string name, string value)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException("name");
            if (value == null) throw new ArgumentNullException("value");

            _Arguments[name] = value;
        }

        public string Translate(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;
            return RegexArg.Replace(input, Replace);
        }

        private string Replace(Match match)
        {
            return GetValue(match.Groups["name"].Value);
        }

        protected virtual string GetValue(string argName)
        {
            string result = null;
            if (!_Arguments.TryGetValue(argName, out result)) result = string.Format("<?{0}>", argName);
            //string result = null;
            //switch (argName)
            //{
            //    case "date":
            //        result = DateTime.Now.ToShortDateString();
            //        break;
            //    default:
            //        _Arguments.TryGetValue(argName, out result);
            //        break;
            //}
            
            return result;
        }

    }
}
