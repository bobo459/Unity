using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;


// 전처리 지시어
// #if
// #endif

// #if
// #elseif
// #endif

// MonoBehaviour != ScriptableObject 둘은 다르다
// MonoBehaviour : 인스펙스 창에 등록할수 있다. 함수 입력을 할수 있다.
// ScriptableObject : 자료저장만 가능. 자료저장클래스,
                    //메모리에 저장한다.(영구저장능력 없음/프로그램을 끄면 영구저장능력이 없어서 지워진다.)
                    //프리팹을 저장하여 사용. 
                    //영구저장하려면 json 파일로 저장해야함
                    //주로 미리 생성해놓고 사용함(런타임 생성 X)

#if USE_SCRIPTABLE_OBJECT
public class SaveLoadManager : MonoBehaviour
// 싱글턴 - SaveLoadManager
// 로드시점은
// Global Data 는 최초 1회(Start 함수)
// SceneData는 씬이 로드될때마다 (OnLoaded 함수)
// 세이브시점은
// 두 종류 데이터 모두 1)씬이 바뀔때 2)종료할때
{
    public static SaveLoadManager Instance { get; private set; }

    public GlobalData globalData;   //ScriptableObject 용이다
    public SceneData sceneData;     //ScriptableObject 용이다

    float checkInterval = 0.2f;
    float tempTime = 0;
    string globalDataPath;
    string sceneDataPath;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Debug.Log("Game started");
        globalDataPath = Path.Combine(Application.persistentDataPath, "GlobalData.json");
        if (File.Exists(globalDataPath))
        {
            // json파일이 있으면 ScriptableObject의 값 업데이트
            globalData = LoadData<GlobalData>(globalDataPath);
            PlayerController.hp = globalData.hp;
            ItemKeeper.hasArrows = globalData.arrows;
            ItemKeeper.hasKeys = globalData.keys;
        }   
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    //씬데이터를 OnSceneLoaded 로 불릴때 이것을 사용하여서 부르겠다.
    {
        Debug.Log($"Scene loaded: {scene.name}");
        sceneData = Resources.Load<SceneData>($"Record/{scene.name}Data");
        if (sceneData == null)
        {
            return;
        }
        sceneDataPath = Path.Combine(Application.persistentDataPath, scene.name + ".json");
        if (File.Exists(sceneDataPath))
        {
            sceneData = LoadData<SceneData>(sceneDataPath);
            foreach (SceneObject obj in sceneData.objects)
            {
                GameObject target = GameObject.Find(obj.objectName);
                if (target != null)
                {
                    if (!obj.isEnabled)
                    {
                        if (target.GetComponent<ItemBox>() != null)
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
            sceneData.objects.Clear();
            AddObjectsToPropsData("ItemBox");
            AddObjectsToPropsData("Door");
            AddObjectsToPropsData("Item");
        }            
    }

    void OnSceneUnloaded(Scene scene)
    {
        Debug.Log($"Scene unloaded: {scene.name}");
        SaveData<SceneData>(sceneData, sceneDataPath);
        SaveData<GlobalData>(globalData, globalDataPath);
    }
    
    void Update()
        //저장을 미리 하고 있기때문에 저장문구를 따로 써주지 않는다.
    {
        tempTime += Time.deltaTime;
        if (tempTime > checkInterval)
        {
            if (globalData.hp != PlayerController.hp)
            {
                globalData.hp = PlayerController.hp;
            }
            if (globalData.arrows != ItemKeeper.hasArrows)
            {
                globalData.arrows = ItemKeeper.hasArrows;
            }
            if (globalData.keys != ItemKeeper.hasKeys)
            {
                globalData.keys = ItemKeeper.hasKeys;
            }
            tempTime = 0;
        }
    }

    public T LoadData<T>(string filePath)
    {
        string jsonData = File.ReadAllText(filePath);
        T data = JsonConvert.DeserializeObject<T>(jsonData); 
        Debug.Log("Data loaded from " + filePath);
        return data;
    }

    public void SaveData<T>(T data, string filePath)
    {
        string jsonData = JsonConvert.SerializeObject(data, Formatting.Indented);
        File.WriteAllText(filePath, jsonData);
        Debug.Log("Data saved to " + filePath);
    }

    void AddObjectsToPropsData(string tag)
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
        foreach (SceneObject obj in sceneData.objects)
        {
            if (obj.objectName == name)
            {
                obj.isEnabled = value;
            }
        }
    }

    void OnDestroy()
    {
        // 이벤트 등록 해제
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    void OnApplicationQuit()
    {
        Debug.Log("Game ended");
        SaveData<SceneData>(sceneData, sceneDataPath);
        SaveData<GlobalData>(globalData, globalDataPath);
    }
}
#endif