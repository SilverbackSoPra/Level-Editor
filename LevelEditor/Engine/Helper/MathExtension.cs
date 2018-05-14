using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace LevelEditor.Engine.Helper
{
    class MathExtension
    {

        public static Vector2 Mix(Vector2 x, Vector2 y, float factor)
        {
            return x + factor * (y - x);
        }

    }
}
