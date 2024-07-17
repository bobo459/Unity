using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    //HP
    public int hp = 3;
    //이동속도
    public float speed = 0.5f;
    //반응 거리
    public float reactionDistance = 4.0f;
    //애니메이션 이름
    public string idleAnime = "EnemyIdle";      //정지
    public string upAnime = "Enemyup";          //위
    public string downAnime = "EnemyDown";          //아래
    public string rightAnime = "EnemyRight";          //오른쪽
    public string leftAnime = "EnemyLeft";          //왼쪽
    public string deadAnime = "EnemyDead";          //사망


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
