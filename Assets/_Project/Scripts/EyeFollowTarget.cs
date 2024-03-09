using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EyeFollowTarget : MonoBehaviour
{
    [SerializeField] private Transform targetTransform;
    [SerializeField] Transform eyeTransform;
    [SerializeField] Eyes eyes;
    [SerializeField] bool followTarget;
    [SerializeField] float followStrength = 0.1f;
    [SerializeField] Toggle followToggle;
    
    private void Awake()
    {
        followToggle.onValueChanged.AddListener(delegate { HandleFollowToggle(); });
        HandleFollowToggle();
    }

    void Update()
    {
        if (!followTarget) return;
        if (targetTransform == null) return;
        
        Vector3 direction = targetTransform.position - eyeTransform.position;
        Vector3 relativeDirection = eyeTransform.InverseTransformDirection(direction);
        relativeDirection.z = 0;
        relativeDirection.Normalize();
        relativeDirection *= followStrength;
        eyes.ChangePupil(new Vector2(-relativeDirection.x, -relativeDirection.y));
    }
    private void HandleFollowToggle()
    {
        followTarget = followToggle.isOn;
    }
}
