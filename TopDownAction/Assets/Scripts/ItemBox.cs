using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBox : MonoBehaviour
{
    public Sprite openImage;            //열린 이미지
    public GameObject itemPrefad;       //담긴 아이템 프리팹
    public bool isClosed = true;        //true= 닫혀있음, false= 열려있음.
    public int arrangeId = 0;           //배치 식별에 사용


    SaveLoadManager saveLoadManager;   //===저장===


    // Start is called before the first frame update
    void Start()
    {
        saveLoadManager = GameObject.FindObjectOfType<SaveLoadManager>();     //===저장===
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //접촉(물리)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(isClosed && collision.gameObject.tag == "Player")
        {
            //상자가 닫혀 있는 상태에서 플레이어와 접촉
            GetComponent<SpriteRenderer>().sprite = openImage;
            isClosed = false;       //열린 상태로 하기
            if(itemPrefad != null)  //프리펩이 존재하면 만들자
            {
                //프리팹으로 아이템 만들기
                Instantiate(itemPrefad, transform.position, Quaternion.identity);

            }

            saveLoadManager.SetSceneData(this.gameObject.name, false);     //===저장===

        }

    }
}
