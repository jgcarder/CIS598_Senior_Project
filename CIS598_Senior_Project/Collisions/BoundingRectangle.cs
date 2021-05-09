/* File: BoundingRectangle.cs
 * Author: Jackson Carder
 */

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace CIS598_Senior_Project.Collisions
{
    public struct BoundingRectangle
    {

        /// <summary>
        /// The rect's starting X coord
        /// </summary>
        public float X;

        /// <summary>
        /// The rect's starting Y coord
        /// </summary>
        public float Y;

        /// <summary>
        /// The rect's width
        /// </summary>
        public float Width;

        /// <summary>
        /// The rect's height
        /// </summary>
        public float Height;

        /// <summary>
        /// The left side of the rect
        /// </summary>
        public float Left => X;

        /// <summary>
        /// The right side of the rect
        /// </summary>
        public float Right => X + Width;

        /// <summary>
        /// The top of the rect
        /// </summary>
        public float Top => Y;

        /// <summary>
        /// The bottom of the rect
        /// </summary>
        public float Bottom => Y + Height;

        /// <summary>
        /// A constructor for a Bounding Rectangle
        /// </summary>
        /// <param name="x">The X coord</param>
        /// <param name="y">The Y coord</param>
        /// <param name="width">The width of the rect</param>
        /// <param name="height">The height of the rect</param>
        public BoundingRectangle(float x, float y, float width, float height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        /// <summary>
        /// A constructor for a Bounding Rectangle
        /// </summary>
        /// <param name="position">The starting position vector</param>
        /// <param name="width">The width of the rect</param>
        /// <param name="height">The height of the rect</param>
        public BoundingRectangle(Vector2 position, float width, float height)
        {
            X = position.X;
            Y = position.Y;
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Invokes the collision to see if the have collided
        /// </summary>
        /// <param name="other">The other Bounding rect to test</param>
        /// <returns>True is the collide</returns>
        public bool CollidesWith(BoundingRectangle other)
        {
            return CollisionHelper.Collides(this, other);
        }

        /// <summary>
        /// Checks for collisions with this and another
        /// </summary>
        /// <param name="other">The other bounding circle to be checked</param>
        /// <returns>true if it has a collision</returns>
        public bool CollidesWith(BoundingCircle other)
        {
            return CollisionHelper.Collides(other, this);
        }

    }
}
