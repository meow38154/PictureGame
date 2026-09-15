using UnityEngine;

namespace Agents
{
    public interface IVfxModule
    {
        void PlayVfx(int hash, Vector3 position, Quaternion rotation, bool deadParticle = false);
        void PlayVfx(int hash, bool deadParticle = false);
        void StopVfx(int hash, bool deadParticle = false);
    }
}