using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Audio : SkillBase
{
    public AudioClip Clip;
    AudioSource source;
     Player player;

    public Skill_Audio(Player _play)
    {
        player = _play;
        source = _play.gameObject.GetComponent<AudioSource>();

    }

    public override void Init()
    {
        base.Init();
        source.clip = Clip;
    }

    public override void Play()
    {
        base.Play();
       
    }

    public override void Stop()
    {
        base.Stop();
        source.Stop();
    }

    public void SetAudioClip(AudioClip clip)
    {
        Clip = clip;
        name = Clip.name;
 
    }

    public override void Bgein()
    {
        if(source!=null)
        {
            source.clip = Clip;
            source.Play();
        }
    }

    public override void Update(float timer)
    {
        base.Update(timer);
    }
}
