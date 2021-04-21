using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace CIS598_Senior_Project.FleetObjects.UpgradeObjects
{
    public class NavTeam : UpgradeCard
    {
        public override bool IsUnique { get; } = false;

        public override bool IsExhausted { get; set; } = false;

        public override bool IsDiscarded { get; set; } = false;

        public override string Text
        {
            get
            {
                return "Navigation: Your Navigation tokens can " +
                       "either change your speed or increase 1 " +
                       "yaw value by 1.";
            }
        }

        public override string Name { get { return "Nav Team"; } }

        public override int PointCost { get { return 4; } }

        public override Texture2D Texture { get; }

        public override Rectangle Source { get; }

        public NavTeam(ContentManager content)
        {

        }
    }
}
