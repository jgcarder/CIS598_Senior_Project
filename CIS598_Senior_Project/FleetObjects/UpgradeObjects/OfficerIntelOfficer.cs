using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace CIS598_Senior_Project.FleetObjects.UpgradeObjects
{
    public class OfficerIntelOfficer : UpgradeCard
    {
        public override bool IsUnique { get; } = false;

        public override bool IsExhausted { get; set; } = false;

        public override bool IsDiscarded { get; set; } = false;

        public override string Text
        {
            get
            {
                return "While attacking, after you roll your " +
                       "attack pool, you may exhaust this card " +
                       "to choose 1 defense token. If that " +
                       "token is spent during this attack, " +
                       "discard that token";
            }
        }

        public override string Name { get { return "Intel Officer"; } }

        public override int PointCost { get { return 7; } }

        public override Texture2D Texture { get; }

        public override Rectangle Source { get; }

        public override UpgradeTypeEnum CardType { get; }

        public OfficerIntelOfficer(ContentManager content)
        {
            CardType = UpgradeTypeEnum.Officers;
        }
    }
}
