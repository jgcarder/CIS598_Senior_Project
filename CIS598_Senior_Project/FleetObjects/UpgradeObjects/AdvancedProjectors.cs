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
                return "When you resolve the Redirect defense " +
                       "token effect, you can choose more than " +
                       "one hull zone to suffer damage, which may " +
                       "include a nonadjacentt hull zone.";
            }
        }

        public override string Name { get { return "Advanced Projectors"; } }

        public override int PointCost { get { return 6; } }

        public override Texture2D Texture { get; }

        public override Rectangle Source { get; }

        public AdvancedProjectors(ContentManager content)
        {

        }
    }
}
