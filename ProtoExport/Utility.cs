using System.Text.RegularExpressions;

namespace GameFrameX.ProtoExport
{
    internal static class Utility
    {
        public static readonly char[] splitChars = { ' ', '\t' };

        public static readonly string[] splitNotesChars = { "//" };

        /// <summary>
        /// 定义正则表达式来匹配以大写字母开头，不允许全部由大写字母组成，不允许存在连续的大写字母，并允许后续字符是小写字母或数字
        /// </summary>
        public const string CamelCasePattern = @"^(?![A-Z]+$)(?!.*[A-Z]{2})[A-Z][a-zA-Z0-9]*$";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsCamelCase(string str)
        {
            return Regex.IsMatch(str, CamelCasePattern);
        }

        public static string ConvertType(string type)
        {
            string typeCs = "";
            switch (type)
            {
                case "int16":
                    typeCs = "short";
                    break;
                case "uint16":
                    typeCs = "ushort";
                    break;
                case "int32":
                case "sint32":
                case "sfixed32":
                    typeCs = "int";
                    break;
                case "uint32":
                case "fixed32":
                    typeCs = "uint";
                    break;
                case "int64":
                case "sint64":
                case "sfixed64":
                    typeCs = "long";
                    break;
                case "uint64":
                case "fixed64":
                    typeCs = "ulong";
                    break;
                case "bytes":
                    typeCs = "byte[]";
                    break;
                case "string":
                    typeCs = "string";
                    break;
                case "bool":
                    typeCs = "bool";
                    break;
                case "double":
                    typeCs = "double";
                    break;
                case "float":
                    typeCs = "float";
                    break;
                default:
                    if (type.StartsWith("map<"))
                    {
                        var typeMap = type.Replace("map", string.Empty).Replace("<", string.Empty).Replace(">", string.Empty).Split(',');
                        if (typeMap.Length == 2)
                        {
                            typeCs = $"Dictionary<{ConvertType(typeMap[0])}, {ConvertType(typeMap[1])}>";
                            break;
                        }
                    }

                    typeCs = type;
                    break;
            }

            return typeCs;
        }
    }
}