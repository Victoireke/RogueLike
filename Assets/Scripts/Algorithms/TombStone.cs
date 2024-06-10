using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TombStone : MonoBehaviour
{
    public string inscription;

    private void Start()
    {
        GameManager.Get.AddTombStone(this);
    }
}
