using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Effecter : SkillBase
{
    Player player;
    ParticleSystem particleSystem;
    public GameObject Clip;
     GameObject obj;

    public Skill_Effecter(Player _player)
    {
        player = _player;

    }


    public void SetEffectClip(GameObject _Clip)
    {
        Clip = _Clip;
        if(Clip.GetComponent<ParticleSystem>())
        {
            obj = GameObject.Instantiate(Clip,player.effectsparent);
           
        }
    }

    public override void Init()
    {
        base.Init();
        if (Clip.GetComponent<ParticleSystem>())
        {
            particleSystem = obj.GetComponent<ParticleSystem>();
            particleSystem.Stop();
        }
    }

    public override void Play()
    {
        base.Play();
       
    }

    public override void Stop()
    {
        base.Stop();
        if(particleSystem!=null)
        {
            particleSystem.Stop();
        }
    }

    public override void Bgein()
    {
        if (particleSystem != null)
        {
            particleSystem = obj.GetComponent<ParticleSystem>();
            particleSystem.Stop();
            particleSystem.Play();
        }
    }

    public override void Update(float timer)
    {
        base.Update(timer);
    }
}
