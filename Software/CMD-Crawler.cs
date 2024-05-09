using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;

namespace GameDev
{
    /**
     * The main class of the Dungeon Game Application
     * 
     * You may add to your project other classes which are referenced.
     * Complete the templated methods and fill in your code where it says "Your code here".
     * Do not rename methods or variables which already eDist or change the method parameters.
     * You can do some checks if your project still aligns with the spec by running the tests in UnitTest1
     * 
     * For Questions do contact us!
     */
    public class Game
    {
        /**
         * use the following to store and control the movement 
         */
        public enum PlayerActions { NOTHING, NORTH, EAST, SOUTH, WEST, PICKUP, ATTACK, DROP, QUIT };
        private PlayerActions action = PlayerActions.NOTHING;
        public enum GameState { UNKOWN, STOP, RUN, START, INIT };
        private GameState status = GameState.INIT;

        // maps 
        private char[][] originalMap = new char[0][];
        private char[][] workingMap = new char[0][];



        /**
        * tracks if the game is running
        */
        private bool advanced = false;

        private string currentMap;


        /**
         * Reads user input from the Console
         * 
         * Please use and implement this method to read the user input.
         * 
         * Return the input as string to be further processed
         * 
         */
        private string ReadUserInput()
        {
            string inputRead = string.Empty;
            //modify
            if (status == GameState.RUN && ReplayOption == false)
            {
                ConsoleKeyInfo keyInput = Console.ReadKey();
                inputRead = keyInput.Key.ToString();
                return inputRead;
            }
            inputRead = Console.ReadLine();
            return inputRead;
        }

        private int counter = -1;

        // private variables to keep track of coins
        private int coins = 0;
        private int MonsterCoins = 0;

        // private variables to control states
        private bool StepOntoCoin = false;
        private bool StepOntoBag = false;

        private bool AttackMode = false;
        private bool PlayerAttack = false;
        private bool MonsterAttack = false;

        private bool ReplayOption = false;

        // private variables related to player
        private int PlayerHealth = 2;
        private int PlayerDamage = 1;

        // private variables related to Monsters
        private int MonsterStartingHealth = 2; // shared health of monsters
        private int MonsterHealth = 2; // monster health of current monster player is attacking
        private bool MonsterDead = false;
        private int MonsterDamage = 0;
        private int[] currentMonster = new int[2]; // position of current monster player is attacking

        /// <summary>
        /// Returns the number of steps a player made on the current map. The counter only counts steps, not actions.
        /// </summary>
        public int GetStepCounter()
        {
            //modify
            return counter;
 
        }

        /**
         * Processed the user input string
         * 
         * takes apart the user input and does control the information flow
         *  * initializes the map ( you must call InitializeMap)
         *  * starts the game when user types in Play
         *  * sets the correct playeraction which you will use in the Update
         *  
         *  DO NOT read any information from command line in here but only act upon what the method receives.
         */
        public void ProcessUserInput(string input)
        {
            //modify
            if (input == "load Simple.map" || input == "load Simple.Map")
            {
                status = GameState.START;
                bool InitializeMap = LoadMapFromFile("Simple.map");
                currentMap = "Simple.map";
            }
            if (input == "load Simple2.map" || input == "load Simple2.Map")
            {
                status = GameState.START;
                bool InitializeMap = LoadMapFromFile("Simple2.map");
                currentMap = "Simple2.map";
            }
            if (input == "load Advanced.map")
            {
                status = GameState.START;
                bool InitializeMap = LoadMapFromFile("Advanced.map");
                currentMap = "Advanced.map";
            }
            if(status == GameState.START && input == "advanced")
            {
                advanced = true;
            }
            if (status == GameState.START && input == "start" || status == GameState.START && input == "Play")
            {
                counter++;
                status = GameState.RUN;
            }
            if (status == GameState.RUN)
            {
                if (input == "w" || input == "W")
                {
                    counter++;
                    action = PlayerActions.NORTH;
                }
                if (input == "a" || input == "A")
                {
                    counter++;
                    action = PlayerActions.WEST;
                }
                if (input == "s" || input == "S")
                {
                    counter++;
                    action = PlayerActions.SOUTH;
                }
                if (input == "d" || input == "D")
                {
                    counter++;
                    action = PlayerActions.EAST;
                }
                if (input == "z" || input == "Z")
                {
                    action = PlayerActions.PICKUP;
                }
                if (input == "q" || input == "Q")
                {
                    action = PlayerActions.ATTACK;
                }
                if (input == "start")
                {
                    counter = 0;
                    action = PlayerActions.NOTHING;
                }
                if (input == "replay" && ReplayOption == true)
                {
                    ResetGame();
                    status = GameState.START;
                    bool InitializeMap = LoadMapFromFile(currentMap);
                }
                if (input == "quit" && ReplayOption == true)
                {
                    status = GameState.STOP;
                }
            }
        }

        // Initializes all the private variables (except currentMap to replay the same map) to their starting values
        public void ResetGame()
        {
            counter = -1;
            coins = 0;
            MonsterCoins = 0;

            StepOntoCoin = false;
            StepOntoBag = false;

            AttackMode = false;
            PlayerAttack = false;
            MonsterAttack = false;

            ReplayOption = false;

            PlayerHealth = 2;
            PlayerDamage = 1;

            MonsterStartingHealth = 2; 
            MonsterHealth = 2;
            MonsterDead = false;
            MonsterDamage = 0;
            currentMonster = new int[2];

            action = PlayerActions.NOTHING;
            status = GameState.INIT;

            originalMap = new char[0][];
            workingMap = new char[0][];

    }

        /**
         * The Main Game Loop. 
         * It updates the game state.
         * 
         * This is the method where you implement your game logic and alter the state of the map/game
         * use playeraction to determine how the character should move/act
         * the input should tell the loop if the game is active and the state should advance
         * 
         * Returns true if the game could be updated and is ongoing
         */
        public bool Update(GameState status)
        {
            //modify
            int[] position = GetPlayerPosition();
            int x = position[1];
            int y = position[0];

            if (action != PlayerActions.NOTHING)
            {
                workingMap[y][x] = '@';
                if (action == PlayerActions.PICKUP && StepOntoCoin == true)
                {
                    coins++;
                    PlayerHealth++;
                    PlayerDamage++;
                    StepOntoCoin = false;
                }
                if (action == PlayerActions.PICKUP && StepOntoBag == true)
                {
                    coins += MonsterCoins;
                    PlayerHealth += MonsterCoins;
                    PlayerDamage += MonsterCoins;
                    MonsterCoins = 0;
                    StepOntoBag = false;
                }
                if (AttackMode == true && action != PlayerActions.ATTACK)
                {
                    AttackMode = false;
                    PlayerAttack = false;
                    MonsterAttack = false;
                }
                if (action == PlayerActions.NORTH)
                {
                    Movement(y - 1, x);
                }
                if (action == PlayerActions.WEST)
                {
                    Movement(y, x - 1);
                }
                if (action == PlayerActions.SOUTH)
                {
                    Movement(y + 1, x);
                }
                if (action == PlayerActions.EAST)
                {
                    Movement(y, x + 1);
                }
                if (action == PlayerActions.ATTACK && AttackMode == true && advanced == true)
                {
                    PlayerAttack = true;
                    MonsterHealth -= PlayerDamage;
                    if (MonsterHealth <= 0 )
                    {
                        MonsterDead = true;
                    }
                    else
                    {
                        MonsterAttack = true;
                        Random random = new Random();
                        MonsterDamage = random.Next(0, 4);
                        PlayerHealth -= MonsterDamage;

                        if (PlayerHealth <= 0)
                        {
                            ReplayOption = true;
                        }
                    }
                }
            }
            if (advanced == true)
            {
                if (AttackMode == false)
                {
                    for (int my = 0; my < GetCurrentMapState().Length; my++)
                    {
                        for (int mx = 0; mx < GetCurrentMapState()[0].Length; mx++)
                        {
                            if (workingMap[my][mx] == 'M')
                            {
                                MonsterBehaviour(my, mx);
                            }
                        }
                    }
                }
            }
            return true;
        }

        // This controls the movement of the player and how it interacts with different entities on the map
        public bool Movement(int directionY, int directionX)
        {
            int[] position = GetPlayerPosition();
            int x = position[1];
            int y = position[0];
            if (workingMap[directionY][directionX] != '#' && workingMap[directionY][directionX] != 'M')
            {
                if (workingMap[directionY][directionX] == 'D' && advanced == false)
                {
                    this.status = GameState.STOP;
                    workingMap[y][x] = '.';
                    workingMap[directionY][directionX] = '@';
                    return false;
                }
                if (workingMap[directionY][directionX] == 'D' && advanced == true)
                {
                    workingMap[y][x] = '.';
                    workingMap[directionY][directionX] = '@';
                    ReplayOption = true;
                    return false;
                }
                else if (StepOntoCoin == true)
                {
                    workingMap[y][x] = 'C';
                    workingMap[directionY][directionX] = '@';
                    StepOntoCoin = false;
                }
                else if (workingMap[directionY][directionX] == 'C')
                {
                    StepOntoCoin = true;
                    workingMap[y][x] = '.';
                    workingMap[directionY][directionX] = '@';
                }
                else if (StepOntoBag == true)
                {
                    workingMap[y][x] = 'B';
                    workingMap[directionY][directionX] = '@';
                    StepOntoBag = false;
                }
                else if (workingMap[directionY][directionX] == 'B')
                {
                    StepOntoBag = true;
                    workingMap[y][x] = '.';
                    workingMap[directionY][directionX] = '@';
                }
                else
                {
                    workingMap[y][x] = '.';
                    workingMap[directionY][directionX] = '@';
                }
            }
            return true;
        }
        
        // This controls the movement of the monster and how it interacts with different entities on the map
        public void MonsterBehaviour(int my, int mx)
        {
            Random random = new Random();
            workingMap[my][mx] = '.';
            int i = 0;
            // while loop randomises monster movement per cycle until an if statement is satisfied.
            while (i == 0)
            {
                int randDirectionY = random.Next(-1, 2);
                int randDirectionX = random.Next(-1, 2);
                // checks the north, west, east and south position of the M for the player. If true then position of monster is noted
                // and attack mode begins.
                if (workingMap[my - 1][mx] == '@' || workingMap[my][mx - 1] == '@' || workingMap[my + 1][mx] == '@' || workingMap[my][mx + 1] == '@')
                {
                    currentMonster[0] = my;
                    currentMonster[1] = mx;
                    workingMap[my][mx] = 'M';
                    AttackMode = true;
                    i++;
                }
                else if (workingMap[my + randDirectionY][mx + randDirectionX] == '.')
                {
                    workingMap[my + randDirectionY][mx + randDirectionX] = 'M';
                    i++;
                }
                else if (workingMap[my + randDirectionY][mx + randDirectionX] == 'C')
                {
                    workingMap[my][mx] = '.';
                    workingMap[my + randDirectionY][mx + randDirectionX] = 'M';
                    MonsterHealth++;
                    MonsterStartingHealth++;
                    MonsterCoins++;
                    i++;
                }
            }
            if (MonsterDead == true && workingMap[currentMonster[0]][currentMonster[1]] != '@')
            {
                workingMap[currentMonster[0]][currentMonster[1]] = 'B';
                MonsterDead = false;
            }
        }

        /**
         * The Main Visual Output element. 
         * It draws the new map after the player did something onto the screen.
         * 
         * This is the method where you implement your the code to draw the map ontop the screen
         * and show the move to the user. 
         * 
         * The method returns true if the game is running and it can draw something, false otherwise.
        */
        public bool PrintMapToConsole()
        {
            //modify
            if (workingMap.Length == 0)
            {
                return false;
            }
            else
            {
                for (int i = 0; i < workingMap.Length; i++)
                {
                    for (int j = 0; j < workingMap[i].Length; j++)
                    {
                        Console.Write(workingMap[i][j]);
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
                return true;
            }

        }
        /**
         * Additional Visual Output element. 
         * It draws the flavour texts and additional information after the map has been printed.
         * 
         * This is the method does not need to be used unless you want to output somethign else after the map onto the screen.
         * 
         */
        public bool PrintExtraInfo()
        {
            //modify
            if (status == GameState.START && advanced == false)
            {
                Console.WriteLine("Type 'advanced' to enter advanced mode, or type 'start' or 'Play' to begin basic mode");
            }
            if (status == GameState.START && advanced == true)
            {
                Console.WriteLine("Type 'start' or 'Play' to begin");
            }
            if (ReplayOption == true && PlayerHealth > 0)
            {
                Console.WriteLine("Congratulations! You've completed the CMD-Crawler map. Type 'replay' to go again or 'quit' to end.");
            }
            if (StepOntoCoin == true && advanced == true)
            {
                Console.WriteLine("You have stepped onto a coin, it'll increase your health and damage by +1. Press Z to pick up");
            }
            if (StepOntoCoin == true && advanced == false)
            {
                Console.WriteLine("You have stepped onto a coin. Press Z to pick up");
            }
            if (StepOntoBag == true)
            {
                Console.WriteLine("You have stepped onto a bag that may contain coins. Press Z to pick up");
            }
            if (PlayerAttack == true)
            {
                Console.WriteLine("You dealt " + PlayerDamage + " damage to the monster. It now has " + MonsterHealth + " health");
                if (MonsterDead == true)
                {
                    Console.WriteLine("Congratulations! you have slain the monster. It has dropped a bag that may contains " + MonsterCoins + " coins");
                    AttackMode = false;
                    MonsterHealth = MonsterStartingHealth;
                }
                PlayerAttack = false;
            }
            if(PlayerAttack == false && MonsterAttack == true && AttackMode == true)
            {
                Console.WriteLine("The monster attacked. You've been dealt " + MonsterDamage + " damage");
                MonsterAttack = false;
                PlayerAttack = true;
            }
            if (AttackMode == true && MonsterAttack == false && PlayerAttack == false)
            {
                Console.WriteLine("You've encountered a monster! It has " + MonsterHealth + " health. Press Q to attack or move any direction to flee");
            }
            if (advanced == true)
            {
                Console.WriteLine("Health: " + PlayerHealth);
                Console.WriteLine("Attack: " + PlayerDamage);
            }
            Console.WriteLine("Coins: " + coins);
            Console.WriteLine("Steps: " + counter);
            if (ReplayOption == true && PlayerHealth <= 0)
            {
                Console.WriteLine("You died! Better luck next time");
                Console.WriteLine("Type 'replay' to go again or 'quit' to end.");
            }
            return false;
        }

        /**
        * Map and GameState get initialized
        * mapName references a file name 
        * Do not use abosolute paths but use the files which are relative to the eDecutable.
        * 
        * Create a private object variable for storing the map in Game and using it in the game.
        */
        public bool LoadMapFromFile(String mapName)
        {
            //modify
            if(File.Exists("maps/" + mapName) != true)
            {
                return false;
            }
            string[] mapXcoord = File.ReadAllLines("maps" + Path.DirectorySeparatorChar + mapName);
            originalMap = new char[10][];
            for (int i = 0; i < originalMap.Length; i++)
            {
                char[] charArray = mapXcoord[i].ToCharArray();
                originalMap[i] = charArray;
            }
            workingMap = new char[10][];
            for (int i = 0; i < originalMap.Length; i++)
            {
                char[] charArray = mapXcoord[i].ToCharArray();
                workingMap[i] = charArray;
            }
            return true;
        }

        /**
         * Returns a representation of the currently loaded map
         * before any move was made.
         * This map should not change when the player moves
         */
        public char[][] GetOriginalMap()
        {
            //modify
            return originalMap;
        }

        /*
         * Returns the current map state and contains the player's move
         * without altering it 
         */
        public char[][] GetCurrentMapState()
        {
            //modify
            return workingMap;
        }

        /**
         * Returns the current position of the player on the map
         * 
         * The first value is the y coordinate and the second is the x coordinate on the map
         */
        public int[] GetPlayerPosition()
        {
            //modify
            int[] position = new int[2];
            for (int i = 0; i < GetCurrentMapState().Length; i++)
            {
                for (int j = 0; j < GetCurrentMapState()[0].Length; j++)
                {
                    if (workingMap[i][j] == 'P' || workingMap[i][j] == '@')
                    {
                        position[1] = j;
                        position[0] = i;
                    }
                }
            }
            return position;
        }

        /**
        * Returns the next player action
        * 
        * This method does not alter any internal state
        */
        public int GetPlayerAction()
        {
            //modify
            return (int)action;
        }

        public GameState GameIsRunning()
        {
            //modify
            return status;
        }


        /**
         * Main method and Dntry point to the program
         * ####
         * Do not change! 
        */
        static void Main(string[] args)
        {
            Game crawler = new Game();

            string input = string.Empty;
            Console.WriteLine("Welcome to the Commandline Dungeon!" + Environment.NewLine +
                "May your Quest be filled with riches!" + Environment.NewLine);

            // Loops through the input and determines when the game should quit
            while (crawler.GameIsRunning() != GameState.STOP && crawler.GameIsRunning() != GameState.UNKOWN)
            {
                Console.Write("Your Command: ");
                input = crawler.ReadUserInput();
                Console.WriteLine(Environment.NewLine);
                crawler.ProcessUserInput(input);
                crawler.Update(crawler.GameIsRunning());
                crawler.PrintMapToConsole();
                crawler.PrintExtraInfo();
            }

            Console.WriteLine("See you again" + Environment.NewLine +
                "In the CMD Dungeon! ");
        }
    }
}