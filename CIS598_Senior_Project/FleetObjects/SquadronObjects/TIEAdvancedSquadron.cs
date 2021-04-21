using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CIS598_Senior_Project.FleetObjects.DiceObjects;
using Microsoft.Xna.Framework.Content;

namespace CIS598_Senior_Project.FleetObjects.SquadronObjects
{
    public class TIEAdvancedSquadron : Squadron
    {
        private int _hull;

        public override int Hull { get { return _hull; } set { _hull = value; } }

        public override int Speed { get; set; } = 4;

        public override int Id { get; }

        public override int PointCost { get; } = 12;

        public override bool HasBeenActivated { get; set; } = false;

        public override bool HasSwarm { get; } = false;

        public override bool HasEscort { get; } = true;

        public override bool HasCounter2 { get; } = false;

        public override bool HasBomber { get; } = false;

        public override bool HasHeavy { get; } = false;

        public override bool IsEngaged { get; set; } = false;

        public override string Name { get { return "TIE Advanced Squadron"; } }

        public override List<Die> AntiSquadronDice { get; }

        public override List<Die> AntiShipDice { get; }

        public override Texture2D Image { get; } //To be set

        public override Rectangle Source { get; } //To be set

        public override double Rotation { get; set; } = 0.0;

        public override Vector2 Position { get; set; } = Vector2.Zero;

        public TIEAdvancedSquadron(int id, ContentManager content)
        {
            _hull = 5;

            Id = id;

            AntiShipDice = new List<Die>();
            AntiSquadronDice = new List<Die>();

            AntiSquadronDice.Add(new BlueDie(DieTypeEnum.Blue));
            AntiSquadronDice.Add(new BlueDie(DieTypeEnum.Blue));
            AntiSquadronDice.Add(new BlueDie(DieTypeEnum.Blue));

            AntiShipDice.Add(new BlackDie(DieTypeEnum.Black));
        }
    }
}
