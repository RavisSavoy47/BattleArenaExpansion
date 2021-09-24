using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace BattleArenaExpansion
{
    class Shop
    {
        private int _money;
        private Item[] _inventory;

        public Shop(Item[] items)
        {
            _money = 0;

            _inventory = items;
        }

        public bool Sell(Player player, int itemIndex)
        {
            Item itemYouWant = _inventory[itemIndex];

            if (player.MoneyAmount >= itemYouWant.Cost)
            {
                return true;
            }
            else
            {
                Console.Clear();
                Console.WriteLine("You can't afford this item.");
                Console.ReadKey(true);
            }
            return false;
        }

        public string[] GetItemNames()
        {
            string[] itemNames = new string[_inventory.Length];

            for (int i = 0; i < _inventory.Length; i++)
            {
                itemNames[i] = _inventory[i].Name;
            }

            return itemNames;
        }


    }
}
