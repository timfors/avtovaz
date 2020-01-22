using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelRotator : MonoBehaviour
{

    bool isFirstTouch = true;
    bool isTouchable = true;

    [SerializeField]
    float refreshSpeed;

    Rigidbody2D rigid;

    float deltaRot = 0;
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();  
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0 && isTouchable)
        {
            Vector3 touchPos = Camera.main.ScreenToWorldPoint(Input.touches[0].position);
            
            float signedAngle = Vector2.SignedAngle(transform.right, touchPos - transform.position);
            if (isFirstTouch)
            {
                deltaRot = signedAngle - rigid.rotation % 360;
                isFirstTouch = false;
            }

            if (Mathf.Abs(signedAngle) >= 1e-1f)
            {
                float angles = rigid.rotation;
                angles += signedAngle - deltaRot;
                rigid.rotation = angles;
            }
        } else
        {
            isTouchable = false;
            isFirstTouch = true;
            StartCoroutine(Refresh());
            deltaRot = 0;
        }
    }


    IEnumerator Refresh()
    {
        int step = rigid.rotation > 0 ? 1 : -1;
        if (Mathf.Abs(rigid.rotation) > 1080)
            rigid.rotation = rigid.rotation % 360 + 1080 * step; 
        while (Mathf.Abs(rigid.rotation) > 1f)
        {
            rigid.rotation -= refreshSpeed * Time.deltaTime * step;
            yield return new WaitForEndOfFrame();
        }
        rigid.rotation = 0;
        isTouchable = true;

    }
}
