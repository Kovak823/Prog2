using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;
using System.Linq;

public class Hand : MonoBehaviour
{
    public List<Kartya> kartyak;

    [SerializeField]
    public TMP_Text szoveg;

    [SerializeField]
    RectTransform keztarto;

    public bool oszto_e = false;
    public int tet;
    private void Awake()
    {
        kartyak = new List<Kartya>();
    }

    public bool BlackJack()
    {
        if (kartyak.Count == 2 && kartyak.Sum(k=>k.Ertek())== 11 && kartyak.Count(k => k.Ertek() == 1) == 1)
        {
            return true;
        }
        return false;
    }


    public async Task Hozzaado(Kartya _kartya) 
    {
        if (!(oszto_e && kartyak.Count == 1))
        {
            _kartya.Felfordit();
        }
        

        Vector3 celv = transform.position - _kartya.transform.position;
        var end = Time.time + GameManager.gm.sebesseg;
        while (Time.time < end)
        {
            _kartya.transform.Translate(celv / GameManager.gm.sebesseg * Time.deltaTime);//kettõ kép kocka idõ(deltaTime)
            await Task.Yield();
        }
        kartyak.Add(_kartya);
        _kartya.transform.SetParent(keztarto, false);
        Frissites();
    }

    public Kartya SplitCard()
    {
        Kartya _kartya = kartyak[1];
        kartyak.RemoveAt(1);
        return _kartya;
    }

    public string KartyaErtek() 
    {
        if (Min_ertek() == Max_ertek()) 
        {
            return Min_ertek().ToString();
        }
        return Min_ertek() + "/" + Max_ertek(); 
    }

    public int Min_ertek() 
    {
        int sum = 0;
        for (int i = 0; i < kartyak.Count; i++)
        {
            if (kartyak[i].felfordit == true)
            {
                sum += kartyak[i].Ertek();
            }
        }
        return sum;
    }

    public int Max_ertek()
    {
        bool voltasz = false;
        int sum = 0;
        for (int i = 0; i < kartyak.Count; i++)
        {
            if (kartyak[i].felfordit == true)
            {
                sum += kartyak[i].Ertek();
                if (kartyak[i].Ertek()==1 && voltasz == false)
                {
                    sum += 10;
                    voltasz = true;
                }
            }

        }
        return sum;
    }

    public int Ervenyes_ertek()
    {
        int ertek = Mathf.Max(Min_ertek(), Max_ertek() % 22);
        return ertek > 21 ? -1 : ertek;
    }

    public void Frissites()
    {
        szoveg.text = KartyaErtek();
    }

    public void Ürit()
    {
        kartyak.Clear();
        for (int i = keztarto.childCount-1; i >= 0; i--)
        {
            Destroy(keztarto.GetChild(i).gameObject);
        }
        szoveg.text = "";
    }

    public int KartyaDB
    {
        get { return kartyak.Count;}
    }

}
