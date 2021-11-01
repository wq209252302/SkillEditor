using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SkillWindow : EditorWindow
{

    Player player;

    List<SkillBase> SkillComponents;
    float currSpeed = 1;
    float animSpeed = 1;

    string ComponentName = string.Empty;
    int TypeIndex = 0;


    string[] Type = new string[] { "null", "动画", "声音", "特效" };

    Vector2 Vector2 = new Vector2(0, 0);

    public void Init(Player _player, List<SkillBase> skills)
    {
        player = _player;
        SkillComponents = skills;
        currSpeed = 1;
    }


    private void OnGUI()
    {
        GUILayout.BeginVertical();

        if (GUILayout.Button("Save"))
        {
            player.SaveSkillsData();
        }

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("播放"))
        {
            foreach (var item in SkillComponents)
            {
                item.Play();
            }
        }

        if (GUILayout.Button("停止"))
        {
            foreach (var item in SkillComponents)
            {
                item.Stop();
            }
        }


        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();

        GUILayout.Label("播放速度");
        currSpeed = EditorGUILayout.Slider(currSpeed, 0, 5);

        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();

        GUILayout.Label("时间速度");
        animSpeed = EditorGUILayout.Slider(animSpeed, 0, 5);

        GUILayout.EndHorizontal();

        ComponentName = GUILayout.TextField(ComponentName);
        GUILayout.BeginHorizontal();
        TypeIndex =  EditorGUILayout.Popup(TypeIndex,Type);
        
        if (GUILayout.Button("添加"))
        {
            switch (TypeIndex)
            {
                case 1:
                    SkillComponents.Add(new Skill_Anim(player));
                    break;
                case 2:
                    SkillComponents.Add(new Skill_Audio(player));
                    break;
                case 3:
                    SkillComponents.Add(new Skill_Effecter(player));
                    break;
            }
        }

        GUILayout.EndHorizontal();

        Vector2 = GUILayout.BeginScrollView(Vector2, false, true);
        foreach (var item in SkillComponents)
        {
            GUILayout.BeginHorizontal();

            GUILayout.Label(item.name);
            if(GUILayout.Button("删除"))
            {
                SkillComponents.Remove(item);
                break;
            }

            GUILayout.EndHorizontal();

            if (item is Skill_Anim)
            {
                ShowSkill_Anim(item as Skill_Anim);
            }
            else if (item is Skill_Audio)
            {
                ShowSkill_Audio(item as Skill_Audio);
            }
            else if (item is Skill_Effecter)
            {
                ShowSkill_Effects(item as Skill_Effecter);
            }
            GUILayout.Space(0.5f);
        }





        GUILayout.EndScrollView();

        GUILayout.EndVertical();
    }

    void ShowSkill_Anim(Skill_Anim _Anim)
    {
        _Anim.tirgger = GUILayout.TextArea(_Anim.tirgger);
        AnimationClip clip = EditorGUILayout.ObjectField(_Anim.Clip,typeof(AnimationClip),false) as AnimationClip;
        if(_Anim.Clip!=clip)
        {
            _Anim.SetAnimClip(clip);
        }
    }

    void ShowSkill_Audio(Skill_Audio _audio)
    {
        _audio.tirgger = GUILayout.TextArea(_audio.tirgger);
        AudioClip audioClip = EditorGUILayout.ObjectField(_audio.Clip, typeof(AudioClip), false) as AudioClip;
        if (_audio.Clip != audioClip)
        {
            _audio.SetAudioClip(audioClip);
        }
    }

    void ShowSkill_Effects(Skill_Effecter _effects)
    {
        _effects.tirgger = GUILayout.TextArea(_effects.tirgger);
        GameObject effectsClip = EditorGUILayout.ObjectField(_effects.Clip, typeof(GameObject), false) as GameObject;
        if (_effects.Clip != effectsClip)
        {
            _effects.SetEffectClip(effectsClip);
        }
    }
}
