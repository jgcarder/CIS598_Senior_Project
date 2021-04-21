using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace CIS598_Senior_Project.FleetObjects.UpgradeObjects
{
    public class EnhancedArmament : UpgradeCard
    {
        public override bool IsUnique { get; } = false;

        public override bool IsExhausted { get; set; } = false;

        public override bool IsDiscarded { get; set; } = false;

        public override string Text
        {
            get
            {
                return "Modification The battery armaments for " +
                       "your left and right hull zones are " +
                       "increased by 1 red die.";
            }
        }

        public override string Name { get { return "Enhanced Armament"; } }

        public override int PointCost { get { return 10; } }

        public override Texture2D Texture { get; }

        public override Rectangle Source { get; }

        public EnhancedArmament(ContentManager content)
        {

        }
    }
}
