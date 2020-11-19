using System.Collections;
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
    private bool isLose = false;
    private bool firstTouch = false;

    [SerializeField] private Generator generator;
    [SerializeField] private Text scoreText;
    [SerializeField] private GameObject canvasHand;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        /*if (!firstTouch)
        {
            firstTouch = true;

            foreach (var item in canvasPage)
            {
                Destroy(item);
            }
        }*/

        if (Input.GetKeyDown(KeyCode.W) && !isJumping)
        {
            score++;
            scoreText.text = score.ToString();
            float zDifference = 0;

            if(transform.position.z % 1 != 0)
            {
                zDifference = Mathf.Round(transform.position.z) - transform.position.z;
            }

            MoveCharacter(new Vector3(1, 0, zDifference), 90);
            Destroy(canvasHand);
        }
        else if (Input.GetKeyDown(KeyCode.A) && !isJumping)
        {
            MoveCharacter(new Vector3(0, 0, 1), 0);
            Destroy(canvasHand);
        }
        else if (Input.GetKeyDown(KeyCode.D) && !isJumping)
        {
            MoveCharacter(new Vector3(0, 0, -1), 180);
            Destroy(canvasHand);
        }
    }

    private void FinishJump()
    {
        isJumping = false;
    }

    private void MoveCharacter(Vector3 difference, float rotationY)
    {
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
}