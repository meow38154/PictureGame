using Agents.FSM;
using Agents.Players.Enum;
using Agents.Players.GrabSystem;
using UnityEngine;

namespace Agents.Players
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : Agent
    {
        [field: SerializeField] public PlayerInputSO PlayerInput { get; private set; }
        [SerializeField] private StateListSo playerStates;
        private StateMachine _stateMachine;
        
        private IGrabController _grabController;
        
        protected override void Awake()
        {
            base.Awake();
            _grabController = GetModule<IGrabController>();
            _stateMachine = new StateMachine(this, playerStates.states);
            _stateMachine.ChangeState((int)PlayerStateEnum.IDLE);
        }

        private void Update()
        {
            _stateMachine.UpdateMachine();
        }
        
        public void ChangeState(PlayerStateEnum newState, float transitionDuration)
            => _stateMachine.ChangeState((int)newState, transitionDuration);

        protected override object CaptureCustomData()
        {
            return _grabController.CurrentGrabbingObject;
        }

        protected override void RestoreCustomData(object data)
        {
            base.RestoreCustomData(data);
            
            if (data is not GameObject saveData) return;
            
            _grabController.CurrentGrabbingObject = saveData;
        }
    }
}