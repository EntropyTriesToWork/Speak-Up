using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugOutOfBounds : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.position = Vector3.zero;
        }
    }
}
