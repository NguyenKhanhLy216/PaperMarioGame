using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    public string tenManChoi;
    public void LoadManChoiMoi()
    {
        SceneManager.LoadScene(tenManChoi);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            LoadManChoiMoi();
        }
    }
}

