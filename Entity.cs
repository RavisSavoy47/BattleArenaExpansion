﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace BattleArenaExpansion
{
    class Entity
    {
        private Shop _shop;
        private string _name;
        private float _health;
        private float _attackPower;
        private float _defensePower;
        private float _money;

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

        public virtual float Money
        {
            get { return _money; }
        }

        public Entity()
        {
            _name = "Default";
            _health = 0;
            _attackPower = 0;
            _defensePower = 0;
            _money = 0;
        }

        public Entity(string name, float health, float attackPower, float defensePower, float money)
        {
            _name = name;
            _health = health;
            _attackPower = attackPower;
            _defensePower = defensePower;
            _money = money;
        }

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
            writer.WriteLine(_money);
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

            if (!float.TryParse(reader.ReadLine(), out _money))
                return false;
            return true;
        }
    }
}