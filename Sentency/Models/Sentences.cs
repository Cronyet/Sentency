using System.Collections.Generic;

namespace Sentency.Models;

public class Sentences
{
    public List<Author>? Authors { get; set; } = new();

    public List<Book>? Books { get; set; } = new();

    public List<Movie>? Movies { get; set; } = new();

    public List<string>? UnknownSentences { get; set; } = new();
}