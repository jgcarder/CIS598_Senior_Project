/* File: BoundingCircle.cs
 * Author: Jackson Carder
 */

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace CIS598_Senior_Project.Collisions
{
    public struct BoundingCircle
    {
        /// <summary>
        /// The center of the Circle
        /// </summary>
        public Vector2 Center;

        /// <summary>
        /// Circle radius
        /// </summary>
        public float Radius;

        /// <summary>
        /// Constructs a new bounding circle
        /// </summary>
        /// <param name="center">The center</param>
        /// <param name="radius">the radius</param>
        public BoundingCircle(Vector2 center, float radius)
        {
            Center = center;
            Radius = radius;
        }

        /// <summary>
        /// Checks for collisions with this and another
        /// </summary>
        /// <param name="other">The other bounding circle to be checked</param>
        /// <returns>true if it has a collision</returns>
        public bool CollidesWith(BoundingCircle other)
        {
            return CollisionHelper.Collides(this, other);
        }

        /// <summary>
        /// Checks for collisions
        /// </summary>
        /// <param name="other">The other bounding rext</param>
        /// <returns>True if there is a collision</returns>
        public bool CollidesWith(BoundingRectangle other)
        {
            return CollisionHelper.Collides(this, other);
        }
    }
}
