using System.Diagnostics;
using System.Text.RegularExpressions;

namespace AdventOfCode2021
{
    public class Day13
    {
        public static async Task SolvePart1()
        {
            var (positions, folds) = await GetInputs();

            var first = folds[0];

            positions = PerformFold(positions, first);

            var count = positions.Length;

            Console.WriteLine($"Day 13 part 1: {count}");
        }

        public static async Task SolvePart2()
        {
            var (positions, folds) = await GetInputs();

            foreach (var fold in folds)
            {
                positions = PerformFold(positions, fold);
            }

            var maxX = positions.Max(pos => pos.X);
            var maxY = positions.Max(pos => pos.Y);

            Console.WriteLine($"Day 13 part 2:");
            for (var i = 0; i <= maxY; i++)
            {
                for (var j = 0 ; j <= maxX; j++)
                {
                    if (positions.Any(pos => pos.X == j && pos.Y == i))
                        Console.Write("█");
                    else
                        Console.Write(" ");
                }
                Console.WriteLine();
            }
        }

        private static async Task<((int X, int Y)[], (bool IsHorizontal, int Value)[])> GetInputs()
        {
            var positionRegex = new Regex(@"^\d+,\d+$");
            var foldRegex = new Regex(@"^fold along ([x,y])=(\d+)$");

            var inputs = await File.ReadAllLinesAsync(@"Day13/Day13Input.txt");

            var positions = inputs
                .Where(line => positionRegex.IsMatch(line))
                .Select(line => line.Split(','))
                .Select(line => (X: int.Parse(line[0]), Y: int.Parse(line[1])))
                .ToArray();

            var folds = inputs
                .Select(line => foldRegex.Match(line))
                .Where(match => match.Success)
                .Select(match => (IsHorizontal: match.Groups[1].Value == "y", Value: int.Parse(match.Groups[2].Value)))
                .ToArray();

            return (positions, folds);
        }

        private static (int X, int Y)[] PerformFold((int X, int Y)[] positions, (bool IsHorizontal, int Value) fold)
        {
            var newPositions = positions
                .Where(pos => (fold.IsHorizontal ? pos.Y : pos.X) > fold.Value)
                .Select(pos => (
                    X: fold.IsHorizontal ? pos.X : (fold.Value - (pos.X - fold.Value)),
                    Y: fold.IsHorizontal ? (fold.Value - (pos.Y - fold.Value)) : pos.Y
                ));

            return positions
                .Where(pos => (fold.IsHorizontal ? pos.Y : pos.X) < fold.Value)
                .Concat(newPositions)
                .Distinct()
                .ToArray();
        }
    }
}
