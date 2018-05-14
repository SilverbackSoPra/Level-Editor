using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using LevelEditor.Engine.Mesh;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace LevelEditor
{
    internal class Terrain
    {

        private const float TerrainScale = 1.0f;
        private const float TerrainMaxHeight = 50.0f;
        private const float TextureRepitions = 1.0f;

        private readonly int mHeight;
        private readonly int mWidth;

        public readonly Mesh mMesh;

        public Terrain(ContentManager content, string heightMapPath, string texturePath, GraphicsDevice device)
        {

            var meshData = new MeshData();

            var heightmap = content.Load<Texture2D>(heightMapPath);

            mWidth = heightmap.Width;
            mHeight = heightmap.Height;

            meshData.mTexture = content.Load<Texture2D>(texturePath);
            meshData.mPrimitiveType = PrimitiveType.TriangleStrip;
            meshData.mPrimitiveCount = (heightmap.Width * 2 + 2) * (heightmap.Height - 1);
            meshData.mRadius = (float)Math.Sqrt(Math.Pow(heightmap.Width /  2.0f, 2) + Math.Pow(heightmap.Height / 2.0f, 2));

            meshData.mVertices = new VertexPositionTexture[mWidth * mHeight];
            meshData.mIndices = new int[(mWidth * 2 + 2) * (mHeight - 1)];
            
            var heightVal = new Color[mWidth * mHeight];
            heightmap.GetData(heightVal);

            for (int z = 0; z < mHeight; z++)
            {
                for (int x = 0; x < mWidth; x++)
                {
                    //Position
                    meshData.mVertices[x + z * mWidth].Position.X = (float)(x - mWidth / 2.0f) * TerrainScale;
                    meshData.mVertices[x + z * mWidth].Position.Y = (float)heightVal[x + z * mWidth].G / 255.0f * TerrainMaxHeight;
                    meshData.mVertices[x + z * mWidth].Position.Z = (float)(z - mHeight / 2.0f) * TerrainScale;

                    //Texture
                    meshData.mVertices[x + z * mWidth].TextureCoordinate.X = 1.0f - ((float)x / mWidth * TextureRepitions);
                    meshData.mVertices[x + z * mWidth].TextureCoordinate.Y = 1.0f - ((float)z / mHeight * TextureRepitions);
                }
            }

            /*
            We don't need normals anymore. They are calculated in the shader.
            for (int z = 0; z < height; z++)
            {
                for (int x = 0; x < width; x++)
                {
                    //Normal vectors
                    var position = z * width + x;

                    var heightL = GetHeightFromData(meshData.mVertices, x - 1, z, width, height);
                    var heightR = GetHeightFromData(meshData.mVertices, x + 1, z, width, height);
                    var heightD = GetHeightFromData(meshData.mVertices, x, z - 1, width, height);
                    var heightU = GetHeightFromData(meshData.mVertices, x, z + 1, width, height);

                    // meshData.mVertices[position].Normal = Vector3.Normalize(new Vector3(heightL - heightR, 1.0f, heightD - heightU));

                }
            }
            */

            //Calculates the triangle strip
            var i = 0;

            for (int z = 0; z < mHeight - 1; z++)
            {

                meshData.mIndices[i++] = (z + 1) * mWidth;

                for (int x = 0; x < mWidth; x++)
                {
                    meshData.mIndices[i++] = (z + 1) * mWidth + x;
                    meshData.mIndices[i++] = z * mWidth + x;
                }

                meshData.mIndices[i++] = z * mWidth + (mWidth - 1);


            }

            mMesh = new Mesh(device, meshData);
        }

        private float GetHeightFromData(VertexPositionTexture[] vertices, int x, int z)
        {

            if (x < mWidth && z < mHeight && z >= 0 && x >= 0)
            {
                return vertices[z * mWidth + x].Position.Y;
            }
            else
            {
                return vertices[0].Position.Y;
            }

        }

        static float BarryCentric(Vector3 p1, Vector3 p2, Vector3 p3, Vector2 pos)
        {
            var det = (p2.Z - p3.Z) * (p1.X - p3.X) + (p3.X - p2.X) * (p1.Z - p3.Z);
            var l1 = ((p2.Z - p3.Z) * (pos.X - p3.X) + (p3.X - p2.X) * (pos.Y - p3.Z)) / det;
            var l2 = ((p3.Z - p1.Z) * (pos.X - p3.X) + (p1.X - p3.X) * (pos.Y - p3.Z)) / det;
            var l3 = 1.0f - l1 - l2;
            return l1 * p1.Y + l2 * p2.Y + l3 * p3.Y;
        }

        public float GetHeight(Vector3 location)
        {
            var height = 0.0f;


            //glm::vec3 translation = glm::vec3(actor->matrix[3].x - imageWidth / 2.0f, 0.0f, actor->matrix[3].z - imageHeight / 2.0f) * scale;
            //x -= translation.x;
            //z -= translation.z;

            
            var x = 0f;
            var y = 0f;
            var z = 0f;

            if (x < -(mWidth / 2.0f) * TerrainScale || z < -(mHeight / 2.0f) * TerrainScale || x > (mWidth / 2.0f) || z > (mHeight / 2.0f))
            {
                return 0.0f;
            }

            var position = new Vector2((float)Math.Floor(x / TerrainScale), (float)Math.Floor(z / TerrainScale));

            if (position.X < -(mWidth / 2.0f) * TerrainScale || position.Y < -(mHeight / 2.0f) * TerrainScale ||
                position.X >= (mWidth / 2.0f) * TerrainScale || position.Y >= (mHeight / 2.0f) * TerrainScale)
            {
                return 0.0f;
            }

            var coord = new Vector2((x % TerrainScale) / TerrainScale, (z % TerrainScale) / TerrainScale);

            if (coord.X > coord.Y)
            {
                height = BarryCentric(new Vector3(0.0f,
                        mMesh.mMeshData.mVertices[(int)(position.X * mWidth * position.Y)].Position.Y,
                        0.0f),
                    new Vector3(1.0f, mMesh.mMeshData.mVertices[(int)(position.X + mWidth * position.Y)].Position.Y, 0.0f),
                    new Vector3(1.0f, mMesh.mMeshData.mVertices[(int)(position.X + mWidth * position.Y)].Position.Y, 1.0f),
                    coord);
            }
            else
            {
                height = BarryCentric(new Vector3(0.0f, mMesh.mMeshData.mVertices[(int)(position.X + mWidth * position.Y)].Position.Y, 0.0f),
                    new Vector3(1.0f, mMesh.mMeshData.mVertices[(int)(position.X + mWidth * position.Y)].Position.Y, 1.0f),
                    new Vector3(0.0f, mMesh.mMeshData.mVertices[(int)(position.X + mWidth * position.Y)].Position.Y, 1.0f),
                    coord);
            }

            return height;
        }


    }
}
