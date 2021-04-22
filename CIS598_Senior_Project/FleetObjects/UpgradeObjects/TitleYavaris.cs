using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace CIS598_Senior_Project.FleetObjects.UpgradeObjects
{
    public class TitleYavaris : UpgradeCard
    {
        public override bool IsUnique { get; } = true;

        public override bool IsExhausted { get; set; } = false;

        public override bool IsDiscarded { get; set; } = false;

        public override string Text
        {
            get
            {
                return "Squadron: Each squadron you activate " +
                       "may choose to only attack during your " +
                       "activation. If it does, while attacking, " +
                       "it may add 1 die to its attack pool of " +
                       "a color already in its attack pool.";
            }
        }

        public override string Name { get { return "Yavaris"; } }

        public override int PointCost { get { return 5; } }

        public override Texture2D Texture { get; }

        public override Rectangle Source { get; }

        public TitleYavaris(ContentManager content)
        {

        }
    }
}
