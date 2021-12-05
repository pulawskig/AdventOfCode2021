using System.Collections;

namespace AdventOfCode2021
{
    public class Day3
    {
        public static async Task SolvePart1()
        {
            var inputs = (await File.ReadAllLinesAsync(@"Day3\Day3Input.txt"))
                .Where(str => !string.IsNullOrEmpty(str))
                .Select(str => str.Select(c => c == '1').ToArray())
                .ToList();

            var binLength = inputs[0].Length;

            var gammaBool = new bool[binLength];
            var sigmaBool = new bool[binLength];
            for (var i = 0; i < binLength; i++)
            {
                gammaBool[i] = inputs
                    .Where(boolArray => boolArray[i])
                    .Count() > (inputs.Count / 2);
                sigmaBool[i] = !gammaBool[i];
            }

            var gamma = ConvertToInt(gammaBool);
            var sigma = ConvertToInt(sigmaBool);

            Console.WriteLine($"Day 3 part 1: {gamma * sigma}");
        }

        public static async Task SolvePart2()
        {
            var inputs = (await File.ReadAllLinesAsync(@"Day3\Day3Input.txt"))
                .Where(str => !string.IsNullOrEmpty(str))
                .Select(str => str.Select(c => c == '1').ToArray())
                .ToList();

            var co2Inputs = inputs.ToArray();
            var scrubberInputs = inputs.ToArray();

            var i = 0;
            while (co2Inputs.Length > 1)
            {
                var mostCommon = co2Inputs
                    .Where(boolArray => boolArray[i])
                    .Count() >= (co2Inputs.Length / 2m);

                co2Inputs = co2Inputs.Where(ba => ba[i] == mostCommon).ToArray();

                i++;
            }

            i = 0;
            while (scrubberInputs.Length > 1)
            {
                var leastCommon = scrubberInputs
                    .Where(boolArray => boolArray[i])
                    .Count() < (scrubberInputs.Length / 2m);

                scrubberInputs = scrubberInputs.Where(ba => ba[i] == leastCommon).ToArray();

                i++;
            }

            var co2 = ConvertToInt(co2Inputs[0]);
            var scrubber = ConvertToInt(scrubberInputs[0]);

            Console.WriteLine($"Day 3 part 2: {co2 * scrubber}");
        }

        private static int ConvertToInt(bool[] source)
        {
            var returnBytes = new byte[4];
            var bitField = new BitArray(source.Reverse().ToArray());
            bitField.CopyTo(returnBytes, 0);
            return BitConverter.ToInt32(returnBytes);
        }
    }
}
