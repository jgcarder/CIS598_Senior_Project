/* File: TIEBomberSquadron.cs
 * Author: Jackson Carder
 */

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CIS598_Senior_Project.FleetObjects.DiceObjects;
using CIS598_Senior_Project.Collisions;
using Microsoft.Xna.Framework.Content;

namespace CIS598_Senior_Project.FleetObjects.SquadronObjects
{
    /// <summary>
    /// The class for the Tie Bomber squad
    /// </summary>
    public class TIEBomberSquadron : Squadron
    {
        private int _hull;

        public override int Hull { get { return _hull; } set { _hull = value; } }

        public override int MaxHull { get { return 5; } }

        public override int Speed { get; set; } = 4;

        public override int Id { get; }

        public override int PointCost { get; } = 9;

        public override bool HasBeenActivated { get; set; } = false;

        public override bool HasSwarm { get; } = false;

        public override bool HasEscort { get; } = false;

        public override bool HasCounter2 { get; } = false;

        public override bool HasBomber { get; } = true;

        public override bool HasHeavy { get; } = true;

        public override bool IsEngaged { get; set; } = false;

        public override string Name { get { return "TIE Bomber Squadron"; } }

        public override List<Die> AntiSquadronDice { get; }

        public override List<Die> AntiShipDice { get; }

        public override Texture2D Image { get; } //To be set

        public override Rectangle Source { get; } //To be set

        public override float Rotation { get; set; } = 0;

        public override Vector2 Position { get; set; } = Vector2.Zero;

        public override Vector2 Origin { get { return new Vector2(25, 25); } }

        public override BoundingCircle Bounds { get { return new BoundingCircle(new Vector2(Position.X, Position.Y), 25); } }

        /// <summary>
        /// The constructor for the squad
        /// </summary>
        /// <param name="id">The squad id</param>
        /// <param name="content">the content loader</param>
        public TIEBomberSquadron(int id, ContentManager content)
        {
            _hull = 5;

            Id = id;

            Image = content.Load<Texture2D>("TIEBomberToken");

            AntiShipDice = new List<Die>();
            AntiSquadronDice = new List<Die>();

            AntiSquadronDice.Add(new BlackDie(DieTypeEnum.Black));

            AntiShipDice.Add(new BlackDie(DieTypeEnum.Black));
        }
    }
}
