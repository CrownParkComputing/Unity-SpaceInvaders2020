﻿
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TextAlignment))]
public class HighscoreText : MonoBehaviour
{
    Text highscore;

    void OnEnable()
    {
        highscore = GetComponent<Text>();
        highscore.text = "Highscore: " + PlayerPrefs.GetInt("Highscore").ToString();
    }
}
