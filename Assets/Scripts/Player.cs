using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    
    public bool isTouchTop;
    public bool isTouchBottom;
    public bool isTouchRight;
    public bool isTouchLeft;

    public int power;
    public int maxPower;
    public float maxShotDelay;
    public float curShotDelay;

    public GameObject bulletObjA;
    public GameObject bulletObjB;

    Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        Move();
        Fire();
        Reload();
    }

    void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        if((isTouchRight && h == 1) || (isTouchLeft && h == -1)) h = 0;
        float v = Input.GetAxisRaw("Vertical");
        if((isTouchTop && v == 1) || (isTouchBottom && v == -1)) v = 0;

        Vector3 curPos = transform.position;
        Vector3 nextPos = new Vector3(h, v, 0) * speed * Time.deltaTime;

        transform.position = curPos + nextPos;

        if(Input.GetButtonDown("Horizontal") || Input.GetButtonUp("Horizontal")) anim.SetInteger("Input", (int)h);
    }

    void Fire()
    {
        if(!Input.GetButton("Fire1")) return;
        if(curShotDelay < maxShotDelay) return;

        switch(power)
        {
            case 1:
                GameObject bullet = objectManager.MakeObj("PlayerBulletA");
                bullet.transform.position = transform.position;
                Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
                rigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                break;
                
            case 2:
                GameObject bulletL = objectManager.MakeObj("PlayerBulletA");
                bulletL.transform.position = transform.position + Vector3.left * 0.1f;
                GameObject bulletR = objectManager.MakeObj("PlayerBulletA");
                bulletR.transform.position = transform.position + Vector3.right * 0.1f;
                
                Rigidbody2D rigidL = bulletL.GetComponent<Rigidbody2D>();
                Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();
                rigidL.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                rigidR.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                break;
   
            default:
                GameObject bulletLL = objectManager.MakeObj("PlayerBulletA");
                bulletLL.transform.position = transform.position + Vector3.left * 0.35f;
                GameObject bulletCC = objectManager.MakeObj("PlayerBulletB");
                bulletCC.transform.position = transform.position;
                GameObject bulletRR = objectManager.MakeObj("PlayerBulletA");
                bulletRR.transform.position = transform.position + Vector3.right * 0.35f;

                Rigidbody2D rigidLL = bulletLL.GetComponent<Rigidbody2D>();
                Rigidbody2D rigidCC = bulletCC.GetComponent<Rigidbody2D>();
                Rigidbody2D rigidRR = bulletRR.GetComponent<Rigidbody2D>();
                rigidLL.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                rigidCC.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                rigidRR.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                break;   
        }

        curShotDelay = 0;
    }

    void Reload()
    {
        curShotDelay += Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Boarder")
        {
            switch(other.gameObject.name)
            {
                case "Top":
                    isTouchTop = true;
                    break;
                case "Bottom":
                    isTouchBottom = true;
                    break;
                case "Right":
                    isTouchRight = true;
                    break;
                case "Left":
                    isTouchLeft = true;
                    break;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.tag == "Boarder")
        {
            switch(other.gameObject.name)
            {
                case "Top":
                    isTouchTop = false;
                    break;
                case "Bottom":
                    isTouchBottom = false;
                    break;
                case "Right":
                    isTouchRight = false;
                    break;
                case "Left":
                    isTouchLeft = false;
                    break;
            }
        }
    }
}
