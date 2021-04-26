using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace CIS598_Senior_Project.FleetObjects.UpgradeObjects
{
    public class FlightControllers : UpgradeCard
    {
        public override bool IsUnique { get; } = false;

        public override bool IsExhausted { get; set; } = false;

        public override bool IsDiscarded { get; set; } = false;

        public override string Text
        {
            get
            {
                return "Squadron: The anti-squadron\n" +
                       "armament of each squadron that\n" +
                       "you activate is increased by one\n" +
                       "blue die until the end of its\n" +
                       "activation.";
            }
        }

        public override string Name { get { return "Flight Controllers"; } }

        public override int PointCost { get { return 6; } }

        public override Texture2D Texture { get; }

        public override Rectangle Source { get; }

        public override UpgradeTypeEnum CardType { get; }

        public FlightControllers(ContentManager content)
        {
            CardType = UpgradeTypeEnum.WeaponsTeam;
        }
    }
}
