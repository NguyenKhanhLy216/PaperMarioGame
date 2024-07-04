using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchFlag : MonoBehaviour
{
void OnTriggerEnter(Collider other) {
    if (other.CompareTag("Player")){
    
    }
    else if (other.CompareTag("Enemy")){

    }
}
}
