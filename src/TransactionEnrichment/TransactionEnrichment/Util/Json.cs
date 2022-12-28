using System.Text.Json;

namespace TransactionEnrichment.Util
{
    public static class Json
    {
        public class SnakeCaseNamingPolicy : JsonNamingPolicy
        {
            public static SnakeCaseNamingPolicy Instance { get; } = new SnakeCaseNamingPolicy();

            public override string ConvertName(string name)
            {
                // Conversion to other naming convention goes here. Like SnakeCase, KebabCase etc.
                return name.ToSnakeCase();
            }
        }
        public static string ToSnakeCase(this string str)
        {
            return string.Concat(str.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString())).ToLower();
        }
    }
}
