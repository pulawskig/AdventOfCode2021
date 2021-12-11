namespace AdventOfCode2021
{
    public class Day11
    {
        public static async Task SolvePart1()
        {
            var inputs = (await File.ReadAllLinesAsync(@"Day11/Day11Input.txt"))
                .Select(line => line.Select(ch => byte.Parse(ch.ToString())).ToArray())
                .ToArray();

            var flashes = 0;
            for(var step = 0; step < 100; step++)
            {
                var copy = inputs
                    .Select((line, i) => line.Select((v, j) => (Value: v, Flashed: false)).ToArray())
                    .ToArray();

                var queue = new Queue<(int X, int Y)>(Enumerable.Range(0, inputs.Length).SelectMany(x => Enumerable.Range(0, inputs[0].Length).Select(y => (X: x, Y: y)).ToArray()));
                while(queue.Count > 0)
                {
                    var coords = queue.Dequeue();

                    SolveCurrentStep(copy, coords, () => flashes++, c => queue.Enqueue(c));
                }

                inputs = copy
                    .Select(line => line.Select(x => x.Value).ToArray())
                    .ToArray();
            }

            Console.WriteLine($"Day 11 part 1: {flashes}");
        }

        public static async Task SolvePart2()
        {
            var inputs = (await File.ReadAllLinesAsync(@"Day11/Day11Input.txt"))
                .Select(line => line.Select(ch => byte.Parse(ch.ToString())).ToArray())
                .ToArray();

            var step = 0;
            while (true)
            {
                step++;

                var copy = inputs
                    .Select((line, i) => line.Select((v, j) => (Value: v, Flashed: false)).ToArray())
                    .ToArray();

                var queue = new Queue<(int X, int Y)>(Enumerable.Range(0, inputs.Length).SelectMany(x => Enumerable.Range(0, inputs[0].Length).Select(y => (X: x, Y: y)).ToArray()));
                while (queue.Count > 0)
                {
                    var coords = queue.Dequeue();

                    SolveCurrentStep(copy, coords, () => { }, c => queue.Enqueue(c));
                }

                if (copy.SelectMany(line => line).All(x => x.Flashed))
                    break;

                inputs = copy
                    .Select(line => line.Select(x => x.Value).ToArray())
                    .ToArray();
            }

            Console.WriteLine($"Day 11 part 2: {step}");
        }

        private static void SolveCurrentStep((byte Value, bool Flashed)[][] copy, (int X, int Y) coords, Action whenFlashTriggered, Action<(int X, int Y)> whenNeighbourTriggered)
        {
            if (copy[coords.X][coords.Y].Flashed)
                return;

            copy[coords.X][coords.Y].Value++;

            if (copy[coords.X][coords.Y].Value > 9)
            {
                whenFlashTriggered();
                copy[coords.X][coords.Y].Value = 0;
                copy[coords.X][coords.Y].Flashed = true;

                var maxI = Math.Clamp(coords.X + 1, 0, copy.Length - 1);
                var maxJ = Math.Clamp(coords.Y + 1, 0, copy[0].Length - 1);
                for (var i = Math.Clamp(coords.X - 1, 0, byte.MaxValue); i <= maxI; i++)
                {
                    for (var j = Math.Clamp(coords.Y - 1, 0, byte.MaxValue); j <= maxJ; j++)
                    {
                        if (i == coords.X && j == coords.Y)
                            continue;
                        whenNeighbourTriggered((i, j));
                    }
                }
            }
        }
    }
}
