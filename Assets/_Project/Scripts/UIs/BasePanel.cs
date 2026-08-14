using UnityEngine;

namespace GoldEater
{
    public abstract class BasePanel : MonoBehaviour
    {
        public bool IsOpen => gameObject.activeSelf;

        public virtual void Open()
        {
            gameObject.SetActive(true);
        }

        public virtual void Close()
        {
            gameObject.SetActive(false);
        }

        public virtual void Toggle()
        {
            if (IsOpen)
                Close();
            else
                Open();
        }
    }
}