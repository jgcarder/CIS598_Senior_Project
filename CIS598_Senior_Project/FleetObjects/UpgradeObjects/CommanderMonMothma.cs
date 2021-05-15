/* File: CommanderMonMothma.cs
 * Author.Jackson Carder
 */

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace CIS598_Senior_Project.FleetObjects.UpgradeObjects
{
    public class CommanderMonMothma : UpgradeCard
    {
        public override bool IsUnique { get; } = true;

        public override bool IsExhausted { get; set; } = false;

        public override bool IsDiscarded { get; set; } = false;

        public override string Text
        {
            get
            {
                return "When a friendly ship resolves\n" +
                       "the Evade Defense token effect,\n" +
                       "it can cancel 1 die at medium\n" +
                       "range or reroll 1 additional\n" +
                       "die at close range or distance\n" +
                       "1.";
            }
        }

        public override string Name { get { return "Mon Mothma"; } }

        public override int PointCost { get { return 27; } }

        public override Texture2D Texture { get; }

        public override Rectangle Source { get; }

        public override UpgradeTypeEnum CardType { get; }

        /// <summary>
        /// The constructor
        /// </summary>
        /// <param name="content">the content loader</param>
        public CommanderMonMothma(ContentManager content)
        {
            CardType = UpgradeTypeEnum.Commander;
        }
    }
}
