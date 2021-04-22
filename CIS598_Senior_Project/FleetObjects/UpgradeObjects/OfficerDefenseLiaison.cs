using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace CIS598_Senior_Project.FleetObjects.UpgradeObjects
{
    public class OfficerDefenseLiaison : UpgradeCard
    {
        public override bool IsUnique { get; } = false;

        public override bool IsExhausted { get; set; } = false;

        public override bool IsDiscarded { get; set; } = false;

        public override string Text
        {
            get
            {
                return "Before you reveal a command, you may " +
                       "spend one command token to change that " +
                       "command to a Navigation or Engineering command";
            }
        }

        public override string Name { get { return "Defense Liaison"; } }

        public override int PointCost { get { return 3; } }

        public override Texture2D Texture { get; }

        public override Rectangle Source { get; }

        public override UpgradeTypeEnum CardType { get; }

        public OfficerDefenseLiaison(ContentManager content)
        {
            CardType = UpgradeTypeEnum.Officers;
        }
    }
}
