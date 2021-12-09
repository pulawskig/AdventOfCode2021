namespace AdventOfCode2021
{
    public class Day9
    {
        public static async Task SolvePart1()
        {
            var inputs = (await File.ReadAllLinesAsync(@"Day9/Day9Input.txt"))
                .Select(line => line.Select(ch => byte.Parse(ch.ToString())).ToArray())
                .ToArray();

            var sum = FindMinima(inputs).Sum(coords => inputs[coords.X][coords.Y] + 1);

            Console.WriteLine($"Day 9 part 1: {sum}");
        }

        public static async Task SolvePart2()
        {
            var inputs = (await File.ReadAllLinesAsync(@"Day9/Day9Input.txt"))
                .Select(line => line.Select(ch => byte.Parse(ch.ToString())).ToArray())
                .ToArray();

            var minima = FindMinima(inputs).ToArray();
            var sizes = new List<int>();

            foreach (var minimum in minima)
            {
                var checks = inputs
                    .Select(line => line.Select(v => (Value: v, Visited: false)).ToArray())
                    .ToArray();

                var queue = new Queue<(int X, int Y)>();
                queue.Enqueue(minimum);

                checks[minimum.X][minimum.Y].Visited = true;

                var size = 0;
                while (queue.Count > 0)
                {
                    var current = queue.Dequeue();

                    size++;

                    if (current.Y > 0 && !checks[current.X][current.Y - 1].Visited && checks[current.X][current.Y - 1].Value < 9)
                    {
                        queue.Enqueue((current.X, current.Y - 1));
                        checks[current.X][current.Y - 1].Visited = true;
                    }

                    if (current.Y < checks[0].Length - 1 && !checks[current.X][current.Y + 1].Visited && checks[current.X][current.Y + 1].Value < 9)
                    {
                        queue.Enqueue((current.X, current.Y + 1));
                        checks[current.X][current.Y + 1].Visited = true;
                    }

                    if (current.X > 0 && !checks[current.X - 1][current.Y].Visited && checks[current.X - 1][current.Y].Value < 9)
                    {
                        queue.Enqueue((current.X - 1, current.Y));
                        checks[current.X - 1][current.Y].Visited = true;
                    }

                    if (current.X < checks.Length - 1 && !checks[current.X + 1][current.Y].Visited && checks[current.X + 1][current.Y].Value < 9)
                    {
                        queue.Enqueue((current.X + 1, current.Y));
                        checks[current.X + 1][current.Y].Visited = true;
                    }
                }

                sizes.Add(size);
            }

            var result = sizes
                .OrderByDescending(x => x)
                .Take(3)
                .Aggregate(1, (a, b) => a * b);

            Console.WriteLine($"Day 9 part 2: {result}");
        }

        private static IEnumerable<(int X, int Y)> FindMinima(byte[][] inputs)
        {
            for (var i = 0; i < inputs.Length; i++)
            {
                for (var j = 0; j < inputs[0].Length; j++)
                {
                    var current = inputs[i][j];
                    var success = true;

                    if (i > 0)
                        success = success && inputs[i - 1][j] > current;
                    if (j > 0)
                        success = success && inputs[i][j - 1] > current;
                    if (i < inputs.Length - 1)
                        success = success && inputs[i + 1][j] > current;
                    if (j < inputs[0].Length - 1)
                        success = success && inputs[i][j + 1] > current;

                    if (success)
                        yield return (i, j);
                }
            }
        }
    }
}
