using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Text;

namespace CIS598_Senior_Project.FleetObjects.UpgradeObjects
{
    public class CommanderAdmiralMotti : UpgradeCard
    {
        public override bool IsUnique { get; } = true;

        public override bool IsExhausted { get; set; } = false;

        public override bool IsDiscarded { get; set; } = false;

        public override string Text 
        { 
            get 
            { 
                return "The hull value of each friendly ship is " +
                       "increased according to its size class: " +
                       "Small ship: 1. " +
                       "Medium ship: 2. " +
                       "Large ship: 3."; 
            } 
        }

        public override string Name { get { return "Admiral Motti"; } }

        public override int PointCost { get { return 24; } }

        public override Texture2D Texture { get; }

        public override Rectangle Source { get; }

        public override UpgradeTypeEnum CardType { get; }

        public CommanderAdmiralMotti(ContentManager content)
        {
            CardType = UpgradeTypeEnum.Commander;
        }
    }
}
