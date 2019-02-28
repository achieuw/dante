using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public Text coinCountText;
    private int coinCount;

    private void Start()
    {
        SetCountText();
    }
    private void Update()
    {
        SetCountText();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Coin"))
        {
            Destroy(collision.gameObject);
            coinCount++;
        }
    }

    void SetCountText()
    {
        coinCountText.text = "x " + coinCount.ToString();
    }
}
