using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CIS598_Senior_Project.FleetObjects.DiceObjects;
using CIS598_Senior_Project.FleetObjects.DefenseTokenObjects;
using CIS598_Senior_Project.FleetObjects.UpgradeObjects;
using CIS598_Senior_Project.Collisions;
using Microsoft.Xna.Framework.Content;

namespace CIS598_Senior_Project.FleetObjects.ShipObjects
{
    public abstract class Ship
    {

        public abstract int Id { get; }

        public abstract int PointCost { get; }

        public abstract int Hull { get; set; }

        public abstract int MaxHull { get; }

        public abstract int Command { get; }

        public abstract int Squadron { get; }

        public abstract int Engineering { get; }

        public abstract int Speed { get; set; }

        public abstract int TokenCount { get; }

        public abstract int[,] Movement { get; }

        public abstract int[] UpgradeTypes { get; }

        public abstract bool HasCommander { get; set; }

        public abstract bool ShipTypeA { get; set; }

        public abstract bool HasEngineeringToken { get; set; }

        public abstract bool HasSquadronToken { get; set; }

        public abstract bool HasNavigationToken { get; set; }

        public abstract bool HasConcentrateFireToken { get; set; }

        public abstract bool BeenActivated { get; set; }

        public abstract string Name { get; }

        public abstract Queue<CommandDialEnum> CommandDials { get; set; }

        public abstract UpgradeCard Title { get; set; }

        public abstract UpgradeCard Commander { get; set; }

        public abstract FiringArc[] Arcs { get; }

        public abstract UpgradeCard[] Upgrades { get; set; }

        public abstract List<DefenseToken> DefenseTokens { get; }

        public abstract List<RedDie> RedAS { get; }

        public abstract List<BlueDie> BlueAS { get; }

        public abstract List<BlackDie> BlackAS { get; }

        public abstract Texture2D Image { get; }

        public abstract Rectangle Source { get; }

        public abstract float Rotation { get; set; }

        public abstract Vector2 Position { get; set; }

        public abstract Vector2 Origin { get; }

        public abstract BoundingCircle Bounds { get; }

        public abstract BoundingCircle BowBounds { get; }

        public abstract BoundingCircle PortBounds { get; }

        public abstract BoundingCircle StarboardBounds { get; }

        public abstract BoundingCircle AftBounds { get; }

        public abstract void setHullDice(bool shipA);

        public abstract void RefreshDefense();

        public abstract void refreshShip();
    }
}
