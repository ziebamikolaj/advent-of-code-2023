using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode
{
    class Day3
    {
        public class EngineSchematic
        {
            public static int[][] CoordsOffsetToCheck = new int[][]
            {
            new int[] {1, 0},
            new int[] {1, 1},
            new int[] {1, -1},
            new int[] {-1, 0},
            new int[] {-1, 1},
            new int[] {-1, -1},
            new int[] {0, 1},
            new int[] {0, -1},
            };
            public static char[][] Schema { get; set; }
            public int SumOfPartNumbers { get; set; }
            public double SumOfGearRatios { get; set; }
            public List<int> PartNumbers { get; set; }
            private List<double> GearRatios { get; set; }
            public EngineSchematic(string fileContent)
            {
                Schema = GetSchema(fileContent);
                PartNumbers = GetPartNumbers();
                GearRatios = GetGearRatios();
                SumOfPartNumbers = PartNumbers.Sum();
                SumOfGearRatios = GearRatios.Sum();
            }

            private List<int> GetPartNumbers()
            {
                List<int> partNumbers = new List<int>();
                for (int i = 0; i < Schema.Length; i++)
                {
                    string tempNumber = "";
                    bool isAdjacent = false;

                    for (int j = 0; j < Schema[i].Length; j++)
                    {
                        if (char.IsDigit(Schema[i][j]))
                        {
                            tempNumber += Schema[i][j];
                            if (!isAdjacent) isAdjacent = IsAdjacentToSymbol(i, j);
                        }
                        if ((!char.IsDigit(Schema[i][j]) || j == Schema[i].Length - 1) && tempNumber.Length > 0)
                        {
                            if (isAdjacent) partNumbers.Add(Convert.ToInt32(tempNumber));
                            tempNumber = "";
                            isAdjacent = false;
                        }
                    }
                }
                return partNumbers;
            }
            private bool IsAdjacentToSymbol(int i, int j)
            {
                foreach (int[] offset in CoordsOffsetToCheck)
                {
                    if (!IsIndexValid(i + offset[0], j + offset[1])) continue;
                    if (IsSymbol(Schema[i + offset[0]][j + offset[1]])) return true;
                }
                return false;
            }

            // PART 2 - Gear Ratios
            private List<double> GetGearRatios()
            {
                var result = new List<double>();
                for (int i = 0; i < Schema.Length; i++)
                {
                    for (int j = 0; j < Schema[i].Length; j++)
                        if (Schema[i][j] == '*')
                        {
                            List<GearNumber> gearNumbers = GetAdjacentNumbers(i, j);
                            gearNumbers = gearNumbers.GroupBy(g => new
                            {
                                g.X,
                                g.Y
                            }).Select(g => g.First()).ToList();
                            if (gearNumbers.Count == 2) result.Add(gearNumbers[0].Value * gearNumbers[1].Value);
                        }
                }
                return result;
            }
            private List<GearNumber> GetAdjacentNumbers(int i, int j)
            {
                List<GearNumber> numbers = new List<GearNumber>();
                foreach (int[] offset in CoordsOffsetToCheck)
                {
                    if (!IsIndexValid(i + offset[0], j + offset[1])) continue;
                    if (!char.IsDigit(Schema[i + offset[0]][j + offset[1]])) continue;
                    numbers.Add(new GearNumber(i + offset[0], j + offset[1]));
                }
                return numbers;
            }
            private bool IsSymbol(char c)
            {
                return (!char.IsDigit(c) && c != '.');
            }
            private static bool IsIndexValid(int i, int j)
            {
                return i >= 0 && i < Schema.Length && j >= 0 && j < Schema[i].Length;
            }
            private char[][] GetSchema(string fileContent)
            {
                string[] fileContentByLine = fileContent.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
                var schema = new char[fileContentByLine.Length][];
                for (int i = 0; i < fileContentByLine.Length; i++)
                {
                    schema[i] = fileContentByLine[i].ToCharArray();
                }
                return schema;
            }
            class GearNumber
            {
                public double Value { get; set; }
                public int X { get; set; }
                public int Y { get; set; }

                public GearNumber(int x, int y)
                {
                    double[] numberInfo = GetNumber(x, y);
                    Value = numberInfo[0];
                    X = x;
                    Y = (int)numberInfo[1];
                }

                private double[] GetNumber(int x, int y)
                {
                    double[] info = new double[2];
                    if (!char.IsDigit(Schema[x][y])) return info;
                    while (y >= 0 && char.IsDigit(Schema[x][y]))
                    {
                        y--;
                    }
                    info[0] = GetNumberFromIndex(x, y + 1);
                    info[1] = y + 1;
                    return info;
                }

                private int GetNumberFromIndex(int x, int y)
                {
                    string number = "";
                    while (char.IsDigit(Schema[x][y]))
                    {
                        number += Schema[x][y];
                        y++;
                        if (y >= Schema[x].Length) break;
                    }

                    return Convert.ToInt32(number);
                }
            }
        }
        public static void ShowResults()
        {
            var fileContent = File.ReadAllText("../../day3input.txt");
            EngineSchematic engineSchematic = new EngineSchematic(fileContent);
            Console.WriteLine("Day 3:");
            Console.WriteLine("First part:");
            Console.WriteLine(engineSchematic.SumOfPartNumbers);
            Console.WriteLine("Second part:");
            Console.WriteLine(engineSchematic.SumOfGearRatios);
        }
    }
}
