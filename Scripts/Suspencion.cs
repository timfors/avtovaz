using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Suspencion : MonoBehaviour
{
    [SerializeField]
    AnimInfo[] animationsInfo;
    SpriteAnim anim;
    int currentAnim;
    int currentAnimState = 0;
    bool isVertical = false;
    bool isHorizontal = false;
    (float x, float y) maxDelta;


    List<Vector3> touchPos = new List<Vector3>(){};
    
    void Start()
    {
        anim = GetComponent<SpriteAnim>();
        maxDelta = (Screen.width / 4, Screen.height / 2);
        StartCoroutine(Monitoring());
    }

    IEnumerator Monitoring()
    {
        while (true)
        {
            foreach (Touch touch in Input.touches)
            {
                touchPos.Add(touch.position);

                Vector3 firstPos = touchPos[0];
                Vector3 lastPos = touchPos[touchPos.Count - 1];

                float deltaX = lastPos.x - firstPos.x;

                float deltaY = lastPos.y - firstPos.y;

                
                float distancePerFrame;
                if (!anim.isPlaying)
                {
                    if (Mathf.Abs(deltaX) > Mathf.Abs(deltaY) && !isVertical)
                    {
                        isHorizontal = true;
                        if (deltaX >= 0)
                            currentAnim = 0;
                        else
                            currentAnim = 1;

                        distancePerFrame = maxDelta.x / animationsInfo[currentAnim].limitFrame;
                        currentAnimState = (int)(deltaX / distancePerFrame);
                    }
                    else if (Mathf.Abs(lastPos.x - firstPos.x) < Mathf.Abs(lastPos.y - firstPos.y) && !isHorizontal)
                    {
                        isVertical = true;
                        if (deltaY > 0)
                        {
                            currentAnim = 2;
                            distancePerFrame = maxDelta.y / animationsInfo[currentAnim].limitFrame;
                            currentAnimState = (int)(deltaY / distancePerFrame);
                        }
                    }
                    currentAnimState = currentAnimState < 0 ? -currentAnimState : currentAnimState;
                    currentAnimState = currentAnimState > animationsInfo[currentAnim].limitFrame ? animationsInfo[currentAnim].limitFrame : currentAnimState;
                    anim.SetState(animationsInfo[currentAnim].name, currentAnimState);
                }
            }

            if (Input.touches.Length == 0 && currentAnimState != 0)
            {
                anim.AnimateFromIndex(animationsInfo[currentAnim].name,   2 * animationsInfo[currentAnim].limitFrame - currentAnimState);
                touchPos.Clear();
                currentAnimState = 0;
                isHorizontal = false;
                isVertical = false;

            } else if (Input.touches.Length == 0 && currentAnimState == 0)
            {
                touchPos.Clear();
                isHorizontal = false;
                isVertical = false;
            }
            
            yield return new WaitForEndOfFrame();
        }
    }
    
}

[System.Serializable]
public struct AnimInfo
{
    public int limitFrame;
    public string name;    
}
