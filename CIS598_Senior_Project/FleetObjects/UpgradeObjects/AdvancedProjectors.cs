/* File: AdvancedProjectors.cs
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
    public class AdvancedProjectors : UpgradeCard
    {
        public override bool IsUnique { get; } = false;

        public override bool IsExhausted { get; set; } = false;

        public override bool IsDiscarded { get; set; } = false;

        public override string Text
        {
            get
            {
                return "When you resolve the Redirect\n" +
                       "token effect, you can choose\n" +
                       "more than one hull zone to\n" +
                       "suffer damage, which may\n" +
                       "include a nonadjacent hull zone.";
            }
        }

        public override string Name { get { return "Advanced Projectors"; } }

        public override int PointCost { get { return 6; } }

        public override Texture2D Texture { get; }

        public override Rectangle Source { get; }

        public override UpgradeTypeEnum CardType { get; }

        /// <summary>
        /// The constructor
        /// </summary>
        /// <param name="content">the content loader</param>
        public AdvancedProjectors(ContentManager content)
        {
            CardType = UpgradeTypeEnum.DefensiveRetrofit;
        }
    }
}
