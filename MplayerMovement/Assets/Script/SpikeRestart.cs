using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeRestart : MonoBehaviour
{
    public GameObject startPoint;
    public GameObject player;

    private void OnCollisionEnter2D (Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player.transform.position = startPoint.transform.position;
        }
    }
}
