using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
    class Day1
    {
        static readonly Dictionary<string, string> NumbersInEnglish = new Dictionary<string, string>
        {
            {"one", "1"},
            {"two", "2"},
            {"three", "3"},
            {"four", "4"},
            {"five", "5"},
            {"six", "6"},
            {"seven", "7"},
            {"eight", "8"},
            {"nine", "9"}
            };

        static string ReplaceNumbers(string input)
        {
            var pattern = string.Join("|", NumbersInEnglish.Keys.OrderByDescending(k => k.Length));
            var regex = new Regex(pattern);
            while (regex.IsMatch(input))
            {
                input = Regex.Replace(input, pattern, m =>
                {
                    var match = m.Value;
                    return NumbersInEnglish[m.Value] + match.Last();
                });
            }
            return input;
        }

        static float GetCalibrationSum(string fileContent)
        {
            float sum = 0;
            foreach (string line in fileContent.Split('\n'))
            {
                if (line.Length == 0) continue;
                var tempArray = line.Where(char.IsDigit).ToArray();
                var tempNumber = $"{tempArray[0]}{tempArray.Last()}";
                sum += Convert.ToInt32(tempNumber);
            }
            return sum;
        }

        static void FirstPart(string fileContent)
        {
            Console.WriteLine("First part:");
            Console.WriteLine(GetCalibrationSum(fileContent));
        }
        static void SecondPart(string fileContent)
        {
            var modifiedFileContent = fileContent.Split('\n');
            for (int i = 0; i < modifiedFileContent.Length; i++) modifiedFileContent[i] = ReplaceNumbers(modifiedFileContent[i]);

            Console.WriteLine("Second part:");
            Console.WriteLine(GetCalibrationSum(string.Join("\n", modifiedFileContent)));
        }
        public static void ShowResults()
        {
            var fileContent = File.ReadAllText("../../Day1/day1input.txt");
            Console.WriteLine("Day 1:");
            FirstPart(fileContent);
            SecondPart(fileContent);
        }
    }
}