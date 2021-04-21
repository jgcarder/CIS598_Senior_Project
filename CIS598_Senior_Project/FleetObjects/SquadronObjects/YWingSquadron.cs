using System;
using System.Collections.Generic;
using System.Text;
using CIS598_Senior_Project.FleetObjects.DiceObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CIS598_Senior_Project.FleetObjects.SquadronObjects
{
    public class YWingSquadron : Squadron
    {
        private int _hull;

        public override int Hull { get { return _hull; } set { _hull = value; } }

        public override int Speed { get; set; } = 3;

        public override int Id { get; }

        public override int PointCost { get; } = 10;

        public override bool HasBeenActivated { get; set; } = false;

        public override bool HasSwarm { get; } = false;

        public override bool HasEscort { get; } = false;

        public override bool HasCounter2 { get; } = false;

        public override bool HasBomber { get; } = true;

        public override bool HasHeavy { get; } = true;

        public override bool IsEngaged { get; set; } = false;

        public override string Name { get { return "Y-Wing Squadron"; } }

        public override List<Die> AntiSquadronDice { get; }

        public override List<Die> AntiShipDice { get; }

        public override Texture2D Image { get; } //To be set

        public override Rectangle Source { get; } //To be set

        public override double Rotation { get; set; } = 0.0;

        public override Vector2 Position { get; set; } = Vector2.Zero;

        public YWingSquadron(int id)
        {
            _hull = 6;

            Id = id;

            AntiShipDice = new List<Die>();
            AntiSquadronDice = new List<Die>();

            AntiSquadronDice.Add(new BlueDie(DieTypeEnum.Blue));
            AntiSquadronDice.Add(new BlueDie(DieTypeEnum.Blue));

            AntiShipDice.Add(new BlackDie(DieTypeEnum.Black));
        }
    }
}
