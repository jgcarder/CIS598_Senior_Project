using System;
using System.Collections.Generic;
using System.Text;

namespace CIS598_Senior_Project.FleetObjects
{
    public class Ship
    {
        private int _points;
        private int[][] _movement;
        private List<int> _possibleUpgrades;
        private bool _hasCommander;
        
        public int Points
        {
            get
            {
                return _points;
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
