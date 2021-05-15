/* File: OfficerLeiaOrgana.cs
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
    public class OfficerLeiaOrgana : UpgradeCard
    {
        public override bool IsUnique { get; } = true;

        public override bool IsExhausted { get; set; } = false;

        public override bool IsDiscarded { get; set; } = false;

        public override string Text
        {
            get
            {
                return "When you reveal a command,\n" +
                       "you may choose another friendly\n" +
                       "ship at distance 1-5 and change\n" +
                       "that ships top command to your\n" +
                       "revealed command.";
            }
        }

        public override string Name { get { return "Leia Organa"; } }

        public override int PointCost { get { return 3; } }

        public override Texture2D Texture { get; }

        public override Rectangle Source { get; }

        public override UpgradeTypeEnum CardType { get; }

        /// <summary>
        /// The constructor the card
        /// </summary>
        /// <param name="content">the content loader</param>
        public OfficerLeiaOrgana(ContentManager content)
        {
            CardType = UpgradeTypeEnum.Officers;
        }
    }
}
