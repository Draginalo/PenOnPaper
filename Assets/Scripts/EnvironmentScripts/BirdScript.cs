using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdScript : ClickableObject
{
    [SerializeField] private Animator animator;

    public override void Clicked()
    {
        animator.SetBool("ScareAway", true);
    }

    public void TriggerNextEventChain()
    {
        EventSystem.TriggerNextEventChain();
    }
}
