/* File: H9Turbolasers.cs
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
    public class H9Turbolasers : UpgradeCard
    {
        public override bool IsUnique { get; } = false;

        public override bool IsExhausted { get; set; } = false;

        public override bool IsDiscarded { get; set; } = false;

        public override string Text
        {
            get
            {
                return "Modification While attacking,\n" +
                    "you may change 1 die face with a\n" +
                    "Hit or Critical icon to a face with\n" +
                    "an Accuracy icon.";
            }
        }

        public override string Name { get { return "H9 Turbolasers"; } }

        public override int PointCost { get { return 8; } }

        public override Texture2D Texture { get; }

        public override Rectangle Source { get; }

        public override UpgradeTypeEnum CardType { get; }

        /// <summary>
        /// The constructor
        /// </summary>
        /// <param name="content">the content loader</param>
        public H9Turbolasers(ContentManager content)
        {
            CardType = UpgradeTypeEnum.Turbolasers;
        }
    }
}
