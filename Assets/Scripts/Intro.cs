using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Intro : MonoBehaviour
{
    // Reference to the button
    public Button nextButton;

    void Start()
    {
        // Add a listener to the button to call the LoadMainMenu method when clicked
        nextButton.onClick.AddListener(LoadMainMenu);
    }

    // Method to load the Main Menu scene
    void LoadMainMenu()
    {
        SceneManager.LoadScene("Main menu");
    }
}
