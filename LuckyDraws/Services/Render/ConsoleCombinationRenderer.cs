namespace LuckyDraws.Services;

public class ConsoleCombinationRenderer
{
    public void Render(HashSet<NumbersPicker.Combination> combinations)
    {
        if (combinations.Count == 0) return;

        Console.WriteLine("Combinations");
        foreach (var combination in combinations)
        {
            Console.WriteLine(string.Join(", ", combination.CombinationNumbers.Select(cn => cn.Number)));
        }
    }
}