using Sentency.Models;
using Sentency.Data;
using System.Text;
using System.Text.Json;
using System.IO;
using System.Threading.Tasks;

namespace Sentency.Utils;

public class Manager
{
    public Sentences? Sentences { get; set; }

    public async Task<Manager> Save()
    {
        if (Sentences is null) throw new InvalidDataException();

        var options = new JsonSerializerOptions()
        {
            WriteIndented = true,
            IncludeFields = true,
        };

        await File.WriteAllTextAsync(GlobalInfo.SentencyFilePath,
            JsonSerializer.Serialize(Sentences, options), Encoding.UTF8);

        return this;
    }

    public void Init()
    {
        if (Sentences is null) throw new InvalidDataException();

        Sentences.Authors?.Add(new());
        Sentences.Books?.Add(new());
        Sentences.Movies?.Add(new());
    }

    public async Task Read(string path)
    {
        path = Path.GetFullPath(path);
        var content = await File.ReadAllTextAsync(path, Encoding.UTF8);

        Sentences = JsonSerializer.Deserialize<Sentences>(content);
    }
}