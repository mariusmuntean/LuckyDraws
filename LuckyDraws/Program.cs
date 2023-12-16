// See https://aka.ms/new-console-template for more information

using LuckyDraws.Services;

var numbersPath = "./Resources/lotto.txt";
Console.WriteLine($"Reading the winning numbers from {numbersPath}");

var ticketReader = new TicketReader();
var allTickets = await ticketReader.ReadAllWinningNumbers(numbersPath);
