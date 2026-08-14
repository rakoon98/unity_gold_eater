using UnityEngine;

namespace GoldEater
{
    public abstract class EnemyState
    {
        protected EnemyController controller;
        protected EnemyAttack Attack;

        protected EnemyState(EnemyController controller)
        {
            this.controller = controller;
            this.Attack = controller.attack;
        }

        public virtual void Enter() { }

        public virtual void Update() { }

        public virtual void FixedUpdate() { }

        public virtual void Exit() { }
    }
}