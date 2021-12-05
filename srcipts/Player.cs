using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Threading.Tasks;

public class Player : MonoBehaviour  
{
    int penz;
    int alaptet;
    bool kert = false;
    int aktualisKezIndex = 0;

    [SerializeField]
    TMP_Text tet_text;

    [SerializeField]
    public TMP_Text osszeg_text;

    [SerializeField]
    GameObject Hand_Prefab;


    List<Hand> kezek;

    public Button duplaGomb;
    public Button startGomb;
    public Button splitGomb;

    GameManager gmp;

    public int Penz { get => penz; set => penz = value; }
    public int Tet { get => alaptet; set => alaptet = value; }
    public void Start()
    {
        gmp = GameManager.gm;
        penz = gmp.GetKezdoPenz();
        
        kezek = new List<Hand>();

        Hand ek = Instantiate(Hand_Prefab, transform).GetComponent<Hand>();
        kezek.Add(ek);//Elsõ kéz
        Debug.Log(-1 % 2);
        UpdateUI();
    }



    public async void Kartyat_fogad(Kartya _kartya) 
    {
        await AktualisKez.Hozzaado(_kartya);
        UpdateUI();
    }

    public void Emel(int osszeg) {
        if (penz < osszeg)
        {
            return;
        }
        alaptet += osszeg;
        penz -= osszeg;
        kezek[0].tet = alaptet;
        UpdateUI();
    }


    public void Minimum() {
        Emel(gmp.GetMinimumTet());
    }
    public void Maximum() {
        Emel(penz);
    }
    public void Tisztit() {
        Emel(-alaptet);
    }
    public void UpdateUI() {
        osszeg_text.text = penz.ToString();

        
        tet_text.text = Osszestet.ToString();

        startGomb.interactable = alaptet > 0;
        splitGomb.interactable = TudSplitelni();
        duplaGomb.interactable = TudDuplazni();

        Color tmp = startGomb.GetComponent<Image>().color;
        tmp.a = startGomb.interactable ? 1f : 0.5f;
        startGomb.GetComponent<Image>().color = tmp;

        tmp = splitGomb.GetComponent<Image>().color;
        tmp.a = splitGomb.interactable ? 1f : 0.5f;
        splitGomb.GetComponent<Image>().color = tmp;

        tmp.a = duplaGomb.interactable ? 1f : 0.5f;
        duplaGomb.GetComponent<Image>().color = tmp;


        foreach (var kez in kezek)
        {
            if (aktualisKezIndex == 0)
            {
                kez.szoveg.color = Color.red;
            }
            else if (aktualisKezIndex > 0)
            {
                kezek[aktualisKezIndex - 1].szoveg.color = Color.red;
            }
            kez.szoveg.color = Color.black;
        }
        AktualisKez.szoveg.color = Color.yellow;
    }
    bool TudSplitelni()
    {
        if (AktualisKez.KartyaDB == 2 && AktualisKez.kartyak[0].Ertek() == AktualisKez.kartyak[1].Ertek() && penz >= AktualisKez.tet)
        {
            return true;
        }
        return false;
    }
    
    bool TudDuplazni()
    {
        if (penz >= AktualisKez.tet)
        {
            return true;
        }

        return false;
    }

    Hand AktualisKez
    {
        get { return kezek[Mathf.Abs(aktualisKezIndex % kezek.Count)]; }
    }

    public void Megall()
    {
        if (kert == false)
        {
            aktualisKezIndex++;
            if (aktualisKezIndex == kezek.Count)
            {
                gmp.StartCoroutine("oszto_jon");
                kert = true;
            }else if (AktualisKez.BlackJack())
            {
                Megall();
            }
            UpdateUI();
        }
    }



    public async void Ker()
    {
        await AktualisKez.Hozzaado(gmp.Kartyahuz());     

        if (AktualisKez.Ervenyes_ertek() == 21 || AktualisKez.Min_ertek() > 21)
        {
            Megall();
        }
        UpdateUI();
    }

    public async void Duplaz()
    {
        if (kert == false)
        {
            AktualisKez.tet = alaptet * 2;
            penz -= alaptet;
            await AktualisKez.Hozzaado(gmp.Kartyahuz());
            Megall();        
        }
    }

    public async void Split()
    {
        Hand ujKez = Instantiate(Hand_Prefab, transform).GetComponent<Hand>();
        ujKez.tet = alaptet;
        penz -= alaptet;
        await ujKez.Hozzaado(AktualisKez.SplitCard());
        kezek.Add(ujKez);


        await AktualisKez.Hozzaado(gmp.Kartyahuz());
        int legutobbiKez = aktualisKezIndex;
        aktualisKezIndex = kezek.IndexOf(ujKez);
        await AktualisKez.Hozzaado(gmp.Kartyahuz());
        aktualisKezIndex = legutobbiKez;

        if (AktualisKez.BlackJack())
        {
            Megall();
        }

        UpdateUI();
    }

    public void Eldob()
    {
        foreach (Hand hand in kezek)
        {
            hand.Ürit();
        }
        for (int i = kezek.Count - 1; i > 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
            kezek.RemoveAt(i);
        }
        kezek[0].tet = 0;
        aktualisKezIndex = 0;
        UpdateUI();
        kert = false;
    }

    public void Kartya_megfigyelo() 
    {
        if (AktualisKez.Min_ertek() > 21 && AktualisKez.Max_ertek() > 21)
        {
            gmp.StartCoroutine("oszto_jon");
        }
    }

    public int Kez_ertek(int kezIndex)
    {
        return kezek[kezIndex].Ervenyes_ertek();
    }
    public Hand Kez(int kezIndex)
    {
        return kezek[kezIndex];
    }

    public int Lap_szam(int kezIndex) 
    {
        return kezek[kezIndex].kartyak.Count;
    }

    public int Kez_szam
    {
        get { return kezek.Count; }
    }

    public int Osszestet
    {
        get
        {
            int osszesTet = 0;
            foreach (var kez in kezek)
            {
                osszesTet += kez.tet;
            }
            return osszesTet;
        }
    }
}
