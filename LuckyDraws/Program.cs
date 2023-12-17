using LuckyDraws.Services;

var numbersPath = "./Resources/lotto.txt";

var ticketReader = new TicketReader();
var allTickets = await ticketReader.ReadAllWinningNumbers(numbersPath);

var nfp = new NumberFrequencyProducer();
var numberFrequencies = nfp.ProduceFrequencies(allTickets);

var np = new NumbersPicker();
var combinations = np.PickMostFrequentCombinations(numberFrequencies);


if (combinations.Count != 0)
{
    Console.WriteLine("Combinations");
    foreach (var combination in combinations)
    {
        Console.WriteLine(string.Join(", ", combination.CombinationNumbers.Select(cn => cn.Number)));
    }
}