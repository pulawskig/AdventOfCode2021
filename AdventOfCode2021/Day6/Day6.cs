namespace AdventOfCode2021
{
    public class Day6
    {
        public static async Task SolvePart1()
        {
            var inputs = (await File.ReadAllLinesAsync(@"Day6/Day6Input.txt"))[0]
                .Split(',')
                .Select(int.Parse)
                .ToList();

            var fishes = new long[9];

            foreach (var input in inputs)
                fishes[input]++;

            for (var x = 0; x < 80; x++)
            {
                var fishesTemp = new long[9];
                for (var i = 8; i > 0; i--)
                    fishesTemp[i - 1] = fishes[i];

                fishesTemp[6] += fishes[0];
                fishesTemp[8] += fishes[0];

                fishes = fishesTemp;
            }

            Console.WriteLine($"Day 6 part 1: {fishes.Sum()}");
        }

        public static async Task SolvePart2()
        {
            var inputs = (await File.ReadAllLinesAsync(@"Day6/Day6Input.txt"))[0]
                .Split(',')
                .Select(int.Parse)
                .ToList();

            var fishes = new long[9];

            foreach (var input in inputs)
                fishes[input]++;

            for (var x = 0; x < 256; x++)
            {
                var fishesTemp = new long[9];
                for (var i = 8; i > 0; i--)
                    fishesTemp[i - 1] = fishes[i];

                fishesTemp[6] += fishes[0];
                fishesTemp[8] += fishes[0];

                fishes = fishesTemp;
            }

            Console.WriteLine($"Day 6 part 2: {fishes.Sum()}");
        }
    }
}
