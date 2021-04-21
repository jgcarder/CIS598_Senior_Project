using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace CIS598_Senior_Project.FleetObjects.UpgradeObjects
{
    public abstract class UpgradeCard
    {

        public abstract bool IsUnique { get; }

        public abstract bool IsExhausted { get; set; }

        public abstract bool IsDiscarded { get; set; }

        public abstract string Text { get; }

        public abstract string Name { get; }

        public abstract int PointCost { get; }

        public abstract Texture2D Texture { get; }

        public abstract Rectangle Source { get; }
    }
}
