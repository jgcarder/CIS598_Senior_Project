using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CIS598_Senior_Project.MenuObjects
{
    public class CustButton
    {
        public int Id { get; set; }

        public Texture2D Texture { get; set; }

        public Color Color { get; set; }

        public Rectangle Area { get; set; }

        public Vector2 Position { get; set; }

        public bool IsActive { get; set; }

        public Action<object, ButtonClickedEventArgs> AnAction { get; set; } = null;

        public CustButton(int id, Rectangle area, bool initialActivity)
        {
            IsActive = initialActivity;
            Id = id;
            Position = new Vector2(area.X, area.Y);
            Area = area;
            Color = Color.White;
        }

        public CustButton(int id, Rectangle area, bool initialActivity, Texture2D image)
        {
            IsActive = initialActivity;
            Id = id;
            Position = new Vector2(area.X, area.Y);
            Area = area;
            Color = Color.White;
            this.Texture = image;
        }
    }
}
