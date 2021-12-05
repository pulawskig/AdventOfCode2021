using System.Text.RegularExpressions;

namespace AdventOfCode2021
{
    public class Day5
    {
        public static async Task SolvePart1()
        {
            var inputs = (await GetInputs())
                .Where(pairs => pairs.x1 == pairs.x2 || pairs.y1 == pairs.y2)
                .ToList();

            var xMax = inputs
                .Select(pairs => pairs.x1)
                .Concat(inputs.Select(pairs => pairs.x2))
                .Max();
            var yMax = inputs
                .Select(pairs => pairs.y1)
                .Concat(inputs.Select(pairs => pairs.y2))
                .Max();

            var board = new int[xMax + 1, yMax + 1];

            foreach (var (x1, x2, y1, y2) in inputs)
            {
                if (x1 == x2)
                {
                    for (var i = 0; i <= Math.Abs(y1 - y2); i++)
                    {
                        board[x1, y1 + (y1 < y2 ? i : -i)]++;
                    }
                }
                else if (y1 == y2)
                {
                    for (var i = 0; i <= Math.Abs(x1 - x2); i++)
                    {
                        board[x1 + (x1 < x2 ? i : -i), y1]++;
                    }
                }
            }

            var count = board
                .Flatten()
                .Where(x => x > 1)
                .Count();

            Console.WriteLine($"Day 5 part 1: {count}");
        }

        public static async Task SolvePart2()
        {
            var inputs = await GetInputs();

            var xMax = inputs
                .Select(pairs => pairs.x1)
                .Concat(inputs.Select(pairs => pairs.x2))
                .Max();
            var yMax = inputs
                .Select(pairs => pairs.y1)
                .Concat(inputs.Select(pairs => pairs.y2))
                .Max();

            var board = new int[xMax + 1, yMax + 1];

            foreach (var (x1, x2, y1, y2) in inputs)
            {
                if (x1 == x2)
                {
                    for (var i = 0; i <= Math.Abs(y1 - y2); i++)
                    {
                        board[x1, y1 + (y1 < y2 ? i : -i)]++;
                    }
                }
                else if (y1 == y2)
                {
                    for (var i = 0; i <= Math.Abs(x1 - x2); i++)
                    {
                        board[x1 + (x1 < x2 ? i : -i), y1]++;
                    }
                }
                else
                {
                    for (var i = 0; i <= Math.Abs(x1 - x2); i++)
                    {
                        board[x1 + (x1 < x2 ? i : -i), y1 + (y1 < y2 ? i : -i)]++;
                    }
                }
            }

            var count = board
                .Flatten()
                .Where(x => x > 1)
                .Count();

            Console.WriteLine($"Day 5 part 2: {count}");
        }

        private static async Task<List<(int x1, int x2, int y1, int y2)>> GetInputs()
        {
            var regex = new Regex(@"(\d+),(\d+) -> (\d+),(\d+)");

            return (await File.ReadAllLinesAsync(@"Day5\Day5Input.txt"))
                .Where(str => !string.IsNullOrEmpty(str))
                .Select(str => regex.Match(str))
                .Select(match => (x1: match.Groups[1].Value, y1: match.Groups[2].Value, x2: match.Groups[3].Value, y2: match.Groups[4].Value))
                .Select(pairs => (x1: int.Parse(pairs.x1), x2: int.Parse(pairs.x2), y1: int.Parse(pairs.y1), y2: int.Parse(pairs.y2)))
                .ToList();
        }
    }
}
