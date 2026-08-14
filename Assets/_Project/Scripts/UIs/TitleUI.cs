using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GoldEater
{
    public class TitleUI : MonoBehaviour
    {
        [Header("Buttons")]
        [SerializeField] private Button startButton;
        [SerializeField] private Button continueButton;
        [SerializeField] private Button optionButton;
        [SerializeField] private Button exitButton;


        private TitleManager titleManager;



        public void Initialize(TitleManager manager)
        {
            titleManager = manager;

            BindButton();
            SetupNavigation();

            SelectDefault();
        }



        private void BindButton()
        {
            startButton.onClick.AddListener(
                titleManager.StartGame
            );


            optionButton.onClick.AddListener(
                titleManager.OpenOption
            );


            exitButton.onClick.AddListener(
                titleManager.QuitGame
            );
        }



        private void SetupNavigation()
        {
            Navigation start = new Navigation();
            start.mode = Navigation.Mode.Explicit;
            start.selectOnUp = exitButton;
            start.selectOnDown = exitButton;
            startButton.navigation = start;



            //Navigation continueGame = new Navigation();
            //continueGame.mode = Navigation.Mode.Explicit;
            //continueGame.selectOnUp = startButton;
            //continueGame.selectOnDown = optionButton;
            //continueButton.navigation = continueGame;



            //Navigation option = new Navigation();
            //option.mode = Navigation.Mode.Explicit;
            //option.selectOnUp = continueButton;
            //option.selectOnDown = exitButton;
            //optionButton.navigation = option;



            Navigation exit = new Navigation();
            exit.mode = Navigation.Mode.Explicit;
            exit.selectOnUp = startButton;
            exit.selectOnDown = startButton;
            exitButton.navigation = exit;
        }



        private void SelectDefault()
        {
            EventSystem.current.SetSelectedGameObject(
                startButton.gameObject
            );
        }
    }
}

