namespace AdventOfCode2021
{
    public class Day4
    {
        public static async Task SolvePart1()
        {
            var (queue, boards) = await GetInputs();

            ValueMark[][] winnerBoard = null;
            var check = 0;

            while (queue.Count > 0 && winnerBoard == null)
            {
                check = queue.Dequeue();

                foreach (var x in boards.SelectMany(b => b).SelectMany(l => l).Where(n => n.Value == check))
                {
                    x.Marked = true;
                }

                foreach (var board in boards)
                {
                    if (board.Any(l => l.All(n => n.Marked)))
                    {
                        winnerBoard = board;
                        break;
                    }

                    for (var i = 0; i < board[0].Length; i++)
                    {
                        if (board.All(l => l[i].Marked))
                        {
                            winnerBoard = board;
                            break;
                        }
                    }

                    if (winnerBoard != null)
                        break;
                }
            }

            var unmarked = winnerBoard
                .SelectMany(l => l)
                .Where(n => !n.Marked)
                .Sum(n => n.Value);

            Console.WriteLine($"Day 4 part 1: {check * unmarked}");
        }

        public static async Task SolvePart2()
        {
            var (queue, boards) = await GetInputs();

            var check = 0;
            ValueMark[][] loserBoard = null;

            while (queue.Count > 0 && loserBoard == null)
            {
                check = queue.Dequeue();

                foreach (var x in boards.SelectMany(b => b).SelectMany(l => l).Where(n => n.Value == check))
                {
                    x.Marked = true;
                }

                foreach (var board in boards.ToList())
                {
                    if (board.Any(l => l.All(n => n.Marked)))
                    {
                        if (boards.Count == 1)
                            loserBoard = board;
                        else
                            boards.Remove(board);
                        continue;
                    }

                    for (var i = 0; i < board[0].Length; i++)
                    {
                        if (board.All(l => l[i].Marked))
                        {
                            if (boards.Count == 1)
                                loserBoard = board;
                            else
                                boards.Remove(board);
                            break;
                        }
                    }
                }
            }

            var unmarked = loserBoard
                .SelectMany(l => l)
                .Where(n => !n.Marked)
                .Sum(n => n.Value);

            Console.WriteLine($"Day 4 part 2: {check * unmarked}");
        }

        private static async Task<(Queue<int>, List<ValueMark[][]>)> GetInputs()
        {
            var allLines = await File.ReadAllLinesAsync(@"Day4\Day4Input.txt");
            var queue = new Queue<int>(
                allLines[0]
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(int.Parse)
            );

            var boards = allLines
                .Skip(2)
                .GroupByEmptyLines()
                .Select(g => g
                    .Select(line => line
                        .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                        .Select(s => new ValueMark { Value = int.Parse(s), Marked = false })
                        .ToArray())
                    .ToArray())
                .ToList();

            return (queue, boards);
        }

        private class ValueMark
        {
            public int Value { get; set; }

            public bool Marked { get; set; }

            public override string ToString()
            {
                return $"{Value}, {Marked}";
            }
        }
    }
}
