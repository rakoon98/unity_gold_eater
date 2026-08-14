namespace GoldEater
{
    using Cysharp.Threading.Tasks;
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public class TitleManager : MonoBehaviour
    {
        //[SerializeField] Button startBtn;
        //[SerializeField] Button continueBtn;
        //[SerializeField] Button settingBtn;
        //[SerializeField] Button exitBtn;

        [SerializeField] private TitleUI titleUI;

        private void Awake()
        {
            titleUI.Initialize(this);
        }


        void Start()
        {
            //if (titleUI != null)
            //{
            //    titleUI.Init(this);
            //}

            //startBtn.onClick.AddListener(() => StartGame());
            //continueBtn.onClick.AddListener(() =>
            //{
            //    SceneLoadManager.instance.ChangeScene(SceneNames.SafeZone).Forget();
            //});
            //settingBtn.onClick.AddListener(() =>
            //{

            //});
            //exitBtn.onClick.AddListener(() =>
            //{

            //});
        }

        public void StartGame()
        {
            SceneLoadManager.instance.ChangeScene(SceneNames.SafeZone).Forget();
        }

        public void OpenOption()
        {
            // 옵션열기 로직
        }

        public void ContinueGame()
        {
            // 이어하기 로직
        }

        public void QuitGame()
        {
            // 종료 로직
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        }

        //public void SetInitialFocus()
        //{
        //    if (EventSystem.current == null) return;

        //    // 잔여 선택 정보를 비우고 첫 버튼을 선택 상태로 지정
        //    EventSystem.current.SetSelectedGameObject(null);
        //    EventSystem.current.SetSelectedGameObject(startBtn.gameObject);
        //}
    }

}