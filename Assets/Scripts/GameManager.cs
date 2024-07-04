using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    void OnEnable()
    {
        ResetLevelCompletionStatus();
    }

    private void ResetLevelCompletionStatus()
    {
        // Xóa tất cả các giá trị đã lưu trong PlayerPrefs
        PlayerPrefs.DeleteKey("Level_1_Completed");
        PlayerPrefs.DeleteKey("Level_2_Completed");
        PlayerPrefs.DeleteKey("Level_3_Completed");

        // Lưu lại trạng thái sau khi xóa
        PlayerPrefs.Save();

        Debug.Log("Đã reset trạng thái hoàn thành của các màn chơi.");
    }
}
