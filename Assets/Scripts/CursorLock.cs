using UnityEngine;

public class CursorLock : MonoBehaviour
{
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void OnDisable()
    {
        // Safety: unlock if this object is disabled
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
