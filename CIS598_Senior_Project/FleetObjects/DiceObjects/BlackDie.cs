/* File: BlackDie.cs
 * Author: Jackson Carder
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace CIS598_Senior_Project.FleetObjects.DiceObjects
{
    /// <summary>
    /// A black die to be rolled when needed
    /// </summary>
    public class BlackDie : Die
    {
        private BlackDieSideEnum _sides;
        private DieTypeEnum _type;

        /// <summary>
        /// Die type, idk why I have this here
        /// </summary>
        public DieTypeEnum Type { get { return _type; } }

        /// <summary>
        /// Constructor for the die
        /// </summary>
        /// <param name="type">The die's type</param>
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
