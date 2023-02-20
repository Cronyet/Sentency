using Sentency;
using Sentency.Models;
using Sentency.Utils;
using System.Threading.Tasks;
using System;
using System.Text;

Console.InputEncoding = Encoding.UTF8;
Console.OutputEncoding = Encoding.UTF8;

var manager = new Manager();

var sentences = () => manager.Sentences;

// await manager.Save();

var appRunning = true;

void PrintAllAuthorsSentences()
{
    foreach (var author in sentences().Authors ?? new())
    {
        Console.WriteLine($"Author: {author.Name}");
        foreach (var sentence in author.Sentences ?? new())
            Console.WriteLine($"\t{sentence}");
    }
}

void PrintAllBooksSentences()
{
    foreach (var book in sentences().Books ?? new())
    {
        Console.WriteLine($"Book: {book.Name}");
        foreach (var sentence in book.Sentences ?? new())
            Console.WriteLine($"\t{sentence}");
    }
}

void PrintAllMoviesSentences()
{
    foreach (var movie in sentences().Movies ?? new())
    {
        Console.WriteLine($"Movie: {movie.Title}");
        foreach (var sentence in movie.Sentences ?? new())
            Console.WriteLine($"\t{sentence}");
    }
}

while (appRunning)
{
    Console.Write("> ");

    var input = await System.Console.In.ReadLineAsync();

    if (input is null) continue;

    switch (input.Trim().ToLower())
    {
        case "exit":
            appRunning = false;
            continue;

        case "init":
            manager.Init();
            continue;

        case "read":
            Console.Write("File path: ");
            var path = Console.ReadLine();

            if (path is null)
            {
                Console.WriteLine("Input is null.");
                continue;
            }

            await manager.Read(path);
            continue;

        case "insert":
            continue;

        case "ls":
            {
                Console.Write("""
                1. Authors
                2. Books
                3. Movies
                Select a collection: 
                """);
                var type = Console.ReadLine();
                if (int.TryParse(type, out var id))
                {
                    switch (id)
                    {
                        case 1:
                            {
                                var index = 0;
                                foreach (var author in sentences().Authors ?? new())
                                {
                                    Console.WriteLine($"{index}:\t{author.Name}");
                                    ++index;
                                }
                                break;
                            }
                        case 2:
                            {
                                var index = 0;
                                foreach (var book in sentences().Books ?? new())
                                {
                                    Console.WriteLine($"{index}:\t{book.Name}");
                                    ++index;
                                }
                                break;
                            }
                        case 3:
                            {
                                var index = 0;
                                foreach (var movie in sentences().Movies ?? new())
                                {
                                    Console.WriteLine($"{index}:\t{movie.Title}");
                                    ++index;
                                }
                                break;
                            }
                    }
                }
                else Console.WriteLine("Invalid input");
            }
            continue;

        case "get":
            {
                Console.Write("""
                0. All
                1. By author index
                2. By book index
                3. By movie index
                4. All authors
                5. All books
                6. All movies
                Specific typs: 
                """);
                var type = Console.ReadLine();
                if (int.TryParse(type, out var id))
                {
                    switch (id)
                    {
                        case 0:
                            {
                                PrintAllAuthorsSentences();

                                Console.WriteLine();

                                PrintAllBooksSentences();

                                Console.WriteLine();

                                PrintAllMoviesSentences();

                                Console.WriteLine();

                                foreach (var sentence in sentences().UnknownSentences ?? new())
                                    Console.WriteLine($"\t{sentence}");
                                break;
                            }
                        case 1:
                            {
                                Console.Write("Author index: ");
                                var author_index = Console.ReadLine();

                                if (author_index is null)
                                    Console.WriteLine("Input is null.");

                                if (int.TryParse(author_index, out var index) && sentences().Authors is not null)
                                {
                                    var author = sentences().Authors[index];

                                    if (author is null)
                                        Console.WriteLine($"No author named {input}.");
                                    else
                                    {
                                        foreach (var sentence in author.Sentences ?? new())
                                            Console.WriteLine($"\t{sentence}");
                                    }
                                }
                                break;
                            }
                        case 4: PrintAllAuthorsSentences(); break;
                        case 5: PrintAllBooksSentences(); break;
                        case 6: PrintAllMoviesSentences(); break;
                    }
                }
                else Console.WriteLine("Invalid input");
            }
            continue;

        case "save":
            await manager.Save();
            continue;
    }
}

