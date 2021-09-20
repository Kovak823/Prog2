using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Kartya : MonoBehaviour
{
    int szam;
    int szín;
    Image sr;

    private void Start()
    {
        sr = GetComponent<Image>();
        sr.sprite = GameManager.gm.lapok[0];
    }
}
