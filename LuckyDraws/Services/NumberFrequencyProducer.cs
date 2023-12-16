namespace LuckyDraws.Services;

public class NumberFrequencyProducer
{
    public List<NumberFrequency> ProduceFrequencies(List<Ticket>? tickets)
    {
        Console.WriteLine($"Producing frequencies for all numbers and their ticket neighbors");
        
        var rawNumberFrequencies = GetRawNumberFrequencies(tickets);
        var numberFrequencies = new List<NumberFrequency>(49);
        foreach (var (number, frequency) in rawNumberFrequencies)
        {
            var ticketsWithCurrentNumber = tickets?.Where(ticket => ticket.Numbers.Contains(number)).ToList();
            numberFrequencies.Add(new NumberFrequency()
            {
                CurrentNumber = number,
                CurrentNumberFrequency = frequency,
                TicketsWithCurrentNumber = ticketsWithCurrentNumber,
                NumberFrequenciesOfTicketNeighbors = ComputeTicketNeighborsFrequencies(new HashSet<byte>() { number }, ticketsWithCurrentNumber)
            });
        }


        return numberFrequencies;
    }

    private List<NumberFrequency>? ComputeTicketNeighborsFrequencies(HashSet<byte> currentNumbers, List<Ticket>? ticketsWithCurrentNumber)
    {
        if (ticketsWithCurrentNumber is null || ticketsWithCurrentNumber.Count == 0)
        {
            return null;
        }

        var rawNumberFrequencies = GetRawNumberFrequencies(ticketsWithCurrentNumber);
        var rawNeighborFrequencies = rawNumberFrequencies.Where(pair => !currentNumbers.Contains(pair.Key)).ToList();

        if (rawNeighborFrequencies.Count == 0)
        {
            return null;
        }

        var neighborFrequencies = new List<NumberFrequency>(rawNeighborFrequencies.Count);
        foreach (var (number, frequency) in rawNeighborFrequencies)
        {
            var ticketsWithNewCurrentNumbers = ticketsWithCurrentNumber.Where(ticket => ticket.Numbers.Contains(number) && ticket.Numbers.IsSupersetOf(currentNumbers)).ToList();
            neighborFrequencies.Add(new NumberFrequency()
            {
                CurrentNumber = number,
                CurrentNumberFrequency = frequency,
                TicketsWithCurrentNumber = ticketsWithNewCurrentNumbers,
                NumberFrequenciesOfTicketNeighbors = ComputeTicketNeighborsFrequencies(new HashSet<byte>(currentNumbers) { number }, ticketsWithNewCurrentNumbers)
            });
        }

        return neighborFrequencies;
    }

    private static Dictionary<byte, int> GetRawNumberFrequencies(List<Ticket>? tickets)
    {
        if (tickets is null || tickets.Count == 0)
        {
            return new Dictionary<byte, int>(0);
        }
        
        var numberFrequencies = new Dictionary<byte, int>(tickets.Count);
        foreach (var ticket in tickets)
        {
            foreach (var winningNumber in ticket.Numbers)
            {
                var currentWinningNumberFrequency = numberFrequencies.GetValueOrDefault(winningNumber, 0);
                numberFrequencies[winningNumber] = currentWinningNumberFrequency + 1;
            }
        }


        return numberFrequencies;
    }
}

public class NumberFrequency
{
    public byte CurrentNumber { get; init; }
    public int CurrentNumberFrequency { get; init; }

    public List<Ticket>? TicketsWithCurrentNumber { get; init; }

    public List<NumberFrequency>? NumberFrequenciesOfTicketNeighbors { get; set; }
}