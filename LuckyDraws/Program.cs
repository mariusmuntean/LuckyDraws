using LuckyDraws.Services;

var numbersPath = "./Resources/lotto.txt";

var ticketReader = new TicketReader();
var allTickets = await ticketReader.ReadAllWinningNumbers(numbersPath);

var nfp = new NumberFrequencyProducer();
var numberFrequencies = nfp.ProduceFrequencies(allTickets);

var np = new NumbersPicker();
var combinations = np.PickMostFrequentCombinations(numberFrequencies);

var sankeyDiagramRenderer = new SankeyDiagramDotNetCombinationRenderer();
var sankey = sankeyDiagramRenderer.Render(combinations);

var sankeyFilePath = "sankey.txt";
await File.WriteAllTextAsync(sankeyFilePath, sankey.ToString());
Console.WriteLine($"Wrote Sankey data to {sankeyFilePath}");

// var consoleRenderer = new ConsoleCombinationRenderer();
// consoleRenderer.Render(combinations);