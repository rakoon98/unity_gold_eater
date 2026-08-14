using UnityEngine;

namespace GoldEater
{
    public abstract class PlayerState
    {
        protected PlayerController player;

        protected PlayerState(PlayerController player)
        {
            this.player = player;
        }

        public virtual void Enter() { }
        public virtual void Update() { }
        public virtual void FixedUpdate() { }
        public virtual void Exit() { }
    }
}

