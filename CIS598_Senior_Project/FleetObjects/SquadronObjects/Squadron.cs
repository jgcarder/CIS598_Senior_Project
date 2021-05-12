using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using CIS598_Senior_Project.FleetObjects.DiceObjects;
using Microsoft.Xna.Framework.Content;
using CIS598_Senior_Project.Collisions;

namespace CIS598_Senior_Project.FleetObjects.SquadronObjects
{
    public abstract class Squadron
    {
        public abstract int Hull { get; set; }

        public abstract int MaxHull { get; }

        public abstract int Speed { get; set; }

        public abstract int Id { get; }

        public abstract int PointCost { get; }

        public abstract bool HasBeenActivated { get; set; }

        public abstract bool HasSwarm { get; }

        public abstract bool HasEscort { get; }

        public abstract bool HasCounter2 { get; }

        public abstract bool HasBomber { get; }

        public abstract bool HasHeavy { get; }

        public abstract bool IsEngaged { get; set; }

        public abstract string Name { get; }

        public abstract List<Die> AntiSquadronDice { get; }

        public abstract List<Die> AntiShipDice { get; }

        public abstract Texture2D Image { get; }

        public abstract Rectangle Source { get; }

        public abstract float Rotation { get; set; }

        public abstract Vector2 Position { get; set; }

        public abstract Vector2 Origin { get; }

        public abstract BoundingCircle Bounds { get; }
    }
}
