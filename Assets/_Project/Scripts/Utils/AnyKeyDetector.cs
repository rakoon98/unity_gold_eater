using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnyKeyDetector : MonoBehaviour
{

    public event Action onAnyKeyAction;

    void Update()
    {
        //// 아무 키나 누르고 있는 동안 감지
        //if (Keyboard.current != null && Keyboard.current.anyKey.isPressed)
        //{
        //    Debug.Log("아무 키나 눌림");
        //}

        // 아무 키나 처음 눌렀을 때(GetKeyDown과 같은 효과)
        if (Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame)
        {
            onAnyKeyAction.Invoke();
            Debug.Log("아무 키나 눌리는 순간");
        }
    }
}