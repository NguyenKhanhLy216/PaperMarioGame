using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public AudioSource healsound;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                healsound.Play();
                player.Heal(1);
                Destroy(gameObject);
            }
            else if (other.CompareTag("Enemy")) { }

        }
    }
}