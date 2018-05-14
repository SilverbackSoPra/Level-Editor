using System.Collections.Generic;

namespace LevelEditor.Engine.Mesh
{
    internal sealed class ActorBatch
    {
        public readonly List<Actor> mActors;

        public readonly LevelEditor.Engine.Mesh.Mesh mMesh;

        public ActorBatch(LevelEditor.Engine.Mesh.Mesh mesh)
        {
            mMesh = mesh;
            mActors = new List<Actor>();
        }

        public void Add(Actor actor)
        {
            mActors.Add(actor);
        }

        public bool Remove(Actor actor)
        {
            return mActors.Remove(actor);
        }
    }
}