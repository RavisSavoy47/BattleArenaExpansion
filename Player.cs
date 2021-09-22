using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace BattleArenaExpansion
{
    class Player : Entity
    {
        private Item[] _items;
        private Item _currentItem;
        private int _currentItemIndex;
        private string _job;
        private int _money;
        private Item[] _inventory;

        public Item[] Inventory
        {
            get
            {
                return _inventory;
            }
        }

        public int Money
        {
            get
            {
                return _money;
            }
        }

        public override float DefensePower
        {
            get
            {
                if (_currentItem.Type == ItemType.DEFENSE)
                    return base.DefensePower + CurrentItem.StatBoost;

                return base.DefensePower;
            }
        }
        public override float AttackPower
        {
            get
            {
                if (_currentItem.Type == ItemType.ATTACK)
                    return base.AttackPower + CurrentItem.StatBoost;

                return base.AttackPower;
            }
        }

        public override float Money
        {
            get
            {
                if (_currentItem.Type == ItemType.NONE)
                    return base.Money + CurrentItem.Cost;

                return base.Money;
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
            _items = new Item[0];
            _currentItem.Name = "Nothing";
            _currentItemIndex = -1;
        }

        public Player(Item[] items) : base()
        {
            _currentItem.Name = "Nothing";
            _items = items;
            _currentItemIndex = -1;
        }

        public Player(string name, float health, float attackPower, float defensePower, float money, Item[] items, string job) : base(name, health, attackPower, defensePower, money)
        {
            _items = items;
            _currentItem.Name = "Nothing";
            _job = job;
            _currentItemIndex = -1;
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
                return false;
            }

            _currentItemIndex = index;

            //Set the current item to be the array of the given index
            _currentItem = _items[_currentItemIndex];

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

        public override void Save(StreamWriter writer)
        {
            writer.WriteLine(_job);
            base.Save(writer);
            writer.WriteLine(_currentItemIndex);
            writer.WriteLine(_money);
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

            //Return whether or not the item was equipped successfully
            TryEquipItem(_currentItemIndex);
            {
                return true;
            }

        }

        public void CollectMoney(Entity )
        {
            _player.Money += _currentEnemy.Money;
        }
    }
}
