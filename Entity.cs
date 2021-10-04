using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace BattleArenaExpansion
{
    class Entity
    {
        private string _name;
        protected float _health;
        protected float _maxHealth;
        private float _attackPower;
        private float _defensePower;
        private float _moneyAmount;
        private Item[] _inventory;

        public string Name
        {
            get { return _name; }
        }

        public float Health
        {
            get { return _health; }
        }


        public virtual float AttackPower
        {
            get { return _attackPower; }
        }

        public virtual float DefensePower
        {
            get { return _defensePower; }
        }

        public float MoneyAmount
        {
            get { return _moneyAmount; }
        }

        //Set the base values for all entities
        public Entity()
        {
            _name = "Default";
            _health = 0;
            _attackPower = 0;
            _defensePower = 0;
            _moneyAmount = 0;
        }

        //Stating the bases for each entity
        public Entity(string name, float health, float attackPower, float defensePower, float moneyAmount)
        {
            _name = name;
            _health = health;
            _attackPower = attackPower;
            _defensePower = defensePower;
            _moneyAmount = moneyAmount;
            _inventory = new Item[16];
        }

        //Gets the damage for each entity
        public float TakeDamage(float damageAmount)
        {
            float damageTaken = damageAmount - DefensePower;

            if (damageTaken < 0)
            {
                damageTaken = 0;
            }

            _health -= damageTaken;

            return damageTaken;
        }

        /// <summary>
        /// Gets the damage and returns it
        /// </summary>
        /// <param name="defender"></param>
        /// <returns></returns>
        public float Attack(Entity defender)
        {
            return defender.TakeDamage(AttackPower);
        }

        public virtual void Save(StreamWriter writer)
        {
            writer.WriteLine(_name);
            writer.WriteLine(_health);
            writer.WriteLine(_attackPower);
            writer.WriteLine(_defensePower);
            writer.WriteLine(_moneyAmount);
        }

        public virtual bool Load(StreamReader reader)
        {
            _name = reader.ReadLine();

            if (!float.TryParse(reader.ReadLine(), out _health))
                return false;

            if (!float.TryParse(reader.ReadLine(), out _attackPower))
                return false;

            if (!float.TryParse(reader.ReadLine(), out _defensePower))
                return false;

            if (!float.TryParse(reader.ReadLine(), out _moneyAmount))
                return false;
            return true;
        }
    }
}
