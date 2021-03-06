/* File: TitleGallantHaven.cs
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
    public class TitleGallantHaven : UpgradeCard
    {
        public override bool IsUnique { get; } = true;

        public override bool IsExhausted { get; set; } = false;

        public override bool IsDiscarded { get; set; } = false;

        public override string Text
        {
            get
            {
                return "Before a friendly squadron at\n" +
                       "distance 1 suffers damage\n" +
                       "from an attack, reduce the\n" +
                       "total damage by one.";
            }
        }

        public override string Name { get { return "Gallant Haven"; } }

        public override int PointCost { get { return 8; } }

        public override Texture2D Texture { get; }

        public override Rectangle Source { get; }

        public override UpgradeTypeEnum CardType { get; }

        /// <summary>
        /// The constructor the card
        /// </summary>
        /// <param name="content">the content loader</param>
        public TitleGallantHaven(ContentManager content)
        {
            CardType = UpgradeTypeEnum.Title;
        }
    }
}
