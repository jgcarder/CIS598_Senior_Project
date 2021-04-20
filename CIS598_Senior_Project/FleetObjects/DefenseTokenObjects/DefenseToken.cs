using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace CIS598_Senior_Project.FleetObjects.DefenseTokenObjects
{
    public abstract class DefenseToken
    {
        public abstract int Id { get; }

        public abstract string Name { get; }

        public abstract string Text { get; }

        public abstract Texture2D Texture { get; }

        public abstract Rectangle Source { get; }

        public abstract DefenseTokenStateEnum State { get; }

        public abstract void Use();

        public abstract void Reset();
    }
}
