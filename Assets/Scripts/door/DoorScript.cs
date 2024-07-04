using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

public partial class Movement
{
    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    public class DoorScript : MonoBehaviour
    {
        public string nextLevel; // Tên của scene tiếp theo

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                SceneManager.LoadScene(nextLevel); // Chuyển sang scene tiếp theo
            }
        }

        private string GetDebuggerDisplay()
        {
            return ToString();
        }
    }
}