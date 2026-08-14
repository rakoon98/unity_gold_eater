using UnityEngine;
using UnityEngine.EventSystems;

public abstract class UIView : MonoBehaviour
{
    [Header("이 뷰가 열릴 때 첫 번째로 포커스될 UI 요소")]
    [SerializeField] protected GameObject firstSelected;

    public virtual void Open()
    {
        gameObject.SetActive(true);
        FocusFirstElement();
    }

    public virtual void Close()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 지정된 firstSelected 버튼으로 키보드 포커스를 강제 이동시킵니다.
    /// </summary>
    public virtual void FocusFirstElement()
    {
        if (EventSystem.current != null && firstSelected != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(firstSelected);
        }
    }
}