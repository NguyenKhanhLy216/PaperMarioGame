using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpItem : MonoBehaviour
{
    public Image hpItem;
    public Sprite hpActive;
    public Sprite hpInactive;

    public void UpdateUI(bool isActive = true)
    {
        if (!hpItem) return;

        if (isActive) 
        { 
            hpItem.sprite = hpActive;
        }
        else
        {
            hpItem.sprite = hpInactive;
        }
    }
}
