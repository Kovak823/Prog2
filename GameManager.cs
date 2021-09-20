using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    Rigidbody2D negyzet;
    [SerializeField]
    float sebesseg;
    [SerializeField]
    public Sprite[] lapok;

    public static GameManager gm {
        get;private set;
    }
    // Start is called before the first frame update
    void Awake()
    {
        if (gm == null)
        {
            gm = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
