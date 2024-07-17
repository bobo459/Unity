using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    arrow,
    key,
    life,
}


public class ItemData : MonoBehaviour
{
    public ItemType type;       //아이템 종류
    public int count = 1;       // 아이템 수

    public int arrangId = 0;    //식별을 위한 값


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
    private void OnTriggerEnter2D ( Collider2D collision )
    {
        if(collision.gameObject.tag == "Player")
        {
            if(type == ItemType.key)
            {
                //열쇠
                ItemKeeper.hasKeys += 1;
            }
            else if(type == ItemType.arrow)
            {
                //화살
                ArrowShoot shoot = collision.gameObject.GetComponent<ArrowShoot>();
                ItemKeeper.hasArrows += count;
            }
            else if(type == ItemType.life)
            {
                if (PlayerController.hp < 3)
                {
                    //HP가 3이하면 추가
                    PlayerController.hp++;
                }
            }

            saveLoadManager.SetSceneData(this.gameObject.name, false);     //===저장===



            //+++++아이템 획득 연출+++++
            //충돌 판정 비활성
            gameObject.GetComponent<CircleCollider2D>().enabled = false;
            //아이템의 Rigidbody2D 가져오기
            Rigidbody2D itemBody = GetComponent<Rigidbody2D>();
            //중력적용
            itemBody.gravityScale = 2.5f;
            //위로 튀어오르는 연출
            itemBody.AddForce(new Vector2(0, 6), ForceMode2D.Impulse);
            //0.5초 뒤에 제거
            Destroy(gameObject, 0.5f);
        }
    }
}
