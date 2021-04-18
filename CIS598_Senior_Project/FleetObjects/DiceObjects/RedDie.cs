using System;
using System.Collections.Generic;
using System.Text;

namespace CIS598_Senior_Project.FleetObjects.DiceObjects
{
    public class RedDie : Die
    {
        private RedDieSideEnum _sides;
        private DieTypeEnum _type;

        public DieTypeEnum Type { get { return _type; } }

        public RedDie(DieTypeEnum type)
        {
            _type = type;
        }

        /// <summary>
        /// Rolls the Die and returns the outcome.
        /// </summary>
        /// <returns>The outcome of rolling the Red die</returns>
        public RedDieSideEnum Roll()
        {
            Random rand = new Random();
            int roll = rand.Next(0, 8);
            if (roll <= 1) return RedDieSideEnum.Hit;
            else if (roll == 2) return RedDieSideEnum.DoubleHit;
            else if (roll > 2 && roll < 5) return RedDieSideEnum.Crit;
            else if (roll == 5) return RedDieSideEnum.Accuracy;
            else return RedDieSideEnum.Blank;

        }
    }
}
