using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem; // inputsystem  https://daekyoulibrary.tistory.com/entry/Unity-New-Input-System-1 
using UnityEngine.U2D;
using System.Runtime.CompilerServices;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    public Vector2 inputVec;
    public float speed = 5f;
   // public Scanner scanner;



    Rigidbody2D rigid;
    SpriteRenderer spriter;
    Animator anime;
    Transform tr;



    void Awake()
    {
        tr = GetComponent<Transform>();
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anime = GetComponent<Animator>();
        //scanner = GetComponent<Scanner>();/
    }

 
    void FixedUpdate() //위치이동
    {
        if (!GameManager.Instance.isLive)
            return;
        Vector2 nextVec = inputVec * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
        anime.SetFloat("Speed", inputVec.magnitude);
    }

    void OnMove(InputValue value)
    {
        inputVec = value.Get<Vector2>();
    }

    private void LateUpdate() //다음 프레임 넘어가기 직전에 실행
    {
        if (!GameManager.Instance.isLive)
            return;

        if (inputVec.x != 0) //양쪽 바라보는 if문
        {
            spriter.flipX = inputVec.x < 0;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if(!GameManager.Instance.isLive)
            return;

        GameManager.Instance.Health -= Time.deltaTime * 10;

        if (GameManager.Instance.Health < 0)
        {
            for(int index=2; index < transform.childCount; index++)
            {
                transform.GetChild(index).gameObject.SetActive(false);  
            }
            anime.SetTrigger("Dead");
            GameManager.Instance.GmaeOver();
        }    
    }
}
