using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace CIS598_Senior_Project.FleetObjects.UpgradeObjects
{
    public class LeadingShots : UpgradeCard
    {
        public override bool IsUnique { get; } = false;

        public override bool IsExhausted { get; set; } = false;

        public override bool IsDiscarded { get; set; } = false;

        public override string Text
        {
            get
            {
                return "While attacking, you may spend\n" +
                       "1 blue die to reroll any\n" +
                       "number of dice in your attack\n" +
                       "pool.";
            }
        }

        public override string Name { get { return "Leading Shots"; } }

        public override int PointCost { get { return 6; } }

        public override Texture2D Texture { get; }

        public override Rectangle Source { get; }

        public override UpgradeTypeEnum CardType { get; }

        public LeadingShots(ContentManager content)
        {
            CardType = UpgradeTypeEnum.IonCannon;
        }
    }
}
