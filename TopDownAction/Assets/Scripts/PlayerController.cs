using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    //이동 속도
    public float speed = 3.0f;    //방향벡터 * 스피드 값
    //애니메이션 이름
    public string upAnime = "PlayerUp";         //위
    public string downAnime = "PlayerDown";     //아래
    public string rightAnime = "PlayerRight";   //오른쪽
    public string leftAnime = "PlayerLeft";     //왼쪽
    public string deadAnime = "PlayerDead";     //사망
    //현재 애니메이션
    string nowAnimation = "";
    //이전 애니메이션
    string oldAnimation = "";

    float axisH;                    //가로축 값 (-0.1~ 0.0 ~ 1.0)
    float axisV;                    //세로축 값 (-0.1~ 0.0 ~ 1.0)
    public float angleZ = -90.0f;   //회전각

    Rigidbody2D rbody;              //Rigidbody2D
    bool isMoving = false;          //이동 중인지 여부


    public static int hp = 3;           //플레이어 HP
    public static string gameState;     //게임 상태
    bool inDamage = false;              //대미지를 받는 중인지 여부


    // Start is called before the first frame update
    void Start()
    {
        //Rigidbody2D 가져오기
        rbody = GetComponent<Rigidbody2D>();
        //애니메이션
        oldAnimation = downAnime;


        //게임 상태르르 플레이 중으로 변경
        gameState = "playing";
    }

    // Update is called once per frame
    void Update()
    {
        if(isMoving == false)
        {
            axisH = Input.GetAxisRaw("Horizontal");  //좌우 키 입력
            axisV = Input.GetAxisRaw("Vertical");    //상하 키 입력
        }
        //키 입력으로 이동 각도 구하기
        Vector2 fromPt = transform.position;
        Vector2 toPt = new Vector2(fromPt.x + axisH, fromPt.y + axisV);
        angleZ = GetAngle(fromPt, toPt);
        //이동 각도에서 방향과 애니메이션 변경
        if(angleZ >= -45 && angleZ < 45)
        {
            //오른쪽
            nowAnimation = rightAnime;
        }
        else if(angleZ >= 45 && angleZ <= 135)
        {
            //위쪽
            nowAnimation = upAnime;
        }
        else if(angleZ >= -135 && angleZ <= -45)
        {
            //아래쪽
            nowAnimation = downAnime;
        }
        else
        {
            //왼쪽
            nowAnimation = leftAnime;
        }

        //애니메이션 변경하기
        if(nowAnimation != oldAnimation)
        {
            oldAnimation = nowAnimation;
            GetComponent<Animator>().Play(nowAnimation);
        }



        //게임 중이 아니거나 대미지를 받는 중에는 아무것도 하지 않음
        if(gameState != "playing" || inDamage)
        {
            return;
        }
    }

    void FixedUpdate()
    {
        //게임중이 아니면 아무것도 하지 않음
        if(gameState != "playing")
        {
            return;
        }
        if (inDamage)
        {
            //대미지를 받는 중에는 점멸시키기
            float val = Mathf.Sin(Time.time * 50);   //Time.time 시간을 초로 나타낸것.
                                                     /* 50 : 싸인이 주기적으로 그래프를 대로 값이 불리는데,
                                                             그 각의 위치를 당겨서 전체주기를 빈도를 짧아지게 만드는 것.
                                                             음수와 양수의 그래프의 초의 수가 짧아져 빠르게 깜박이게 만드는 느낌.*/
            Debug.Log(val);
            if (val > 0)
            {
                //스프라이트 표시
                gameObject.GetComponent<SpriteRenderer>().enabled = true;
            }
            else
            {
                //스프라이트 비표시
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
            }
            return; //대미지를 받는 중에는 조작할 수 없게 하기
        }
        //이동 속도 변경하기
        rbody.velocity = new Vector2(axisH, axisV) * speed;
    }

    public void SetAxis(float h, float v)
    {
        axisH = h;
        axisV = v;
        if(axisH == 0 && axisV == 0)
        {
            isMoving = false;
        }
        else
        {
            isMoving = true;
        }
    }

    //p1에서 p2까지의 각도를 계산
    float GetAngle(Vector2 p1, Vector2 p2)
    {
        float angle;
        if(axisH != 0 || axisV != 0)
        {
            //이동 중이면 각도를 변경
            //p1과 p2 차이 구하기(원점을 0으로 하기 위해)
            float dx = p2.x - p1.x;
            float dy = p2.y - p1.y;
            //아크 탄젠트 함수로 각도(라디안) 구하기
            float rad = Mathf.Atan2(dy, dx);
            //라디안 각으로 변환하여 반환
            angle = rad * Mathf.Rad2Deg;
        }
        else
        {
            //정지 중이면 이전 각도를 유지
            angle = angleZ;
        }
        return angle;
    }
    //접촉(물리)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            GetDamage(collision.gameObject);
        }
    }
    //대미지
    void GetDamage(GameObject enemy)
    {
        if(gameState == "playing")
        {
            hp--;       //HP 감소
            if(hp > 0)
            {
                //이동 중지
                rbody.velocity = new Vector2(0, 0);
                //적 캐릭터의 반대 방향으로 히트백(넉백:공격당해서 밀리거나 기절상태)
                Vector3 toPos = (transform.position - enemy.transform.position).normalized;
                rbody.AddForce(new Vector2(toPos.x * 4,   /* z를 뺀 x,y 만 속도를 주기 위해 이렇게 따로 사용.
                                                             <아쉬운점은 *4 의 값을 변수로 만들어서 적용했으면 좋았을 것> */
                                           toPos.y * 4), ForceMode2D.Impulse);
                //대미지를 받는 중으로 설정
                inDamage = true;
                Invoke("DamageEnd", 0.25f);
            }
            else
            {
                //게임오버
                GameOver();
            }
        }
    }
    //대미지를 받기 끝
    void DamageEnd()
    {
        //대미지를 받는 중이 아님으로 설정
        inDamage = false;
        //스프라이트 되돌리기
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
    }
    //게임오버
    void GameOver()
    {
        Debug.Log("게임 오버!");
        gameState = "gameover";
        //========
        //게임 오버 연출
        //========
        //플레이어 충돌 판정 비활성
        GetComponent<CircleCollider2D>().enabled = false;
        //이동중지
        rbody.velocity = new Vector2(0, 0);
        //중력을 적용해 플레이어를 위로 튀어오르게 하는 연출
        rbody.gravityScale = 1;
        rbody.AddForce(new Vector2(0, 5), ForceMode2D.Impulse);
        //애니메이션 변경하기
        GetComponent<Animator>().Play(deadAnime);
        //1초 후에 플레이어 캐릭터 제거하기
        Destroy(gameObject, 1.0f);
    }
}
