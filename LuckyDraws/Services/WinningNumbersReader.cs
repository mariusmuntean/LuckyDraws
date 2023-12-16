using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

namespace LuckyDraws.Services;

public class WinningNumbersReader
{
    private readonly CsvConfiguration _csvConfig;

    public WinningNumbersReader()
    {
        _csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = "\t"
        };
    }

    public async Task<List<WinningNumbers>> ReadAllWinningNumbers(string filePath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);

        using var sr = new StreamReader(filePath);
        using var csvReader = new CsvReader(sr, _csvConfig);
        csvReader.Context.RegisterClassMap<WinningNumbersMap>();

        var allWinningNumbers = new List<WinningNumbers>();
        // Skip header row
        await csvReader.ReadAsync();
        csvReader.ReadHeader();
        await foreach (var winningNumbers in csvReader.GetRecordsAsync<WinningNumbers>())
        {
            allWinningNumbers.Add(winningNumbers);
        }

        return allWinningNumbers;
    }
}

public class WinningNumbers
{
    public DateOnly Date { get; init; }
    public int[] Numbers { get; init; }
    public byte SuperNumber { get; init; }

    public void Deconstruct(out DateOnly Date, out int[] Numbers, out byte SuperNumber)
    {
        Date = this.Date;
        Numbers = this.Numbers;
        SuperNumber = this.SuperNumber;
    }
}

internal class WinningNumbersMap : ClassMap<WinningNumbers>
{
    public WinningNumbersMap()
    {
        Map(numbers => numbers.Date)
            .Convert(args => new DateOnly(Convert.ToInt32(args.Row[2]), Convert.ToInt32(args.Row[1]), Convert.ToInt32(args.Row[0])));

        Map(numbers => numbers.Numbers)
            .Convert(args => new int[6]
            {
                Convert.ToInt32(args.Row[3]),
                Convert.ToInt32(args.Row[4]),
                Convert.ToInt32(args.Row[5]),
                Convert.ToInt32(args.Row[6]),
                Convert.ToInt32(args.Row[7]),
                Convert.ToInt32(args.Row[8]),
            });

        // There are two 'tabs' between the Zusatzzahl and the Superzahl so we're reading the 12'th column.
        Map(numbers => numbers.SuperNumber).Convert(args => Convert.ToByte(args.Row[11]));
    }
}