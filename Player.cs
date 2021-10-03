using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour  
{
    string name;
    int penz;
    int tet;
    [SerializeField]
    TMP_Text tet_text;

    [SerializeField]
    TMP_Text osszeg_text;

    GameManager gmp;
    public void Start()
    {
        gmp = GameManager.gm;
        penz = gmp.GetKezdoPenz();
        UpdateUI();
    }

    public void Passz()
    {

    }

    public void Emel(int osszeg) {
        if (penz < osszeg)
        {
            return;
        }
        tet += osszeg;
        penz -= osszeg;
        UpdateUI();

    }

    public void Minimum() {
        Emel(gmp.GetMinimumTet());
    }
    public void Maximum() {
        Emel(penz);
    }
    public void Tisztit() {
        Emel(-tet);
    }
    public void UpdateUI() {
        osszeg_text.text = penz.ToString();
        tet_text.text = tet.ToString();
    }

}
