/* File: TitleDemolisher.cs
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
    public class TitleDemolisher : UpgradeCard
    {
        public override bool IsUnique { get; } = true;

        public override bool IsExhausted { get; set; } = false;

        public override bool IsDiscarded { get; set; } = false;

        public override string Text
        {
            get
            {
                return "During your Attack step, you\n" +
                       "can perform only 1 attack.\n" +
                       "You can perform 1 of your\n" +
                       "attacks after you execute\n" +
                       "your first maneuver during\n" +
                       "your activation.";
            }
        }

        public override string Name { get { return "Demolisher"; } }

        public override int PointCost { get { return 10; } }

        public override Texture2D Texture { get; }

        public override Rectangle Source { get; }

        public override UpgradeTypeEnum CardType { get; }

        /// <summary>
        /// The constructor the card
        /// </summary>
        /// <param name="content">the content loader</param>
        public TitleDemolisher(ContentManager content)
        {
            CardType = UpgradeTypeEnum.Title;
        }
    }
}
