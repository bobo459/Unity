using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rbody;     // Rigidbody2D형 변수
    float axisH = 0.0f;    // 입력

    public float speed = 6.0f;   // 이동속도  => public : 유니티 창에 speed라는 게 뜬다. 실시간 디버깅 (실시간으로 (유니티에서) 수치값을 변경해서 볼수 있다.)이 가능해진다.

    public float jump = 12.0f;   //점프력
    public LayerMask groundLayer;    //착지할 수 있는 레이어
    bool goJump = false;             //점프 개시 플래그
    bool onGround = false;           //지면에 서 있는 플래그

    // 애니메이션 처리
    Animator animator; // 애니메이터
    public string stopAnime = "PlayerStop";  // 클립이 아님!
    public string moveAnime = "PlayerMove";
    public string jumpAnime = "PlayerJump";
    public string goalAnime = "PlayerGoal";
    public string deadAnime = "PlayerOver";

    string nowAnime = "";
    string oldAnime = "";

    public static string gameState = "playing"; // 게임 상태

    public int score = 0;   // 점수


    // Start is called before the first frame update
    void Start()
    {

        gameState = "playing"; // 게임 중

        if(gameState != "playing")
        {
            return;
        }
        // Rigidbody2D 가져오기
        rbody = this.GetComponent<Rigidbody2D>();
        // Animator 가져오기
        animator = GetComponent<Animator>();
        nowAnime = stopAnime;
        oldAnime = stopAnime;
        Debug.Log(gameState);

    }

    // Update is called once per frame
    void Update()
    {
        // 수평 방향으로의 입력 확인
        axisH = Input.GetAxisRaw("Horizontal");  //GetAxisRaw : 좌표를 가져온다.


        // 방향조절
        if (axisH > 0.0f)
        {
            //오른쪽 이동
            Debug.Log("오른쪽 이동");
            transform.localScale = new Vector2(1, 1);  //this.transform.localScale = new Vector2(1, 1); => this.가 빠진 상태이다. 기본적으로 적용이 된다. 왜냐하면 유니티의 transform을 이야기한다.
        }
        else if (axisH < 0.0f)
        {
            // 왼쪽 이동
            Debug.Log("왼쪽 이동");
            transform.localScale = new Vector2(-1, 1); // 좌우 반전시키기

        }
        //캐릭터 점프하기
        if (Input.GetButtonDown("Jump"))
        {
            Jump();  //점프
        }
    }

    //특종 사건(=0.02초)마다 한번 호출됨. 반드시 지정된 시간에 호출됨!! <물리적인 이동 시>
    void FixedUpdate()
    {

        if (gameState != "playing")
        {
            return;
        }
        //착지판정
        onGround = Physics2D.Linecast(transform.position,
                                       transform.position - (transform.up * 0.1f),  //x = 0 , y = 0.1, z=0  => .up : (0, 1, 0) / .forward (0,0,1)  , 벡터의 길이가 속도이다.
                                       groundLayer);
        Debug.DrawRay(transform.position, transform.up * -0.1f, Color.red);


        if (onGround || axisH != 0)
        {
            //지면 위 or 속도가 0 아님
            // 속도 갱신하기=> 벡터는 갱신하기
            rbody.velocity = new Vector2(speed * axisH, rbody.velocity.y);  //3.0f 숫자가 높을 수록 속도가 빨라짐. 2D는 2D 든 3D든 상관없이 사용가능! 단, 3D 는 3D로 사용해야함.

            //rbody.velocity.y 을 0값이 아닌 글로 적은 이유는? 매 0.02초 마다 벡터를 갱신,부여를 해준다. 1회성이 아니다. 언제든 새로 갱신이 가능해서 멈출수도 있게 해준다. 
            // => 임의로 이동하는 값은 x뿐이고 y는 중력의 힘이 적용되어야하므로 0값을 준다면 점프든, 떨어지든 0값이라서 중력의 값이 적용되지 않아서
            // 속도 데이터에 rbody.velocity.y 을 사용하여 중력의 값이 그대로 적용되도록 해준다. 

        }
        if (onGround && goJump)
        {
            //지면 위에서 점프 키 눌림
            //점프하기
            Debug.Log("점프! ");
            Vector2 jumpPw = new Vector2(0, jump);  //점프를 위한 벡터 생성
            rbody.AddForce(jumpPw, ForceMode2D.Impulse);    //순간적인 힘 가하기
            goJump = false; // 점프 플래그 끄기
        }

        if(onGround)
        {
            // 지면 위
            if(axisH == 0)
            {
                nowAnime = stopAnime; // 정지
            }
            else
            {
                nowAnime = moveAnime; // 이동
            }
        }
        else
        {
            // 공중
            nowAnime = jumpAnime;
        }

        if(nowAnime != oldAnime)
        {
            oldAnime = nowAnime;
            animator.Play(nowAnime);  // 애니메이션 재생
            // 매 프레임마다 시작 이미지부터 보여주면 안되므로
            // 애니메이션이 같지 않을때만 실행됨
        }

    }
    //점프
    public void Jump()
    {
        goJump = true; // 점프 플래그 켜기
        Debug.Log("점프 버튼 눌림! ");
    }

    // 접촉 시작
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Goal")
        {
            Goal(); // 골
        }
        else if (collision.gameObject.tag == "Dead")
        {
            GameOver(); // 게임 오버
        }
        else if(collision.gameObject.tag == "ScoreItem")
        {
            // 점수 아이템
            // ItemData 가져오기
            ItemData item = collision.gameObject.GetComponent<ItemData>();
            // 점수 얻기
            score = item.value;
            // 아이템 제거
            Destroy(collision.gameObject);
        }
    }
    // 골
    public void Goal()
    {
        animator.Play(goalAnime);
        gameState = "gameclear";
        GameStop(); // 게임 중지
    }
    // 게임 오버
    public void GameOver()
    {
        animator.Play(deadAnime);
        gameState = "gameover";
        GameStop(); // 게임 중지
        // ====================
        // 게임 오버 연출
        // ====================
        // 플레이어의 충돌 판정 비활성
        GetComponent<CapsuleCollider2D>().enabled = false;
        // 플레이어를 위로 튀어 오르게 하는 연출
        rbody.AddForce(new Vector2(0, 5), ForceMode2D.Impulse);
    }
    // 게임 중지
    void GameStop()
    {
        // Rigidbody2D 가져오기
        Rigidbody2D rbody = GetComponent<Rigidbody2D>();
        // 속도를 0으로 하여 강제 정지
        rbody.velocity = new Vector2(0, 0);


        // 게임 중지 시에 주로 사용하는 이벤트 :
        // 1. 콜라이더 비활성화
        //BoxCollider2D collider = GetComponent<CapsuleCollider2D>();
        //collider.enabled = false;
        // 2. 플레이어 static 으로 못움직이기 만듬
        //rbody.bodyType = RigidbodyType2D.static;
    }

}
