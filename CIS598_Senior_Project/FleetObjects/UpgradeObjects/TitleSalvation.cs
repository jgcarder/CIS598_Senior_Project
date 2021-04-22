using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace CIS598_Senior_Project.FleetObjects.UpgradeObjects
{
    public class TitleSalvation : UpgradeCard
    {
        public override bool IsUnique { get; } = true;

        public override bool IsExhausted { get; set; } = false;

        public override bool IsDiscarded { get; set; } = false;

        public override string Text
        {
            get
            {
                return "While attacking a ship from your front " +
                       "hull zone, your  icons count as two " +
                       "damage instead of one.";
            }
        }

        public override string Name { get { return "Salvation"; } }

        public override int PointCost { get { return 7; } }

        public override Texture2D Texture { get; }

        public override Rectangle Source { get; }

        public override UpgradeTypeEnum CardType { get; }

        public TitleSalvation(ContentManager content)
        {
            CardType = UpgradeTypeEnum.Title;
        }
    }
}
