using System;
using System.Collections.Generic;

namespace Sentency.Models;

public class Author
{
    public string? Name { get; set; }

    public DateTime? BirthDate { get; set; }

    public List<string>? Sentences { get; set; } = new();
}