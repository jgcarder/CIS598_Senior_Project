using System;
using System.Collections.Generic;
using System.Text;

namespace CIS598_Senior_Project.FleetObjects.DiceObjects
{
    public class BlackDie : Die
    {
        private BlackDieSideEnum _sides;
        private DieTypeEnum _type;

        public DieTypeEnum Type { get { return _type; } }

        public BlackDie(DieTypeEnum type)
        {
            _type = type;
        }

        /// <summary>
        /// Rolls the Die and returns the outcome.
        /// </summary>
        /// <returns>The outcome of rolling the Red die</returns>
        public BlackDieSideEnum Roll()
        {
            Random rand = new Random();
            int roll = rand.Next(0, 8);
            if (roll <= 3) return BlackDieSideEnum.Hit;
            else if (roll > 3 && roll < 6) return BlackDieSideEnum.HitCrit;
            else return BlackDieSideEnum.Blank;

        }
    }
}
