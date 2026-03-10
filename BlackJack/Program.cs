using System;
using System.Collections.Generic;
using System.Linq;

namespace BlackjackGame
{
    public class Card
    {
        public int Value { get; }
        public string Name { get; }

        public Card(int value, string name)
        {
            Value = value;
            Name = name;
        }
    }

    public class Deck
    {
        private List<Card> _cards;
        private readonly Random _rnd = new Random();

        public Deck() => Reset();

        public void Reset()
        {
            _cards = new List<Card>();
            string[] faces = { "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A" };
            int[] values = { 2, 3, 4, 5, 6, 7, 8, 9, 10, 10, 10, 10, 11 };

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < faces.Length; j++)
                {
                    _cards.Add(new Card(values[j], faces[j]));
                }
            }
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

        public int Score
        {
            get
            {
                int score = Hand.Sum(c => c.Value);
                int acesCount = Hand.Count(c => c.Value == 11);

                while (score > 21 && acesCount > 0)
                {
                    score -= 10; 
                    acesCount--;
                }
                return score;
            }
        }

        public Participant(string name) => Name = name;

        public void TakeCard(Card card, bool isHidden = false)
        {
            Hand.Add(card);
            if (!isHidden)
            {
                Console.WriteLine($"{Name} drew: [{card.Name}]. Current score: {Score}");
            }
            else
            {
                Console.WriteLine($"{Name} drew a hidden card.");
            }
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

            _player.TakeCard(_deck.Draw());
            _player.TakeCard(_deck.Draw());

            Console.WriteLine();

            _dealer.TakeCard(_deck.Draw());
            Card hiddenDealerCard = _deck.Draw();
            _dealer.TakeCard(hiddenDealerCard, isHidden: true);

            Console.WriteLine();

            while (_player.Score < 21)
            {
                Console.WriteLine($"Your score: {_player.Score}. Hit? (Y/N)");
                string input = Console.ReadLine()?.ToUpper();
                if (input == "N") break;

                _player.TakeCard(_deck.Draw());
            }

            if (_player.Score > 21)
            {
                DetermineWinner();
                return;
            }
            Console.WriteLine($"\n--- Dealer's Turn ---");
            Console.WriteLine($"Dealer reveals hidden card: [{hiddenDealerCard.Name}]. Dealer's score: {_dealer.Score}");

            while (_dealer.Score < 17)
            {
                Console.WriteLine("Dealer score is under 17. Drawing...");
                _dealer.TakeCard(_deck.Draw());
            }

            DetermineWinner();
        }

        private void DetermineWinner()
        {
            Console.WriteLine("\n--- Results ---");
            int pScore = _player.Score;
            int dScore = _dealer.Score;

            if (pScore > 21)
            {
                Console.WriteLine($"You busted with {pScore}! Dealer wins.");
                _losses++;
            }
            else if (dScore > 21)
            {
                Console.WriteLine($"Dealer busted with {dScore}! You win.");
                _wins++;
            }
            else if (pScore > dScore)
            {
                Console.WriteLine($"You win! {pScore} vs {dScore}");
                _wins++;
            }
            else if (pScore == dScore)
            {
                Console.WriteLine($"It's a tie! Both have {pScore}");
                _ties++;
            }
            else
            {
                Console.WriteLine($"Dealer wins! {dScore} vs {pScore}");
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