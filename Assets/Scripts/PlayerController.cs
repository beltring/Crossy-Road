﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    private Animator animator;
    private bool isJumping;
    private int score = 0;
    private int bestScore;
    private bool isBackStep = false;
    //private bool isLose = false;
    public static PlayerController player;
    private int coins;

    [SerializeField] private Generator generator;
    [SerializeField] private Text scoreText;
    [SerializeField] private GameObject canvasHand;
    [SerializeField] private GameObject canvasMusic;
    [SerializeField] private GameObject canvasRestart;
    [SerializeField] private GameObject canvasBestStore;
    [SerializeField] private GameObject canvasCoin;

    private void Start()
    {
        //PlayerPrefs.SetInt("coin", 0);
        player = this;
        coins = PlayerPrefs.GetInt("coin");
        canvasCoin.GetComponent<Text>().text = coins.ToString();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.W) && !isJumping)
        {
            score++;
            scoreText.text = score.ToString();
            bestScore = PlayerPrefs.GetInt("score");
            isBackStep = true;

            if (score > bestScore)
            {
                PlayerPrefs.SetInt("score", score);
            }
            canvasBestStore.GetComponent<Text>().text = $"Рекорд:{PlayerPrefs.GetInt("score")}";

            float zDifference = 0;

            if(transform.position.z % 1 != 0)
            {
                zDifference = Mathf.Round(transform.position.z) - transform.position.z;
            }

            MoveCharacter(new Vector3(1, 0, zDifference), 90);
        }
        else if (Input.GetKeyDown(KeyCode.A) && !isJumping)
        {
            MoveCharacter(new Vector3(0, 0, 1), 0);
        }
        else if (Input.GetKeyDown(KeyCode.D) && !isJumping)
        {
            MoveCharacter(new Vector3(0, 0, -1), 180);
        }
        else if(Input.GetKeyDown(KeyCode.S) && !isJumping && isBackStep)
        {
            float zDifference = 0;

            if (transform.position.z % 1 != 0)
            {
                zDifference = Mathf.Round(transform.position.z) - transform.position.z;
            }
            MoveCharacter(new Vector3(-1, 0, zDifference), 90);
        }
    }

    private void FinishJump()
    {
        isJumping = false;
    }

    private void MoveCharacter(Vector3 difference, float rotationY)
    {
        Destroy(canvasHand);

        GetJumpSound();

        animator.SetTrigger("jump");
        isJumping = true;
        var finalPos = transform.position + difference;
        transform.DOMove(finalPos, 0f);
        transform.DORotate(new Vector3(0, rotationY, 0), 0f, RotateMode.Fast);
        generator.SpawnTerrain(false, transform.position);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.GetComponent<MovingObject>() != null)
        {
            if(collision.collider.GetComponent<MovingObject>().movingObject.tag == "Log")
            {
                transform.parent = collision.collider.transform;
            }
        }
        else
        {
            transform.parent = null;
        }
    }

    public void Lose()
    {
        canvasMusic.SetActive(true);
        canvasRestart.SetActive(true);
        canvasBestStore.SetActive(true);
        
    }

    private void GetJumpSound()
    {
        if (PlayerPrefs.GetString("music") != "No")
        {
            GetComponent<AudioSource>().Play();
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Coin")
        {
            coins++;
            PlayerPrefs.SetInt("coin", coins);
            Destroy(collider.gameObject);
            canvasCoin.GetComponent<Text>().text = coins.ToString();
            if (PlayerPrefs.GetString("music") != "No")
            {
                canvasCoin.GetComponent<AudioSource>().Play();
            }
        }
    }
}