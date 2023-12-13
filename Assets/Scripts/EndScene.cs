using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndScene : MonoBehaviour
{
    private GameManager gameManager;

    //private float delay = 2f;
    //private float fadeTime = 2f;

    //private float time;
    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    //private void Update()
    //{
    //    time += Time.deltaTime;
    //}

    //private void OnTriggerEnter(Collider other)
    //{
    //    //player.canMove = false;
    //}

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            gameManager.FloatEndScenePanel();

        }
    }

}
