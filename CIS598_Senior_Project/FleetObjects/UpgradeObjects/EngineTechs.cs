using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace CIS598_Senior_Project.FleetObjects.UpgradeObjects
{
    public class EngineTechs : UpgradeCard
    {
        public override bool IsUnique { get; } = false;

        public override bool IsExhausted { get; set; } = false;

        public override bool IsDiscarded { get; set; } = false;

        public override string Text
        {
            get
            {
                return "Navication: After you execute\n" +
                       "a maneuver, you may exhaust\n" +
                       "this card to execute a 1-speed\n" +
                       "maneuver.";
            }
        }

        public override string Name { get { return "Engine Techs"; } }

        public override int PointCost { get { return 8; } }

        public override Texture2D Texture { get; }

        public override Rectangle Source { get; }

        public override UpgradeTypeEnum CardType { get; }

        public EngineTechs(ContentManager content)
        {
            CardType = UpgradeTypeEnum.SupportTeam;
        }
    }
}
