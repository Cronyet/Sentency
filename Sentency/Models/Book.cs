using System;
using System.Collections.Generic;

namespace Sentency.Models;

public class Book
{
    public string? Name { get; set; }

    public string? Author { get; set; }

    public DateTime? PublishDate { get; set; }

    public List<string>? Sentences { get; set; } = new();
}