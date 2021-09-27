using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace BattleArenaExpansion
{
    class Shop
    {
        private Item[] _inventory;

        public Shop(Item[] items)
        {
            _inventory = items;
        }

        /// <summary>
        /// Checks if th eplaye rhas enough money to buy the item
        /// </summary>
        /// <param name="player"></param>
        /// <param name="itemIndex"></param>
        /// <returns></returns>
        public bool Sell(Player player, int itemIndex)
        {
            //checks the index of the selected item
            Item itemYouWant = _inventory[itemIndex];

            //checks if the player has enough money to buy item
            if (player.Money >= itemYouWant.Cost)
            {
                //if yes then they buy teh item
                return true;
            }
            else
            {
                //if the player does not have enough money 
                Console.Clear();
                Console.WriteLine("You can't afford this item.");
                Console.ReadKey(true);
            }
            return false;
        }

        /// <summary>
        /// gives the player the names of the items
        /// </summary>
        /// <returns></returns>
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
