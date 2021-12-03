namespace AdventOfCode2021
{
    public class Day2
    {
        public static async Task SolvePart1()
        {
            var inputs = (await File.ReadAllLinesAsync(@"Day2\Day2Input.txt"))
                .Where(str => !string.IsNullOrEmpty(str))
                .Select(str => str.Split(' '))
                .Select(str => (Command: str[0], Value: int.Parse(str[1])))
                .Select(x => x.Command switch
                {
                    "down" => (Command: "depth", x.Value),
                    "up" => (Command: "depth", Value: -x.Value),
                    _ => x,
                })
                .ToList();

            var depth = inputs.Where(x => x.Command == "depth").Sum(x => x.Value);
            var horizontal = inputs.Where(x => x.Command == "forward").Sum(x => x.Value);

            Console.WriteLine($"Day 2 part 1: {depth * horizontal}");
        }

        public static async Task SolvePart2()
        {
            var inputs = (await File.ReadAllLinesAsync(@"Day2\Day2Input.txt"))
                .Where(str => !string.IsNullOrEmpty(str))
                .Select(str => str.Split(' '))
                .Select(str => (Command: str[0], Value: int.Parse(str[1])))
                .ToList();

            var aim = 0;
            var depth = 0;
            var horizontal = 0;

            foreach(var x in inputs)
            {
                switch(x.Command)
                {
                    case "down":
                        aim += x.Value;
                        break;
                    case "up":
                        aim -= x.Value;
                        break;
                    case "forward":
                        horizontal += x.Value;
                        depth += aim * x.Value;
                        break;
                }
            }

            Console.WriteLine($"Day 2 part 2: {horizontal * depth}");
        }
    }
}
