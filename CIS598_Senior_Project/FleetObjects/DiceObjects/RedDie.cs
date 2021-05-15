/* File: RedDie.cs
 * Author: Jackson Carder
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace CIS598_Senior_Project.FleetObjects.DiceObjects
{
    /// <summary>
    /// A class for a red die to roll
    /// </summary>
    public class RedDie : Die
    {
        private RedDieSideEnum _sides;
        private DieTypeEnum _type;

        /// <summary>
        /// The type of die that it is? again idk why I added this here lol.
        /// </summary>
        public DieTypeEnum Type { get { return _type; } }

        /// <summary>
        /// The red Die constructor
        /// </summary>
        /// <param name="type">The type of die you want</param>
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
