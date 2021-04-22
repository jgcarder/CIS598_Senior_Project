using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace CIS598_Senior_Project.FleetObjects.UpgradeObjects
{
    public class IonCannonBatteries : UpgradeCard
    {
        public override bool IsUnique { get; } = false;

        public override bool IsExhausted { get; set; } = false;

        public override bool IsDiscarded { get; set; } = false;

        public override string Text
        {
            get
            {
                return "BLUE Critical: Choose and discard 1 " +
                       "command token from the defender. If " +
                       "the defender does not have any " +
                       "command tokens, the defending hull " +
                       "zone loses one shield instead.";
            }
        }

        public override string Name { get { return "Ion Cannon Batteries"; } }

        public override int PointCost { get { return 5; } }

        public override Texture2D Texture { get; }

        public override Rectangle Source { get; }

        public override UpgradeTypeEnum CardType { get; }

        public IonCannonBatteries(ContentManager content)
        {
            CardType = UpgradeTypeEnum.IonCannon;
        }
    }
}
