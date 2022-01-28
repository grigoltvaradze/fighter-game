using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game_app
{
    internal class Ranged : Character
    {
        public Ranged(int startLocation)
        {
            CurrentLocation = startLocation;
        }

        public override int MaxHealth { get; protected set; } = 100;
        public override int MaxAttackSpeed { get; protected set; } = 2;
        public override int MaxAttackDamage { get; protected set; } = 20;
        public override int MinAttackDamage { get; protected set; } = 5;
        public override int AttRange { get; protected set; } = 5;

        
    }
}
