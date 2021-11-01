using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Anim : SkillBase
{
    Player player;
    Animator anim;
    public AnimationClip Clip;

    public AnimatorStateInfo animInfo, lastinfo;
    AnimatorOverrideController overrideController;

    public Skill_Anim(Player play)
    {
        player = play;
        overrideController = player.overrideController;
        anim = player.gameObject.GetComponent<Animator>();

    }

    public override void Init()
    {
        base.Init();
        overrideController["Start"] = Clip;
       
    }

    public override void Play()
    {
        base.Play();

        //if()
    }

    public override void Stop()
    {
        base.Stop();
        anim.StopPlayback();
    }

    public void SetAnimClip(AnimationClip _clip)
    {
        
        Clip = _clip;
        name = Clip.name;
       
      
    }


    public void SetAnimSpeed(float speed)
    {
        anim.speed = speed;
    }

    public override void Bgein()
    {
        anim.StopPlayback();
        overrideController["Start"] = Clip;
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("Idle1"))
        {
            anim.SetTrigger("Play");
        }
        
         //animatorInfo.normalizedTime

    }

    public override void Update(float timer)
    {
        base.Update(timer);
        animInfo = anim.GetCurrentAnimatorStateInfo(0);


       

    }
}
