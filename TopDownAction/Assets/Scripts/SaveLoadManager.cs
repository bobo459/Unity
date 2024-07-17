using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;   //���̽� ����ȭ, ���̽� ������ȭ? �� ���� ���

public class GlobalData
{
    public int arrows;
    public int keys;
    public int hp;

    public GlobalData()
    {
        arrows = 0;
        keys = 0;
        hp = 0;
    }
}

public class SceneDate
{
    public string scene;
    public List<SceneObject> objects;
    public SceneDate()
    {
        objects = new List<SceneObject>();
    }
}

public class SceneObject
{
    public string objectName;
    public bool isEnabled;
}

public class SaveLoadManager : MonoBehaviour
{
    string filePathGlobal;
    public GlobalData globalData;
    public SceneDate sceneDate;


    // Start is called before the first frame update
    void Start()
    {
        // �۷ι� ������ ���������� ������ �ε� -> �۷ι� ������ ������Ʈ
        // ������ -> �⺻���� �۷ι� �����Ϳ� �Է��ϰ� ���Ϸ� ����
        // �ش� ���̸��� ���������� ������ �ε� -> ���� ���� props�� Ȱ��ȭ���� ����
        // ������ -> �ش� ���� �����ϴ� props�� �� �����Ϳ� �����ϰ� Ȱ��ȭ���δ� true
        filePathGlobal = Path.Combine(Application.persisitentDataPath,
            "GlobalData.json");
        if (File.Exists(filePathGlobal))
        {
            LoadGloadDate();
            PlayerController.hp = GlobalData.hp;
            ItemKeeper.hasArrows = globalData.arrows;
            ItemKeeper.hasKeys = globalData.keys;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadGlobalDate()
    {
        if (File.Exists(filePathGlobal))
        {
            string jsonDate = File.ReadAllText(filePathGlobal);
            globalData = jsonConvert.DesrializeObject<GlobalData>(jsonDate);
            Debug.Log("Data loaded from" + filePathGlobal);
        }
    }
}
