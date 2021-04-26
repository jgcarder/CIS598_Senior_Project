using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace CIS598_Senior_Project.FleetObjects.UpgradeObjects
{
    public class OfficerWeaponsLiaison : UpgradeCard
    {
        public override bool IsUnique { get; } = false;

        public override bool IsExhausted { get; set; } = false;

        public override bool IsDiscarded { get; set; } = false;

        public override string Text
        {
            get
            {
                return "Before you reveal a command,\n" +
                       "you may spend one command\n" +
                       "token to change that command\n" +
                       "to a Concentrate Fire or\n" +
                       "Squadron command.";
            }
        }

        public override string Name { get { return "Weapons Liaison"; } }

        public override int PointCost { get { return 3; } }

        public override Texture2D Texture { get; }

        public override Rectangle Source { get; }

        public override UpgradeTypeEnum CardType { get; }

        public OfficerWeaponsLiaison(ContentManager content)
        {
            CardType = UpgradeTypeEnum.Officers;
        }
    }
}
