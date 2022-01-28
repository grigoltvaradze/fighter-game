using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game_app
{
    public abstract class Character
    {
        public abstract int MaxHealth { get; protected set; }
        public abstract int MaxAttackSpeed { get; protected set; }
        public abstract int MaxAttackDamage { get; protected set; }
        public abstract int MinAttackDamage { get; protected set; }
        public abstract int AttRange { get; protected set; }
        public int CurrentLocation { get; set; }
        protected int currentHealth;
        public Character()
        {
            currentHealth = MaxHealth;
        }
        public bool IsAlive => currentHealth > 0;
        public void Destroy() => currentHealth = 0;


        public virtual int CountDamage()
        {
            Random random = new Random();
            int attackSpeed = random.Next(1, MaxAttackSpeed);
            int attDMG = random.Next(MinAttackDamage, MaxAttackDamage);
            int damage = attackSpeed * attDMG;
            return damage;
        }
        public virtual void TakeDamage(int damage)
        {
            currentHealth -= damage;
            if (!IsAlive) Destroy();
        }
    }

}

