using System.Collections.Generic;
using System.Linq;
using CoreSystem.Effect;
using DevLib.ModuleSystem;
using UnityEngine;

namespace Agents
{
    public class VfxModule : Module, IVfxModule
    {
        private Dictionary<int, IPlayableVFX> _playableDict;

        public override void Initialize(ModuleOwner owner)
        {
            _owner = owner;

            _playableDict = GetComponentsInChildren<IPlayableVFX>()
                .ToDictionary(vfx => vfx.VfxName.AssetHash);
        }

        public void PlayVfx(
            int hash,
            Vector3 position,
            Quaternion rotation,
            bool deadParticle = false)
        {
            if (!_playableDict.TryGetValue(hash, out var vfx))
            {
                Debug.LogWarning($"VFX with hash : {hash} not found in {gameObject.name}");
                return;
            }

            if (deadParticle)
            {
                PlayDetachedVfx(vfx, position, rotation);
                return;
            }

            vfx.PlayVFX(position, rotation);
        }

        public void PlayVfx(int hash, bool deadParticle = false)
        {
            if (!_playableDict.TryGetValue(hash, out var vfx))
            {
                Debug.LogWarning($"VFX with hash : {hash} not found in {gameObject.name}");
                return;
            }

            if (deadParticle)
            {
                if (vfx is not Component component)
                    return;

                PlayDetachedVfx(
                    vfx,
                    component.transform.position,
                    component.transform.rotation);

                return;
            }

            vfx.PlayVFX();
        }

        public void StopVfx(int hash, bool deadParticle = false)
        {
            if (_playableDict.TryGetValue(hash, out var vfx))
            {
                vfx.StopVFX();
            }
        }

        private void PlayDetachedVfx(
            IPlayableVFX source,
            Vector3 position,
            Quaternion rotation)
        {
            if (source is not Component sourceComponent)
            {
                Debug.LogWarning("IPlayableVFX must be implemented by a Component.");
                return;
            }

            GameObject instance = Instantiate(
                sourceComponent.gameObject,
                position,
                rotation);

            IPlayableVFX detachedVfx = instance.GetComponent<IPlayableVFX>();

            if (detachedVfx == null)
            {
                Destroy(instance);
                return;
            }

            instance.AddComponent<DetachedVfxLifetime>();

            detachedVfx.PlayVFX(position, rotation);
        }
    }
}