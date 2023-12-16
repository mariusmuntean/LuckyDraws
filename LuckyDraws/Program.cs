using LuckyDraws.Services;

var numbersPath = "./Resources/lotto.txt";

var ticketReader = new TicketReader();
var allTickets = await ticketReader.ReadAllWinningNumbers(numbersPath);

var nfp = new NumberFrequencyProducer();
var numberFrequencies = nfp.ProduceFrequencies(allTickets);

var np = new NumbersPicker();
np.PickMostFrequentCombinations(numberFrequencies);