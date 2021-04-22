using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace CIS598_Senior_Project.FleetObjects.UpgradeObjects
{
    public class TitleInsidious : UpgradeCard
    {
        public override bool IsUnique { get; } = true;

        public override bool IsExhausted { get; set; } = false;

        public override bool IsDiscarded { get; set; } = false;

        public override string Text
        {
            get
            {
                return "The black dice in your battery armament " +
                       "can be used at medium range. This effect " +
                       "applies only when attacking the rear hull " +
                       "zone of a ship.";
            }
        }

        public override string Name { get { return "Insidious"; } }

        public override int PointCost { get { return 3; } }

        public override Texture2D Texture { get; }

        public override Rectangle Source { get; }

        public TitleInsidious(ContentManager content)
        {

        }
    }
}
