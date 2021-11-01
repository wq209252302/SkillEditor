using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEngine.Animations;
using Newtonsoft.Json;

public class Player : MonoBehaviour
{
    public Animator anim;
    Skill_Anim _anim;

    RuntimeAnimatorController controller;
    public AnimatorOverrideController overrideController;

    public Transform effectsparent;

    AudioSource audioSource;
    AnimationClip _clip;

    public bool isway = false;

    public bool isMove = false;

    public Transform target;

    public Dictionary<string, List<SkillBase>> Components = new Dictionary<string, List<SkillBase>>();

    public List<SkillBase> hostSkills = new List<SkillBase>();


    private void Awake()
    {
        anim = gameObject.GetComponent<Animator>();
        _clip = AssetDatabase.LoadAssetAtPath<AnimationClip>("Assets/GameDate/Anim/Run1.anim");
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    //角色初始化
    public void InitData()
    {
        overrideController = new AnimatorOverrideController();
        controller = Resources.Load<RuntimeAnimatorController>("Player1");
        overrideController.runtimeAnimatorController = controller;
        anim.runtimeAnimatorController = overrideController;

        audioSource = gameObject.AddComponent<AudioSource>();
        effectsparent = transform.Find("effectsparent");
    }

    public void SetData(string skillName)
    {
        List<SkillXml> skillList = GameData.Ins.GetSkillsByRoleName("boss_maoyou");
        hostSkills.Clear();
        foreach (var item in skillList)
        {
            if (item.name == skillName)
            {
                foreach (var ite in item.skills)
                {
                    foreach (var it in ite.Value)
                    {
                        if (ite.Key.Equals("动画"))
                        {
                            AnimationClip _clip = AssetDatabase.LoadAssetAtPath<AnimationClip>("Assets/GameDate/Anim/" + it.name + ".anim");
                            Skill_Anim _Anim = new Skill_Anim(this);
                            _Anim.tirgger = it.tirgger;
                            _Anim.SetAnimClip(_clip);
                            hostSkills.Add(_Anim);
                            //_anim.

                        }
                        else if (ite.Key.Equals("声音"))
                        {
                            AudioClip _clip = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/GameData/Audio/" + it.name + ".mp3");
                            Skill_Audio _Aduio = new Skill_Audio(this);
                            _Aduio.tirgger = it.tirgger;
                            _Aduio.SetAudioClip(_clip);
                            hostSkills.Add(_Aduio);
                        }
                        else if (ite.Key.Equals("特效"))
                        {
                            GameObject _clip = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/GameData/Effect" + it.name + ".prefab");
                            Skill_Effecter _Effect = new Skill_Effecter(this);
                            _Effect.tirgger = it.tirgger;
                            _Effect.SetEffectClip(_clip);
                            hostSkills.Add(_Effect);

                        }
                    }
                }
            }
        }
    }
    //private Skill_Anim _Anim;
    //private Skill_Audio _Aduio;
    //private Skill_Effecter _Effect;

    public void Play()
    {
        anim.SetBool("isRun", false);
        foreach (var item in hostSkills)
        {

            item.Play();
        }
        // anim.SetBool("isRun", true);

    }


    //编译器使用
    public static Player Init(string name)
    {
        if (name != null)
        {
            string path = "Assets/Prefabs/" + name + ".prefab";
            GameObject obj = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (obj != null)
            {

                Player player = Instantiate(obj).GetComponent<Player>();
                if (player == null)
                {
                    player = obj.AddComponent<Player>();
                    player.anim = obj.GetComponent<Animator>();
                }



                player.overrideController = new AnimatorOverrideController();
                player.controller = Resources.Load<RuntimeAnimatorController>("Player");
                player.overrideController.runtimeAnimatorController = player.controller;
                player.anim.runtimeAnimatorController = player.overrideController;

                player.audioSource = player.gameObject.AddComponent<AudioSource>();
                player.effectsparent = player.transform.Find("effectsparent");

                player.gameObject.name = name;

                player.LoadSkills();
                return player;



            }

        }

        return null;
        // 
    }

    void LoadSkills()
    {

        if (File.Exists("Assets/" + gameObject.name + ".txt"))
        {
            string str = File.ReadAllText("Assets/" + gameObject.name + ".txt");
            List<SkillXml> skills = JsonConvert.DeserializeObject<List<SkillXml>>(str);

            foreach (var item in skills)
            {
                Components.Add(item.name, new List<SkillBase>());
                foreach (var ite in item.skills)
                {
                    foreach (var it in ite.Value)
                    {
                        if (ite.Key.Equals("动画"))
                        {
                            AnimationClip _clip = AssetDatabase.LoadAssetAtPath<AnimationClip>("Assets/GameDate/Anim/" + it.name + ".anim");
                            Skill_Anim _anim = new Skill_Anim(this);
                            _anim.tirgger = it.tirgger;
                            _anim.SetAnimClip(_clip);
                            Components[item.name].Add(_anim);
                            //_anim.

                        }
                        else if (ite.Key.Equals("声音"))
                        {
                            AudioClip _clip = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/GameData/Audio/" + it.name + ".mp3");
                            Skill_Audio audio = new Skill_Audio(this);
                            audio.tirgger = it.tirgger;
                            audio.SetAudioClip(_clip);
                            Components[item.name].Add(audio);
                        }
                        else if (ite.Key.Equals("特效"))
                        {
                            GameObject _clip = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/GameData/Effect" + it.name + ".prefab");
                            Skill_Effecter effecter = new Skill_Effecter(this);
                            effecter.tirgger = it.tirgger;
                            effecter.SetEffectClip(_clip);
                            Components[item.name].Add(effecter);

                        }
                    }
                }
            }


        }
    }

    public void SaveSkillsData()
    {
        List<SkillXml> Datas = new List<SkillXml>();
        foreach (var item in Components)
        {
            SkillXml data = new SkillXml();
            data.name = item.Key;

            foreach (var ite in item.Value)
            {
                if (ite is Skill_Anim)
                {
                    if (!data.skills.ContainsKey("动画"))
                    {
                        data.skills.Add("动画", new List<ComponentData>());
                    }
                    data.skills["动画"].Add(new ComponentData(ite.name, ite.tirgger));
                }
                else if (ite is Skill_Anim)
                {
                    if (!data.skills.ContainsKey("音效"))
                    {
                        data.skills.Add("音效", new List<ComponentData>());
                    }
                    data.skills["音效"].Add(new ComponentData(ite.name, ite.tirgger));
                }
                else if (ite is Skill_Anim)
                {
                    if (!data.skills.ContainsKey("特效"))
                    {
                        data.skills.Add("特效", new List<ComponentData>());
                    }
                    data.skills["特效"].Add(new ComponentData(ite.name, ite.tirgger));
                }
            }
            Datas.Add(data);
        }

        string str = JsonConvert.SerializeObject(Datas);
        File.WriteAllText("Assets/" + gameObject.name + ".txt", str);
    }


    public List<SkillBase> AddNewSkill(string name)
    {
        if (Components.ContainsKey(name))
        {
            return Components[name];
        }

        Components.Add(name, new List<SkillBase>());

        return Components[name];
    }


    public void RevSkill(string name)
    {
        if (Components.ContainsKey(name))
        {
            Components.Remove(name);
        }
    }

    public List<SkillBase> GetSkill(string name)
    {
        if (Components.ContainsKey(name))
        {
            return Components[name];
        }

        return null;
    }

    private void Update()
    {

        foreach (var ite in hostSkills)
        {
            ite.Update(Time.time);
        }

        if (UIMgr.Ins != null)
        {
            foreach (var item in UIMgr.Ins.Updates)
            {
                item?.Invoke(Time.time);
            }
        }
        if (isMove)
        {
            _anim.Update(Time.time);

        }

        if(isway)
        {
            World.Ins.m_player.m_go.transform.position = Vector3.MoveTowards(World.Ins.m_player.m_go.transform.position,target.position,3*Time.deltaTime);
           // _anim.Update(Time.time);
            World.Ins.m_player.m_go.transform.LookAt(target.position);
        }


    }
    Notification notify = new Notification();

    public void Atk(int atk)
    {
        foreach (var item in World.Ins.enemys.Values)
        {
            if (item != null)
            {
                if (item.Hit())
                {

                    notify.Refresh("Hit", World.Ins.m_player.m_insID, item.m_insID, atk);
                    MsgCenter.Ins.SendMsg("ServerMsg", notify);
                }
            }
        }
    }

    public void Run()
    {
        anim.SetBool("isRun", false);

        _anim = new Skill_Anim(this);
        _anim.SetAnimClip(_clip);

        _anim.Play();
        isway = false;
    }


    public void motionWay(int index)
    {
        isway = true;
        target = World.Ins.m_insDic[index].m_go.transform;


    }
}

public class SkillXml
{
    public string name;
    public Dictionary<string, List<ComponentData>> skills = new Dictionary<string, List<ComponentData>>();
}


public class ComponentData
{
    public string name;
    public string tirgger;
    public ComponentData(string _name, string _tirgger)
    {
        name = _name;
        tirgger = _tirgger;

    }
}