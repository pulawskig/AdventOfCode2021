using System.Numerics;

namespace AdventOfCode2021
{
    public class Day16
    {
        private static Dictionary<char, string> HexDict = new Dictionary<char, string>
            {
                { '0', "0000" },
                { '1', "0001" },
                { '2', "0010" },
                { '3', "0011" },
                { '4', "0100" },
                { '5', "0101" },
                { '6', "0110" },
                { '7', "0111" },
                { '8', "1000" },
                { '9', "1001" },
                { 'A', "1010" },
                { 'B', "1011" },
                { 'C', "1100" },
                { 'D', "1101" },
                { 'E', "1110" },
                { 'F', "1111" },
            };

    public static async Task SolvePart1()
        {
            var input = string.Join(string.Empty,
                (await File.ReadAllLinesAsync("Day16/Day16Input.txt"))[0]
                    .Select(ch => HexDict[ch]));

            var (packet, bits) = GetPacket(input, 0);
            var sum = SumVersions(packet);

            Console.WriteLine($"Day 16 part 1: {sum}");
        }

        public static async Task SolvePart2()
        {
            var input = string.Join(string.Empty,
                (await File.ReadAllLinesAsync("Day16/Day16Input.txt"))[0]
                    .Select(ch => HexDict[ch]));

            var (packet, bits) = GetPacket(input, 0);

            var value = packet.Calculate();

            Console.WriteLine($"Day 16 part 2: {value}");
        }

        private static (Packet Packet, int Bits) GetPacket(string input, int startIndex)
        {
            var version = input[startIndex..(startIndex + 3)];
            var type = input[(startIndex + 3)..(startIndex + 6)];
            var bits = 6;

            var packet = new Packet
            {
                VersionId = Convert.ToInt32(version, 2),
                TypeId = Convert.ToInt32(type, 2),
            };

            if (packet.Type == Packet.Types.Value)
            {
                var last = false;
                var result = string.Empty;
                while (!last)
                {
                    var lastCheck = input[startIndex + bits];
                    if (lastCheck == '0')
                        last = true;

                    result += input[(startIndex + bits + 1)..(startIndex + bits + 5)];
                    bits += 5;
                }
                packet.Value = Convert.ToInt64(result, 2);
            }
            else
            {
                var length = input[startIndex + bits];
                packet.LengthId = length == '1' ? 1 : 0;
                bits++;

                if (packet.Length == Packet.Lengths.Bits)
                {
                    var bitCountBits = input[(startIndex + bits)..(startIndex + bits + 15)];
                    var bitCount = Convert.ToInt32(bitCountBits, 2);
                    bits += 15;

                    var bitsUsed = 0;
                    while (bitsUsed < bitCount)
                    {
                        var (p, b) = GetPacket(input, startIndex + bits);

                        bits += b;
                        bitsUsed += b;

                        packet.SubPackets.Add(p);
                    }
                }
                else
                {
                    var countBits = input[(startIndex + bits)..(startIndex + bits + 11)];
                    var count = Convert.ToInt32(countBits, 2);
                    bits += 11;

                    for (var i = 0; i < count; i++)
                    {
                        var (p, b) = GetPacket(input, startIndex + bits);

                        bits += b;
                        packet.SubPackets.Add(p);
                    }
                }
            }

            return (packet, bits);
        }

        private static int SumVersions(Packet packet)
        {
            var sum = packet.VersionId;

            foreach (var sp in packet.SubPackets)
                sum += SumVersions(sp);

            return sum;
        }

        private class Packet
        {
            public enum Types
            {
                Sum = 0,
                Product = 1,
                Minimum = 2,
                Maximum = 3,
                Value = 4,
                GreaterThan = 5,
                LessThan = 6,
                EqualTo = 7,
            }

            public enum Lengths
            {
                Bits = 0,
                Packets = 1,
            }

            public int VersionId { get; set; }

            public int TypeId { get; set; }

            public Types Type => (Types)TypeId;

            public BigInteger? Value { get; set; }

            public List<Packet> SubPackets { get; } = new List<Packet>();

            public int LengthId { get; set; }

            public Lengths Length => (Lengths)LengthId;

            public BigInteger Calculate()
            {
                switch(Type)
                {
                    case Types.Value:
                        return Value ?? BigInteger.Zero;
                    case Types.Sum:
                        return SubPackets
                            .Select(sp => sp.Calculate())
                            .Aggregate(BigInteger.Zero, (a, b) => a + b);
                    case Types.Product:
                        return SubPackets
                            .Select(sp => sp.Calculate())
                            .Aggregate(BigInteger.One, (a, b) => a * b);
                    case Types.Minimum:
                        return SubPackets
                            .Select(sp => sp.Calculate())
                            .Min();
                    case Types.Maximum:
                        return SubPackets
                            .Select(sp => sp.Calculate())
                            .Max();
                    case Types.GreaterThan:
                        return SubPackets[0].Calculate() > SubPackets[1].Calculate() ? BigInteger.One : BigInteger.Zero;
                    case Types.LessThan:
                        return SubPackets[0].Calculate() < SubPackets[1].Calculate() ? BigInteger.One : BigInteger.Zero;
                    case Types.EqualTo:
                        return SubPackets[0].Calculate() == SubPackets[1].Calculate() ? BigInteger.One : BigInteger.Zero;
                }
                return 0L;
            }
        }
    }
}
