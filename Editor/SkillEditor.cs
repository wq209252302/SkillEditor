using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
public class PlayerEditor
{
    public int characterIndex = 0;
    public int folderIndex = 0;
    public string characterName = string.Empty;
    public string folderName = string.Empty;
    public Player player = null;
}

public class SkillEditor : EditorWindow
{
    int _characterIndex = 0;
    int _folderIndex = 0;
    PlayerEditor m_play = new PlayerEditor();
    string newskill = string.Empty;


    List<string> CharacterList = new List<string>();


    [MenuItem("Tools/技能编辑器 _%q")]
    public static void OpenWindow()
    {
        if (Application.isPlaying)
        {
            var win = EditorWindow.GetWindow<SkillEditor>("技能编辑器");
            if (win != null)
            {
                win.Show();
            }
        }
    }

    private void OnEnable()
    {
        SearchCharacter();
    }

    private void OnGUI()
    {

        GUILayout.BeginVertical();
        GUILayout.Label("请选择");
        _characterIndex = EditorGUILayout.Popup(m_play.characterIndex, CharacterList.ToArray());

        if (_characterIndex != m_play.characterIndex)
        {
            m_play.characterIndex = _characterIndex;
            m_play.characterName = CharacterList[m_play.characterIndex];
            if (m_play.player != null)
            {
                m_play.player.Destroy();
            }
            m_play.player = Player.Init(m_play.characterName);


        }


        newskill = GUILayout.TextField(newskill);

        if (GUILayout.Button("创建"))
        {
            if (m_play.player != null)
            {
                m_play.player.AddNewSkill(newskill);
            }

        }

        if (m_play.player != null)
        {
            foreach (var item in m_play.player.Components)
            {
                if (GUILayout.Button(item.Key))
                {

                    List<SkillBase> components = m_play.player.GetSkill(item.Key);
                    foreach (var ite in components)
                    {
                        ite.Init();
                    }

                    var win = EditorWindow.GetWindow<SkillWindow>(item.Key);
                    win.Init(m_play.player, components);
                    m_play.player.hostSkills = components;
                    win.Show();
                    win.Repaint();
                }
            }
        }


        GUILayout.EndVertical();
    }

    string GetDadaPath()
    {
        return "Assets/Prefabs";
    }




    void SearchCharacter()
    {
        CharacterList.Clear();
        string[] strs = Directory.GetFiles(GetDadaPath(), "*.prefab", SearchOption.AllDirectories);
        foreach (var item in strs)
        {
            CharacterList.Add(Path.GetFileNameWithoutExtension(item));
        }
        CharacterList.Insert(0, "null");

    }


}
