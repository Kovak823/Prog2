using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    int Kezdopenz = 1000;
    int Minimumtet = 100;
    int HG = 0;

    const string WIN = "You Won!";
    const string LOSE = "You Lose!";
    const string DRAW = "Draw!";
    const string HS_KEY = "HighScore";

    [SerializeField]
    TMP_Text W_L;

    [SerializeField]
    TMP_Text VegO;

    [SerializeField]
    TMP_Text Draw_info;

    [SerializeField]
    TMP_Text Best_HG;

    [SerializeField]
    GameObject EndScreen;

    [SerializeField]
    GameObject Kiszall_gomb;

    [SerializeField]
    Kartya kartyaprefab;

    [SerializeField]
    public GameObject panel;

    [SerializeField]
    public float sebesseg;//kártya animácó sebessége

    [SerializeField]
    public Sprite[] lapok;

    [SerializeField]
    public Sprite hatlap;

    [SerializeField]
    RectTransform kartyatarto;

    [SerializeField]
    Hand oszto;


    [SerializeField] Transform VegOsztoLapok;
    [SerializeField] Transform VegJatekosLapok;


    List<Kartya> pakli;
    [SerializeField]
    Player jatekos;

    public bool jatekos_soros;

    public static GameManager gm {
        get;private set;
    }

    void Awake()
    {
        gm = this;
        lapok = Resources.LoadAll<Sprite>("pakli");
        pakli = new List<Kartya>();
        Pakli_keszit();
        oszto.oszto_e = true;

        if (PlayerPrefs.HasKey(HS_KEY))
        {
            HG = PlayerPrefs.GetInt(HS_KEY);
        }
        else
        {
            PlayerPrefs.SetInt(HS_KEY, 0);
        }
    }

    public void Pakli_keszit() 
    {
        for (int i = 0; i < lapok.Length; i++)
        {
            Kartya kt = Kartya.Kartyakeszit(kartyaprefab, i % 13, lapok[i]);
            pakli.Add(kt);
            kt.transform.SetParent(kartyatarto,false);
        }
    }



    public void Kever()
    {
        List<Kartya> k_pakli = new List<Kartya>();

        
        var rnd = new System.Random();
        for (int i = 0; i < pakli.Count; i++)
        {
            int szam = rnd.Next(52-i);
            k_pakli.Add(pakli[szam]);
            pakli.RemoveAt(szam);
        }
        pakli = k_pakli;
        
    }

    public Kartya Kartyahuz() 
    {
        Kartya kartya = pakli[pakli.Count-1];
        pakli.RemoveAt(pakli.Count - 1);
        return kartya;
    }

    public void Osztas() 
    {
        Kever();
        StartCoroutine("Lapotad");
    }


    IEnumerator Lapotad() 
    {
        panel.SetActive(false);
        for (int i = 0; i < 2; i++)
        {
            jatekos.Kartyat_fogad(Kartyahuz());
            yield return new WaitForSeconds(1);
            oszto.Hozzaado(Kartyahuz());
            yield return new WaitForSeconds(1);
        }
        panel.SetActive(true);

      
        if (jatekos.Kez(0).BlackJack() || oszto.BlackJack())//Instant BlackJack
        {
            oszto.kartyak[1].Felfordit();
            oszto.Frissites();
            Jatek_vege();
        }
        
    }


    public void jatekos_jon() 
    {
        jatekos_soros = true;
    }

    IEnumerator oszto_jon() 
    {
        jatekos_soros = false;
        foreach (var kartya in oszto.kartyak)
        {
            kartya.Felfordit();
        }
        oszto.Frissites();
        // max > 21 && min < 17 || max < 17
        int tulmentKezek = 0;
        int legnagyobbKez = 0;
        for (int i = 0; i < jatekos.Kez_szam; i++)
        {
            if (jatekos.Kez(i).Min_ertek() > 21)
            {
                tulmentKezek++;
            }
            else if (jatekos.Kez(i).Ervenyes_ertek() > legnagyobbKez)
            {
                legnagyobbKez = jatekos.Kez(i).Ervenyes_ertek();
            }
        }

        if (tulmentKezek < jatekos.Kez_szam)
        {


            while ((oszto.Max_ertek() > 21 && oszto.Min_ertek() < 17 || oszto.Max_ertek() < 17) && oszto.Ervenyes_ertek() < legnagyobbKez)
            {
                oszto.Hozzaado(Kartyahuz());
                yield return new WaitForSeconds(1);
            }
        }
        Jatek_vege();
    }

    public int GetKezdoPenz()
    {
        return Kezdopenz;
    }
    public int GetMinimumTet()
    {
        return Minimumtet;
    }

    public void Jatek_vege() 
    {
        //win:hand=21(2lap,2,5x tét), hand=21(nem két lap, 2x tét), hand>oszto && hand<=21
        //lose:hand > 21, (hand <= 21 && oszto <= 21) && oszto > hand 
        //draw:hand==oszto && (hand<=21 && oszto <=21) else(hand=2lap == 21)

        int osszestet = jatekos.Osszestet;
        int nyereseg = 0;
        for (int i = 0; i < jatekos.Kez_szam; i++)
        {
            Debug.Log($"Jatekos {i}-edik kezén {jatekos.Kez(i).tet} peták van");
        }
        //win
        for (int i = 0; i < jatekos.Kez_szam; i++)
        {
            if (jatekos.Kez(i).BlackJack())
            {
                if (oszto.BlackJack())
                {
                    //Mind2 BJ
                    nyereseg += jatekos.Kez(i).tet;
                }

                nyereseg += (int)(jatekos.Kez(i).tet * 2.5) - jatekos.Kez(i).tet;
            }

            if  (jatekos.Kez_ertek(i) <= 21 && (oszto.Ervenyes_ertek() < jatekos.Kez_ertek(i)))
            {
                nyereseg += jatekos.Kez(i).tet * 2;
            }
            //lose
            if (oszto.Ervenyes_ertek() <= 21 && (oszto.Ervenyes_ertek() > jatekos.Kez_ertek(i)))
            {
                //kihagyható
            }
            //draw
            if (oszto.Ervenyes_ertek() == jatekos.Kez_ertek(i) && (oszto.Ervenyes_ertek() <= 21 && jatekos.Kez_ertek(i) <= 21))
            {
                nyereseg += jatekos.Kez(i).tet;
            }
        }
        jatekos.Penz += nyereseg;
        Debug.Log("Nyereség: " + nyereseg);
        int vegosszeg = nyereseg - osszestet;
        if (nyereseg > osszestet)
        {
            //wineltünk
            W_L.color = Color.green;
            W_L.text = WIN;
            VegO.text = $"{vegosszeg} peták";
            Draw_info.text = "";
        }
        else if (nyereseg == osszestet)
        {
            // draw
            W_L.color = Color.white;
            W_L.text = DRAW;
            Draw_info.text = "(You don't lose or win peták)";
            VegO.text = "";
        }
        else
        {
            //lose
            W_L.color = Color.red;
            W_L.text = LOSE;
            VegO.text = $"{vegosszeg*-1} peták";
            Draw_info.text = "";
        }

        jatekos.Tet = 0;
        EndScreen.SetActive(true);
        //Lapok Áthelyezése
        EndLapokMegjelenitese();


        if (jatekos.Penz > HG)
        {
            HG = jatekos.Penz;
            PlayerPrefs.SetInt(HS_KEY, HG);
        }


    }

    void EndLapokMegjelenitese()
    {
        Instantiate(oszto.gameObject, VegOsztoLapok).transform.GetChild(1).GetComponent<TMP_Text>().SetText(oszto.transform.GetChild(1).GetComponent<TMP_Text>().text);

        for (int i = 0; i < jatekos.transform.childCount; i++)
        {
            Instantiate(jatekos.transform.GetChild(i), VegJatekosLapok).GetComponent<Hand>().szoveg.color = Color.white;
        }
    }

    public void EndLapokElrejtése()
    {
        if (VegOsztoLapok.childCount != 0 && VegJatekosLapok.childCount != 0)
        {
            Destroy(VegOsztoLapok.GetChild(0).gameObject);
            for (int i = VegJatekosLapok.childCount - 1; i >= 0; i--)
            {
                Destroy(VegJatekosLapok.GetChild(i).gameObject);
            }
        }
    }

    public void Uj_jatek() 
    {
        oszto.Ürit();
        jatekos.Eldob();
        EndLapokElrejtése();

        for (int i = kartyatarto.childCount - 1; i >= 0; i--)
        {
            Destroy(kartyatarto.GetChild(i).gameObject);
        }
        pakli.Clear();


        Pakli_keszit();
        //+megfelelõ phase megjelenítése
        jatekos_soros = true;
        Kiszall_gomb.SetActive(true);
    }

    public void Kiszall() 
    {
        jatekos.Penz = Kezdopenz;
        jatekos.Tet = 0;
        jatekos.osszeg_text.text = jatekos.Penz.ToString();
        Best_HG.text = $"{HG} peták";
    }

    public void Kilepes() 
    {
        Application.Quit();
    }

}
