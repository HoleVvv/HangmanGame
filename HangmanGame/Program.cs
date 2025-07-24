using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        var hangmanGame = new HangmanGame();
        bool playAgain = true;
        while (playAgain)
        {
            hangmanGame.NewGame();
            Console.WriteLine("Would you like to play again (y/n)?");
            string response = Console.ReadLine()?.ToUpper() ?? "";
            playAgain = response == "Y" || response == "YES";
        }
        Console.WriteLine("See you!");
    }
}

public class HangmanGame
{
    public void NewGame()
    {
        var wordProvider = new HangmanWords();
        string visibleGameWord = wordProvider.GetWord().ToUpper();
        int wordLength = visibleGameWord.Length;
        HashSet<char> guessedLetters = new HashSet<char>();
        List<char> hiddenGameWord = new List<char>(new string('_', wordLength));
        var hangmanFailPoints = new HangmanFailPoints();

        Console.WriteLine("Let's start the Hangman Game.\nHere is your word to guess:");
        Console.WriteLine(string.Join(" ", hiddenGameWord) + "\n");

        bool isGameInProgress = true;
        while (isGameInProgress)
        {
            Console.WriteLine("Guessed letters: " + (guessedLetters.Any() ? string.Join(", ", guessedLetters) : "None"));
            char userGuessLetter = AskForLetter();

            if (!guessedLetters.Add(userGuessLetter))
            {
                Console.WriteLine("You've already guessed that letter.\n");
                continue;
            }

            if (visibleGameWord.Contains(userGuessLetter))
            {
                Console.WriteLine("Yes!");
                for (int i = 0; i < wordLength; i++)
                {
                    if (visibleGameWord[i] == userGuessLetter)
                    {
                        hiddenGameWord[i] = userGuessLetter;
                    }
                }
                Console.WriteLine("\n" + string.Join(" ", hiddenGameWord) + "\n");
            }
            else
            {
                Console.WriteLine("Unfortunately not!\n");
                hangmanFailPoints.AddFailPoint();
                Console.WriteLine("\n" + string.Join(" ", hiddenGameWord) + "\n");
            }

            isGameInProgress = !IsGameOver(hiddenGameWord, visibleGameWord, hangmanFailPoints.IsGameLosing());
        }
    }

    private char AskForLetter()
    {
        Console.Write("Guess a letter: ");
        string input = Console.ReadLine()?.ToUpper();

        while (string.IsNullOrEmpty(input) || input.Length != 1 || !char.IsLetter(input[0]))
        {
            Console.Write("Please enter a single letter: ");
            input = Console.ReadLine()?.ToUpper();
        }

        return input[0];
    }

    private bool IsGameOver(List<char> hiddenWord, string visibleWord, bool failPointsStatus)
    {
        bool areAllLettersGuessed = hiddenWord.SequenceEqual(visibleWord.ToCharArray());
        if (areAllLettersGuessed)
        {
            Console.WriteLine("Congratulations! You won the game!");
            return true;
        }
        if (failPointsStatus)
        {
            Console.WriteLine("Sorry, you lost the game!");
            Console.WriteLine($"The word was: {visibleWord}");
            return true;
        }
        return false;
    }
}

public class HangmanFailPoints
{
    private int FailPoint = 0;
    private readonly string[] VisualFailPoint = new string[]
    {
        "",
        "=========",
        """
            |
            |
            |
            |
            |
      =========
      """,
        """
        +---+
        |   |
            |
            |
            |
            |
      =========
      """,
        """
        +---+
        |   |
        O   |
            |
            |
            |
      =========
      """,
        """
        +---+
        |   |
        O   |
       /|\  |
            |
            |
      =========
      """,
        """
        +---+
        |   |
        O   |
       /|\  |
       / \  |
            |
      =========
      """
    };
    private const int MaxFailPoints = 6;

    public void AddFailPoint()
    {
        FailPoint++;
        Console.WriteLine(VisualFailPoint[FailPoint] + "\n");
    }

    public bool IsGameLosing()
    {
        return FailPoint >= MaxFailPoints;
    }
}

public class HangmanWords
{
    private readonly string[] Words = new string[]
    {
        "apple", "banana", "cherry", "grape", "orange", "lemon", "mango", "peach", "pear", "plum",
        "apricot", "kiwi", "lime", "melon", "fig", "date", "berry", "guava", "papaya", "quince",
        "book", "pen", "paper", "desk", "chair", "table", "lamp", "clock", "shelf", "rug",
        "window", "door", "floor", "wall", "ceiling", "mirror", "vase", "candle", "frame", "pillow",
        "car", "bus", "train", "plane", "boat", "bike", "truck", "van", "scooter", "ship",
        "road", "bridge", "tunnel", "river", "lake", "sea", "ocean", "hill", "mountain", "valley",
        "tree", "flower", "grass", "bush", "forest", "meadow", "desert", "beach", "cliff", "cave",
        "dog", "cat", "bird", "fish", "horse", "cow", "pig", "sheep", "goat", "duck",
        "lion", "tiger", "bear", "wolf", "fox", "deer", "rabbit", "mouse", "snake", "eagle",
        "sun", "moon", "star", "cloud", "rain", "snow", "wind", "storm", "fog", "mist",
        "sky", "dawn", "dusk", "noon", "night", "day", "spring", "summer", "autumn", "winter",
        "house", "room", "kitchen", "bedroom", "bathroom", "garden", "yard", "garage", "attic", "basement",
        "food", "bread", "cheese", "meat", "fish", "rice", "pasta", "soup", "salad", "cake",
        "milk", "juice", "water", "coffee", "tea", "sugar", "salt", "pepper", "oil", "butter",
        "shirt", "pants", "dress", "skirt", "jacket", "coat", "hat", "scarf", "gloves", "shoes",
        "ring", "watch", "belt", "tie", "bag", "purse", "wallet", "umbrella", "key", "lock",
        "phone", "computer", "tablet", "screen", "keyboard", "mouse", "camera", "radio", "speaker", "light",
        "game", "ball", "dice", "card", "board", "puzzle", "toy", "doll", "robot", "kite",
        "pen", "brush", "paint", "canvas", "clay", "stone", "wood", "metal", "glass", "plastic",
        "city", "town", "village", "street", "park", "square", "market", "shop", "school", "church",
        "love", "hope", "joy", "peace", "dream", "fear", "trust", "faith", "luck", "wish"
    };

    public string GetWord()
    {
        Random random = new Random();
        int randomIndex = random.Next(Words.Length);
        return Words[randomIndex].ToUpper();
    }
}