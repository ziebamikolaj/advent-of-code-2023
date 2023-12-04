using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
    class Day2
    {
        class Game
        {
            const int MaxRedCubes = 12;
            const int MaxGreenCubes = 13;
            const int MaxBlueCubes = 14;
            public int GameId { get; set; }
            public int Power { get; set; }
            public List<Round> Rounds { get; set; }
            public Game(int gameId, List<Round> rounds)
            {
                GameId = gameId;
                Rounds = rounds;
                Power = getPower();
            }
            public static List<Game> GetValidGames(List<Game> games)
            {
                List<Game> validGames = new List<Game>();
                foreach (Game game in games)
                {
                    if (game.IsGameValid()) validGames.Add(game);
                }
                return validGames;
            }
            public static List<Game> GetGamesFromInput(string fileContent)
            {
                List<Game> games = new List<Game>();
                foreach (string line in fileContent.Split('\n'))
                {
                    if (line.Length == 0) continue;
                    games.Add(GetGameFromLine(line));
                }
                return games;
            }
            public static int GetSumOfIndexesOfValidGames(List<Game> games)
            {
                int sum = 0;
                List<Game> validGames = GetValidGames(games);
                foreach (Game game in validGames)
                {
                    sum += game.GameId;
                }
                return sum;
            }
            public class Round
            {
                public int RedCubes { get; set; }
                public int GreenCubes { get; set; }
                public int BlueCubes { get; set; }
                public Round(int redCubes, int greenCubes, int blueCubes)
                {
                    RedCubes = redCubes;
                    GreenCubes = greenCubes;
                    BlueCubes = blueCubes;
                }
                public static Round GetRoundFromLine(string line)
                {
                    string redCubesPattern = @"(\d+) red";
                    string greenCubesPattern = @"(\d+) green";
                    string blueCubesPattern = @"(\d+) blue";
                    int redCubes = 0;
                    int greenCubes = 0;
                    int blueCubes = 0;
                    if (Regex.IsMatch(line, redCubesPattern))
                    {
                        redCubes = Convert.ToInt32(Regex.Match(line, redCubesPattern).Groups[1].Value);
                    }
                    if (Regex.IsMatch(line, greenCubesPattern))
                    {
                        greenCubes = Convert.ToInt32(Regex.Match(line, greenCubesPattern).Groups[1].Value);
                    }
                    if (Regex.IsMatch(line, blueCubesPattern))
                    {
                        blueCubes = Convert.ToInt32(Regex.Match(line, blueCubesPattern).Groups[1].Value);
                    }
                    return new Round(redCubes, greenCubes, blueCubes);
                }
                public override string ToString()
                {
                    return $"Red cubes: {RedCubes}\tGreen cubes: {GreenCubes}\tBlue cubes: {BlueCubes}";
                }
            }
            private bool IsGameValid()
            {
                foreach (Round round in Rounds)
                {
                    if (round.RedCubes > MaxRedCubes || round.GreenCubes > MaxGreenCubes || round.BlueCubes > MaxBlueCubes) return false;
                }
                return true;
            }

            private static Game GetGameFromLine(string line)
            {
                string[] roundsText = line.Split(';');
                List<Round> rounds = new List<Round>();
                foreach (var item in roundsText)
                {
                    rounds.Add(Round.GetRoundFromLine(item));
                }
                int gameId = Convert.ToInt32(Regex.Match(line, @"(\d+)").Value);
                return new Game(gameId, rounds);
            }
            private int getPower()
            {
                int maxRed = 0;
                int maxGreen = 0;
                int maxBlue = 0;
                foreach (Round round in Rounds)
                {
                    if (round.RedCubes > maxRed) maxRed = round.RedCubes;
                    if (round.GreenCubes > maxGreen) maxGreen = round.GreenCubes;
                    if (round.BlueCubes > maxBlue) maxBlue = round.BlueCubes;
                }
                return maxRed * maxGreen * maxBlue;
            }
            public static int GetSumOfPowers(List<Game> games)
            {
                int sum = 0;
                foreach (Game game in games) sum += game.Power;
                return sum;
            }
        }
        public static void ShowResults()
        {
            var fileContent = File.ReadAllText("../../day2input.txt");
            List<Game> games = Game.GetGamesFromInput(fileContent);
            Console.WriteLine("Day 2:");
            Console.WriteLine("First part:");
            Console.WriteLine(Game.GetSumOfIndexesOfValidGames(games));
            Console.WriteLine("Second part:");
            Console.WriteLine(Game.GetSumOfPowers(games));

        }
    }
}
