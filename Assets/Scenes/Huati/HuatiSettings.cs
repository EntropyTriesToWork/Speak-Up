using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Huati Settings")]
public class HuatiSettings : ScriptableObject
{
    public float sensitivity = 5;
    public float jumpThreshhold = 0.1f;
    public float minJumpTime = 1f;
    public float jumpForce = 20;
}
