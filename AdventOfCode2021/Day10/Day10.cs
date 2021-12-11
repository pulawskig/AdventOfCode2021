namespace AdventOfCode2021
{
    public static class Day10
    {
        private static readonly Dictionary<char, char> Closes = new Dictionary<char, char>
            {
                { '(', ')' },
                { '[', ']' },
                { '{', '}' },
                { '<', '>' },
            };

        public static async Task SolvePart1()
        {
            var sum = (await File.ReadAllLinesAsync(@"Day10/Day10Input.txt"))
                .PerformCheck((_, failedCharacter) => failedCharacter switch
                {
                    ')' => 3,
                    ']' => 57,
                    '}' => 1197,
                    '>' => 25137,
                    _ => 0
                })
                .Sum();

            Console.WriteLine($"Day 10 part 1: {sum}");
        }

        public static async Task SolvePart2()
        {
            var result = (await File.ReadAllLinesAsync(@"Day10/Day10Input.txt"))
                .PerformCheck((remainingStack, failedCharacter) =>
                    failedCharacter.HasValue
                        ? -1
                        : remainingStack.Aggregate(0L, (acc, x) => acc * 5 + x switch
                            {
                                '(' => 1,
                                '[' => 2,
                                '{' => 3,
                                '<' => 4,
                                _ => 0
                            }))
                .Where(x => x > 0)
                .Median();

            Console.WriteLine($"Day 10 part 2: {result}");
        }

        public static IEnumerable<long> PerformCheck(this IEnumerable<string> source, Func<IEnumerable<char>, char?, long> pointSelector)
        {
            foreach (var input in source)
            {
                var stack = new Stack<char>();
                char? failed = null;

                stack.Push(input[0]);
                foreach (var ch in input.Skip(1))
                {
                    if (Closes[stack.Peek()] == ch)
                        stack.Pop();
                    else if (Closes.Keys.Contains(ch))
                        stack.Push(ch);
                    else
                    {
                        failed = ch;
                        break;
                    }
                }

                yield return pointSelector(stack.ToArray(), failed);
            }
        }
    }
}
