using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace CIS598_Senior_Project.FleetObjects.UpgradeObjects
{
    public class CommanderGeneralDodonna : UpgradeCard
    {
        public override bool IsUnique { get; } = true;

        public override bool IsExhausted { get; set; } = false;

        public override bool IsDiscarded { get; set; } = false;

        public override string Text
        {
            get
            {
                return "Before an enemy ship is dealt a faceup " +
                       "damage card, look at the top four cards " +
                       "of the damage deck, place one on top of " +
                       "the deck and discard the others.";
            }
        }

        public override string Name { get { return "General Dodonna"; } }

        public override int PointCost { get { return 20; } }

        public override Texture2D Texture { get; }

        public override Rectangle Source { get; }

        public override UpgradeTypeEnum CardType { get; }

        public CommanderGeneralDodonna(ContentManager content)
        {
            CardType = UpgradeTypeEnum.Commander;
        }
    }
}
