using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace CIS598_Senior_Project.MenuObjects
{
    public class Button
    {
        public int Id { get; set; }

        public Rectangle TouchArea { get; set; }

        public Vector2 Position { get; set; }

        public Action<object, ButtonClickedEventArgs> AnAction { get; set; } = null;

        public Button(int id, Vector2 position)
        {
            Id = id;
            Position = position;
            TouchArea = new Rectangle((int)position.X, (int)position.Y, 0, 0);
        }
    }
}
