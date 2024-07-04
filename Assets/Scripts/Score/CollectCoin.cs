using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CollectCoin : MonoBehaviour
{
    public AudioSource coinsound;
void OnTriggerEnter(Collider other) {
    if (other.CompareTag("Player")){
        ScoreSystem.score += 100;
        coinsound.Play();
        Destroy(gameObject);

    }
    else if (other.CompareTag("Enemy")){

    }
}
}
