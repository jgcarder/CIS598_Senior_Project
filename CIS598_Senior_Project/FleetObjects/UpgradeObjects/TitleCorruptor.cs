using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace CIS598_Senior_Project.FleetObjects.UpgradeObjects
{
    public class TitleCorruptor : UpgradeCard
    {
        public override bool IsUnique { get; } = true;

        public override bool IsExhausted { get; set; } = false;

        public override bool IsDiscarded { get; set; } = false;

        public override string Text
        {
            get
            {
                return "Squadron: The speed of each\n" +
                       "squadron with BOMBER you\n" +
                       "activate is increased by 1\n" +
                       "until the end of its\n" +
                       "activation.";
            }
        }

        public override string Name { get { return "Corruptor"; } }

        public override int PointCost { get { return 5; } }

        public override Texture2D Texture { get; }

        public override Rectangle Source { get; }

        public override UpgradeTypeEnum CardType { get; }

        public TitleCorruptor(ContentManager content)
        {
            CardType = UpgradeTypeEnum.Title;
        }
    }
}
