using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace CIS598_Senior_Project.FleetObjects.DefenseTokenObjects
{
    public abstract class DefenseToken
    {
        public int Id { get; }

        public string Name { get; }

        public string Text { get; }

        public Texture2D Texture { get; }

        public DefenseTokenStateEnum State { get; }
    }
}
