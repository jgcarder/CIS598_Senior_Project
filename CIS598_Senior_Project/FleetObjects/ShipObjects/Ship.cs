using System;
using System.Collections.Generic;
using System.Text;
using CIS598_Senior_Project.FleetObjects.DiceObjects;

namespace CIS598_Senior_Project.FleetObjects.ShipObjects
{
    public abstract class Ship
    {

        public abstract int Id { get; }

        public abstract int PointCost { get; }

        public abstract int Hull { get; }

        public abstract int Command { get; }

        public abstract int Squadron { get; }

        public abstract int Engineering { get; }

        public abstract int[][] Movement { get; }

        public abstract int[] UpgradeTypes { get; }

        public abstract bool HasCommander { get; set; }

        public abstract bool ShipTypeA { get; set; }

        public abstract string Name { get; }

        public abstract FiringArc[] Arcs { get; }

        public abstract List<UpgradeCard> Upgrades { get; }

        public abstract List<DefenseToken> DefenseTokens { get; }

        public abstract List<RedDie> RedAS { get; }

        public abstract List<BlueDie> BlueAS { get; }

        public abstract List<BlackDie> BlackAS { get; }
    }
}
