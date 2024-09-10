using System.Text.Json;

namespace Sample
{
    public static class Helpers
    {
        private static readonly JsonSerializerOptions Options = new() { WriteIndented = true };

        public static void PrintAsJson<T>(this T obj)
        {
            var json = JsonSerializer.Serialize(obj, Options);
            Console.WriteLine(json);
        }
    }
}