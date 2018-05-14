﻿using Microsoft.Xna.Framework;

namespace LevelEditor.Engine.Mesh
{
    /// <summary>
    /// An actor represents an instance of a mesh in a scene.
    /// </summary>
    internal sealed class Actor
    {

        public readonly LevelEditor.Engine.Mesh.Mesh mMesh;
        public Matrix mModelMatrix;

        public bool mRender;

        /// <summary>
        /// Constructs an <see cref="Actor"/>.
        /// </summary>
        /// <param name="mesh"></param>
        public Actor(LevelEditor.Engine.Mesh.Mesh mesh)
        {

            mMesh = mesh;
            mModelMatrix = Matrix.Identity;

            // Actor should be rendered
            mRender = true;

        }
    }
}
