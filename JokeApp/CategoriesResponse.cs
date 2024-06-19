using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace JokeApp
{
    public class CategoriesResponse
    {
        [JsonPropertyName("categories")]
        public List<string> Categories { get; set; }
    }
}
