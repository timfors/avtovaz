using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour
{
    [SerializeField]
    AnimInfo[] allAnims;

    [SerializeField]
    Rigidbody2D rotator;

    SpriteAnim anim;

    string animName;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<SpriteAnim>();
        animName = "Right";
    }

    private void Update()
    {
        float index;
        GetClipInfo clip = anim.GetClipInfo();
        if (rotator.rotation > 0)
            animName = "Right";
        else if (rotator.rotation < 0)
            animName = "Left";
        if (Mathf.Abs(rotator.rotation) > 1080)
            index = clip.frameCount;
        else
            index = Mathf.Abs(rotator.rotation) * clip.frameCount / 1080;
        if (!anim.isPlaying)
            anim.SetState(animName, index);
    }
}
