using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace motorola
{
    class Hangman
    {
        private static List<string> capitals = new();
        private static List<string> countires = new();

        static void Main(string[] args)
        {
            var lines =
                File.ReadAllLines(
                    "../../../countries_and_capitals.txt");
            Console.WriteLine(lines);

            foreach (var s in lines)
            {
                var splitedLine = s.Split(" | ");
                countires.Add(splitedLine[0]);
                capitals.Add(splitedLine[1].ToLower());
            }

            while (true)
            {
                var randomlyChosenIndex = GetRandomCapitalIndex();
                var chosenCapital = capitals[randomlyChosenIndex];
                var chosenCountry = countires[randomlyChosenIndex];

                var life = 5;
                var secret = CreateSecretWord(chosenCapital);
                var lettersNotInWord = new List<char>();
                var guessingStartingTime = DateTime.Now;


                while (life > 0)
                {
                    Console.WriteLine("You have " + life + " lives");
                    if (life == 1)
                    {
                        Console.WriteLine("Hint: The capital of " + chosenCountry);
                    }

                    if (lettersNotInWord.Count > 0)
                    {
                        PrintNotInWord(lettersNotInWord);
                    }

                    Console.WriteLine(secret);
                    Console.WriteLine("Do you want to guess word or a letter? Type 'word' or 'letter'.");
                    var userDecision = Console.ReadLine();
                    if (userDecision == "letter")
                    {
                        Console.WriteLine("Choose the letter!");
                        var chosenLatter = Console.ReadKey().KeyChar;

                        if (chosenCapital.Contains(chosenLatter.ToString().ToLower()))
                        {
                            secret = FillSecretWord(chosenCapital, secret, chosenLatter);
                            Console.WriteLine("\nYou guessed the letter!");
                        }
                        else
                        {
                            life -= 1;
                            Console.WriteLine("\nWrong answer!");
                            lettersNotInWord.Add(chosenLatter);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Choose the word!");
                        var chosenWord = Console.ReadLine();
                        if (chosenWord.ToLower() == chosenCapital)
                        {
                            Console.WriteLine("Congratulations! You correctly guessed the word!");
                            break;
                        }
                        else
                        {
                            life -= 2;
                            Console.WriteLine(chosenWord + " - it's not a correct answer.");
                        }
                    }
                }

                Console.WriteLine("What's your name?");
                var userName = Console.ReadLine();
                var guessingEndingTime = DateTime.Now;
                var guessingTimeInSecond = Convert.ToInt32((guessingEndingTime - guessingStartingTime).TotalSeconds);
                if (life < 0)
                {
                    life = 0;
                }
                WriteScoreToFile(userName, chosenCapital, guessingEndingTime, guessingTimeInSecond, 5 - life);
                if (life == 0)
                {
                    Console.WriteLine("Game over!");
                }

                Console.WriteLine("Try again? Type 'yes' or 'no'.");
                var shouldTryAgain = Console.ReadLine();
                if (shouldTryAgain.ToLower() == "no")
                {
                    break;
                }
            }
        }

        private static void WriteScoreToFile(string userName, string guessedWord,
            DateTime date, int guessingTimeInSeconds, int guessingTries)
        {
            using (StreamWriter sw = File.AppendText("../../../highscore.txt"))
            {
                sw.WriteLine(
                    userName + "|" + date + "|" + guessingTimeInSeconds + "|" + guessingTries + "|" + guessedWord);
            }
          
        }

        private static string FillSecretWord(string word, string currentSecret, char letter)
        {
            var newSecret = new StringBuilder(currentSecret);
            for (int i = 0; i < word.Length; i++)
            {
                if (letter == word[i])
                {
                    newSecret[i] = letter;
                }
            }

            return newSecret.ToString();
        }

        private static string CreateSecretWord(string word)
        {
            var secret = "";
            foreach (var c in word)
            {
                if (c == ' ')
                {
                    secret += ' ';
                }
                else
                {
                    secret += "_";
                }
            }

            return secret;
        }

        private static int GetRandomCapitalIndex()
        {
            var random = new Random();
            var index = random.Next(capitals.Count);
            return index;
        }

        private static void PrintNotInWord(List<char> list)
        {
            var chars = "";
            foreach (var c in list)
            {
                chars += c + ", ";
            }

            Console.WriteLine("Characters not in word = " + chars);
        }
    }
}