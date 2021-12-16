namespace AdventOfCode2021
{
    public class Day14
    {
        public static async Task SolvePart1()
        {
            var inputs = await File.ReadAllLinesAsync("Day14/Day14Input.txt");

            var template = inputs[0];
            var rules = inputs.Skip(2).ToDictionary(line => line.Substring(0, 2), line => line.Last().ToString());

            for (var i = 0; i < 10; i++)
            {
                template = template
                    .Take(template.Length - 1)
                    .Zip(template.Skip(1))
                    .Join(rules, pair => pair, rule => (rule.Key[0], rule.Key[1]), (pair, rule) => $"{pair.First}{rule.Value}{pair.Second}")
                    .Aggregate("A", (a, b) => a.Substring(0, a.Length - 1) + b);
            }

            var counts = template
                .GroupBy(ch => ch)
                .Select(g => g.Count())
                .OrderByDescending(count => count)
                .ToArray();

            var count = counts[0] - counts[counts.Length - 1];

            Console.WriteLine($"Day 14 part 1: {count}");
        }

        public static async Task SolvePart2()
        {
            var inputs = await File.ReadAllLinesAsync("Day14/Day14Input.txt");

            var template = inputs[0];
            var rules = inputs
                .Skip(2)
                .Select(line => new PolymerPair
                {
                    Key = line.Substring(0, 2),
                    Result = line.Last(),
                    Children = new List<PolymerPair>(),
                })
                .ToList();

            foreach (var rule in rules)
            {
                rule.Children.Add(rules.First(r => r.Key == $"{rule.First}{rule.Result}"));
                rule.Children.Add(rules.First(r => r.Key == $"{rule.Result}{rule.Second}"));
            }

            var counts = rules.ToDictionary(rule => rule.Key, _ => 0L);
            var letterCounts = Enumerable.Range(0, 26).ToDictionary(i => Convert.ToChar('A' + i), _ => 0L);

            for (var i = 0; i < template.Length; i++)
            {
                if (i < template.Length - 1)
                    counts[template.Substring(i, 2)]++;

                letterCounts[template[i]]++;
            }

            for (var i = 0; i < 40; i++)
            {
                var countsCopy = counts.ToDictionary(count => count.Key, count => count.Value);
                foreach (var rule in rules)
                {
                    foreach (var child in rule.Children)
                    {
                        countsCopy[child.Key] += counts[rule.Key];
                    }

                    letterCounts[rule.Result] += counts[rule.Key];
                    countsCopy[rule.Key] -= counts[rule.Key];
                }
                counts = countsCopy;
            }

            var count = letterCounts.Max(x => x.Value) - letterCounts.Where(x => x.Value > 0).Min(x => x.Value);

            Console.WriteLine($"Day 14 part 2: {count}");
        }

        private class PolymerPair
        {
            public string Key { get; set; }

            public char First => Key[0];

            public char Second => Key[1];

            public char Result { get; set; }

            public List<PolymerPair> Children { get; set; }
        }
    }
}
