using UnityEngine;
using System.Runtime.InteropServices;

public class CursorManager : MonoBehaviour
{
    [SerializeField] private bool showCursor = false;

    [DllImport("user32.dll")]
    static extern bool SetCursorPos(int X, int Y);

    void Start()
    {
        Cursor.visible = showCursor;
        Cursor.lockState = showCursor ? CursorLockMode.None : CursorLockMode.Locked;

        // Jika ingin pindahkan kursor ke pojok kiri atas
        if (showCursor)
        {
            // Pindah ke (0,0) layar monitor
            SetCursorPos(0, 0);
        }
    }
}