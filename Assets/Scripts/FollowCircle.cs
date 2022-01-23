using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCircle : MonoBehaviour
{
    [Header("Circle to follow")] 
    [SerializeField] private Transform circleTargetFollow;

    [SerializeField] private float speedMove = 5;

    private void LateUpdate()
    {
        Vector3 directionFInal = new Vector3(Mathf.Clamp(circleTargetFollow.position.x, 155.69f, Mathf.Infinity), Mathf.Clamp(circleTargetFollow.position.y, 81.37f, Mathf.Infinity), -10);
        transform.position = Vector3.MoveTowards(transform.position, directionFInal, speedMove);
    }
}
