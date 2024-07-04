using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerButtonController : MonoBehaviour
{
    public Player player; 
    public Button jumpButton; 
    public Button hammerButton; 

    private void Start()
    {
        jumpButton.onClick.AddListener(OnJumpButtonClick);
        hammerButton.onClick.AddListener(OnHammerButtonClick);
    }

    private void OnJumpButtonClick()
    {
        player.Jump();
    }

    private void OnHammerButtonClick()
    {
        player.UseHammer();
    }
}
