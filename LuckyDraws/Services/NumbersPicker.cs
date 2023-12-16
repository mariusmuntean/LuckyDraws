namespace LuckyDraws.Services;

public class NumbersPicker
{
    public void PickMostFrequentCombinations(List<NumberFrequency>? numberFrequencies)
    {
        Console.WriteLine("Picking the most frequent number combinations");
        if (numberFrequencies is null || numberFrequencies.Count == 0)
        {
            return;
        }

        var maxF = numberFrequencies.OrderByDescending(frequency => frequency.CurrentNumberFrequency).Take(1).ToList();
        var combinations = new HashSet<byte[]>(new CombinationComparer());
        foreach (var numberFrequency in maxF)
        {
            PickMostFrequentCombinationsInternal([numberFrequency.CurrentNumber], numberFrequency, combinations);
        }

        if (combinations.Count != 0)
        {
            Console.WriteLine("Combinations");
            foreach (var combination in combinations)
            {
                Console.WriteLine(string.Join(", ", combination));
            }
        }
    }

    static void PickMostFrequentCombinationsInternal(byte[] numbers, NumberFrequency numberFrequencies, ISet<byte[]> combinations)
    {
        if (numbers is { Length: >= 6 })
        {
            combinations.Add(numbers);
        }

        if (numberFrequencies.NumberFrequenciesOfTicketNeighbors is null || numberFrequencies.NumberFrequenciesOfTicketNeighbors.Count == 0)
        {
            return;
        }

        var orderedNeighborFrequencies = numberFrequencies.NumberFrequenciesOfTicketNeighbors.OrderByDescending(frequency => frequency.CurrentNumberFrequency).ToList();
        foreach (var numberFrequency in orderedNeighborFrequencies)
        {
            PickMostFrequentCombinationsInternal([..numbers, numberFrequency.CurrentNumber], numberFrequency, combinations);
        }
    }

    class CombinationComparer : IEqualityComparer<byte[]>
    {
        public bool Equals(byte[]? x, byte[]? y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (x is null && y is not null) return false;
            if (x is not null && y is null) return false;

            return x.OrderBy(b => b).SequenceEqual(y.OrderBy(b => b));
        }

        public int GetHashCode(byte[] obj) => obj.OrderBy(x => x).Aggregate(17, (current, b) => current * 31 + b);
    }
}