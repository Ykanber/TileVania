using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] AudioClip audioClip;
    [SerializeField] int coinScore = 100;

    bool wasCollected = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" && !wasCollected)
        {
            wasCollected = true;
            AudioSource.PlayClipAtPoint(audioClip, Camera.main.transform.position);
            FindObjectOfType<GameSession>().AddToScore(coinScore);
            Destroy(gameObject);

        }
    }
}
