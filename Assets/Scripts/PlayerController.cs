using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    private Animator animator;
    private bool isJumping;

    [SerializeField]
    private Generator generator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) && !isJumping)
        {
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
        //transform.position += difference;
        transform.DOMove(finalPos, 0f);
        transform.DORotate(new Vector3(0, rotationY, 0), 0f, RotateMode.Fast);
        generator.SpawnTerrain(false, transform.position);
    }
}