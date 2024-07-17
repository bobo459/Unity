using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;   //���̽� ����ȭ, ���̽� ������ȭ? �� ���� ��� =>�������� ����Ƽ���� ������ �޴������� ���̽� ��ġ�ߴ��� Ȯ���ϱ�
using System.Threading.Tasks;  //�񵿱⸦ ����Ҽ� �ְ� ����


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

public class SceneData
{
    public string scene;
    public List<SceneObject> objects;
    public SceneData()
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
    string filePathScene;
    public GlobalData globalData = new GlobalData();
    public SceneData sceneData = new SceneData();

    float checkInterval = 0.2f;
    float tempTime = 0;


    // Start is called before the first frame update
    void Start()
    {
        // �۷ι� ������ ���������� ������ �ε� -> �۷ι� ������ ������Ʈ
        // ������ -> �ڵ忡 ���ǵ� �ʱⰪ�� �۷ι� �����Ϳ� �Է��ϰ� ���Ϸ� ����
        // �ش� ���̸��� ���������� ������ �ε� -> ���� ���� props�� Ȱ��ȭ���� ����
        // ������ -> �ش� ���� �����ϴ� props�� �� �����Ϳ� �����ϰ� Ȱ��ȭ���δ� true -> ����
        filePathGlobal = Path.Combine(Application.persistentDataPath
            , "GlobalData.json");
        if (File.Exists(filePathGlobal))
        {
            LoadGlobalData();
            PlayerController.hp = globalData.hp;
            ItemKeeper.hasArrows = globalData.arrows;
            ItemKeeper.hasKeys = globalData.keys;
        }
        else
        {
            globalData.hp = PlayerController.hp;
            globalData.arrows = ItemKeeper.hasArrows;
            globalData.keys = ItemKeeper.hasKeys;
            SaveGlobalData();
        }



        filePathScene = Path.Combine(Application.persistentDataPath
            , SceneManager.GetActiveScene().name + ".json");
        if (File.Exists(filePathScene))
        {
            LoadSceneData();
            foreach ( SceneObject obj in sceneData.objects)
            {
                if (!obj.isEnabled)  //�ƴϸ� ��Ȱ��ȭ 
                {
                    GameObject target = GameObject.Find(obj.objectName);
                    if(target != null)  //�ش� �������� ������,
                    {
                        if(target.GetComponent<ItemBox>() != null)
                        {
                            target.GetComponent<ItemBox>().isClosed = false;
                        }
                        else
                        {
                            target.SetActive(false);
                        }
                       
                    }
                }
            }

        }
        else
        {
/*            sceneData.scene = SceneManager.GetActiveScene().name;
            AddObjectToSceneDate("Item");   //tag �̸�
            AddObjectToSceneDate("ItemBox");
            AddObjectToSceneDate("Door");
            SaveSceneData();
*/
            sceneData.scene = SceneManager.GetActiveScene().name;
            AddObjectToSceneData("Item"); // Tag�̸�
            AddObjectToSceneData("ItemBox");
            AddObjectToSceneData("Door");
            SaveSceneData();

        }

    }

    // Update is called once per frame
    void Update()
    {
        tempTime += Time.deltaTime;
        if(tempTime> checkInterval)
        {
            bool dataChanged = false;
            if (globalData.hp != PlayerController.hp)
            {
                globalData.hp = PlayerController.hp;     //�ٸ��� ������Ʈ�� �����ش�. ���Կ� �����Ѵ�.
                dataChanged = true;
            }
            if (globalData.arrows != ItemKeeper.hasArrows)
            {
                globalData.arrows = ItemKeeper.hasArrows;
                dataChanged = true;
            }
            if (globalData.keys != ItemKeeper.hasKeys)
            {
                globalData.keys = ItemKeeper.hasKeys;
                dataChanged = true;
            }
            if (dataChanged)  //���� ���� ������ �ٲ�� �ִٸ� ������Ʈ�� �Ѵ�.
            {
                SaveGlobalData();
            }

            tempTime = 0;
        }
      

    }

    public void LoadGlobalData()
    {

        if (File.Exists(filePathGlobal))
        {
            string jsonDate = File.ReadAllText(filePathGlobal);
            globalData = JsonConvert.DeserializeObject<GlobalData>(jsonDate);
            Debug.Log("Data loaded from" + filePathGlobal);
        }

    }




    /*   public void SaveGlobalData()
    {
             string jsonData = JsonConvert.SerializeObject(globalData
                    ,Formatting.Indented);
                File.WriteAllText(filePathGlobal, jsonData);
                Debug.Log("Data saved to " + filePathGlobal);
    }*/
        public async void SaveGlobalData()
        {
            await SaveGlobalDataAsync();  //�������� �񵿱� ó��.
        }

        async Task SaveGlobalDataAsync()   //Task�� �����ϴ� �񵿱� �Լ�.
        {
            string jsonData = JsonConvert.SerializeObject(globalData
            , Formatting.Indented);
            using (StreamWriter writer = new StreamWriter(filePathGlobal))
            {
                await writer.WriteAsync(jsonData);
            }
            Debug.Log("Data saved to " + filePathGlobal);
        }
    



    public void LoadSceneData()
    {
            string jsonData = File.ReadAllText(filePathScene);
            sceneData = JsonConvert.DeserializeObject<SceneData>(jsonData);
            Debug.Log("Data loaded from" + filePathScene);
    }

    /*public void SaveSceneData()
    {
                string jsonData = JsonConvert.SerializeObject(sceneData
                    , Formatting.Indented);
                File.WriteAllText(filePathScene, jsonData);
                Debug.Log("Data saved to " + filePathScene);
        }
        */
        public async void SaveSceneData()
        {
            await SaveSceneDataAsync();  //�������� �񵿱� ó��.
        }

        async Task SaveSceneDataAsync()   //Task�� �����ϴ� �񵿱� �Լ�.
        {
            string jsonData = JsonConvert.SerializeObject(sceneData
            , Formatting.Indented);
            using (StreamWriter writer = new StreamWriter(filePathScene))
            {
                await writer.WriteAsync(jsonData);
            }
            Debug.Log("Data saved to " + filePathScene);
        }
    


    public void AddObjectToSceneData(string tag)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject obj in objects)
        {
            SceneObject sceneObject = new SceneObject();
            sceneObject.objectName = obj.name;
            sceneObject.isEnabled = obj.activeSelf;
            sceneData.objects.Add(sceneObject);
        }
    }

    public void SetSceneData(string name, bool value)
    {
        bool dataChanged = false;
        foreach (SceneObject obj in sceneData.objects)
        {
            if(obj.objectName == name)
            {
                obj.isEnabled = value;
                dataChanged = true;
            }
        
            if(dataChanged)
            {
            SaveSceneData();
            }
        }
    }
}
