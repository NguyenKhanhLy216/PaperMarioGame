using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public void PauseGame()
    {
        Time.timeScale = 0; // Tạm dừng trò chơi
    }

    public void ResumeGame()
    {
        Time.timeScale = 1; // Tiếp tục chơi
    }
}
