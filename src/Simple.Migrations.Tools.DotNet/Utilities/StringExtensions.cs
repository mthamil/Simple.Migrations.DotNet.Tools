using System.Text.RegularExpressions;

namespace Simple.Migrations.Tools.DotNet.Utilities
{
    public static class StringExtensions
    {
        public static string ToPascalCase(this string s) => 
            Regex.Replace(s, "(_|-|^)[a-z]", m => m.Value.TrimStart('-').ToUpperInvariant());
    }
}
