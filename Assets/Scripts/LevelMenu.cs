using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelMenu : MonoBehaviour
{
    public void OpenLevel(int levelId)
    {
        if (CanSelectLevel(levelId))
        {
            string levelName = "Level " + levelId;
            SceneManager.LoadScene(levelName);
        }
        else
        {
            Debug.Log("Bạn chưa thể chọn màn chơi " + levelId + ". Hãy hoàn thành màn chơi trước đó.");
        }
    }

    private bool CanSelectLevel(int levelId)
    {
        if (levelId == 1)
        {
            return true; // Màn 1 có thể chọn mà không cần điều kiện
        }
        else if (levelId == 2)
        {
            int level1Completed = PlayerPrefs.GetInt("Level_1_Completed", 0);
            return level1Completed == 1;
        }
        else if (levelId == 3)
        {
            int level2Completed = PlayerPrefs.GetInt("Level_2_Completed", 0);
            return level2Completed == 1;
        }
        else if (levelId == 4)
        {
            int level3Completed = PlayerPrefs.GetInt("Level_3_Completed", 0);
              return level3Completed == 1;
        }
        else
        {
            // Kiểm tra các màn chơi khác tương tự nếu bạn có thêm màn chơi
            return false;
        }
    }
}
