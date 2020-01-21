using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnim : MonoBehaviour
{   
    [SerializeField]
    Animator allAnims;



    int currentAnim;
    public float framesPerSecond { get;  private set; }
    public bool isPlaying { get; private set; }
    private void Start()
    {
        allAnims = GetComponent<Animator>();
        isPlaying = false;
    }

    public void SetState(string name, int index)
    {
        try
        {
            framesPerSecond = allAnims.GetCurrentAnimatorClipInfo(0)[0].clip.frameRate;
            allAnims.Play(name, 0, index / framesPerSecond);
            allAnims.speed = 0;
        } catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    IEnumerator Animate(string name, int beginFrame, int endFrame)
    {
        isPlaying = true;
        while (beginFrame <= endFrame)
        {
            if (endFrame - beginFrame >= 0)
            {
                SetState(name, beginFrame++);
            } else if (endFrame - beginFrame < 0)
            {
                SetState(name, beginFrame--);
            }
            yield return new WaitForSeconds(1 / framesPerSecond);
        }
        isPlaying = false;
    }
    public void AnimateFromIndex(string name, int index)
    {
        StopAllCoroutines();
        StartCoroutine(Animate(name, index, (int)(allAnims.GetCurrentAnimatorClipInfo(0)[0].clip.length * framesPerSecond)));
    }

    public void AnimateFromTo(string name, int beginFrame, int endFrame)
    {
        StopAllCoroutines();
        StartCoroutine(Animate(name, beginFrame, endFrame));
    }


}
