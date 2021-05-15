/* File: CommanderGarmBelIblis.cs
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
    public class CommanderGarmBelIblis : UpgradeCard
    {
        public override bool IsUnique { get; } = true;

        public override bool IsExhausted { get; set; } = false;

        public override bool IsDiscarded { get; set; } = false;

        public override string Text
        {
            get
            {
                return "After deploying fleets, place\n" +
                       "2 non-consecutive round tokens\n" +
                       "on this card. At the start of\n" +
                       "the Ship Phase during each \n" +
                       "round matching 1 of those\n" +
                       "tokens, each friendly ship may\n" +
                       "gain a number of command\n" +
                       "tokens equal to its command\n" +
                       "value.";
            }
        }

        public override string Name { get { return "Garm Bel Iblis"; } }

        public override int PointCost { get { return 25; } }

        public override Texture2D Texture { get; }

        public override Rectangle Source { get; }

        public override UpgradeTypeEnum CardType { get; }

        /// <summary>
        /// The constructor
        /// </summary>
        /// <param name="content">the content loader</param>
        public CommanderGarmBelIblis(ContentManager content)
        {
            CardType = UpgradeTypeEnum.Commander;
        }
    }
}
