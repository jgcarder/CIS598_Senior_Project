/* File: CommanderGrandMoffTarkin.cs
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
    public class CommanderGrandMoffTarkin : UpgradeCard
    {
        public override bool IsUnique { get; } = true;

        public override bool IsExhausted { get; set; } = false;

        public override bool IsDiscarded { get; set; } = false;

        public override string Text
        {
            get
            {
                return "At the start of each Ship\n" +
                       "Phase, you may choose one\n" +
                       "command. Each friendly ship\n" +
                       "gains a command token matching\n" +
                       "that command.";
            }
        }

        public override string Name { get { return "Grand Moff Tarkin"; } }

        public override int PointCost { get { return 28; } }

        public override Texture2D Texture { get; }

        public override Rectangle Source { get; }

        public override UpgradeTypeEnum CardType { get; }

        /// <summary>
        /// The constructor
        /// </summary>
        /// <param name="content">the content loader</param>
        public CommanderGrandMoffTarkin(ContentManager content)
        {
            CardType = UpgradeTypeEnum.Commander;
        }
    }
}
