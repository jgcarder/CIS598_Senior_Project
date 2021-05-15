﻿/* File: AWingSquadron.cs
 * Author: Jackson Carder
 */


using CIS598_Senior_Project.FleetObjects.DiceObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Text;
using CIS598_Senior_Project.Collisions;

namespace CIS598_Senior_Project.FleetObjects.SquadronObjects
{
    /// <summary>
    /// The awing class
    /// </summary>
    public class AWingSquadron : Squadron
    {
        private int _hull;

        public override int Hull { get { return _hull; } set { _hull = value; } }

        public override int MaxHull { get { return 4; } }

        public override int Speed { get; set; } = 5;

        public override int Id { get; }

        public override int PointCost { get; } = 11;

        public override bool HasBeenActivated { get; set; } = false;

        public override bool HasSwarm { get; } = false;

        public override bool HasEscort { get; } = false;

        public override bool HasCounter2 { get; } = true;

        public override bool HasBomber { get; } = false;

        public override bool HasHeavy { get; } = false;

        public override bool IsEngaged { get; set; } = false;

        public override string Name { get { return "A-Wing Squadron"; } }

        public override List<Die> AntiSquadronDice { get; }

        public override List<Die> AntiShipDice { get; }

        public override Texture2D Image { get; } //To be set

        public override Rectangle Source { get; } //To be set

        public override float Rotation { get; set; } = 0;

        public override Vector2 Position { get; set; } = Vector2.Zero;

        public override Vector2 Origin { get { return new Vector2(25, 25); } }
        
        public override BoundingCircle Bounds { get { return new BoundingCircle(new Vector2(Position.X, Position.Y), 25); } }

        /// <summary>
        /// The constructor for awings
        /// </summary>
        /// <param name="id">the id</param>
        /// <param name="content">the content loader</param>
        public AWingSquadron(int id, ContentManager content)
        {
            _hull = 4;

            Id = id;

            Image = content.Load<Texture2D>("AWingToken");

            AntiShipDice = new List<Die>();
            AntiSquadronDice = new List<Die>();

            AntiSquadronDice.Add(new BlueDie(DieTypeEnum.Blue));
            AntiSquadronDice.Add(new BlueDie(DieTypeEnum.Blue));
            AntiSquadronDice.Add(new BlueDie(DieTypeEnum.Blue));

            AntiShipDice.Add(new BlackDie(DieTypeEnum.Black));
        }
    }
}
