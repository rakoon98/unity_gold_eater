namespace GoldEater
{
    using Cysharp.Threading.Tasks;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class PersistentManager : MonoBehaviour
    {
        void Start()
        {
            Debug.Log("[PersistentManager] start 시작");

            SceneLoadManager.instance.Initialize().Forget();

            Debug.Log("[PersistentManager] start 종료");
        }
    }

}