using LuckyDraws.Services;

var numbersPath = "./Resources/lotto.txt";

var ticketReader = new TicketReader();
var allTickets = await ticketReader.ReadAllWinningNumbers(numbersPath);

var nfp = new NumberFrequencyProducer();
var numberFrequencies = nfp.ProduceFrequencies(allTickets);

var np = new NumbersPicker();
var combinations = np.PickMostFrequentCombinations(numberFrequencies);


var combinationsSortedByTotalFrequencies = combinations.OrderByDescending(combination => combination.CombinationNumbers.Sum(cn => cn.Frequency));
var sortedCombinationsFilePath = "SortedCombinations.txt";
using var fs = new FileStream(sortedCombinationsFilePath, FileMode.Create);
using var sw = new StreamWriter(fs);
foreach (var c in combinationsSortedByTotalFrequencies)
{
    await sw.WriteLineAsync($"Combination: {string.Join(", ", c.CombinationNumbers.Select(cn => $"{cn.Number:00} (f:{cn.Frequency:000})"))}      Total Frequency: {c.CombinationNumbers.Sum(cn => cn.Frequency):000} ");
}

Console.WriteLine($"Wrote sorted combinations to {sortedCombinationsFilePath}");

var consoleRenderer = new ConsoleCombinationRenderer();
consoleRenderer.Render(combinationsSortedByTotalFrequencies.Take(10).ToHashSet());