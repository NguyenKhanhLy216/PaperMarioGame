using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public void Yes()
    {
        ScoreSystem.score = 0;
        ScoreSystem.count = 0;
        ScoreSystem.hp = 3;
        SceneManager.LoadScene("Level 1");
    }
        public void No()
        {
            SceneManager.LoadScene("Main menu");
        }
}

