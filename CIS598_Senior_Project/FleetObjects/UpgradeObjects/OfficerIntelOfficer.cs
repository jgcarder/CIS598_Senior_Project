﻿/* File: OfficerIntelOfficer.cs
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
    public class OfficerIntelOfficer : UpgradeCard
    {
        public override bool IsUnique { get; } = false;

        public override bool IsExhausted { get; set; } = false;

        public override bool IsDiscarded { get; set; } = false;

        public override string Text
        {
            get
            {
                return "While attacking, after you\n" +
                       "roll your attack pool, you may\n" +
                       "exhaust this card to choose 1\n" +
                       "defense token. If that token is\n" +
                       "spent during this attack, \n" +
                       "discard that token";
            }
        }

        public override string Name { get { return "Intel Officer"; } }

        public override int PointCost { get { return 7; } }

        public override Texture2D Texture { get; }

        public override Rectangle Source { get; }

        public override UpgradeTypeEnum CardType { get; }

        /// <summary>
        /// The constructor
        /// </summary>
        /// <param name="content">the content loader</param>
        public OfficerIntelOfficer(ContentManager content)
        {
            CardType = UpgradeTypeEnum.Officers;
        }
    }
}
