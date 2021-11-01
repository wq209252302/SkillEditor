using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Newtonsoft.Json;
using System.IO;

public class MonsterEditor : EditorWindow
{

    //public static List<GameObject> MonsterPrefabs = new List<GameObject>();
    public static List<GameObject> MonsterPrefabs = new List<GameObject>();
    public static List<MondelsType> modelTypes = new List<MondelsType>();
    public static List<MondelsType> modelTypesSave = new List<MondelsType>();
    public static GameObject NpcRoot;
    string[] type = new string[] { "Null", "Normal", "Gatther", "Biaoche", "NPC" };


    [MenuItem("Tools/怪物编辑器 _%w")]
    public static void OpenWindow()
    {
        MonsterPrefabs.Clear();
        modelTypes.Clear();
        modelTypesSave.Clear();
        var win = EditorWindow.GetWindow<MonsterEditor>("怪物编辑器");

        if (win != null)
        {
            win.Show();

            NpcRoot = GameObject.Find("NpcRoot");

            if (NpcRoot == null)
            {
                Debug.LogError("npc父物体为空");
            }
            else
            {

                //读取本地文件
                LoadNpcList();

                //获取父物体下面所有的子物体

                //string monstername;
                //for (int i = 0; i < NpcRoot.transform.childCount; i++)
                //{
                //    if (NpcRoot.transform.GetChild(i).name.Contains("("))
                //    {
                //        monstername = NpcRoot.transform.GetChild(i).name.Split('(')[0];
                //    }
                //    else
                //    {
                //        monstername = NpcRoot.transform.GetChild(i).name;
                //    }
                //    MonsterPrefabs.Add(NpcRoot.transform.GetChild(i).gameObject);
                //    MondelsType mondels = new MondelsType(true, monstername, 0, NpcRoot.transform.GetChild(i).position, NpcRoot.transform.GetChild(i).eulerAngles);
                //    modelTypes.Add(mondels);
                //    modelTypesSave.Add(mondels);
                //}

            }
        }

    }

    Vector2 v2 = new Vector2(0,0);

    private void OnGUI()
    {
        GUILayout.BeginVertical();
      
        if (GUILayout.Button("SAVE"))
        {
            SaveData();
        }
        GUILayout.Space(1f);
        GUILayout.BeginScrollView(v2);
        foreach (var item in modelTypes)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(item.name);
            item.Toggle = GUILayout.Toggle(item.Toggle, "");
            GUILayout.Label("类型");
            item.TypeIndex = EditorGUILayout.Popup(item.TypeIndex, type);
            GUILayout.EndHorizontal();
        }
        GUILayout.EndScrollView();

        

        GUILayout.EndVertical();
    }


    private void OnDisable()
    {

        for (int i = MonsterPrefabs.Count-1; i >=0; i--)
        {
            Destroy(MonsterPrefabs[i]);
        }
        MonsterPrefabs.Clear();
    }

    private void Update()
    {
        if (NpcRoot.transform.childCount != modelTypes.Count)
        {
            MonsterPrefabs.Clear();
            modelTypes.Clear();
            string monstername;
            //int index = 0;
            for (int i = 0; i < NpcRoot.transform.childCount; i++)
            {
                if (NpcRoot.transform.GetChild(i).name.Contains("("))
                {
                    monstername = NpcRoot.transform.GetChild(i).name.Split('(')[0];
                }
                else
                {
                    monstername = NpcRoot.transform.GetChild(i).name;
                }


                MonsterPrefabs.Add( NpcRoot.transform.GetChild(i).gameObject);

                MondelsType mondels = new MondelsType(true, monstername, BackIndex(i), NpcRoot.transform.GetChild(i).position, NpcRoot.transform.GetChild(i).eulerAngles);
             //   index += 1;
                modelTypes.Add(mondels);



                //modelTypesSave.Add(mondels);
            }
        }
    }


    int BackIndex(int index)
    {
        if(index>=modelTypesSave.Count)
        {
            return 0;
        }
        else
        {
            return modelTypesSave[index].TypeIndex;
        }
    }

    MondelsType Select(string name,List<MondelsType> list)
    {
        foreach (var item in list)
        {
            if (item.name == name)
                return item;
        }
        return null;
    }



    void SaveData()
    {
        modelTypesSave.Clear();
        string monstername;
         int index = 0;
        foreach (var item in MonsterPrefabs)
        {
            // monstername = item.name;
            if (item.name.Contains("("))
            {
                monstername = item.name.Split('(')[0];
            }
            else
            {
                monstername = item.name;
            }
            MondelsType mondels = new MondelsType(true, monstername,modelTypes[index].TypeIndex, item.transform.position, item.transform.eulerAngles);
            index += 1;
            modelTypesSave.Add(mondels);
        }
        string str = JsonConvert.SerializeObject(modelTypesSave);
        File.WriteAllText("Assets/NpcList.txt", str);
        File.WriteAllText(Application.streamingAssetsPath + @"/monster.json",str);

        Dictionary<int, TaksBase> dic = new Dictionary<int, TaksBase>();


        TaksBase a = new TaksBase(1, 3, 0, 100, TaskType.gather);
        TaksBase b = new TaksBase(2, 1, 0, 99, TaskType.atk);
        dic.Add(1, a);
        dic.Add(2, b);
        str = JsonConvert.SerializeObject(dic);
        File.WriteAllText("Assets/TaskList.txt", str);

    }



    static void LoadNpcList()
    {
        if (File.Exists("Assets/NpcList.txt"))
        {
            string str = File.ReadAllText("Assets/NpcList.txt");

            modelTypesSave = JsonConvert.DeserializeObject<List<MondelsType>>(str);

            foreach (var item in modelTypesSave)
            {


                GameObject obj = Instantiate(Resources.Load<GameObject>(item.name), NpcRoot.transform);
                obj.transform.position = new Vector3(item.pos.x, item.pos.y, item.pos.z);
                obj.transform.eulerAngles = new Vector3(item.rot.x, item.rot.y, item.rot.z);
                MonsterPrefabs.Add(obj);
                modelTypes.Add(item);

            }

        }
        // string str = File.ReadAllText();
    }



}
