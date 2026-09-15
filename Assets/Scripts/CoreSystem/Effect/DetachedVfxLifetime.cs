using UnityEngine;

namespace CoreSystem.Effect
{
    public class DetachedVfxLifetime : MonoBehaviour
    {
        private ParticleSystem[] _particles;
        private bool _started;

        private void Awake()
        {
            _particles = GetComponentsInChildren<ParticleSystem>(true);
        }

        private void Update()
        {
            if (_particles.Length == 0)
            {
                Destroy(gameObject);
                return;
            }

            foreach (ParticleSystem particle in _particles)
            {
                if (particle != null && particle.IsAlive(true))
                {
                    _started = true;
                    return;
                }
            }

            if (_started)
            {
                Destroy(gameObject);
            }
        }
    }
}