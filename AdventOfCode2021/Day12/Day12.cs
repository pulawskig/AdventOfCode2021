namespace AdventOfCode2021
{
    public static class Day12
    {
        public static async Task SolvePart1()
        {
            var count = (await File.ReadAllLinesAsync(@"Day12/Day12Input.txt"))
                .GetPaths((path, neighbour) => !neighbour.IsLarge && path.Any(x => x.Name == neighbour.Name))
                .Count();

            Console.WriteLine($"Day 12 part 1: {count}");
        }

        public static async Task SolvePart2()
        {
            var count = (await File.ReadAllLinesAsync(@"Day12/Day12Input.txt"))
                .GetPaths((path, neighbour) => !neighbour.IsLarge
                        && path.Any(x => x.Name == neighbour.Name)
                        && path.Where(x => !x.IsLarge).GroupBy(x => x.Name).Any(g => g.Count() > 1))
                .Count();

            Console.WriteLine($"Day 12 part 2: {count}");
        }

        private static IEnumerable<Cave[]> GetPaths(this IEnumerable<string> inputs, Func<Cave[], Cave, bool> neighbourExclusionPredicate)
        {
            var splits = inputs.Select(line => line.Split('-')).ToArray();

            var nodes = splits
                .SelectMany(line => line)
                .Distinct()
                .Select(x => new Cave
                {
                    Name = x,
                    IsLarge = x == x.ToUpper(),
                    IsStart = x == "start",
                    IsEnd = x == "end",
                })
                .ToArray();

            foreach (var node in nodes)
            {
                node.Neighbours = splits
                    .Where(line => line.Any(x => x == node.Name))
                    .Select(line => line[0] == node.Name ? line[1] : line[0])
                    .Select(x => nodes.Single(n => n.Name == x))
                    .ToList();
            }

            var start = nodes.Single(x => x.IsStart);

            var queue = new Queue<Cave[]>();
            queue.Enqueue(new[] { start });

            while (queue.Count > 0)
            {
                var path = queue.Dequeue();
                var current = path.Last();

                foreach (var neighbour in current.Neighbours)
                {
                    if (neighbour.IsEnd)
                    {
                        yield return path;
                        continue;
                    }

                    if (neighbour.IsStart || neighbourExclusionPredicate(path, neighbour))
                    {
                        continue;
                    }

                    queue.Enqueue(path.Append(neighbour).ToArray());
                }
            }
        }

        private class Cave
        {
            public string Name { get; set; }

            public List<Cave> Neighbours { get; set; }

            public bool IsLarge { get; set; }

            public bool IsStart { get; set; }

            public bool IsEnd { get; set; }
        }
    }
}
