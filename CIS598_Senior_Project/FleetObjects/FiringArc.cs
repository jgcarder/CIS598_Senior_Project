using System;
using System.Collections.Generic;
using System.Text;
using CIS598_Senior_Project.FleetObjects.DiceObjects;

namespace CIS598_Senior_Project.FleetObjects
{
    public class FiringArc
    {
        private int _shields;
        private double _arc;
        private List<RedDie> _redDice;
        private List<BlueDie> _blueDice;
        private List<BlackDie> _blackDice;

        public int Shields
        {
            get { return _shields; }
            set { _shields = value; }
        }

        public double Arc
        {
            get { return _arc; }
            set { _arc = value; }
        }

        public List<RedDie> RedDice
        {
            get { return _redDice; }
        }

        public List<BlueDie> BlueDice
        {
            get { return _blueDice; }
        }

        public List<BlackDie> BlackDice
        {
            get { return _blackDice; }
        }

        public FiringArc(int shields, double angle)
        {
            _shields = shields;
            _arc = angle;
        }

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
    }
}
