using UnityEngine;

namespace CoreSystem.Effect
{
    [RequireComponent(typeof(ParticleSystem))]
    public class ParticleTargetPosMoving : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float destroyDistance = 0.1f;

        private ParticleSystem _particleSystem;
        private ParticleSystem.Particle[] _particles;

        private void Awake()
        {
            _particleSystem = GetComponent<ParticleSystem>();
            _particles = new ParticleSystem.Particle[_particleSystem.main.maxParticles];
        }

        private void LateUpdate()
        {
            if (target == null) return;

            int count = _particleSystem.GetParticles(_particles);

            for (int i = 0; i < count; i++)
            {
                _particles[i].position = Vector3.MoveTowards(_particles[i].position, target.position, moveSpeed * Time.deltaTime);

                if (Vector3.Distance(_particles[i].position, target.position) <= destroyDistance) _particles[i].remainingLifetime = 0f;
            }

            _particleSystem.SetParticles(_particles, count);
        }
    }
}