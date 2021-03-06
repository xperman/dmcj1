﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OtherSeting : MonoBehaviour
{
    public float f_UpdateInterval = 0.5F;
    private float f_LastInterval;
    private int i_Frames = 0;
    private float f_Fps;

    void Start()
    {
        f_LastInterval = Time.realtimeSinceStartup;

        i_Frames = 0;
    }

    void Update()
    {
        ++i_Frames;
        if (Time.realtimeSinceStartup > f_LastInterval + f_UpdateInterval)
        {
            f_Fps = i_Frames / (Time.realtimeSinceStartup - f_LastInterval);
            i_Frames = 0;
            f_LastInterval = Time.realtimeSinceStartup;
        }
        this.GetComponent<UIManager>().fpsText.text = "fps : " + ((int)f_Fps).ToString();
    }
}
