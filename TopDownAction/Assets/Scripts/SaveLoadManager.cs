using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;   //제이슨 직렬화, 제이슨 역직렬화? 를 위해 사용 =>에러나면 유니티에서 위도우 메니져에서 제이슨 설치했는지 확인하기
using System.Threading.Tasks;  //비동기를 사용할수 있게 해줌


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
        // 글로벌 데이터 저장파일이 있으면 로딩 -> 글로벌 데이터 업데이트
        // 없으면 -> 코드에 저의된 초기값을 글로벌 데이터에 입력하고 파일로 저장
        // 해당 씬이름의 저장파일이 있으면 로딩 -> 씬의 구성 props의 활성화여부 설정
        // 없으면 -> 해당 씬의 관리하는 props를 씬 데이터에 적용하고 활성화여부는 true -> 저장
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
                if (!obj.isEnabled)  //아니면 비활성화 
                {
                    GameObject target = GameObject.Find(obj.objectName);
                    if(target != null)  //해당 아이템이 있으면,
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
            AddObjectToSceneDate("Item");   //tag 이름
            AddObjectToSceneDate("ItemBox");
            AddObjectToSceneDate("Door");
            SaveSceneData();
*/
            sceneData.scene = SceneManager.GetActiveScene().name;
            AddObjectToSceneData("Item"); // Tag이름
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
                globalData.hp = PlayerController.hp;     //다르면 업데이트를 시켜준다. 대입에 주의한다.
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
            if (dataChanged)  //만약 위의 내용중 바뀐게 있다면 업데이트를 한다.
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
            await SaveGlobalDataAsync();  //저장기능은 비동기 처리.
        }

        async Task SaveGlobalDataAsync()   //Task를 리턴하는 비동기 함수.
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
            await SaveSceneDataAsync();  //저장기능은 비동기 처리.
        }

        async Task SaveSceneDataAsync()   //Task를 리턴하는 비동기 함수.
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
