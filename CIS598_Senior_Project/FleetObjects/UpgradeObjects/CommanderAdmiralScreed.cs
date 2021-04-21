using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace CIS598_Senior_Project.FleetObjects.UpgradeObjects
{
    public class CommanderAdmiralScreed : UpgradeCard
    {
        public override bool IsUnique { get; } = true;

        public override bool IsExhausted { get; set; } = false;

        public override bool IsDiscarded { get; set; } = false;

        public override string Text
        {
            get
            {
                return "Once per activation, when a friendly ship " +
                       "is attacking, it may spend one die to change " +
                       "a die to a face with a critical icon.";
            }
        }

        public override string Name { get { return "Admiral Screed"; } }

        public override int PointCost { get { return 26; } }

        public override Texture2D Texture { get; }

        public override Rectangle Source { get; }

        public CommanderAdmiralScreed(ContentManager content)
        {

        }
    }
}
