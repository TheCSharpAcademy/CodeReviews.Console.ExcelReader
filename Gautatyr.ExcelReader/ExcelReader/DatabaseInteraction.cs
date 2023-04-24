using ExcelReader.Model;

namespace ExcelReader;

public class DatabaseInteraction
{
    private readonly ExcelReaderContext Context;

    public DatabaseInteraction(ExcelReaderContext context)
    {
        Context = context;
    }

    public void SeedDatabaseFromList(List<Aliment> aliments)
    {
        foreach (var aliment in aliments)
        {
            Context.Aliments.Add(aliment);
            Context.SaveChanges();
        }
    }

    public List<Aliment> GetRandomAliments()
    {
        List<Aliment> aliments = new();
        Random random = new();
        int numberOfAliments = random.Next(1, 5);

        for (int i = 0; i < numberOfAliments; i++)
        {
            aliments.Add(Context.Aliments.Find(random.Next(1, 15)));
        }

        return aliments;
    }
}