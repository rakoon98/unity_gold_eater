namespace GoldEater
{
    using Cysharp.Threading.Tasks;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.SceneManagement;

    public class IntroManager : MonoBehaviour
    {

        AnyKeyDetector anyKeyDetector;

        private void Awake()
        {
            anyKeyDetector = GetComponent<AnyKeyDetector>();
        }

        private void Start()
        {
            anyKeyDetector.onAnyKeyAction += AnyKeyCheck;
        }

        private void OnDestroy()
        {
            anyKeyDetector.onAnyKeyAction -= AnyKeyCheck;
        }

        private void AnyKeyCheck() => SceneLoadManager.instance.ChangeScene(SceneNames.Title).Forget();

    }

}