using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace System
{
    internal static class StringExtensions
    {
       
        private static char[] _separators = new []
                                                {
                                                    '/', '\\', '>', '<',
                                                    '\t', ' ', ';', ':',
                                                    '[', ']', '{', '}',
                                                    '(', '@', '=', '?', ')'
                                                };

        public static string ReplaceHttpSeparators(this string s)
        {
            string result = s;
            _separators.Each( (ch) => result = result.Replace(ch, '_'));
            return result;
        }
    }

}
