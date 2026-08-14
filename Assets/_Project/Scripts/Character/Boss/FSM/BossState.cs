namespace GoldEater
{
    public abstract class BossState
    {
        protected BossController controller;

        public BossState(BossController controller)
        {
            this.controller = controller;
        }

        public virtual void Enter() { }
        public virtual void Update() { }
        public virtual void Exit() { }        
    }
}