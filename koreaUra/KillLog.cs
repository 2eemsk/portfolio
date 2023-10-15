using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class KillLog : MonoBehaviour
{
    Text text;
    int killed;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        //killed = player.GetComponent<PlayerAttack>().deadMob;
        //text.text = killed.ToString();
    }
}
