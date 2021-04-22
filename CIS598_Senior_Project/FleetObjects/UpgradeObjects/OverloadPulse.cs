using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace CIS598_Senior_Project.FleetObjects.UpgradeObjects
{
    public class OverloadPulse : UpgradeCard
    {
        public override bool IsUnique { get; } = false;

        public override bool IsExhausted { get; set; } = false;

        public override bool IsDiscarded { get; set; } = false;

        public override string Text
        {
            get
            {
                return "BLUE Critical: Exhaust all of the " +
                       "defenders defense tokens.";
            }
        }

        public override string Name { get { return "Overload Pulse"; } }

        public override int PointCost { get { return 8; } }

        public override Texture2D Texture { get; }

        public override Rectangle Source { get; }

        public override UpgradeTypeEnum CardType { get; }

        public OverloadPulse(ContentManager content)
        {
            CardType = UpgradeTypeEnum.IonCannon;
        }
    }
}
