﻿/* File: TitleRedemption.cs
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
    public class TitleRedemption : UpgradeCard
    {
        public override bool IsUnique { get; } = true;

        public override bool IsExhausted { get; set; } = false;

        public override bool IsDiscarded { get; set; } = false;

        public override string Text
        {
            get
            {
                return "When a friendly ship at\n" +
                       "distance 1-5 resoves an\n" +
                       "Engineering command, it gains\n" +
                       "one additional engineering point.";
            }
        }

        public override string Name { get { return "Redemption"; } }

        public override int PointCost { get { return 8; } }

        public override Texture2D Texture { get; }

        public override Rectangle Source { get; }

        public override UpgradeTypeEnum CardType { get; }

        /// <summary>
        /// The constructor the card
        /// </summary>
        /// <param name="content">the content loader</param>
        public TitleRedemption(ContentManager content)
        {
            CardType = UpgradeTypeEnum.Title;
        }
    }
}
