using System.Collections.Generic;

namespace Sentency.Models;

public class Movie
{
    public string? Title { get; set; }

    public string? OriginalTitle { get; set; }

    public string? OriginalLanguage { get; set; }

    public List<string>? Sentences { get; set; } = new();

}