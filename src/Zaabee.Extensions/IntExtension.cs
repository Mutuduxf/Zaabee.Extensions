using System;
using System.Collections.Generic;
using System.Text;
using Zaabee.Extensions.Commons;

namespace Zaabee.Extensions
{
    public static class IntExtension
    {
        public static string ToString(this int dec, NumerationSystem numerationSystem) =>
            dec.ToString((int) numerationSystem);
        
        public static string ToString(this int dec, int fromBase)
        {
            var stack = new Stack<int>();
            var sb = new StringBuilder();
            
            if (dec < 0)
            {
                sb.Append('-');
                dec = Math.Abs(dec);
            }

            while (dec > 0)
            {
                stack.Push(dec % fromBase);
                dec /= fromBase;
            }

            while (stack.Count > 0) sb.Append(Consts.LetterAndDigit[stack.Pop()]);
            return sb.ToString();
        }
    }
}