using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode
{
    class Day7
    {
        public enum HandRank
        {
            HighCard,
            OnePair,
            TwoPairs,
            ThreeOfAKind,
            FullHouse,
            FourOfAKind,
            FiveOfAKind,
        }
        public readonly static Dictionary<char, int> cardValue = new Dictionary<char, int>()
        {
            { '2', 2 },
            { '3', 3 },
            { '4', 4 },
            { '5', 5 },
            { '6', 6 },
            { '7', 7 },
            { '8', 8 },
            { '9', 9 },
            { 'T', 10 },
            { 'J', 11 },
            { 'Q', 12 },
            { 'K', 13 },
            { 'A', 14 },
        };
        class Hand : IComparable<Hand>
        {
            public HandRank Type { get; set; }
            public char[] Cards { get; set; }
            public int Bid { get; set; }
            public bool WithJokers { get; set; }
            public Hand(string line, bool withJokers)
            {
                Cards = line.Split(' ')[0].ToCharArray();
                Type = GetHandRank();
                Bid = Convert.ToInt32(line.Split(' ')[1]);
                WithJokers = withJokers;
            }
            public static List<Hand> GetHandsFromFile(string[] fileContent)
            {
                List<Hand> hands = new List<Hand>();
                foreach (var line in fileContent)
                {
                    hands.Add(new Hand(line, false));
                }
                return hands;
            }
            public static List<Hand> GetHandsFromFileWithJokers(string[] fileContent)
            {
                List<Hand> hands = new List<Hand>();
                foreach (var line in fileContent)
                {
                    hands.Add(new Hand(line, true));
                }
                return hands;
            }
            private HandRank GetHandRank()
            {
                Dictionary<char, int> keyValuePairs = new Dictionary<char, int>();
                foreach (var card in Cards)
                {
                    if (keyValuePairs.ContainsKey(card))
                    {
                        keyValuePairs[card]++;
                    }
                    else
                    {
                        keyValuePairs.Add(card, 1);
                    }
                }
                if (WithJokers && keyValuePairs.ContainsKey('J') && keyValuePairs['J'] != 5)
                {
                    int jokerCount = keyValuePairs['J'];
                    char mostCommonCardWithoutJoker = keyValuePairs.Where(Cards => Cards.Key != 'J').OrderByDescending(Cards => Cards.Value).First().Key;
                    for (int i = 0; i < jokerCount; i++)
                    {
                        keyValuePairs[mostCommonCardWithoutJoker]++;
                    }
                    keyValuePairs.Remove('J');

                }
                switch (keyValuePairs.Count)
                {
                    case 5:
                        return HandRank.HighCard;
                    case 4:
                        return HandRank.OnePair;
                    case 3:
                        return keyValuePairs.ContainsValue(3) ? HandRank.ThreeOfAKind : HandRank.TwoPairs;
                    case 2:
                        return keyValuePairs.ContainsValue(4) ? HandRank.FourOfAKind : HandRank.FullHouse;
                    case 1:
                        return HandRank.FiveOfAKind;
                    default:
                        throw new Exception("Invalid hand");
                }
            }
            public static Dictionary<Hand, int> RankHands(List<Hand> hands)
            {
                Dictionary<Hand, int> handRanks = new Dictionary<Hand, int>();
                hands.Sort();
                int rank = 1;
                foreach (var hand in hands)
                {
                    handRanks.Add(hand, rank);
                    rank++;
                }
                return handRanks;
            }
            public static double CalculateTotalWinnings(List<Hand> hands)
            {
                var handRanks = RankHands(hands);
                double totalWinnings = 0;
                foreach (var hand in hands)
                {
                    totalWinnings += hand.Bid * handRanks[hand];
                }
                return totalWinnings;
            }
            public int CompareTo(Hand otherHand)
            {
                if (Type > otherHand.Type)
                {
                    return 1;
                }
                else if (Type < otherHand.Type)
                {
                    return -1;
                }
                for (int i = 0; i < Cards.Length; i++)
                {
                    if (WithJokers && otherHand.Cards[i] == 'J' && Cards[i] != 'J')
                    {
                        return 1;
                    }
                    if (WithJokers && Cards[i] == 'J' && otherHand.Cards[i] != 'J')
                    {
                        return -1;
                    }
                    if (cardValue[Cards[i]] > cardValue[otherHand.Cards[i]])
                    {
                        return 1;
                    }
                    else if (cardValue[Cards[i]] < cardValue[otherHand.Cards[i]])
                    {
                        return -1;
                    }
                    else
                    {
                        continue;
                    }
                }
                return 0;
            }
            public override string ToString()
            {
                return $"{new string(Cards)} {Bid}\t{Type}";
            }
        }
        static void FirstPart(string fileContent)
        {
            Console.WriteLine("First part:");
            List<Hand> hands = Hand.GetHandsFromFile(fileContent.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries));
            Console.WriteLine($"Total winnings: {Hand.CalculateTotalWinnings(hands)}");
            Console.WriteLine();
        }
        static void SecondPart(string fileContent)
        {
            Console.WriteLine("Second part:");
            List<Hand> hands = Hand.GetHandsFromFileWithJokers(fileContent.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries));
            hands.Sort();
            Console.WriteLine($"Total winnings: {Hand.CalculateTotalWinnings(hands)}");
            Console.WriteLine();
        }
        public static void ShowResults()
        {
            var fileContent = File.ReadAllText("../../Day7/day7input.txt");
            Console.WriteLine("Day 7:");
            FirstPart(fileContent);
            SecondPart(fileContent);
        }
    }
}