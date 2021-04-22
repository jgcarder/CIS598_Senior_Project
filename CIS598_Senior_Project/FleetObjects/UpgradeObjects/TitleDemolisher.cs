using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace CIS598_Senior_Project.FleetObjects.UpgradeObjects
{
    public class TitleDemolisher : UpgradeCard
    {
        public override bool IsUnique { get; } = true;

        public override bool IsExhausted { get; set; } = false;

        public override bool IsDiscarded { get; set; } = false;

        public override string Text
        {
            get
            {
                return "During your Attack step, you can " +
                       "perform only 1 attack. You can " +
                       "perform 1 of your attacks after you " +
                       "execute your first maneuver during your " +
                       "activation.";
            }
        }

        public override string Name { get { return "Demolisher"; } }

        public override int PointCost { get { return 10; } }

        public override Texture2D Texture { get; }

        public override Rectangle Source { get; }

        public override UpgradeTypeEnum CardType { get; }

        public TitleDemolisher(ContentManager content)
        {
            CardType = UpgradeTypeEnum.Title;
        }
    }
}
