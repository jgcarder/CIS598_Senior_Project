﻿/* File: AssaultConcussionMissiles.cs
 * Author: Jackson Carder
 */

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace CIS598_Senior_Project.FleetObjects.UpgradeObjects
{
    public class AssaultConcussionMissiles : UpgradeCard
    {
        public override bool IsUnique { get; } = false;

        public override bool IsExhausted { get; set; } = false;

        public override bool IsDiscarded { get; set; } = false;

        public override string Text
        {
            get
            {
                return "BLACK Critical: Exhaust this\n" +
                       "card. Each hull zone adjacent\n" +
                       "to the defending hull zone\n" +
                       "suffers 1 damage.";
            }
        }

        public override string Name { get { return "Assault Concussion Missiles"; } }

        public override int PointCost { get { return 5; } }

        public override Texture2D Texture { get; }

        public override Rectangle Source { get; }

        public override UpgradeTypeEnum CardType { get; }

        /// <summary>
        /// The constructor
        /// </summary>
        /// <param name="content">the content loader</param>
        public AssaultConcussionMissiles(ContentManager content)
        {
            CardType = UpgradeTypeEnum.Ordinance;
        }
    }
}
