namespace GoldEater
{
    using Cysharp.Threading.Tasks;
    using UnityEngine;
    using UnityEngine.UI;

    public class StageCard : MonoBehaviour
    {
        public bool CanEnter;

        [SerializeField] private Image border;
        [SerializeField] private SceneNames sceneName;

        public void SetSelected(bool selected)
        {
            //border.enabled = selected;
            border.gameObject.SetActive(selected);
        }

        public void Enter()
        {
            if (!CanEnter)
                return;

            SceneLoadManager.instance.ChangeScene(sceneName).Forget();
        }
    }
}