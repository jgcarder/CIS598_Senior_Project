/* File: ExpandedLaunchers.cs
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
    public class ExpandedLaunchers : UpgradeCard
    {
        public override bool IsUnique { get; } = false;

        public override bool IsExhausted { get; set; } = false;

        public override bool IsDiscarded { get; set; } = false;

        public override string Text
        {
            get
            {
                return "Modification. The battery\n" +
                       "armament for your front hull\n" +
                       "zone is increased by 2 black\n" +
                       "dice.";
            }
        }

        public override string Name { get { return "Expanded Launchers"; } }

        public override int PointCost { get { return 13; } }

        public override Texture2D Texture { get; }

        public override Rectangle Source { get; }

        public override UpgradeTypeEnum CardType { get; }

        /// <summary>
        /// The constructor
        /// </summary>
        /// <param name="content">the content loader</param>
        public ExpandedLaunchers(ContentManager content)
        {
            CardType = UpgradeTypeEnum.Ordinance;
        }
    }
}
