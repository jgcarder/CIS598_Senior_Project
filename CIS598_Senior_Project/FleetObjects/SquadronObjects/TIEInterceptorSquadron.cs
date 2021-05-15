/* File: TIEInterceptorSquadron.cs
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
    /// The class for the TIE interceptor
    /// </summary>
    public class TIEInterceptorSquadron : Squadron
    {
        private int _hull;

        public override int Hull { get { return _hull; } set { _hull = value; } }

        public override int MaxHull { get { return 3; } }

        public override int Speed { get; set; } = 5;

        public override int Id { get; }

        public override int PointCost { get; } = 11;

        public override bool HasBeenActivated { get; set; } = false;

        public override bool HasSwarm { get; } = true;

        public override bool HasEscort { get; } = false;

        public override bool HasCounter2 { get; } = true;

        public override bool HasBomber { get; } = false;

        public override bool HasHeavy { get; } = false;

        public override bool IsEngaged { get; set; } = false;

        public override string Name { get { return "TIE Interceptor Squadron"; } }

        public override List<Die> AntiSquadronDice { get; }

        public override List<Die> AntiShipDice { get; }

        public override Texture2D Image { get; } //To be set

        public override Rectangle Source { get; } //To be set

        public override float Rotation { get; set; } = 0;

        public override Vector2 Position { get; set; } = Vector2.Zero;

        public override Vector2 Origin { get { return new Vector2(25, 25); } }

        public override BoundingCircle Bounds { get { return new BoundingCircle(new Vector2(Position.X, Position.Y), 25); } }

        /// <summary>
        /// The squad constructor
        /// </summary>
        /// <param name="id">the is</param>
        /// <param name="content">the content loader</param>
        public TIEInterceptorSquadron(int id, ContentManager content)
        {
            _hull = 3;

            Id = id;

            Image = content.Load<Texture2D>("TIEInterceptorToken");

            AntiShipDice = new List<Die>();
            AntiSquadronDice = new List<Die>();

            AntiSquadronDice.Add(new BlueDie(DieTypeEnum.Blue));
            AntiSquadronDice.Add(new BlueDie(DieTypeEnum.Blue));
            AntiSquadronDice.Add(new BlueDie(DieTypeEnum.Blue));
            AntiSquadronDice.Add(new BlueDie(DieTypeEnum.Blue));

            AntiShipDice.Add(new BlackDie(DieTypeEnum.Black));
        }
    }
}
