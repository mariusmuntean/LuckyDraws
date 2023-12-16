using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

namespace LuckyDraws.Services;

public class TicketReader
{
    private readonly CsvConfiguration _csvConfig;

    public TicketReader()
    {
        _csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = "\t"
        };
    }

    public async Task<List<Ticket>?> ReadAllWinningNumbers(string filePath)
    {
        Console.WriteLine($"Reading the winning numbers from {filePath}");
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);

        using var sr = new StreamReader(filePath);
        using var csvReader = new CsvReader(sr, _csvConfig);
        csvReader.Context.RegisterClassMap<TicketMap>();

        var allWinningNumbers = new List<Ticket>();
        // Skip header row
        await csvReader.ReadAsync();
        csvReader.ReadHeader();
        await foreach (var winningNumbers in csvReader.GetRecordsAsync<Ticket>())
        {
            allWinningNumbers.Add(winningNumbers);
        }

        return allWinningNumbers;
    }
}

public class Ticket
{
    public DateOnly Date { get; init; }
    public HashSet<byte> Numbers { get; init; }
    public byte SuperNumber { get; init; }

    public void Deconstruct(out DateOnly Date, out HashSet<byte> Numbers, out byte SuperNumber)
    {
        Date = this.Date;
        Numbers = this.Numbers;
        SuperNumber = this.SuperNumber;
    }
}

internal class TicketMap : ClassMap<Ticket>
{
    public TicketMap()
    {
        Map(numbers => numbers.Date)
            .Convert(args => new DateOnly(Convert.ToInt32(args.Row[2]), Convert.ToInt32(args.Row[1]), Convert.ToInt32(args.Row[0])));

        Map(numbers => numbers.Numbers)
            .Convert(args => new HashSet<byte>
            {
                Convert.ToByte(args.Row[3]),
                Convert.ToByte(args.Row[4]),
                Convert.ToByte(args.Row[5]),
                Convert.ToByte(args.Row[6]),
                Convert.ToByte(args.Row[7]),
                Convert.ToByte(args.Row[8]),
            });

        // There are two 'tabs' between the Zusatzzahl and the Superzahl so we're reading the 12'th column.
        Map(numbers => numbers.SuperNumber).Convert(args => Convert.ToByte(args.Row[11]));
    }
}