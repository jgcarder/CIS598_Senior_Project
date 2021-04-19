using System;
using System.Collections.Generic;
using System.Text;
using CIS598_Senior_Project.FleetObjects.DiceObjects;

namespace CIS598_Senior_Project.FleetObjects.ShipObjects
{
    public class AssaultFrigateMarkII : Ship
    {
        private int _hull;

        public override int Id { get; }

        public override int PointCost 
        { 
            get 
            {
                if(ShipTypeA)
                {
                    int x = 81;
                    foreach(var upgd in Upgrades)
                    {
                        x += upgd.PointCost;
                    }
                    return x;
                }
                else
                {
                    int x = 72;
                    foreach (var upgd in Upgrades)
                    {
                        x += upgd.PointCost;
                    }
                    return x;
                }
            } 
        }

        public override int Hull { get { return _hull; } }

        public override int Command { get { return 3; } }

        public override int Squadron
        {
            get
            {
                if(ShipTypeA)
                {
                    return 2;
                }
                else
                {
                    return 3;
                }
            }
        }

        public override int Engineering { get { return 4; } }

        public override int[][] Movement { get; }
        
                                                    //{Officer, Support, Weapons, Ordinance, Offensive, Turbolasers, Ion, Defensive}
        public override int[] UpgradeTypes { get { return new int[]{1, 0, 1, 0, 1, 1, 0, 1}; } }

        public override bool HasCommander { get; set; }

        public override bool ShipTypeA { get; set; } = true;

        public override string Name { get { return "Assault Frigate Mark II"; } }

        public override FiringArc[] Arcs { get; }

        public override List<UpgradeCard> Upgrades { get; }

        public override List<DefenseToken> DefenseTokens
        {
            get
            {
                //Not based on ship types
            }
        }

        public override List<RedDie> RedAS { get; }

        public override List<BlueDie> BlueAS { get; }

        public override List<BlackDie> BlackAS { get; }

        public AssaultFrigateMarkII(int id)
        {
            Id = id;
            RedAS = new List<RedDie>();
            BlueAS = new List<BlueDie>();
            BlackAS = new List<BlackDie>();
            
        }
    }
}
