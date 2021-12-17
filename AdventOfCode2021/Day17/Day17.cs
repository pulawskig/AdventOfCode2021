using System.Text.RegularExpressions;

namespace AdventOfCode2021
{
    public class Day17
    {
        public static async Task SolvePart1()
        {
            var (x1, x2, y1, y2) = await GetInputs();

            var highest = 0;
            for (var i = 0; i < 1000; i++)
            {
                for (var j = -1000; j < 1000; j++)
                {
                    var previousHeight = 0;
                    var velocity = (X: i, Y: j);
                    var position = (X: 0, Y: 0);
                    var found = false;

                    for (var k = 0; k < 1000; k++)
                    {
                        PerformMove(ref position, ref velocity);

                        if (position.Y > previousHeight)
                            previousHeight = position.Y;

                        if (position.X.IsBetween(x1, x2) && position.Y.IsBetween(y1, y2))
                        {
                            found = true;
                            break;
                        }
                    }

                    if (found && previousHeight > highest)
                    {
                        highest = previousHeight;
                    }
                }
            }

            Console.WriteLine($"Day 17 part 1: {highest}");
        }

        public static async Task SolvePart2()
        {
            var (x1, x2, y1, y2) = await GetInputs();

            var results = new List<(int X, int Y)>();
            for (var i = 0; i < 1000; i++)
            {
                for (var j = -100; j < 1000; j++)
                {
                    var velocity = (X: i, Y: j);
                    var position = (X: 0, Y: 0);

                    for (var k = 0; k < 1000; k++)
                    {
                        PerformMove(ref position, ref velocity);

                        if (position.X.IsBetween(x1, x2) && position.Y.IsBetween(y1, y2))
                        {
                            results.Add((i, j));
                            break;
                        }
                    }
                }
            }

            var count = results.Distinct().Count();

            Console.WriteLine($"Day 17 part 2: {count}");
        }

        private static async Task<(int x1, int x2, int y1, int y2)> GetInputs()
        {
            var input = (await File.ReadAllLinesAsync("Day17/Day17Input.txt"))[0];

            var regex = new Regex(@"target area: x=(\d+)\.\.(\d+), y=(-?\d+)\.\.(-?\d+)");
            var match = regex.Match(input);

            return (int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value), int.Parse(match.Groups[3].Value), int.Parse(match.Groups[4].Value));
        }

        private static void PerformMove(ref (int X, int Y) position, ref (int X, int Y) velocity)
        {
            position = (position.X + velocity.X, position.Y + velocity.Y);
            velocity = (velocity.X + (velocity.X > 0 ? -1 : velocity.X < 0 ? 1 : 0), velocity.Y - 1);
        }
    }
}
