using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace CIS598_Senior_Project.FleetObjects.UpgradeObjects
{
    public class OfficerWullfYularen : UpgradeCard
    {
        public override bool IsUnique { get; } = true;

        public override bool IsExhausted { get; set; } = false;

        public override bool IsDiscarded { get; set; } = false;

        public override string Text
        {
            get
            {
                return "When you spend a command token,\n" +
                       "you may exhaust this card to\n" +
                       "gain one command token of the\n" +
                       "same type.";
            }
        }

        public override string Name { get { return "Wullf Yularen"; } }

        public override int PointCost { get { return 7; } }

        public override Texture2D Texture { get; }

        public override Rectangle Source { get; }

        public override UpgradeTypeEnum CardType { get; }

        public OfficerWullfYularen(ContentManager content)
        {
            CardType = UpgradeTypeEnum.Officers;
        }
    }
}
