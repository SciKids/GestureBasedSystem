﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Module : MonoBehaviour
{

    public void Back()
    {
        // Call to new scene
        SceneManager.LoadScene("Home");

    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
