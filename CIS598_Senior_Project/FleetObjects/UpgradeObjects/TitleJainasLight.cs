using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace CIS598_Senior_Project.FleetObjects.UpgradeObjects
{
    public class TitleJainasLight : UpgradeCard
    {
        public override bool IsUnique { get; } = true;

        public override bool IsExhausted { get; set; } = false;

        public override bool IsDiscarded { get; set; } = false;

        public override string Text
        {
            get
            {
                return "You can ignore the effects of overlapping " +
                       "obstacles. Your attacks cannot be obstructed.";
            }
        }

        public override string Name { get { return "Jaina's Light"; } }

        public override int PointCost { get { return 2; } }

        public override Texture2D Texture { get; }

        public override Rectangle Source { get; }

        public override UpgradeTypeEnum CardType { get; }

        public TitleJainasLight(ContentManager content)
        {
            CardType = UpgradeTypeEnum.Title;
        }
    }
}
