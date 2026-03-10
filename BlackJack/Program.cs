using System;
using System.Collections.Generic;
using System.Linq;

namespace BlackjackGame
{
    public class Card
    {
        public int Value { get; }
        public Card(int value) => Value = value;
    }

    public class Deck
    {
        private List<Card> _cards;
        private readonly Random _rnd = new Random();

        public Deck() => Reset();

        public void Reset()
        {
            int[] values = {
                2,2,2,2, 3,3,3,3, 4,4,4,4, 5,5,5,5, 6,6,6,6, 7,7,7,7, 8,8,8,8, 9,9,9,9,
                10,10,10,10, 2,2,2,2, 3,3,3,3, 4,4,4,4, 11,11,11,11
            };
            _cards = values.Select(v => new Card(v)).ToList();
        }

        public Card Draw()
        {
            if (_cards.Count == 0) Reset();
            int index = _rnd.Next(_cards.Count); 
            Card card = _cards[index];
            _cards.RemoveAt(index);
            return card;
        }
    }

    public class Participant
    {
        public string Name { get; }
        public List<Card> Hand { get; } = new List<Card>();
        public int Score => Hand.Sum(c => c.Value);

        public Participant(string name) => Name = name;

        public void TakeCard(Card card)
        {
            Hand.Add(card);
            Console.WriteLine($"{Name} drew: {card.Value}. Total score: {Score}");
        }

        public void ClearHand() => Hand.Clear();
    }

    public class BlackjackEngine
    {
        private readonly Deck _deck = new Deck();
        private readonly Participant _player = new Participant("Player");
        private readonly Participant _dealer = new Participant("Dealer");

        private int _wins = 0, _losses = 0, _ties = 0;

        public void Start()
        {
            string exitChoice = "";
            while (exitChoice?.ToUpper() != "N")
            {
                PlayRound();

                Console.WriteLine("\nType 'N' to exit, or any other key to continue:");
                exitChoice = Console.ReadLine();
            }
            Console.WriteLine($"\nFinal Stats - Wins: {_wins}, Losses: {_losses}, Ties: {_ties}");
        }

        private void PlayRound()
        {
            _player.ClearHand();
            _dealer.ClearHand();
            _deck.Reset();

            Console.WriteLine("\n--- New Round ---");

            for (int i = 0; i < 3; i++) _dealer.TakeCard(_deck.Draw());
            Console.WriteLine($"Dealer has a total of: {_dealer.Score}\n");

            for (int i = 0; i < 2; i++) _player.TakeCard(_deck.Draw());

            while (_player.Score <= 21)
            {
                Console.WriteLine("Do you want to draw another card? (N to stop)");
                string input = Console.ReadLine()?.ToUpper();
                if (input == "N") break;

                _player.TakeCard(_deck.Draw());
                Console.WriteLine($"Current total: {_player.Score}");
            }

            DetermineWinner();
        }

        private void DetermineWinner()
        {
            int pScore = _player.Score;
            int dScore = _dealer.Score;

            if (dScore > 21)
            {
                Console.WriteLine($"You win! Dealer busted with: {dScore}");
                _wins++;
            }
            else if (pScore > 21)
            {
                Console.WriteLine($"You lost! You busted with: {pScore}");
                _losses++;
            }
            else if (pScore > dScore)
            {
                Console.WriteLine($"You win! {pScore} vs {dScore}");
                _wins++;
            }
            else if (pScore == dScore)
            {
                Console.WriteLine($"It's a tie! Both have: {pScore}");
                _ties++;
            }
            else
            {
                Console.WriteLine($"You lost! {pScore} vs {dScore}");
                _losses++;
            }
        }
    }

    class Program
    {
        static void Main()
        {
            var game = new BlackjackEngine();
            game.Start();
        }
    }
}