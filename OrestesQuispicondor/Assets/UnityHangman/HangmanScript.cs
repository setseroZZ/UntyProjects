﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HangmanScript : MonoBehaviour {
    public string palabra;
    string palabraEscondida;
    public Text outputText;
    public InputField inputText;

    public AudioClip successSound;
    public AudioClip failSound;
    public AudioSource camSource;

    // Use this for initialization
    void Start() { 
        
        foreach (char c in palabra)
        {
            palabraEscondida += "*";
        }
    }
    
	// Update is called once per frame
	void Update () {
        if (outputText.text != palabraEscondida)
        {
            outputText.text = palabraEscondida;
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log("Enter pressed!");
            //char letra = inputText.text[0];
            string letra = inputText.text.Substring(0, 1);
            if (palabra.Contains(letra))
            {
                string palabraTemporal = "";
                for (int i = 0; i < palabra.Length; i++)
                {
                    //if (palabra[i] == letra)
                    if (palabra[i] == letra[0])
                    {
                        palabraTemporal += letra;
                    }
                    else
                    {
                        palabraTemporal += palabraEscondida[i];
                    }
                }
                palabraEscondida = palabraTemporal;
                camSource.PlayOneShot(successSound);
            }
            else
            {
                camSource.PlayOneShot(failSound);
            }
            inputText.text = "";
        }

        //Debug.Log(palabra);
        if (palabraEscondida== palabra)
        {
            Debug.Log("Felicidades");
        }
    }
}
