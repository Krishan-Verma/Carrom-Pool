using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CoinCollector : MonoBehaviour
{
    GameObject Striker;
    public int Coin=0;
    public TMP_Text P1coins;
    public TMP_Text P2coins;
    public static CoinCollector instance;
    public bool IsCoinCollected=false;
    public bool RedCoinCollected = false;
    

    private void Awake()
    {
        instance = this;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
       
        if (collision.gameObject.tag == "Coin")
        {
            Destroy(collision.gameObject);

            if (RedCoinCollected)
            {
                IncreaseScore();
                IncreaseScore();
                IncreaseScore();
                IncreaseScore();
                IncreaseScore();
            }
            else
            {   
                
                IncreaseScore();
            }

            RedCoinCollected = false;

        }

        if (collision.gameObject.tag == "Striker")
        {
            Striker = collision.gameObject;
            Striker.GetComponent<StrikerCtrl>().ChangePos();
        }

        if (collision.gameObject.tag == "RedCoin")
        {
            Destroy(collision.gameObject);
            RedCoinCollected = true;
            IsCoinCollected = true;
        }

    }

    void IncreaseScore()
    {
        
        if (StrikerCtrl.StartPos == 1.74f)
        {
            Coin = int.Parse(P2coins.text);
            Coin++;
            P2coins.text = Coin.ToString();
            IsCoinCollected = true;
          
        }
        else
        {
            Coin = int.Parse(P1coins.text);
            Coin++;
            P1coins.text = Coin.ToString();
            IsCoinCollected = true;
            
        }
    }

    
}
