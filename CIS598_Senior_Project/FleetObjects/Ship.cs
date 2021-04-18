using System;
using System.Collections.Generic;
using System.Text;

namespace CIS598_Senior_Project.FleetObjects
{
    public class Ship
    {
        private int _pointCost;
        private int[][] _movement;
        private List<int> _possibleUpgrades;
        private bool _hasCommander;
        private List<DefenseToken> _defenseTokens;
        private int _hull;
        private int _command;
        private int _squadron;
        private int _engineering;

        private 
        
        public int PointCost
        {
            get
            {
                return _pointCost;
            }
        }

        public int[][] Movement
        {
            get
            {
                return _movement;
            }
        }
        public List<int> PossibleUpgrades
        {
            get
            {
                return _possibleUpgrades;
            }
        }
        public bool HasCommander
        {
            get
            {
                return _hasCommander;
            }
        }
    }
}
