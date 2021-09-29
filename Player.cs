using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace BattleArenaExpansion
{
    class Player : Entity
    {
        private Item[] _items;
        private Item[] _equippedItem;
        private Item _currentItem;
        private int _currentItemIndex;
        private string _job;
        private Item[] _inventory;
        private float _money;

        public float Money
        {
            get { return _money; }
        }

        public Item[] Inventory
        {
            get { return _inventory; }
        }

        public override float DefensePower
        {
            get
            {
                if (_equippedItem[1].Type == ItemType.DEFENSE)
                    return base.DefensePower + CurrentItem.StatBoost;

                return base.DefensePower;
            }
        }
        public override float AttackPower
        {
            get
            {
                if (_equippedItem[0].Type == ItemType.ATTACK)
                    return base.AttackPower + CurrentItem.StatBoost;

                return base.AttackPower;
            }
        }

        public Item CurrentItem
        {
            get { return _currentItem; }
        }

        public string Job
        {
            get
            {
                return _job;
            }
            set
            {
                _job = value;
            }
        }

        public Player()
        {
            _inventory = new Item[16];
            _items = new Item[16];
            _currentItem.Name = "Nothing";
            _currentItemIndex = -1;
            _equippedItem = new Item[2];
        }

        public Player(Item[] items) : base()
        {
            _currentItem.Name = "Nothing";
            _items = items;
            _currentItemIndex = -1;
            _equippedItem = new Item[2];
        }

        public Player(string name, float health, float attackPower, float defensePower, int money, Item[] items, string job) : base(name, health, attackPower, defensePower, money)
        {
            _money = money;
            _items = items;
            _currentItem.Name = "Nothing";
            _job = job;
            _currentItemIndex = -1;
            _equippedItem = new Item[2];
        }
        /// <summary>
        /// Sets the item at thh egiven index to be the current item
        /// </summary>
        /// <param name="index">The item of th eitem in the array </param>
        /// <returns>False if the index is outside the bounds of the array</returns>
        public bool TryEquipItem(int index)
        {
            //If the index is out of bounds..
            if (index >= _items.Length || index < 0)
            {
                //..returns false
                return true;
            }

            _currentItemIndex = index;

            //Set the current item to be the array of the given index
            _currentItem = _items[_currentItemIndex];

            // Sets an attack item to the first equipped item slot
            if (_currentItem.Type == ItemType.ATTACK)
            {
                _equippedItem[0] = _currentItem;
            }

            // Sets a defense item to the second equipped item slot
            else if (_currentItem.Type == ItemType.DEFENSE)
            {
                _equippedItem[1] = _currentItem;
            }

            //if the boost health
            else if (_currentItem.Type == ItemType.Health)
            {
                //add the statboost to the player's health
                _health += _currentItem.StatBoost;

                if (_health > _maxHealth)
                {
                    _health = _maxHealth;
                }
                Console.WriteLine("You recovered " + _currentItem.StatBoost + " health!");
                Console.ReadKey(true);
                Console.Clear();

                Item[] newInventory = new Item[_items.Length - 1];

                int j = 0;

                bool itemRemoved = false;

                //..and remove the item from the inventory
                for (int i = 0; i < _items.Length; i++)
                {
                    if(_items[i].Name != _currentItem.Name || itemRemoved)
                    {
                        newInventory[j] = _items[i];
                        j++;
                    }
                    else
                    {
                        itemRemoved = true;
                    }
                }

                //set the old inventory equal to the new one
                _items = newInventory;
            }


            return true;
        }
        /// <summary>
        /// Set the current item to be nothing
        /// </summary>
        /// <returns>False if there is no item equipped</returns>
        public bool TryRemoveCurrentItem()
        {
            //If the current item is set to nothing...
            if (CurrentItem.Name == "Nothing")
            {
                return false;
            }

            _currentItemIndex = -1;

            //Set item to be nothing
            _currentItem = new Item();
            _currentItem.Name = "Nothing";

            return true;
        }

        /// <returns>The names of all the items in the player inventory</returns>
        public string[] GetItemNames()
        {
            string[] itemNames = new string[_items.Length];

            for (int i = 0; i < _items.Length; i++)
            {
                itemNames[i] = _items[i].Name;
            }

            return itemNames;
        }
        /// <summary>
        /// When the player it adds it to their item list and substacts their money
        /// </summary>
        /// <param name="item"></param>
        public void Buy(Item item)
        {
            //takes the money needed to buy th eitem
            _money -= item.Cost;

            //makes an array equal to the items length
            Item[] GetItem = new Item[_items.Length + 1];

            //copies the item from its array to the items array
            for (int i = 0; i < _items.Length; i++)
            {
                GetItem[i] = _items[i];
            }

            //the item is added to the inventory 
            GetItem[GetItem.Length - 1] = item;

            //makes the item apart of its item list
            _items = GetItem;

        }

        /// <summary>
        /// Adds the enemy money to te players money
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public float GetMoney(Entity entity)
        {
            //gets the enemy money and adds it to the players money
            _money += entity.MoneyAmount;

            //returns the money to the player
            return _money;

        }

        /// <summary>
        /// writes down all things that need to be saved
        /// </summary>
        /// <param name="writer"></param>
        public override void Save(StreamWriter writer)
        {
            writer.WriteLine(_job);
            base.Save(writer);
            writer.WriteLine(_currentItemIndex);
            writer.WriteLine(Money);
            writer.WriteLine(_items.Length);

            for (int i = 0; i < _items.Length; i++)
            {
                writer.WriteLine(_items[i].Name);
            }

            
        }

        public override bool Load(StreamReader reader)
        {
            //If the base loading fuction fails...
            if (!base.Load(reader))
                //...return false
                return false;

            //If the current line can't be converted into an int..
            if (!int.TryParse(reader.ReadLine(), out _currentItemIndex))
                //...return false
                return false;

            if (!float.TryParse(reader.ReadLine(), out _money))         
                return false;
            
            //If the current line can't be converted into an int..
            if (!int.TryParse(reader.ReadLine(), out int _itemsLength))
                //...return false
                return false;

            _items = new Item[_itemsLength];

            for (int i = 0; i < _items.Length; i++)
            {
                _items[i].Name = reader.ReadLine();
            }
            return true;

            //Return whether or not the item was equipped successfully
            TryEquipItem(_currentItemIndex);
            {
                return true;
            }

        }


    }
}
