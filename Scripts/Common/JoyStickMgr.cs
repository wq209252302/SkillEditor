using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoyStickMgr : SingTon<JoyStickMgr>
{
    public GameObject m_joyGo;
    public ETCJoystick m_joystick;
    public List<ETCButton> m_skillBtn;
    HostPlayer m_target;

   
    public bool JoyAction
    {
        set
        {
            if (m_joyGo.activeSelf!= value)
            {
                m_joyGo.SetActive(value);
            }
        }

    }

    public void SetJoyArg(Camera camera,HostPlayer targer)
    {
        m_target = targer;
        m_joystick.cameraLookAt = targer.m_go.transform;
        m_joystick.cameraTransform = camera.transform;
        SetJoytick();
    }

    Notification notify = new Notification();

    public void SetJoytick()
    {
        m_joystick.OnPressDown.AddListener(() => m_target.JoystickHandlerMoving(m_joystick.axisX.axisValue, m_joystick.axisY.axisValue));
        m_joystick.OnPressLeft.AddListener(() => m_target.JoystickHandlerMoving(m_joystick.axisX.axisValue, m_joystick.axisY.axisValue));
        m_joystick.OnPressRight.AddListener(() => m_target.JoystickHandlerMoving(m_joystick.axisX.axisValue, m_joystick.axisY.axisValue));
        m_joystick.OnPressUp.AddListener(() => m_target.JoystickHandlerMoving(m_joystick.axisX.axisValue, m_joystick.axisY.axisValue));

        m_joystick.onMoveEnd.AddListener(()=> {

            World.Ins.m_player.StopMove();
        });

        if(m_skillBtn.Count!=0 && m_target.m_go)
        {
            foreach (var item in m_skillBtn)
            {
                item.onUp.AddListener(() => {
                    notify.Refresh("Skill",World.Ins.m_player.m_insID, item.name);
                    MsgCenter.Ins.SendMsg("ServerMsg", notify);
                });
               
            }
        }
       
    }




}
