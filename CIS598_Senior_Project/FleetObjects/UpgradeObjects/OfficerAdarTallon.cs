using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace CIS598_Senior_Project.FleetObjects.UpgradeObjects
{
    public class OfficerAdarTallon : UpgradeCard
    {
        public override bool IsUnique { get; } = true;

        public override bool IsExhausted { get; set; } = false;

        public override bool IsDiscarded { get; set; } = false;

        public override string Text
        {
            get
            {
                return "After you resolve a Squadron\n" +
                       "command, exhaust this card to\n" +
                       "toggle the activation slider\n" +
                       "of one squadron activated with\n" +
                       "that command.";
            }
        }

        public override string Name { get { return "Adar Tallon"; } }

        public override int PointCost { get { return 10; } }

        public override Texture2D Texture { get; }

        public override Rectangle Source { get; }

        public override UpgradeTypeEnum CardType { get; }

        public OfficerAdarTallon(ContentManager content)
        {
            CardType = UpgradeTypeEnum.Officers;
        }
    }
}
