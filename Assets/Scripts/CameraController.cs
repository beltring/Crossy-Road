using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using DG.Tweening;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    
    [SerializeField]
    private Vector3 offset;

    [SerializeField]
    private float smoothness;

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, player.transform.position + offset, smoothness);
        //transform.DOMove(player.transform.position + offset, duration);
    }
}
