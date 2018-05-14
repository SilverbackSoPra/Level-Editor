using Microsoft.Xna.Framework.Graphics;


namespace LevelEditor.Engine
{
    /// <summary>
    /// 
    /// </summary>
    class RenderTarget
    {

        public RenderTarget2D mMainRenderTarget;
        private Texture2D mShadowMap;

        /// <summary>
        /// Constructs a <see cref="RenderTarget"/>.
        /// </summary>
        /// <param name="device"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="shadowMapResolution"></param>
        public RenderTarget(GraphicsDevice device, int width, int height, int shadowMapResolution)
        {
            mMainRenderTarget = new RenderTarget2D(device, width, height, true, SurfaceFormat.HalfVector4, DepthFormat.Depth24);
        }

    }
}
