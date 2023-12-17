using System.Diagnostics;

namespace LuckyDraws.Services;

public class NumbersPicker
{
    public HashSet<Combination> PickMostFrequentCombinations(List<NumberFrequency>? numberFrequencies)
    {
        if (numberFrequencies is null || numberFrequencies.Count == 0)
        {
            return new HashSet<Combination>();
        }

        Console.WriteLine("Picking the most frequent number combinations");
        var sw = Stopwatch.StartNew();

        var maxF = numberFrequencies.OrderByDescending(frequency => frequency.CurrentNumberFrequency).Take(1).ToList();
        // var combinations = new HashSet<Combination>(new CombinationComparer());
        var combinations = new HashSet<Combination>();
        foreach (var numberFrequency in maxF)
        {
            PickMostFrequentCombinationsInternal([new CombinationNumber(numberFrequency.CurrentNumber, numberFrequency.CurrentNumberFrequency)], numberFrequency, combinations);
        }

        Console.WriteLine($"Picked most frequent combinations in {sw.Elapsed.TotalSeconds} seconds");
        return combinations;
    }

    static void PickMostFrequentCombinationsInternal(CombinationNumber[] currentCombinationNumbers, NumberFrequency numberFrequencies, ISet<Combination> combinations)
    {
        if (currentCombinationNumbers is { Length: >= 6 })
        {
            combinations.Add(new Combination(currentCombinationNumbers));
        }

        if (numberFrequencies.NumberFrequenciesOfTicketNeighbors is null || numberFrequencies.NumberFrequenciesOfTicketNeighbors.Count == 0)
        {
            return;
        }

        var orderedNeighborFrequencies = numberFrequencies.NumberFrequenciesOfTicketNeighbors.OrderByDescending(frequency => frequency.CurrentNumberFrequency).ToList();
        foreach (var numberFrequency in orderedNeighborFrequencies)
        {
            PickMostFrequentCombinationsInternal([..currentCombinationNumbers, new CombinationNumber(numberFrequency.CurrentNumber, numberFrequency.CurrentNumberFrequency)], numberFrequency, combinations);
        }
    }

    public record CombinationNumber(byte Number, int Frequency);

    public record Combination(CombinationNumber[] CombinationNumbers);

    private class CombinationComparer : IEqualityComparer<byte[]>
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