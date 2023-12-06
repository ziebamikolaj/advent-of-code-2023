using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode
{
    class Day4
    {
        public class Part1
        {
            public class Card
            {
                public HashSet<int> WinningNumbers { get; private set; } = new HashSet<int>();
                public List<int> Numbers { get; private set; } = new List<int>();
                public int CardId { get; private set; }
                public int Points { get; set; }
                public static int GetSumOfPoints(List<Card> cards)
                {
                    return cards.Sum(x => x.Points);
                }
                public static List<Card> GetCards(string fileContent)
                {
                    List<Card> cards = new List<Card>();
                    var lines = fileContent.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

                    foreach (var line in lines)
                    {
                        if (line.Length < 1) continue;
                        var card = ParseLineToCard(line);
                        if (card != null) cards.Add(card);
                    }
                    return cards;
                }
                private static Card ParseLineToCard(string line)
                {
                    Card card = new Card();
                    bool winningNumbersEnded = false;

                    foreach (var number in line.Split(' '))
                    {
                        if (number.Contains(":"))
                        {
                            if (int.TryParse(number.Replace(":", ""), out int cardId))
                            {
                                card.CardId = cardId;
                            }
                        }
                        else if (number.Contains("|"))
                        {
                            winningNumbersEnded = true;
                            continue;
                        }
                        else if (winningNumbersEnded)
                        {
                            if (int.TryParse(number, out int num))
                            {
                                card.Numbers.Add(num);
                            }
                        }
                        else
                        {
                            if (int.TryParse(number, out int winningNum))
                            {
                                card.WinningNumbers.Add(winningNum);
                            }
                        }
                    }
                    card.Points = card.GetPoints();
                    return card;
                }
                private int GetPoints()
                {
                    int hits = 0;
                    foreach (var item in Numbers)
                    {
                        if (WinningNumbers.Contains(item)) hits++;
                    }
                    if (hits == 0) return 0;
                    return (int)Math.Pow(2, hits - 1);
                }
            }
        }
        public class Part2
        {
            public class Card
            {
                public HashSet<int> WinningNumbers { get; private set; } = new HashSet<int>();
                public List<int> Numbers { get; private set; } = new List<int>();
                public int CardId { get; private set; }
                public int CardCount { get; private set; }

                public static List<Card> GetCards(string fileContent)
                {
                    List<Card> cards = new List<Card>();
                    var lines = fileContent.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

                    foreach (var line in lines)
                    {
                        if (line.Length < 1) continue;
                        var card = ParseLineToCard(line);
                        if (card != null) cards.Add(card);
                    }
                    return cards;
                }
                private static Card ParseLineToCard(string line)
                {
                    Card card = new Card();
                    bool winningNumbersEnded = false;

                    foreach (var number in line.Split(' '))
                    {
                        if (number.Contains(":"))
                        {
                            if (int.TryParse(number.Replace(":", ""), out int cardId))
                            {
                                card.CardId = cardId;
                            }
                        }
                        else if (number.Contains("|"))
                        {
                            winningNumbersEnded = true;
                            continue;
                        }
                        else if (winningNumbersEnded)
                        {
                            if (int.TryParse(number, out int num))
                            {
                                card.Numbers.Add(num);
                            }
                        }
                        else
                        {
                            if (int.TryParse(number, out int winningNum))
                            {
                                card.WinningNumbers.Add(winningNum);
                            }
                        }
                    }
                    card.CardCount = 1;
                    return card;
                }
                public static int GetTotalCards(List<Card> cards)
                {
                    int totalScratchCards = 0;
                    for (int i = 0; i < cards.Count; i++)
                    {
                        totalScratchCards += cards[i].CardCount;
                        //Console.WriteLine(cards[i] + "\tMatches: " + cards[i].GetNumberOfMatches() + "\tCount: " + cards[i].CardCount + "\tsum: " + totalScratchCards);
                        for (int j = 1; j <= cards[i].GetNumberOfMatches(); j++)
                        {
                            cards[i + j].CardCount += cards[i].CardCount;
                        }
                    }
                    return totalScratchCards;
                }

                private int GetNumberOfMatches()
                {
                    int matches = 0;
                    foreach (var item in Numbers)
                    {
                        if (WinningNumbers.Contains(item)) matches++;
                    }
                    return matches;
                }
                public override string ToString()
                {
                    string winningNumbers = string.Empty;
                    string numbers = string.Empty;
                    foreach (var item in Numbers)
                    {
                        numbers += $"{item:00} ";
                    }
                    foreach (var item in WinningNumbers)
                    {
                        winningNumbers += $"{item:00} ";
                    }
                    return $"Card {CardId}: {winningNumbers} | {numbers}";
                }
            }
        }
        public static void ShowResults()
        {
            var fileContent = File.ReadAllText("../../Day4/day4input.txt");
            var cardsPart1 = Part1.Card.GetCards(fileContent);
            var cardsPart2 = Part2.Card.GetCards(fileContent);
            Console.WriteLine("Day 4:");
            Console.WriteLine("First part:");
            Console.WriteLine(Part1.Card.GetSumOfPoints(cardsPart1));
            Console.WriteLine("Second part:");
            Console.WriteLine(Part2.Card.GetTotalCards(cardsPart2));
        }
    }
}