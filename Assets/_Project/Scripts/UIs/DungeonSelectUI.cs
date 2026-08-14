namespace GoldEater
{
    using UnityEngine;

    public class DungeonSelectUI : MonoBehaviour
    {
        [SerializeField] private StageCard[] stageCards;

        private int currentIndex;

        private void OnEnable()
        {
            currentIndex = GetFirstSelectableStage();
            RefreshSelection();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
                Move(-1);

            if (Input.GetKeyDown(KeyCode.RightArrow))
                Move(1);

            if (Input.GetKeyDown(KeyCode.Return))
                stageCards[currentIndex].Enter();
        }

        private void Move(int dir)
        {
            int next = currentIndex + dir;

            if (next < 0 || next >= stageCards.Length)
                return;

            if (!stageCards[next].CanEnter)
                return;

            currentIndex = next;

            RefreshSelection();
        }

        private void RefreshSelection()
        {
            for (int i = 0; i < stageCards.Length; i++)
            {
                stageCards[i].SetSelected(i == currentIndex);
            }
        }

        private int GetFirstSelectableStage()
        {
            for (int i = 0; i < stageCards.Length; i++)
            {
                if (stageCards[i].CanEnter)
                    return i;
            }

            return 0;
        }
    }

}