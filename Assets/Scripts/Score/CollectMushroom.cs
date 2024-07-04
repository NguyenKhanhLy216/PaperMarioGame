using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CollectItemsScore : MonoBehaviour
{
    public AudioSource mushroomsound;
void OnTriggerEnter(Collider other) {
    if (other.CompareTag("Player")){
        ScoreSystem.score += 200;
        mushroomsound.Play();
        if(ScoreSystem.count <3){
            ScoreSystem.count += 1;
        }
        Destroy(gameObject);

    }
    else if (other.CompareTag("Enemy")){

    }
}
}

