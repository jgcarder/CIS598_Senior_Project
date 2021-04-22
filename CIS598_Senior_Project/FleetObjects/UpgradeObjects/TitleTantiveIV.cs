using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace CIS598_Senior_Project.FleetObjects.UpgradeObjects
{
    public class TitleTantiveIV : UpgradeCard
    {
        public override bool IsUnique { get; } = true;

        public override bool IsExhausted { get; set; } = false;

        public override bool IsDiscarded { get; set; } = false;

        public override string Text
        {
            get
            {
                return "Before you gain a command token, 1 " +
                       "friendly ship at distance 1-5 may gain " +
                       "that token instead.";
            }
        }

        public override string Name { get { return "Tantive IV"; } }

        public override int PointCost { get { return 3; } }

        public override Texture2D Texture { get; }

        public override Rectangle Source { get; }

        public override UpgradeTypeEnum CardType { get; }

        public TitleTantiveIV(ContentManager content)
        {
            CardType = UpgradeTypeEnum.Title;
        }
    }
}
