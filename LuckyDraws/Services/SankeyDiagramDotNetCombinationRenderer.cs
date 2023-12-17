using System.Diagnostics;
using System.Text;

namespace LuckyDraws.Services;

public class SankeyDiagramDotNetCombinationRenderer
{
    public StringBuilder Render(HashSet<NumbersPicker.Combination> combinations)
    {
        var sw = Stopwatch.StartNew();

        // Group combinations by their first number and process each group
        var sb = new StringBuilder();
        foreach (var combinationsGroup in combinations.GroupBy(combination => combination.CombinationNumbers.First()))
        {
            sb.AppendLine("// Combinations starting with " + combinationsGroup.Key);
            foreach (var combination in combinationsGroup)
            {
                for (var index = 0; index < combination.CombinationNumbers.Length; index++)
                {
                    var combinationNumber = combination.CombinationNumbers[index];
                    if (index == 0)
                    {
                        sb.Append($"Number {combinationNumber.Number} ({index + 1}/{combination.CombinationNumbers.Length}) [{combinationNumber.Frequency}]");
                    }
                    else if (index == combination.CombinationNumbers.Length - 1)
                    {
                        sb.AppendLine($" Number {combinationNumber.Number}");
                        sb.AppendLine();
                    }
                    else
                    {
                        sb.AppendLine($" Number {combinationNumber.Number}");
                        sb.Append($"Number {combinationNumber.Number} [{combinationNumber.Frequency}]");
                    }
                }
            }
        }

        Console.WriteLine($"Produced Sankey diagram data in {sw.Elapsed.TotalSeconds} seconds");
        return sb;
    }
}