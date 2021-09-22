using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace BattleArenaExpansion
{
    public enum ItemType
    {
        DEFENSE,
        ATTACK,
        NONE
    }

    public enum Scene
    {
        STARTMENU,
        NAMECREATION,
        CHARACTERSELECTION,
        BATTLE,
        RESTARTMENU
    }

    public struct Item
    {
        public string Name;
        public float StatBoost;
        public ItemType Type;
        public int Cost;
    }

    class Game
    {
        private Shop _shop;
        private bool _gameOver;
        private Scene _currentScene;
        private Player _player;
        private Entity[] _enemies;
        private int _currentEnemyIndex;
        private Entity _currentEnemy;
        private string _playerName;
        private Item[] _wizardItems;
        private Item[] _knightItems;
        private Item[] _shopItems;
        /// <summary>
        /// Function that starts the main game loop
        /// </summary>
        public void Run()
        {
            Start();

            while (!_gameOver)
            {
                Update();
            }

            End();

        }

        public void InitializeItems()
        {
            //Shop Items
            


            //Wizard Items
            Item bigWand = new Item { Name = "Big Wand", StatBoost = 30, Type = ItemType.ATTACK, Cost = 0};
            Item bigSheild = new Item { Name = "Big Sheild", StatBoost = 35, Type = ItemType.DEFENSE, Cost = 0};

            //Knight Items
            Item stick = new Item { Name = "Kitchen Gun", StatBoost = 50, Type = ItemType.ATTACK, Cost = 0};
            Item shoes = new Item { Name = "The Drip", StatBoost = 1000, Type = ItemType.DEFENSE, Cost = 0};

            //Initialize arrays
            _wizardItems = new Item[] { bigWand, bigSheild };
            _knightItems = new Item[] { stick, shoes };
        }

        public void InitializeEnimes()
        {
            Entity SmallFrog = new Entity("Nice Frog", 35, 10, 5, 5);

            Entity StackedFrog = new Entity("Delux Frog", 55, 35, 13, 10);

            Entity MegaFrog = new Entity("Captain Frog", 100, 70, 13, 35);

            Entity KingFrog = new Entity("The Guardians of Frogs", 150, 55, 25, 100);

            //enemies array
            _enemies = new Entity[] { SmallFrog, StackedFrog, MegaFrog, KingFrog };

            _currentEnemy = _enemies[_currentEnemyIndex];
        }

        /// <summary>
        /// Function used to initialize any starting values by default
        /// </summary>
        public void Start()
        {
            _gameOver = false;
            _currentScene = 0;
            InitializeEnimes();
            InitializeItems();
        }

        /// <summary>
        /// This function is called every time the game loops.
        /// </summary>
        public void Update()
        {
            DisplayCurrentScene();
        }

        /// <summary>
        /// This function is called before the applications closes
        /// </summary>
        public void End()
        {
            //Gives the player the end game text then closes the application
            Console.WriteLine("Fairwell Nerd!");
            Console.ReadKey(true);
        }

        public void Save()
        {
            bool loadSuccessfull = true;

            //Create a new stream writer
            StreamWriter writer = new StreamWriter("SaveData.txt");
            //...return false
            loadSuccessfull = false;
            //Save current enemy index
            writer.WriteLine(_currentEnemyIndex);

            //Save player and enemy stats
            _player.Save(writer);
            _currentEnemy.Save(writer);

            //Close writer when done saving
            writer.Close();
        }

        public bool Load()
        {
            bool loadSuccessfull = true;

            //If the file exist..
            if (!File.Exists("SaveData.txt"))
                //..return false
                loadSuccessfull = false;

            //Create a new reader to read from the text file
            StreamReader reader = new StreamReader("SaveData.txt");

            //If the first line can't be converted into a integer..
            if (!int.TryParse(reader.ReadLine(), out _currentEnemyIndex))
                //...return false
                loadSuccessfull = false;

            //Load player job
            string job = reader.ReadLine();

            //Assign items based on player job
            if (job == "Wizard")
                _player = new Player(_wizardItems);
            else if (job == "knight")
                _player = new Player(_knightItems);
            else
                loadSuccessfull = false;

            _player.Job = job;

            if (!_player.Load(reader))
                loadSuccessfull = false;

            //Create a new instance and try to load the enemy
            _currentEnemy = new Entity();
            if (!_currentEnemy.Load(reader))
                loadSuccessfull = false;

            //Update the array to match the current enemy stats
            _enemies[_currentEnemyIndex] = _currentEnemy;

            //Close the reader once loading is finished
            reader.Close();
            return true;
        }

        /// <summary>
        /// Gets an input from the player based on some given decision
        /// </summary>
        /// <param name="description">The context for the input</param>
        /// <param name="option1">The first option the player can choose</param>
        /// <param name="option2">The second option the player can choose</param>
        /// <returns></returns>
        int GetInput(string description, params string[] options)
        {
            string input = "";
            int inputReceived = -1;

            while (inputReceived == -1)
            {
                //Print options
                Console.WriteLine(description);
                for (int i = 0; i < options.Length; i++)
                {
                    Console.WriteLine((i + 1) + ". " + " " + options[i]);
                }
                Console.Write("> ");

                //Get input from player
                input = Console.ReadLine();

                //If the player typed an int...
                if (int.TryParse(input, out inputReceived))
                {
                    //...decrement the input and check if it's within the bounds of the array
                    inputReceived--;
                    if (inputReceived < 0 || inputReceived >= options.Length)
                    {
                        //Set input received to be the default value
                        inputReceived = -1;
                        //Display error message
                        Console.WriteLine("Invalid Input");
                        Console.ReadKey(true);
                    }
                    Console.Clear();
                }
                //If the player didn't type an int
                else
                {
                    //set inpurt recieved to be default value
                    inputReceived = -1;
                    Console.WriteLine("Invalid Input Bro!");
                    Console.ReadKey(true);
                    Console.Clear();
                }


            }
            return inputReceived;
        }

        /// <summary>
        /// Calls the appropriate function(s) based on the current scene index
        /// </summary>
        void DisplayCurrentScene()
        {
            //Goes through each Scene
            switch (_currentScene)
            {
                case Scene.STARTMENU:
                    DisplayStartMenu();
                    break;
                case Scene.NAMECREATION:
                    GetPlayerName();
                    break;
                case Scene.CHARACTERSELECTION:
                    CharacterSelection();
                    break;
                case Scene.BATTLE:
                    Battle();
                    CheckBattleResults();
                    break;
                case Scene.RESTARTMENU:
                    DisplayRestartMenu();
                    break;

                    Console.WriteLine("Invaild scene index");
                    break;
            }
        }

        /// <summary>
        /// Displays the menu that allows the player to start or quit the game
        /// </summary>
        void DisplayRestartMenu()
        {
            int Choice = GetInput("Do you want to Restart your Adventure?", "Yes", "No");

            if (Choice == 0)
            {
                _currentScene = 0;
                _currentEnemyIndex = 0;
                _currentEnemy = _enemies[_currentEnemyIndex];
            }

            else if (Choice == 1)
            {
                _gameOver = true;
            }
        }

        public void DisplayStartMenu()
        {
            int choice = GetInput("Welcome to Battle Arena!", "Start New Game", "Load Game");

            if (choice == 0)
            {
                _currentScene = Scene.NAMECREATION;
            }
            else if (choice == 1)
            {
                if (Load())
                {
                    Console.WriteLine("Load Successful");
                    Console.ReadKey(true);
                    Console.Clear();
                    _currentScene = Scene.BATTLE;
                }
                else
                {
                    Console.WriteLine("Load Failed.");
                    Console.ReadKey(true);
                    Console.Clear();
                }
            }
        }

        /// <summary>
        /// Displays text asking for the players name. Doesn't transition to the next section
        /// until the player decides to keep the name.
        /// </summary>
        void GetPlayerName()
        {
            Console.WriteLine("Welcome to Hell! What's your name?");
            Console.Write(">");
            _playerName = Console.ReadLine();

            Console.Clear();
            //Checks if the player wants to keep their name
            int Choice = GetInput("You've entered " + _playerName + ". Are you sure you want to keep this name?", "Yes", "No");

            if (Choice == 0)
            {
                _currentScene++;
            }
        }

        /// <summary>
        /// Gets the players choice of character. Updates player stats based on
        /// the character chosen.
        /// </summary>
        public void CharacterSelection()
        {
            int choice = GetInput("Hope you survive" + _playerName + ". Please pick your character.", "Wizard", "Knight");

            if (choice == 0)
            {
                _player = new Player(_playerName, 200, 45, 35, 0, _wizardItems, "Wizard");
                _currentScene++;
            }

            else if (choice == 1)
            {
                _player = new Player(_playerName, 100, 75, 40, 0, _knightItems, "Knight");
                _currentScene++;
            }
        }

        /// <summary>
        /// Prints a characters stats to the console
        /// </summary>
        /// <param name="">The character that will have its stats shown</param>
        void DisplayStats(Entity character)
        {
            Console.WriteLine("Name: " + character.Name);
            Console.WriteLine("Health: " + character.Health);
            Console.WriteLine("Attack: " + character.AttackPower);
            Console.WriteLine("Defense: " + character.DefensePower);
            Console.WriteLine("Cash: " + character.Money);
            Console.WriteLine();
        }

        public void DisplayEquipItemMenu()
        {
            //Get items index
            int choice = GetInput("Select an item to equip.", _player.GetItemNames());

            //Equip item at given index
            if (!_player.TryEquipItem(choice))
                Console.WriteLine("Sorry bro I failed u....");
            //Print Feedback
            Console.WriteLine(" You equipped " + _player.CurrentItem.Name + "!!");
        }


        /// <summary>
        /// Simulates one turn in the current monster fight
        /// </summary>
        public void Battle()
        {
            float damageDealth = 0;

            //Print PLayer stats
            DisplayStats(_player);
            //Print Enemy stats
            DisplayStats(_currentEnemy);

            int choice = GetInput("A " + _currentEnemy.Name + " approaches you!", "Attack", "Equip Item", "Remove Current Item", "Save");

            if (choice == 0)
            {
                //player attaks enemy 
                damageDealth = _player.Attack(_currentEnemy);
                Console.WriteLine("You dealt " + damageDealth + " damage!");
            }
            else if (choice == 1)
            {
                DisplayEquipItemMenu();
                Console.ReadKey(true);
                Console.Clear();
                return;
            }
            else if (choice == 2)
            {
                if (!_player.TryRemoveCurrentItem())
                    Console.WriteLine("You have no items equipped.");

                else
                    Console.WriteLine("You placed the item in your bag.");

                Console.ReadKey(true);
                Console.Clear();
                return;
            }
            else if (choice == 3)
            {
                Save();
                Console.WriteLine("Saved Game");
                Console.ReadKey(true);
                Console.Clear();
                return;
            }

            damageDealth = _currentEnemy.Attack(_player);
            Console.WriteLine("The " + _currentEnemy.Name + " dealt " + damageDealth + " damage!");

            Console.ReadKey(true);
            Console.Clear();
        }

        /// <summary>
        /// Checks to see if either the player or the enemy has won the current battle.
        /// Updates the game based on who won the battle..
        /// </summary>
        void CheckBattleResults()
        {
            Console.Clear();
            //If the player dies they are asked if they want to keep playing or not
            if (_player.Health <= 0)
            {
                Console.WriteLine("You died! Get Gud!");
                Console.ReadKey(true);
                Console.Clear();
                _currentScene = Scene.RESTARTMENU;
            }
            //check if the enemy dies
            else if (_currentEnemy.Health <= 0)
            {
                Console.WriteLine("You Defeated " + _currentEnemy.Name);
                _player.money += _currentEnemy.Money;
                Console.ReadKey();
                _currentEnemyIndex++;

                //If all enemies have died continue to next scene
                if (_currentEnemyIndex >= _enemies.Length)
                {
                    _currentScene = Scene.RESTARTMENU;
                    return;
                }
                _currentEnemy = _enemies[_currentEnemyIndex];
            }

        }

        
    }
}
