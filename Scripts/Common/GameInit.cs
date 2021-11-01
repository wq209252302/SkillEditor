using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameInit : MonoBehaviour
{
    public GameObject[] DonDesTory;
    public List<ETCButton> Attack;
    public ETCJoystick Joystick;
    public GameObject uiroot;


    private void Start()
    {
        for (int i = 0; i < DonDesTory.Length; i++)
        {
            GameObject.DontDestroyOnLoad(DonDesTory[i]);
        }
        GameSceneUtils.LoadSceneAsync("Lobby",()=>
        {
            JoyStickMgr.Ins.m_joyGo = DonDesTory[0];
            JoyStickMgr.Ins.m_joystick = Joystick;
            JoyStickMgr.Ins.m_skillBtn = Attack;
            GameData.Ins.InitByRoleName("boss_maoyou");
            // GameData.Ins.ini

            World.Ins.Init();
        }
        );
        
    }

 
}


static public class GameSceneUtils
{
    static public void LoadSceneAsync(string sceneName, Action call)
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(sceneName);
        ao.completed += (_ao) => {
            call?.Invoke();
        };
    }
}



