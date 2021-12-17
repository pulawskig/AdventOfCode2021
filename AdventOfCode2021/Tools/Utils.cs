namespace AdventOfCode2021
{
    public static class Utils
    {
        public static List<List<string>> GroupByEmptyLines(this IEnumerable<string> lines)
        {
            var groups = GroupByLines(lines, line => string.IsNullOrEmpty(line));

            foreach (var group in groups)
            {
                group.RemoveAt(0);
            }

            return groups;
        }

        public static List<List<string>> GroupByLines(IEnumerable<string> input, Func<string, bool> predicate)
        {
            var result = new List<List<string>>();

            var properInput = input as string[] ?? input.ToArray();

            var last = 0;
            for (var i = 0; i < properInput.Length; i++)
            {
                var line = properInput[i];

                if (predicate(line))
                {
                    result.Add(input.Skip(last).Take(i - last).ToList());
                    last = i;
                }
            }

            result.Add(input.Skip(last).Take(properInput.Length - last).ToList());

            return result;
        }

        public static IEnumerable<T> Flatten<T>(this T[,] map)
        {
            for (int row = 0; row < map.GetLength(0); row++)
            {
                for (int col = 0; col < map.GetLength(1); col++)
                {
                    yield return map[row, col];
                }
            }
        }

        public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, IComparer<TKey>? comparer = null)
        {
            comparer ??= Comparer<TKey>.Default;

            using (var sourceIterator = source.GetEnumerator())
            {
                if (!sourceIterator.MoveNext())
                {
                    throw new InvalidOperationException("Sequence contains no elements");
                }

                var min = sourceIterator.Current;
                var minKey = selector(min);

                while (sourceIterator.MoveNext())
                {
                    var candidate = sourceIterator.Current;
                    var candidateProjected = selector(candidate);

                    if (comparer.Compare(candidateProjected, minKey) < 0)
                    {
                        min = candidate;
                        minKey = candidateProjected;
                    }
                }

                return min;
            }
        }

        public static TSource Median<TSource>(this IEnumerable<TSource> source)
        {
            return source.OrderBy(x => x).Skip(source.Count() / 2).First();
        }

        public static bool IsBetween(this int item, int first, int second)
        {
            var start = Math.Min(first, second);
            var end = Math.Max(first, second);

            return item >= start && item <= end;
        }
    }
}
