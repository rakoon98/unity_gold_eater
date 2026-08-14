using UnityEngine;

public abstract class BaseSingletonManager<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T instance { get; private set; }

    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}