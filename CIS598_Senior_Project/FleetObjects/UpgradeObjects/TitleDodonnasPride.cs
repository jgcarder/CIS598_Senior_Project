﻿/* File: TitleDodonnasPride.cs
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
    public class TitleDodonnasPride : UpgradeCard
    {
        public override bool IsUnique { get; } = true;

        public override bool IsExhausted { get; set; } = false;

        public override bool IsDiscarded { get; set; } = false;

        public override string Text
        {
            get
            {
                return "BLUE Critical: Cancel all\n" +
                       "attack dice to deal one\n" +
                       "faceup damage card to the\n" +
                       "defender.";
            }
        }

        public override string Name { get { return "Dodonna's Pride"; } }

        public override int PointCost { get { return 6; } }

        public override Texture2D Texture { get; }

        public override Rectangle Source { get; }

        public override UpgradeTypeEnum CardType { get; }

        /// <summary>
        /// The constructor the card
        /// </summary>
        /// <param name="content">the content loader</param>
        public TitleDodonnasPride(ContentManager content)
        {
            CardType = UpgradeTypeEnum.Title;
        }
    }
}
