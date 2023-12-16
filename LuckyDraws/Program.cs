using LuckyDraws.Services;

var numbersPath = "./Resources/lotto.txt";
Console.WriteLine($"Reading the winning numbers from {numbersPath}");

var ticketReader = new TicketReader();
var allTickets = await ticketReader.ReadAllWinningNumbers(numbersPath);

Dictionary<byte, int> numberFrequencies = new Dictionary<byte, int>(49);
foreach (var ticket in allTickets)
{
    foreach (var winningNumber in ticket.Numbers)
    {
        var currentWinningNumberFrequency = numberFrequencies.GetValueOrDefault(winningNumber, 0);
        numberFrequencies[winningNumber] = currentWinningNumberFrequency + 1;
    }
}

byte number = 43;
Console.WriteLine($"Frequency of {number} is {numberFrequencies.GetValueOrDefault(number, 0)}");