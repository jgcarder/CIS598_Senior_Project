/* File: BlueDie.cs
 * Author: Jackson Carder
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace CIS598_Senior_Project.FleetObjects.DiceObjects
{
    /// <summary>
    /// The class for the blue varient of Die
    /// </summary>
    public class BlueDie : Die
    {
        private BlueDieSideEnum _sides;
        private DieTypeEnum _type;

        /// <summary>
        /// The Die's type
        /// </summary>
        public DieTypeEnum Type { get { return _type; } }

        /// <summary>
        /// The blue die constructor
        /// </summary>
        /// <param name="type">The type of die it is</param>
        public BlueDie(DieTypeEnum type)
        {
            _type = type;
        }

        /// <summary>
        /// Rolls the Die and returns the outcome.
        /// </summary>
        /// <returns>The outcome of rolling the Red die</returns>
        public BlueDieSideEnum Roll()
        {
            Random rand = new Random();
            int roll = rand.Next(0, 8);
            if (roll <= 3) return BlueDieSideEnum.Hit;
            else if (roll > 3 && roll < 6) return BlueDieSideEnum.Crit;
            else return BlueDieSideEnum.Accuracy;

        }
    }
}
