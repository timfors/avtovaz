using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnim : MonoBehaviour
{   
    [SerializeField]
    Animator allAnims;

    public float speed { get; set; }
    
    int currentAnim;
    public bool isPlaying { get; private set; }
    private GetClipInfo clipInfo = new GetClipInfo();

    private void Start()
    {
        speed = 1;
        allAnims = GetComponent<Animator>();
        clipInfo.framesPerSecond = allAnims.GetCurrentAnimatorClipInfo(0)[0].clip.frameRate;
        clipInfo.frameCount = (int)(allAnims.GetCurrentAnimatorClipInfo(0)[0].clip.length * clipInfo.framesPerSecond);
        isPlaying = false;
    }

    public void SetState(string name, float index)
    {
        try
        {
            clipInfo.currentFrame = index;
            allAnims.Play(name, 0, index / clipInfo.frameCount);
            allAnims.speed = 0;
        } catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    public GetClipInfo GetClipInfo()
    {
        return clipInfo;
    }
    IEnumerator Animate(string name, int beginFrame, int endFrame)
    {
        allAnims.Play(name, 0, 0);

        clipInfo.name = name;
        clipInfo.framesPerSecond = allAnims.GetCurrentAnimatorClipInfo(0)[0].clip.frameRate;
        clipInfo.frameCount = (int)(allAnims.GetCurrentAnimatorClipInfo(0)[0].clip.length * clipInfo.framesPerSecond);

        isPlaying = true;
        while (beginFrame != endFrame)
        {
            if (endFrame - beginFrame >= 0)
            {
                SetState(name, beginFrame++);
            } else if (endFrame - beginFrame < 0)
            {
                SetState(name, beginFrame--);
            }
            yield return new WaitForSeconds(1 / (clipInfo.framesPerSecond * speed));
        }
        isPlaying = false;
    }
    public void AnimateFromIndex(string name, int index)
    {
        StopAllCoroutines();
        StartCoroutine(Animate(name, index, (int)(allAnims.GetCurrentAnimatorClipInfo(0)[0].clip.length * clipInfo.framesPerSecond)));
    }

    public void AnimateFromTo(string name, int beginFrame, int endFrame)
    {
        StopAllCoroutines();
        StartCoroutine(Animate(name, beginFrame, endFrame));
    }


}
public struct GetClipInfo
{
    public float framesPerSecond;
    public float currentFrame;
    public string name;
    public float frameCount;
}
