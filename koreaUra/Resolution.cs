﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resolution : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Screen.SetResolution(Screen.width, Screen.width * 1920 / 1080, true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
