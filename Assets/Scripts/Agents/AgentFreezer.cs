using DevLib.ModuleSystem;
using UnityEngine;

namespace Agents
{
    public class AgentFreezer : Module, IAgentFreezer
    {
        private Rigidbody2D _rb;
        private RigidbodyConstraints2D _originalConstraints;
        public bool IsFreeze { get; private set; }

        public override void Initialize(ModuleOwner owner)
        {
            base.Initialize(owner);
            _rb = owner.GetComponent<Rigidbody2D>();
            
            _originalConstraints = _rb.constraints;
        }


        public void Freeze()
        {
            IsFreeze = false;
            _rb.linearVelocity = Vector2.zero;
            _rb.angularVelocity = 0f;
            _rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        public void UnFreeze()
        {
            IsFreeze = true;
            _rb.constraints = _originalConstraints;
        }
    }
}