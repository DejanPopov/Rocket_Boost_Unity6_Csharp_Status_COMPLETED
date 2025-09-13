using UnityEngine;
using UnityEngine.InputSystem;

public class QuitApplication : MonoBehaviour
{
    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Debug.Log("Escape pushed, exiting application");
            Application.Quit();
        }
    }
}
