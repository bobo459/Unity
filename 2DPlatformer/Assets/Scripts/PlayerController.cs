using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rbody;     // Rigidbody2D�� ����
    float axisH = 0.0f;    // �Է�

    public float speed = 6.0f;   // �̵��ӵ�  => public : ����Ƽ â�� speed��� �� ���. �ǽð� ����� (�ǽð����� (����Ƽ����) ��ġ���� �����ؼ� ���� �ִ�.)�� ����������.

    public float jump = 12.0f;   //������
    public LayerMask groundLayer;    //������ �� �ִ� ���̾�
    bool goJump = false;             //���� ���� �÷���
    bool onGround = false;           //���鿡 �� �ִ� �÷���

    // �ִϸ��̼� ó��
    Animator animator; // �ִϸ�����
    public string stopAnime = "PlayerStop";  // Ŭ���� �ƴ�!
    public string moveAnime = "PlayerMove";
    public string jumpAnime = "PlayerJump";
    public string goalAnime = "PlayerGoal";
    public string deadAnime = "PlayerOver";

    string nowAnime = "";
    string oldAnime = "";

    public static string gameState = "playing"; // ���� ����

    public int score = 0;   // ����


    // Start is called before the first frame update
    void Start()
    {

        gameState = "playing"; // ���� ��

        if(gameState != "playing")
        {
            return;
        }
        // Rigidbody2D ��������
        rbody = this.GetComponent<Rigidbody2D>();
        // Animator ��������
        animator = GetComponent<Animator>();
        nowAnime = stopAnime;
        oldAnime = stopAnime;
        Debug.Log(gameState);

    }

    // Update is called once per frame
    void Update()
    {
        // ���� ���������� �Է� Ȯ��
        axisH = Input.GetAxisRaw("Horizontal");  //GetAxisRaw : ��ǥ�� �����´�.


        // ��������
        if (axisH > 0.0f)
        {
            //������ �̵�
            Debug.Log("������ �̵�");
            transform.localScale = new Vector2(1, 1);  //this.transform.localScale = new Vector2(1, 1); => this.�� ���� �����̴�. �⺻������ ������ �ȴ�. �ֳ��ϸ� ����Ƽ�� transform�� �̾߱��Ѵ�.
        }
        else if (axisH < 0.0f)
        {
            // ���� �̵�
            Debug.Log("���� �̵�");
            transform.localScale = new Vector2(-1, 1); // �¿� ������Ű��

        }
        //ĳ���� �����ϱ�
        if (Input.GetButtonDown("Jump"))
        {
            Jump();  //����
        }
    }

    //Ư�� ���(=0.02��)���� �ѹ� ȣ���. �ݵ�� ������ �ð��� ȣ���!! <�������� �̵� ��>
    void FixedUpdate()
    {

        if (gameState != "playing")
        {
            return;
        }
        //��������
        onGround = Physics2D.Linecast(transform.position,
                                       transform.position - (transform.up * 0.1f),  //x = 0 , y = 0.1, z=0  => .up : (0, 1, 0) / .forward (0,0,1)  , ������ ���̰� �ӵ��̴�.
                                       groundLayer);
        Debug.DrawRay(transform.position, transform.up * -0.1f, Color.red);


        if (onGround || axisH != 0)
        {
            //���� �� or �ӵ��� 0 �ƴ�
            // �ӵ� �����ϱ�=> ���ʹ� �����ϱ�
            rbody.velocity = new Vector2(speed * axisH, rbody.velocity.y);  //3.0f ���ڰ� ���� ���� �ӵ��� ������. 2D�� 2D �� 3D�� ������� ��밡��! ��, 3D �� 3D�� ����ؾ���.

            //rbody.velocity.y �� 0���� �ƴ� �۷� ���� ������? �� 0.02�� ���� ���͸� ����,�ο��� ���ش�. 1ȸ���� �ƴϴ�. ������ ���� ������ �����ؼ� ������� �ְ� ���ش�. 
            // => ���Ƿ� �̵��ϴ� ���� x���̰� y�� �߷��� ���� ����Ǿ���ϹǷ� 0���� �شٸ� ������, �������� 0���̶� �߷��� ���� ������� �ʾƼ�
            // �ӵ� �����Ϳ� rbody.velocity.y �� ����Ͽ� �߷��� ���� �״�� ����ǵ��� ���ش�. 

        }
        if (onGround && goJump)
        {
            //���� ������ ���� Ű ����
            //�����ϱ�
            Debug.Log("����! ");
            Vector2 jumpPw = new Vector2(0, jump);  //������ ���� ���� ����
            rbody.AddForce(jumpPw, ForceMode2D.Impulse);    //�������� �� ���ϱ�
            goJump = false; // ���� �÷��� ����
        }

        if(onGround)
        {
            // ���� ��
            if(axisH == 0)
            {
                nowAnime = stopAnime; // ����
            }
            else
            {
                nowAnime = moveAnime; // �̵�
            }
        }
        else
        {
            // ����
            nowAnime = jumpAnime;
        }

        if(nowAnime != oldAnime)
        {
            oldAnime = nowAnime;
            animator.Play(nowAnime);  // �ִϸ��̼� ���
            // �� �����Ӹ��� ���� �̹������� �����ָ� �ȵǹǷ�
            // �ִϸ��̼��� ���� �������� �����
        }

    }
    //����
    public void Jump()
    {
        goJump = true; // ���� �÷��� �ѱ�
        Debug.Log("���� ��ư ����! ");
    }

    // ���� ����
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Goal")
        {
            Goal(); // ��
        }
        else if (collision.gameObject.tag == "Dead")
        {
            GameOver(); // ���� ����
        }
        else if(collision.gameObject.tag == "ScoreItem")
        {
            // ���� ������
            // ItemData ��������
            ItemData item = collision.gameObject.GetComponent<ItemData>();
            // ���� ���
            score = item.value;
            // ������ ����
            Destroy(collision.gameObject);
        }
    }
    // ��
    public void Goal()
    {
        animator.Play(goalAnime);
        gameState = "gameclear";
        GameStop(); // ���� ����
    }
    // ���� ����
    public void GameOver()
    {
        animator.Play(deadAnime);
        gameState = "gameover";
        GameStop(); // ���� ����
        // ====================
        // ���� ���� ����
        // ====================
        // �÷��̾��� �浹 ���� ��Ȱ��
        GetComponent<CapsuleCollider2D>().enabled = false;
        // �÷��̾ ���� Ƣ�� ������ �ϴ� ����
        rbody.AddForce(new Vector2(0, 5), ForceMode2D.Impulse);
    }
    // ���� ����
    void GameStop()
    {
        // Rigidbody2D ��������
        Rigidbody2D rbody = GetComponent<Rigidbody2D>();
        // �ӵ��� 0���� �Ͽ� ���� ����
        rbody.velocity = new Vector2(0, 0);


        // ���� ���� �ÿ� �ַ� ����ϴ� �̺�Ʈ :
        // 1. �ݶ��̴� ��Ȱ��ȭ
        //BoxCollider2D collider = GetComponent<CapsuleCollider2D>();
        //collider.enabled = false;
        // 2. �÷��̾� static ���� �������̱� ����
        //rbody.bodyType = RigidbodyType2D.static;
    }

}
