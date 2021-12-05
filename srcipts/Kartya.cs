using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Kartya : MonoBehaviour
{
    public int ertek;

    [SerializeField]
    Image kep_kartya;

    Sprite elolap;

    public bool felfordit = false;

    public int Valosertek() 
    {
        return ertek;
    }

    public int Ertek() 
    {
        if (ertek > 10)
        {
            return 10;
        }
        else {
            return ertek;
        }
    }

    public static Kartya Kartyakeszit(Kartya kartyprefab, int _ertek, Sprite _kepkartya){

        Kartya kartya = Instantiate<Kartya>(kartyprefab);
        kartya.ertek = _ertek + 1;
        kartya.elolap = _kepkartya;
        kartya.Lefordit();

        return kartya;
    }

    public void Felfordit() 
    {
        kep_kartya.sprite = elolap;
        felfordit = true;
    }

    public void Lefordit() 
    {
        kep_kartya.sprite = GameManager.gm.hatlap;
        felfordit = false;
    }

}
