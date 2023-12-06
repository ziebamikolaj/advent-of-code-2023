using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode
{
    class Day6
    {
        class Race
        {
            private const int AccelerationPerMs = 1;
            public double RecordDistance { get; set; }
            public double AllowedTime { get; set; }
            private List<double> PossibleWins { get; set; }

            public Race(double recordDistance, double allowedTime)
            {
                RecordDistance = recordDistance;
                AllowedTime = allowedTime;
                PossibleWins = GetPossibleWins();
            }
            private List<double> GetPossibleWins()
            {
                List<double> possibleWins = new List<double>();
                for (int i = 1; i < AllowedTime; i++)
                {
                    double distance = (AccelerationPerMs * i) * (AllowedTime - i);
                    if (distance > RecordDistance)
                    {
                        possibleWins.Add(i);
                    }
                }
                return possibleWins;
            }
            public static int CalculateTotalWinCombinations(List<Race> races)
            {
                int result = 1;
                foreach (var race in races)
                {
                    result *= race.PossibleWins.Count;
                }
                return result;
            }
            public static List<Race> GetRacesFromFile(string[] fileContent)
            {
                List<Race> races = new List<Race>();
                var times = fileContent[0].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                var distances = fileContent[1].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 1; i < times.Length; i++) // skipping header
                {
                    races.Add(new Race(Convert.ToInt32(distances[i]), Convert.ToInt32(times[i])));
                }
                return races;
            }
            public static Race GetCombinedRace(string[] fileContent)
            {
                double time = Convert.ToDouble(new string(fileContent[0].Where(x => char.IsDigit(x)).ToArray()));
                double distance = Convert.ToDouble(new string(fileContent[1].Where(x => char.IsDigit(x)).ToArray()));
                return new Race(distance, time);
            }
            public override string ToString()
            {
                return $"Current record distance: {RecordDistance} mil in {AllowedTime}ms{Environment.NewLine}Possible winning situations: {PossibleWins.Count}{Environment.NewLine}";
            }
        }
        static void FirstPart(string fileContent)
        {
            Console.WriteLine("First part:");
            Console.WriteLine();
            List<Race> races = Race.GetRacesFromFile(fileContent.Split(new[] { Environment.NewLine }, StringSplitOptions.None));
            foreach (var item in races)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine($"Multiplied numbers of possible wins: {Race.CalculateTotalWinCombinations(races)}{Environment.NewLine}");
        }
        static void SecondPart(string fileContent)
        {
            Console.WriteLine("Second part:");
            Console.WriteLine();
            Race bigRace = Race.GetCombinedRace(fileContent.Split(new[] { Environment.NewLine }, StringSplitOptions.None));
            Console.WriteLine(bigRace);
        }
        public static void ShowResults()
        {
            var fileContent = File.ReadAllText("../../Day6/day6input.txt");
            Console.WriteLine("Day 6:");
            FirstPart(fileContent);
            SecondPart(fileContent);
        }

    }
}