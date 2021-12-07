namespace AdventOfCode2021
{
    public class Day7
    {
        public static async Task SolvePart1()
        {
            var inputs = (await File.ReadAllLinesAsync(@"Day7/Day7Input.txt"))[0]
                .Split(',')
                .Select(int.Parse)
                .OrderBy(x => x)
                .ToList();

            var median = inputs[inputs.Count / 2];

            var sum = inputs.Sum(i => Math.Abs(i - median));

            Console.WriteLine($"Day 7 part 1: {sum}");
        }

        public static async Task SolvePart2()
        {
            var inputs = (await File.ReadAllLinesAsync(@"Day7/Day7Input.txt"))[0]
                .Split(',')
                .Select(int.Parse)
                .ToList();

            var average = (int) inputs.Average();
            var average2 = average + 1;

            var sum1 = inputs
                    .Select(x => Math.Abs(x - average))
                    .Select(x => Enumerable.Range(1, x).Sum())
                    .Sum();

            var sum2 = inputs
                    .Select(x => Math.Abs(x - average2))
                    .Select(x => Enumerable.Range(1, x).Sum())
                    .Sum();

            Console.WriteLine($"Day 7 part 2: {Math.Min(sum1, sum2)}");
        }
    }
}
