using System;
using System.Collections.Generic;
using System.Text;
using CIS598_Senior_Project.FleetObjects.SquadronObjects;
using CIS598_Senior_Project.FleetObjects.ShipObjects;

namespace CIS598_Senior_Project.FleetObjects
{
    public class Fleet
    {
        private List<Ship> _ships;
        private List<Squadron> _squadrons;
        private String _name;
        private bool _isRebelFleet;

        public int TotalPoints
        {
            get
            {
                int x = 0;
                foreach (var ship in _ships)
                {
                    x += ship.PointCost;
                }
                foreach (var squad in _squadrons)
                {
                    x += squad.PointCost;
                }
                return x;
            }
        }

        public int SquadronPoints
        {
            get
            {
                int x = 0;
                foreach (var squad in _squadrons)
                {
                    x += squad.PointCost;
                }
                return x;
            }
        }

        public List<Ship> Ships
        {
            get
            {
                return _ships;
            }

            set
            {
                _ships = value;
            }
        }

        public List<Squadron> Squadrons
        {
            get
            {
                return _squadrons;
            }
            set
            {
                _squadrons = value;
            }
        }

        public String Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        public bool IsRebelFleet
        {
            get { return _isRebelFleet; }
            set { _isRebelFleet = value; }
        }

        public Fleet(string name)
        {
            _ships = new List<Ship>();
            _name = "";
            _squadrons = new List<Squadron>();
            _isRebelFleet = false;
        }
    }
}
