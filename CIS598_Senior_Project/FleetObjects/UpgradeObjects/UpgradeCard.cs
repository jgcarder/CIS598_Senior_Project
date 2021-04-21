using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace CIS598_Senior_Project.FleetObjects.UpgradeObjects
{
    public class UpgradeCard
    {

        private bool _isUnique;
        private string _text;
        private int _pointCost;
        private Texture2D _texture;
        private int _id;
        private string _name;
        private Rectangle _source;

        public bool IsUnique
        {
            get { return _isUnique; }
        }

        public string Text
        {
            get { return _text; }
        }

        public int PointCost
        {
            get { return _pointCost; }
        }

        public Texture2D Texture
        {
            get { return _texture; }
        }

        public int Id { get { return _id; } }

        public string Name { get { return _name; } }

        public Rectangle Source { get { return _source; } }

        public UpgradeCard(int id, int points, string name, string text, Texture2D texture, Rectangle source, bool uniq)
        {

        }

    }
}
