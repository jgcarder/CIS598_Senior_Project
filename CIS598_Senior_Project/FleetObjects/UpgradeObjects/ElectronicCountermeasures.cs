﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Text;

namespace CIS598_Senior_Project.FleetObjects.UpgradeObjects
{
    public class ElectronicCountermeasures : UpgradeCard
    {
        public override bool IsUnique { get; } = false;

        public override bool IsExhausted { get; set; } = false;

        public override bool IsDiscarded { get; set; } = false;

        public override string Text
        {
            get
            {
                return "NON-RECURRING Right sidebar: 1 Engineering token. " +
                       "While defending, you may exhaust this " +
                       "card to spend 1 defense token that your " +
                       "opponent targeted with an  result.";
            }
        }

        public override string Name { get { return "Electronic Countermeasures"; } }

        public override int PointCost { get { return 7; } }

        public override Texture2D Texture { get; }

        public override Rectangle Source { get; }

        public ElectronicCountermeasures(ContentManager content)
        {

        }
    }
}
