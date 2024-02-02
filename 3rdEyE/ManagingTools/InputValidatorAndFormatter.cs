using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _3rdEyE.ManagingTools
{
    public class InputValidatorAndFormatter
    {
        public static string FormateMobileNumber_BD(string inputString, out bool isValid, out string formatError)
        {
            var outputString = inputString;
            if (string.IsNullOrEmpty(outputString))
            {
                isValid = false;
                formatError = " is null";
            }
            else
            {
                outputString = outputString.Replace(" ", "").Replace("+", "").Replace("-", "");
                outputString = outputString.StartsWith("8") ? outputString.TrimStart('8') : outputString;
                outputString = !outputString.StartsWith("0") ? "0" + outputString : outputString;

                if (outputString.Length != 11)
                {
                    isValid = false;
                    formatError = " is invalid";
                }
                else
                {
                    isValid = true;
                    formatError = "";
                }
            }
            return outputString;
        }
    }
}