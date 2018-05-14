using Microsoft.Xna.Framework.Graphics;

namespace LevelEditor.Engine.Mesh
{
    internal sealed class MeshData
    {

        public VertexPositionTexture[] mVertices;
        public int[] mIndices;

        public Texture2D mTexture;

        public PrimitiveType mPrimitiveType;
        public int mPrimitiveCount;

        public float mRadius;

        public MeshData()
        {

            mVertices = null;
            mIndices = null;
            mTexture = null;

            mPrimitiveType = PrimitiveType.TriangleList;
            mPrimitiveCount = 0;

            mRadius = 0.0f;
        }

    }

}
