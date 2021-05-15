/* File: FiringArc.cs
 * Author: Jackson Carder
 */

using System;
using System.Collections.Generic;
using System.Text;
using CIS598_Senior_Project.FleetObjects.DiceObjects;

namespace CIS598_Senior_Project.FleetObjects.ShipObjects
{
    /// <summary>
    /// Class for the firing arcs of the ships
    /// </summary>
    public class FiringArc
    {
        private int _shields;
        private int _maxShields;
        private double _arc;
        private List<RedDie> _redDice;
        private List<BlueDie> _blueDice;
        private List<BlackDie> _blackDice;

        /// <summary>
        /// The shields of the arc
        /// </summary>
        public int Shields
        {
            get { return _shields; }
            set { _shields = value; }
        }

        /// <summary>
        /// The max amount of shields
        /// </summary>
        public int MaxShields
        {
            get { return _maxShields; }
        }

        /// <summary>
        /// The arc radians 
        /// </summary>
        public double ArcRadians
        {
            get { return _arc; }
            set { _arc = value; }
        }

        /// <summary>
        /// Red dice on the arc
        /// </summary>
        public List<RedDie> RedDice
        {
            get { return _redDice; }
        }

        /// <summary>
        /// The blue dice in the arc
        /// </summary>
        public List<BlueDie> BlueDice
        {
            get { return _blueDice; }
        }

        /// <summary>
        /// The black dice in the arc
        /// </summary>
        public List<BlackDie> BlackDice
        {
            get { return _blackDice; }
        }

        /// <summary>
        /// The constructor 
        /// </summary>
        /// <param name="shields">the shields of the arc</param>
        /// <param name="rads">The radians of the arc</param>
        public FiringArc(int shields, double rads)
        {
            _shields = shields;
            _maxShields = shields;
            _arc = rads;

            _blackDice = new List<BlackDie>();
            _blueDice = new List<BlueDie>();
            _redDice = new List<RedDie>();
        }

        /// <summary>
        /// Adds dice to the arc
        /// </summary>
        /// <param name="numDice">The number of dice to add</param>
        /// <param name="dieType">The type of die to add</param>
        public void AddDice(int numDice, DieTypeEnum dieType)
        {
            switch(dieType)
            {
                case DieTypeEnum.Red:
                    for(int i = 0; i < numDice; i++)
                    {
                        _redDice.Add(new RedDie(DieTypeEnum.Red));
                    }
                    break;
                case DieTypeEnum.Blue:
                    for (int i = 0; i < numDice; i++)
                    {
                        _blueDice.Add(new BlueDie(DieTypeEnum.Blue));
                    }
                    break;
                case DieTypeEnum.Black:
                    for (int i = 0; i < numDice; i++)
                    {
                        _blackDice.Add(new BlackDie(DieTypeEnum.Black));
                    }
                    break;
            }
        }

        /// <summary>
        /// Removes the selected type of dic
        /// </summary>
        /// <param name="dieType">The type to remove</param>
        public void RemoveDie(DieTypeEnum dieType)
        {
            switch (dieType)
            {
                case DieTypeEnum.Red:
                    _redDice.RemoveAt(0);
                    break;
                case DieTypeEnum.Blue:
                    _blueDice.RemoveAt(0);
                    break;
                case DieTypeEnum.Black:
                    _blackDice.RemoveAt(0);
                    break;
            }
        }

        /// <summary>
        /// Cleears the dice
        /// </summary>
        public void ClearDice()
        {
            _blackDice.Clear();
            _blueDice.Clear();
            _redDice.Clear();
        }
    }
}
