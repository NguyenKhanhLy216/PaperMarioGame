using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door1 : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerPrefs.SetInt("Level_1_Completed", 1);
            PlayerPrefs.Save();

            SceneManager.LoadScene("Level 2");

            ScoreSystem.count = 0;
            ScoreSystem.hp = 3;

        }
        else if (other.CompareTag("Enemy"))
        {
         
        }
    }
}
