// See https://aka.ms/new-console-template for more information

using LuckyDraws.Services;

var numbersPath = "./Resources/lotto.txt";
Console.WriteLine($"Reading the winning numbers from {numbersPath}");

var wnr = new TicketReader();
var allWinningNumbers = await wnr.ReadAllWinningNumbers(numbersPath);