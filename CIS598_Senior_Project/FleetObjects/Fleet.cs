using System;
using System.Collections.Generic;
using System.Text;
using CIS598_Senior_Project.FleetObjects.SquadronObjects;
using CIS598_Senior_Project.FleetObjects.ShipObjects;

namespace CIS598_Senior_Project.FleetObjects
{
    public class Fleet
    {
        private int _totalPoints;
        private List<Ship> _ships;
        private List<Squadron> _squadrons;
        private String _name;

        public int TotalPoints
        {
            get
            {
                return _totalPoints;
            }
        }

        public List<Ship> Ships
        {
            get
            {
                return _ships;
            }
        }

        public List<Squadron> Squadrons
        {
            get
            {
                return _squadrons;
            }
        }

        public String Name
        {
            get
            {
                return _name;
            }
        }

        public Fleet(string name)
        {

        }
    }
}
