using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewScene : MonoBehaviour
{
    private Transform player;
    // Start is called before the first frame update
    private void Awake() {
        player = GameObject.FindWithTag("Player").transform;
    }
    // Update is called once per frame
    private void LateUpdate()
    {
        Vector3 cameraPosition = transform.position;
        cameraPosition.x = player.position.x;
        cameraPosition.z = player.position.z - 12f;
        //cameraPosition.x = Mathf.Max(cameraPosition.x, player.position.x);
        transform.position = cameraPosition;
    }
}
