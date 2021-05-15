/* File: TitleDominator.cs
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
    public class TitleDominator : UpgradeCard
    {
        public override bool IsUnique { get; } = true;

        public override bool IsExhausted { get; set; } = false;

        public override bool IsDiscarded { get; set; } = false;

        public override string Text
        {
            get
            {
                return "While attacking at\n" +
                       "close-medium range, you may\n" +
                       "spend up to 2 shields from any\n" +
                       "of your hull zones to add that\n" +
                       "number of blue dice to your\n" +
                       "attack pool.";
            }
        }

        public override string Name { get { return "Dominator"; } }

        public override int PointCost { get { return 12; } }

        public override Texture2D Texture { get; }

        public override Rectangle Source { get; }

        public override UpgradeTypeEnum CardType { get; }

        /// <summary>
        /// The constructor the card
        /// </summary>
        /// <param name="content">the content loader</param>
        public TitleDominator(ContentManager content)
        {
            CardType = UpgradeTypeEnum.Title;
        }
    }
}
