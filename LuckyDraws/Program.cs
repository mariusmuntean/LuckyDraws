using LuckyDraws.Services;
using LuckyDraws.Services.Render;

var numbersPath = "./Resources/lotto.txt";

var ticketReader = new TicketReader();
var allTickets = await ticketReader.ReadAllWinningNumbers(numbersPath);

var nfp = new NumberFrequencyProducer();
var numberFrequencies = nfp.ProduceFrequencies(allTickets);

var np = new NumbersPicker();
var combinations = np.PickMostFrequentCombinations(numberFrequencies);

var fcr = new FileCombinationRenderer();
var combinationsFile = await fcr.Render(combinations);

// Output some combinations to the console
File.ReadLines(combinationsFile).Take(10).ToList().ForEach(Console.WriteLine);