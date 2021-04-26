using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace CIS598_Senior_Project.FleetObjects.UpgradeObjects
{
    public class GunneryTeam : UpgradeCard
    {
        public override bool IsUnique { get; } = false;

        public override bool IsExhausted { get; set; } = false;

        public override bool IsDiscarded { get; set; } = false;

        public override string Text
        {
            get
            {
                return "Concentrate Fire: The next\n" +
                       "attack you perform this\n" +
                       "activation can be performed\n" +
                       "from this hull zone. Each of\n" +
                       "your hull zones cannot target\n" +
                       "the same ship or squadron more\n" +
                       "than once during your activation.";
            }
        }

        public override string Name { get { return "Gunnery Team"; } }

        public override int PointCost { get { return 7; } }

        public override Texture2D Texture { get; }

        public override Rectangle Source { get; }

        public override UpgradeTypeEnum CardType { get; }

        public GunneryTeam(ContentManager content)
        {
            CardType = UpgradeTypeEnum.WeaponsTeam;
        }
    }
}
