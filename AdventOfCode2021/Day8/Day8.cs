namespace AdventOfCode2021
{
    public class Day8
    {
        public static async Task SolvePart1()
        {
            var inputs = (await File.ReadAllLinesAsync(@"Day8/Day8Input.txt"))
                .SelectMany(x => x.Split(" | ")[1].Split(' '));

            var count = inputs
                .Where(str => str.Length == 2 || str.Length == 3 || str.Length == 4 || str.Length == 7)
                .Count();

            Console.WriteLine($"Day 8 part 1: {count}");
        }

        public static async Task SolvePart2()
        {
            var inputs = (await File.ReadAllLinesAsync(@"Day8/Day8Input.txt"))
                .Select(line => line.Split(" | "))
                .Select(split => (Unique: split[0].Split(' ').OrderBy(x => x.Length).ToArray(), Output: split[1].Split(' ')))
                .ToList();

            var outputs = new List<int>();
            foreach (var input in inputs)
            {
                var n1 = input.Unique[0];
                var n7 = input.Unique[1];
                var n4 = input.Unique[2];
                var n8 = input.Unique[9];
                var n3 = input.Unique.Single(x => x.Length == 5 && n7.All(ch => x.Contains(ch)));

                var b = n4.Single(ch => !n3.Contains(ch));

                var n5 = input.Unique.Single(x => x.Length == 5 && x.Contains(b));
                var n2 = input.Unique.Single(x => x.Length == 5 && x != n3 && x != n5);
                var n9 = input.Unique.Single(x => x.Length == 6 && n3.All(ch => x.Contains(ch)));
                var n0 = input.Unique.Single(x => x.Length == 6 && x != n9 && n1.All(ch => x.Contains(ch)));
                var n6 = input.Unique.Single(x => x.Length == 6 && x != n9 && x != n0);

                var a = n7.Single(ch => !n1.Contains(ch));
                var c = n1.Single(ch => !n5.Contains(ch));
                var d = n4.Single(ch => !n0.Contains(ch));
                var e = n8.Single(ch => !n9.Contains(ch));
                var f = n1.Single(ch => !n2.Contains(ch));
                var g = n8.Single(ch => !n4.Append(a).Append(e).Contains(ch));

                var keys = new[]
                    {
                        new[] { a, b, c, e, f, g },
                        new[] { c, f },
                        new[] { a, c, d, e, g },
                        new[] { a, c, d, f, g },
                        new[] { b, c, d, f },
                        new[] { a, b, d, f, g },
                        new[] { a, b, d, e, f, g },
                        new[] { a, c, f },
                        new[] { a, b, c, d, e, f, g },
                        new[] { a, b, c, d, f, g }
                    }
                    .Select((x, i) => (Key: x.ToList(), Index: i))
                    .ToList();

                var output = input.Output
                    .Select(x => Match(x, keys))
                    .Reverse()
                    .Select((x, i) => x * (int) Math.Pow(10, i))
                    .Sum();
                
                outputs.Add(output);
            }

            var sum = outputs.Sum();
            Console.WriteLine($"Day 8 part 2: {sum}");
        }
        private static int Match(string cipher, List<(List<char> Key, int Index)> keys) => keys.Single(x => cipher.Length == x.Key.Count && cipher.All(ch => x.Key.Contains(ch))).Index;
    }
}
