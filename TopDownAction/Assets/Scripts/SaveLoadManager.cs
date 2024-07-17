using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;   //제이슨 직렬화, 제이슨 역직렬화? 를 위해 사용

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
        // 글로벌 데이터 저장파일이 있으면 로딩 -> 글로벌 데이터 업데이트
        // 없으면 -> 기본값을 글로벌 데이터에 입력하고 파일로 저장
        // 해당 씬이름의 저장파일이 있으면 로딩 -> 씬의 구성 props의 활성화여부 설정
        // 없으면 -> 해당 씬의 관리하는 props를 씬 데이터에 적용하고 활성화여부는 true
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
