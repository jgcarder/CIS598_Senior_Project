/* File: PointDefenseReroute.cs
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
    public class PointDefenseReroute : UpgradeCard
    {
        public override bool IsUnique { get; } = false;

        public override bool IsExhausted { get; set; } = false;

        public override bool IsDiscarded { get; set; } = false;

        public override string Text
        {
            get
            {
                return "While attacking a squadron at\n" +
                       "close range, you may reroll\n" +
                       "your Critical icons.";
            }
        }

        public override string Name { get { return "Point-Defense Reroute"; } }

        public override int PointCost { get { return 5; } }

        public override Texture2D Texture { get; }

        public override Rectangle Source { get; }

        public override UpgradeTypeEnum CardType { get; }

        /// <summary>
        /// The constructor the card
        /// </summary>
        /// <param name="content">the content loader</param>
        public PointDefenseReroute(ContentManager content)
        {
            CardType = UpgradeTypeEnum.OffensiveRetrofit;
        }
    }
}
