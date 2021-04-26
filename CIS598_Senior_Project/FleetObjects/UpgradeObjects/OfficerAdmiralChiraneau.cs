using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace CIS598_Senior_Project.FleetObjects.UpgradeObjects
{
    public class OfficerAdmiralChiraneau : UpgradeCard
    {
        public override bool IsUnique { get; } = true;

        public override bool IsExhausted { get; set; } = false;

        public override bool IsDiscarded { get; set; } = false;

        public override string Text
        {
            get
            {
                return "Squadron Token: Squadrons that\n" +
                       "you activate can move even if\n" +
                       "they are engaged. When an\n" +
                       "engaged squadron moves in this\n" +
                       "way, treat it as having a \n" +
                       "printed speed of '2'.";
            }
        }

        public override string Name { get { return "Admiral Chiraneau"; } }

        public override int PointCost { get { return 10; } }

        public override Texture2D Texture { get; }

        public override Rectangle Source { get; }

        public override UpgradeTypeEnum CardType { get; }

        public OfficerAdmiralChiraneau(ContentManager content)
        {
            CardType = UpgradeTypeEnum.Officers;
        }
    }
}
