using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door3 : MonoBehaviour
{
void OnTriggerEnter(Collider other) {
    if (other.CompareTag("Player")){
        Application.LoadLevel("Winning");
        //Destroy(gameObject);

    }
    else if (other.CompareTag("Enemy")){

    }
}
}
