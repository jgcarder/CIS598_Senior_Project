/* File: OfficerRaymusAntilles.cs
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
    public class OfficerRaymusAntilles : UpgradeCard
    {
        public override bool IsUnique { get; } = true;

        public override bool IsExhausted { get; set; } = false;

        public override bool IsDiscarded { get; set; } = false;

        public override string Text
        {
            get
            {
                return "When you reveal a command,\n" +
                       "you may gain one matching\n" +
                       "command token without spending\n" +
                       "the command dial.";
            }
        }

        public override string Name { get { return "Raymus Antilles"; } }

        public override int PointCost { get { return 7; } }

        public override Texture2D Texture { get; }

        public override Rectangle Source { get; }

        public override UpgradeTypeEnum CardType { get; }

        /// <summary>
        /// The constructor the card
        /// </summary>
        /// <param name="content">the content loader</param>
        public OfficerRaymusAntilles(ContentManager content)
        {
            CardType = UpgradeTypeEnum.Officers;
        }
    }
}
