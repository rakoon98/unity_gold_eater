using UnityEngine;

namespace GoldEater
{
    public class EnemyStateMachine
    {
        public EnemyState CurrentState { get; private set; }

        public void ChangeState(EnemyState newState)
        {
            CurrentState?.Exit();

            CurrentState = newState;

            CurrentState.Enter();
        }

        public void Update()
        {
            CurrentState?.Update();
        }

        public void FixedUpdate()
        {
            CurrentState?.FixedUpdate();
        }
    }
}