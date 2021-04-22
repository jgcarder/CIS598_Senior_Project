﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace CIS598_Senior_Project.FleetObjects.UpgradeObjects
{
    public class SensorTeam : UpgradeCard
    {
        public override bool IsUnique { get; } = false;

        public override bool IsExhausted { get; set; } = false;

        public override bool IsDiscarded { get; set; } = false;

        public override string Text
        {
            get
            {
                return "While attacking, you may exhaust this " +
                       "card and spend one die to change one " +
                       "of your dice to a face with an Accuracy icon.";
            }
        }

        public override string Name { get { return "Sensor Team"; } }

        public override int PointCost { get { return 5; } }

        public override Texture2D Texture { get; }

        public override Rectangle Source { get; }

        public override UpgradeTypeEnum CardType { get; }

        public SensorTeam(ContentManager content)
        {
            CardType = UpgradeTypeEnum.WeaponsTeam;
        }
    }
}
