using System;
using System.Diagnostics;
using System;
using System.IO;
using Newtonsoft.Json;

public class Die
{
    private readonly Random _randomValue;  // Random number generator
    public int CurrentValue { get; private set; }  // Current value of the die

    public Die() // random number generator
    {
        _randomValue = new Random();
    }

    public int Roll() // Generate new random value for die
    {
        CurrentValue = _randomValue.Next(1, 7); // random int (1, 7)
        return CurrentValue;
    }
}

public class Game
{
    protected Die Die1 { get; } // die object 1
    protected Die Die2 { get; } // die object 2

    public Game()
    {
        Die1 = new Die(); 
        Die2 = new Die();
    }

    public virtual int TotalValue() // returns total value of both dice

    {
        return Die1.Roll() + Die2.Roll();
    }

    public virtual void DiceValues() //current values of both dice
    {
        Console.WriteLine("Dice 1: " + Die1.CurrentValue);
        Console.WriteLine("Dice 2: " + Die2.CurrentValue);
    }
}

public class SevensOut : Game
{
    public SevensOut() : base() { } //calls base that its game 

    public override int TotalValue() //checks for total of 7
    {
        int sum = base.TotalValue();
        if (sum == 7)
        {
            Console.WriteLine("End Of Game! Total is 7."); //end of game message
        }
        return sum;
    }

    public void Play()
    {
        int score = 0; //initial score
        while (true) //loop until game ends
        {
            RollDice();
            base.DiceValues();
            int sum = TotalValue(); //total value
            if (sum == 7) //check if total is 7
            {
                break;
            }
            else if (IsDouble()) //check if dice values are the same
            {
                sum *= 2;
                Console.WriteLine("You got Double! Total is now " + sum);
            }
            score += sum; // add total value to score
            Console.WriteLine("Current score: " + score);
            Console.Write("Press any key to roll! ");
            Console.ReadKey();
         
        }
        Console.WriteLine("End of Game! Press any key to return!");
        Console.ReadKey();
        Console.Clear();
    }

    private void RollDice() //rolls dices 
    {
        Die1.Roll();
        Die2.Roll();
    }

    private bool IsDouble() //check if dices are the same value
    { 
        return Die1.CurrentValue == Die2.CurrentValue;
    }
}

public class ThreeOrMore : Game
{
    protected Die Die3 { get; } //dices objects
    protected Die Die4 { get; }
    protected Die Die5 { get; }

    public ThreeOrMore() : base() 
    {
        Die3 = new Die();
        Die4 = new Die();
        Die5 = new Die();
    }

    public override int TotalValue() //calculates total value of five dice
    {
        int[] diceValues = new int[5] { Die1.CurrentValue, Die2.CurrentValue, Die3.CurrentValue, Die4.CurrentValue, Die5.CurrentValue }; //store dice values
        DiceValues(diceValues);
        int sum = diceValues.Sum();
        if (sum >= 20) //checks if sum is 20 or more
        {
            Console.WriteLine("End Of Game! Total is 20 or more.");
        }
        return sum;
    }

    public void Play()
    {
        int score = 0; //start score is  0
        while (score < 20) // loop until game ends
        {
            RollDice();
            int sum = TotalValue(); //total value
            if (sum >= 20) //check if total is 20 or more

            {
                break;
            }
            else if (ThreeOfkind()) //check if three of a kind
            {
                score += GetScore(sum);
                Console.WriteLine("Current score: " + score);
            }
            else if (TwoOfkind()) //check if two of a kind
            {
                Console.WriteLine("Two of a kind! Rethrow all or the remaining dice? [A] Rethrow All [R] Remain Dice"); //gives user two choices
                string input = Console.ReadLine();
                if (input == "a") //rolls dices again
                {
                    RollDice();
                }
                else
                {
                    int[] remainingDice = GetRemainingDice(); //rolls remain dices
                    RollDice(remainingDice);
                }
            }
            Console.Write("Press any key to roll!");
            Console.ReadKey();
            Console.Clear();
        }
        Console.WriteLine("End Of Game! Press any key to return!");
        Console.ReadKey();
        Console.Clear();
    }

    private void RollDice() //dices roll
    {
        Die1.Roll();
        Die2.Roll();
        Die3.Roll();
        Die4.Roll();
        Die5.Roll();
    }

    private void RollDice(int[] dice)
    {
        for (int i = 0; i < dice.Length; i++) //dice array
        {
            dice[i] = new Die().Roll(); //dice value
        }
        
    }

    private void DiceValues(int[] diceValues)
    {
        Console.WriteLine("Dice values:"); //output dices value
        foreach (int value in diceValues)
        {
            Console.WriteLine(value);
           
        }

    }

    private bool ThreeOfkind() //check 
    {
        return NotOfKind(3);
    }

    private bool TwoOfkind() //check
    {
        return NotOfKind(2);
    }

    private bool NotOfKind(int n)
    {
        int count = 0;
        int currentValue = Die1.CurrentValue;
        for (int i = 0; i < 5; i++)
        {
            if (Die1.CurrentValue == currentValue) //checks if all dices are the same value
            {
                count++;
            }
            else
            {
                currentValue = Die1.CurrentValue; //reset count
                count = 1;
            }
            if (count == n) //check if count is n
            {
                return true;
            }
        }
        return false;
    }

    private int[] GetRemainingDice()
    {
        int count = 0; 
        int currentValue = Die1.CurrentValue; //count with the reamin dices
        for (int i = 0; i < 5; i++)
        {
            if (Die1.CurrentValue != currentValue) 
            {
                count++;
            }
        }
        int[] remainingDice = new int[count]; //array for remain dice values
        int index = 0;
        for (int i = 0; i < 5; i++) //loop dices value 
        {
            if (Die1.CurrentValue != currentValue) //add dice to array if different 
            {
                remainingDice[index] = Die1.CurrentValue;
                index++;
            }
            currentValue = Die1.CurrentValue; //update array 
        }
        return remainingDice;
    }

    private int GetScore(int sum)
    {
        int score = 0;
        if (sum == 3) //checks sum and gives out score 
        {
            score = 3;
        }
        else if (sum == 4)
        {
            score = 6;
        }
        else if (sum == 5)
        {
            score = 12;
        }
        return score;
    }
}

public class Statistics
{
    private int sevensOutPlays; //object for plays
    private int threeOrMorePlays; //object for plays
    private int sevensOutHScore; //obeject for high score
    private int threeOrMoreHScore; //object for high score

    private const string STATISTICS_FILE = "statistics.json"; // file name

    public Statistics() //loads file 
    {
        LoadStatistics();
    }

    public void UpdateStatistics(Game game) //updates files
    {
        if (game is SevensOut) //sevensout update
        {
            sevensOutPlays++;
            if (game.TotalValue() > sevensOutHScore)
            {
                sevensOutHScore = game.TotalValue();
            }
        }
        else if (game is ThreeOrMore) //threeormore update
        {
            threeOrMorePlays++;
            if (game.TotalValue() > threeOrMoreHScore)
            {
                threeOrMoreHScore = game.TotalValue();
            }
        }
        SaveStatistics(); //saves to file
    }

    public void DisplayStatistics() //output files stored information 
    {
        Console.WriteLine("Sevens Out plays: " + sevensOutPlays);
        Console.WriteLine("Sevens Out high score: " + sevensOutHScore);
        Console.WriteLine("Three Or More plays: " + threeOrMorePlays);
        Console.WriteLine("Three Or More high score: " + threeOrMoreHScore);
    }

    private void LoadStatistics() //loads stats from files 
    {
        if (File.Exists(STATISTICS_FILE)) //check if file is there 
        {
            string json = File.ReadAllText(STATISTICS_FILE); //reads files with json string 
            StatisticsData data = JsonConvert.DeserializeObject<StatisticsData>(json); //sets stored data on file to the program object 
            sevensOutPlays = data.SevensOutPlays; //gather info
            threeOrMorePlays = data.ThreeOrMorePlays; //gather info
            sevensOutHScore = data.SevensOutHighScore; //gather info
            threeOrMoreHScore = data.ThreeOrMoreHighScore; //gather info
        }
        else
        {
            sevensOutPlays = 0; //sets 0 file does not exist
            threeOrMorePlays = 0; //sets 0 file does not exist
            sevensOutHScore = 0; //sets 0 file does not exist
            threeOrMoreHScore = 0; //sets 0 file does not exist
        }
    }

    private void SaveStatistics() 
    {
        StatisticsData data = new StatisticsData //new object data for new data
        {
            SevensOutPlays = sevensOutPlays, 
            ThreeOrMorePlays = threeOrMorePlays,
            SevensOutHighScore = sevensOutHScore,
            ThreeOrMoreHighScore = threeOrMoreHScore
        };
        string json = JsonConvert.SerializeObject(data); //serialize into json 
        File.WriteAllText(STATISTICS_FILE, json); //writes onto json file 
    }

    private class StatisticsData //holds data for serialization or desserialization 
    {
        public int SevensOutPlays { get; set; }
        public int ThreeOrMorePlays { get; set; }
        public int SevensOutHighScore { get; set; }
        public int ThreeOrMoreHighScore { get; set; }
    }
}

public class Testing //tests
{
    public void TestSevensOut() 
    {
        SevensOut game = new SevensOut();
        int sum = game.TotalValue();
        Debug.Assert(sum != 7, "Sevens Out game should not stop immediately."); //tests if sum is not 7 
    }

    public void TestThreeOrMore()
    {
        ThreeOrMore game = new ThreeOrMore();
        int sum = game.TotalValue();
        Debug.Assert(sum < 20, "Three Or More game should not stop immediately."); //tests if sum is not over 20
    }
}

class Program // MAIN 
{
    static void Main(string[] args)
    {
        Statistics stats = new Statistics();  //new object for stats
        while (true) //validation loop
        {
            int choice = DisplayMainMenu(); 
            if (choice < 1 || choice > 4) //checks if input is valid
            {
                Console.WriteLine("Invalid choice. Please enter a number between 1 and 4.");
                continue;
            }
            switch (choice) //plays games of choices
            {
                case 1:
                    SevensOut game1 = new SevensOut();
                    game1.Play(); 
                    stats.UpdateStatistics(game1);
                    break;
                case 2:
                    ThreeOrMore game2 = new ThreeOrMore();
                    game2.Play();
                    stats.UpdateStatistics(game2);
                    break;
                case 3:
                    stats.DisplayStatistics();
                    break;
                case 4:
                    Testing testing = new Testing();
                    testing.TestSevensOut();
                    testing.TestThreeOrMore();
                    break;
            }
        }
    }
    private static int DisplayMainMenu()  //ouput main message
    {
        Console.WriteLine("Welcome to the game menu!");
        Console.WriteLine("[1] Play Sevens Out");
        Console.WriteLine("[2] Play Three Or More");
        Console.WriteLine("[3] View statistics");
        Console.WriteLine("[4] Run tests");
        Console.Write("Enter your choice: ");
        while (true) //validation loop
        {
            if (int.TryParse(Console.ReadLine(), out int choice) && choice >= 1 && choice <= 4) //checks if its a number 
            {
                return choice;
            }
            else
            {
                Console.WriteLine("Invalid choice. Please enter a number between 1 and 4.");
                Console.Write("Enter your choice: ");
            }
        }
    }
}