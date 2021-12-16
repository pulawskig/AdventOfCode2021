using System.Diagnostics;

namespace AdventOfCode2021
{
    public class Day15
    {
        public static async Task SolvePart1()
        {
            var inputs = (await File.ReadAllLinesAsync("Day15/Day15Input.txt"))
                .Select((line, i) => line
                    .Select(ch => byte.Parse(ch.ToString()))
                    .Select((b, j) => new Node { Value = b, Position = (i, j), Distance = int.MaxValue, Previous = (-1, -1) })
                    .ToArray())
                .ToArray();

            var finalNode = DoTheDijkstra(inputs);

            Console.WriteLine($"Day 15 part 1: {finalNode.Distance}");
        }

        public static async Task SolvePart2()
        {
            var inputs = (await File.ReadAllLinesAsync("Day15/Day15Input.txt"))
                .Select((line, i) => line
                    .Select(ch => byte.Parse(ch.ToString()))
                    .Select((b, j) => new Node { Value = b, Position = (i, j), Distance = int.MaxValue, Previous = (-1, -1) })
                    .ToArray())
                .ToArray();

            var originalHeight = inputs.Length;
            var originalWidth = inputs[0].Length;

            inputs = Enumerable.Repeat(inputs
                .Select(line => Enumerable.Repeat(line, 5)
                    .SelectMany((copy, i) => copy.Select(x => new Node { Value = (byte)(x.Value + i <= 9 ? x.Value + i : x.Value + i - 9), Distance = x.Distance, Position = (x.Position.X, x.Position.Y + (i * originalWidth)), Previous = x.Previous }))
                    .ToArray()), 5)
                .SelectMany((copy, i) => copy.Select(copy2 => copy2.Select(x => new Node { Value = (byte)(x.Value + i <= 9 ? x.Value + i : x.Value + i - 9), Distance = x.Distance, Position = (x.Position.X + (i * originalHeight), x.Position.Y), Previous = x.Previous }).ToArray()))
                .ToArray();

            var finalNode = DoTheDijkstra(inputs);

            Console.WriteLine($"Day 15 part 2: {finalNode.Distance}");
        }

        private static Node DoTheDijkstra(Node[][] inputs)
        {
            var stopwatch = Stopwatch.StartNew();
            try
            {
                inputs[0][0].Distance = 0;
                var queue = new List<Node>();
                queue.Add(inputs[0][0]);

                while (queue.Count > 0)
                {
                    var current = queue.MinBy(x => x.Distance);
                    queue.Remove(current);

                    if (current.Position.X == inputs.Length - 1 && current.Position.Y == inputs[0].Length - 1)
                    {
                        return current;
                    }

                    var neighbours = new List<Node>();

                    if (current.Position.X > 0)
                        neighbours.Add(inputs[current.Position.X - 1][current.Position.Y]);
                    if (current.Position.X < inputs.Length - 1)
                        neighbours.Add(inputs[current.Position.X + 1][current.Position.Y]);
                    if (current.Position.Y > 0)
                        neighbours.Add(inputs[current.Position.X][current.Position.Y - 1]);
                    if (current.Position.Y < inputs[0].Length - 1)
                        neighbours.Add(inputs[current.Position.X][current.Position.Y + 1]);

                    foreach (var neighbour in neighbours)
                    {
                        var dist = current.Distance + neighbour.Value;

                        if (dist < neighbour.Distance)
                        {
                            neighbour.Distance = dist;
                            neighbour.Previous = current.Position;

                            if (!queue.Contains(neighbour))
                                queue.Add(neighbour);
                        }
                    }
                }

                return null;
            }
            finally
            {
                stopwatch.Stop();
                Console.WriteLine($"{stopwatch.ElapsedMilliseconds} ms");
            }
        }

        public class Node
        {
            public byte Value { get; set; }

            public (int X, int Y) Position { get; set; }

            public decimal Distance { get; set; }

            public (int X, int Y) Previous { get; set; }
        }
    }
}
