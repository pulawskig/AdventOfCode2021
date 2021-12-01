using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021
{
    public class Day1
    {
        public static async Task SolvePart1()
        {
            var inputs = (await File.ReadAllLinesAsync(@"Day1\Day1Input.txt"))
                .Where(str => !string.IsNullOrEmpty(str))
                .Select(int.Parse)
                .ToArray();

            var count = inputs.Skip(1)
                .Zip(inputs.SkipLast(1), (a, b) => a - b)
                .Count(x => x > 0);

            Console.WriteLine($"Day 1 part 1: {count}");
        }

        public static async Task SolvePart2()
        {
            var inputs = (await File.ReadAllLinesAsync(@"Day1\Day1Input.txt"))
                .Where(str => !string.IsNullOrEmpty(str))
                .Select(int.Parse)
                .ToArray();

            var windows = Enumerable.Repeat(0, inputs.Length).ToArray();

            for (var i = 0; i < inputs.Length; i++)
            {
                var val = inputs[i];

                if (i > 1)
                    windows[i - 2] += val;
                if (i > 0)
                    windows[i - 1] += val;
                windows[i] += val;
            }

            var count = windows.Skip(1)
                .Zip(windows.SkipLast(1), (a, b) => a - b)
                .Count(x => x > 0);

            Console.WriteLine($"Day 1 part 2: {count}");
        }
    }
}
