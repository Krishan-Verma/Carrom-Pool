using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StrikerCtrl : MonoBehaviour
{

    [SerializeField]
    Slider StrikerSlider;

    [SerializeField]
    Transform StrikerBG;

    [SerializeField]
    Collider2D Holder;

    [SerializeField]
    Transform CenterPos;

    [SerializeField]
    GameObject RedCoin;

    Rigidbody2D rb; 
    RaycastHit2D hit;

    [SerializeField]
    TMP_Text WarrningMsg;

    [SerializeField]
    GameObject StrikerGlow;
    public Animator anim;
    public static float StartPos = -1.74f;
    bool StrikForce;
    float ScaleValue;
    float errOffset;
    bool ValidationStatus=true;
    


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
    }

   
    void Update()
    {
        
        if (rb.velocity.magnitude == 0 && ValidationStatus)
        {
            if (Input.GetMouseButton(0))
            {
                
                hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.forward);

                if (hit.collider)
                {
                    if (hit.transform.name == "Striker")
                    {
                        StrikForce = true;

                    }

                    if (StrikForce)
                    {
                        StrikerBG.LookAt(hit.point);
                        GetComponent<Collider2D>().enabled = StrikForce;
                    }

                    ScaleValue = Mathf.Abs(Vector2.Distance(transform.position, hit.point));
                    ScaleValue = Mathf.Clamp(ScaleValue, 0, 1);
                    StrikerBG.localScale = new Vector3(ScaleValue, ScaleValue, ScaleValue);


                }

            }
            else if (Input.GetMouseButtonUp(0))
            {

                if (ScaleValue > 0.2)
                {
                    ShootStriker();
                }

            }
        }
        
    }

    private void FixedUpdate()
    {
        if ( 0 < rb.velocity.magnitude && rb.velocity.magnitude < 0.01)
        {
            if (CoinCollector.instance.IsCoinCollected)
            {
                CoinCollector.instance.IsCoinCollected = false;
                ResetPos();
            }
            else
            {
                if (CoinCollector.instance.RedCoinCollected)
                {
                    Instantiate(RedCoin, CenterPos);
                    CoinCollector.instance.RedCoinCollected = false;
                }

                ChangePos();

            }
        }

       
        
       
    }

    void ShootStriker()  
    {
        
        StrikForce = false;
        StrikerBG.localScale = Vector3.zero;
        rb.AddForce(-StrikerBG.forward * 1000 * ScaleValue);
        anim.SetFloat("Velocity", rb.velocity.magnitude);
        ScaleValue = 0;
        
    }

    public void ResetPos()
    {
        
        float startx = Mathf.Clamp(transform.position.x, -1.3f, 1.3f);
        transform.position = new Vector3(startx,StartPos,0);
        rb.velocity = Vector2.zero;

    }
    public void StrikerPos(float x)
    {
        ValidationStatus= Validate(Physics2D.OverlapCircleAll(transform.position, 0.2f));
        GetComponent<Collider2D>().enabled = StrikForce;
        transform.position = new Vector3(x, StartPos, 0);
  
        

    }

    bool Validate(Collider2D[] collision)
    {
        for (int i = 0; i < collision.Length; i++)
        {
            if (collision[i].gameObject.tag == "Coin")
            {
                WarrningMsg.text="Striker Overlaps Token";
                StrikerGlow.GetComponent<Renderer>().material.color = Color.red;
                return false;
            }

        }

        StrikerGlow.GetComponent<Renderer>().material.color = Color.white;
        WarrningMsg.text = "";
        return true;

    }

    public void ChangePos()
    {
        if (StartPos == -1.74f)
            StartPos = 1.74f;
        else
            StartPos = -1.74f;

        ResetPos();
    }

    public void MoveOnError()
    {
        float Xpos=transform.position.x;

        if (!ValidationStatus)
        {
            if (Xpos > 1)
                errOffset =- 0.7f;

            else if (Xpos <-1)

                errOffset = 0.7f;
            else
                errOffset = 0.7f;

            errOffset = Mathf.Clamp(Xpos+errOffset, -1.3f, 1.3f);
            transform.position = new Vector3(errOffset, StartPos, 0);
            StrikerGlow.GetComponent<Renderer>().material.color = Color.white;
            WarrningMsg.text = "";
            ValidationStatus = true;
        }
    }
}
