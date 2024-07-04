using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreSystem : MonoBehaviour
{
    public Text scoreText;
    public Text countText;
    public Text highScore;
    public static int score = 0;
    public static int hs = 0;
    public static int count = 0;
    public Text mariohp;
    public static int hp = 3;
    public GameObject objectToShow;
    void Start()
    {
        // Load highscore from PlayerPrefs
        hs = PlayerPrefs.GetInt("highscore", 0);
    }
    // Start is called before the first frame update
    void Update(){
        scoreText.GetComponent<Text>().text = "SCORE: " + score;
        countText.GetComponent<Text>().text = count + "/3";
        highScore.GetComponent<Text>().text = "HIGHSCORE: " + hs;
        mariohp.GetComponent<Text>().text = "HP: " + hp;

        if (hp <= 0){
            hp = 0;
        }
         if (score > hs)
            {
                // Save highscore to PlayerPrefs
                PlayerPrefs.SetInt("highscore", score);
            }
        if (count == 3){
            objectToShow.SetActive(true);
        }
        else
        {
            objectToShow.SetActive(false);
        }
    }
    
}
