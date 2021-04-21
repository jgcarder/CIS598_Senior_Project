using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace CIS598_Senior_Project.FleetObjects.UpgradeObjects
{
    public class XX9Turbolasers : UpgradeCard
    {
        public override bool IsUnique { get; } = false;

        public override bool IsExhausted { get; set; } = false;

        public override bool IsDiscarded { get; set; } = false;

        public override string Text
        {
            get
            {
                return "Critical: The first 2 damage cards " +
                       "dealt to the defender by this attack " +
                       "are dealt faceup.";
            }
        }

        public override string Name { get { return "XX-9 Turbolasers"; } }

        public override int PointCost { get { return 5; } }

        public override Texture2D Texture { get; }

        public override Rectangle Source { get; }

        public XX9Turbolasers(ContentManager content)
        {

        }
    }
}
