namespace LuckyDraws.Services.Render;

public class FileCombinationRenderer
{
    public async Task<string> Render(HashSet<NumbersPicker.Combination> combinations)
    {
        var combinationsSortedByTotalFrequencies = combinations.OrderByDescending(combination => combination.CombinationNumbers.Sum(cn => cn.Frequency));
        var sortedCombinationsFilePath = "SortedCombinations.txt";
        await using var fs = new FileStream(sortedCombinationsFilePath, FileMode.Create);
        await using var sw = new StreamWriter(fs);
        foreach (var c in combinationsSortedByTotalFrequencies)
        {
            await sw.WriteLineAsync($"Combination: {string.Join(", ", c.CombinationNumbers.Select(cn => $"{cn.Number:00} (f:{cn.Frequency:000})"))}      Total Frequency: {c.CombinationNumbers.Sum(cn => cn.Frequency):000} ");
        }

        Console.WriteLine($"Wrote sorted combinations to {sortedCombinationsFilePath}");

        return sortedCombinationsFilePath;
    }
}