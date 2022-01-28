using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game_app
{
    internal class Player : Character
    {
        public override int MaxHealth { get; protected set; } = 150;
        public override int MaxAttackSpeed { get; protected set; } = 3;
        public override int MaxAttackDamage { get; protected set; } = 40;
        public override int MinAttackDamage { get; protected set; } = 10;
        public override int AttRange { get; protected set; } = 1;
        int StepLength = 1;
        public int Move() => CurrentLocation += StepLength;
        public bool CanBeHealed { get { return currentHealth < MaxHealth; } }
        public int Heal()
        {
            if (CanBeHealed)
            {
                currentHealth = MaxHealth;
                Console.WriteLine("You recieved heal");
                return currentHealth;
            }
            else
            {
                Console.WriteLine("You already have max hp");
                return 0;
            }
        }

        public readonly string Name;
        public Player(string name)
        {
            Name = name;
            CurrentLocation = 0;
        }

        public void TakeDamage(Character from)
        {
            int damage = from.CountDamage();
            base.TakeDamage(damage);
            Console.WriteLine(this.Name + " took " + damage + " damage from " + from.GetType().Name);
        }

        public override int CountDamage()
        {
            int damage = base.CountDamage();
            Console.WriteLine(this.Name + " damaged enemy by " + damage);

            return damage;
        }
    }
}
