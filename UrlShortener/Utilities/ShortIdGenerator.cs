
using shortid;
using shortid.Configuration;

namespace UrlShortener.Utilities
{
    public class ShortIdGenerator
    {
        public static string GenerateShortId(bool useSpecialCharacters = false, int length = 9)
        {
            var options = new GenerationOptions(useSpecialCharacters: useSpecialCharacters, useNumbers: true, length: length);
            string id = ShortId.Generate(options);
            return id;
        }
    }
}
