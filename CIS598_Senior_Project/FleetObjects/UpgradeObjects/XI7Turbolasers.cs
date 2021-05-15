/* File: XI7Turbolasers.cs
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
    public class XI7Turbolasers : UpgradeCard
    {
        public override bool IsUnique { get; } = false;

        public override bool IsExhausted { get; set; } = false;

        public override bool IsDiscarded { get; set; } = false;

        public override string Text
        {
            get
            {
                return "While attacking, if the\n" +
                       "defender spends a Redirect\n" +
                       "Defense token, it cannot suffer\n" +
                       "more than 1 damage on each hull\n" +
                       "zone other than the defending\n" +
                       "hull zone when it resolves the\n" +
                       "Redirect Defense effect.";
            }
        }

        public override string Name { get { return "XI7 Turbolasers"; } }

        public override int PointCost { get { return 6; } }

        public override Texture2D Texture { get; }

        public override Rectangle Source { get; }

        public override UpgradeTypeEnum CardType { get; }

        /// <summary>
        /// The constructor the card
        /// </summary>
        /// <param name="content">the content loader</param>
        public XI7Turbolasers(ContentManager content)
        {
            CardType = UpgradeTypeEnum.Turbolasers;
        }
    }
}
