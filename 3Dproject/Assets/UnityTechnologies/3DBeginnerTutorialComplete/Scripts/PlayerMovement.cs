using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float turnSpeed = 20f;    //회전의 속도, 왜 움직임의 속도는 없지?? 이유는 하단에

    Animator m_Animator;
    Rigidbody m_Rigidbody;
    AudioSource m_AudioSource;
    Vector3 m_Movement;             //움직임을 계산해서 속도를 주겠다는 의미. 퍼블릭이 아님.프라이빗하게 사용.
    Quaternion m_Rotation = Quaternion.identity;

    void Start ()
    {
        m_Animator = GetComponent<Animator> ();
        m_Rigidbody = GetComponent<Rigidbody> ();
        m_AudioSource = GetComponent<AudioSource> ();
    }

    void FixedUpdate ()
    {
        float horizontal = Input.GetAxis ("Horizontal");
        float vertical = Input.GetAxis ("Vertical");
        
        m_Movement.Set(horizontal, 0f, vertical);    //horizontal : 좌우, vertical : 앞뒤
        m_Movement.Normalize ();

        bool hasHorizontalInput = !Mathf.Approximately (horizontal, 0f);   //소수점을 사용할때 거의 0일때는 0으로 보고 0이 아니면 이동한것으로 인식하겠다.
        bool hasVerticalInput = !Mathf.Approximately (vertical, 0f);
        bool isWalking = hasHorizontalInput || hasVerticalInput;
        m_Animator.SetBool ("IsWalking", isWalking);
        
        if (isWalking)   //걷는 소리 재생
        {
            if (!m_AudioSource.isPlaying)
            {
                m_AudioSource.Play();
            }
        }
        else
        {
            m_AudioSource.Stop ();
        }


        //특수한 키 조합만 대각선이나 옆으로 이동하는 것을 하게 한다.
        Vector3 desiredForward = Vector3.RotateTowards (transform.forward, m_Movement, turnSpeed * Time.deltaTime, 0f);  //Time:시간계산을 하는이유는 1프레임당 얼마큼 이동하는지 알아야해서 값을 곱한것.
        m_Rotation = Quaternion.LookRotation (desiredForward);
    }

    void OnAnimatorMove ()  //움직임. 주기적으로 불리는 콜백함수이다. 애니메이션이 재생하는 만큼 프레임당으로 불린다. 
    {
        m_Rigidbody.MovePosition (m_Rigidbody.position + m_Movement * m_Animator.deltaPosition.magnitude);   // m_Movement : 방향. 값이 1이라 속도가 빨라지진않는다.
                                                                                                             // m_Animator.deltaPosition.magnitude : magnitude 빗변의 길이->속도는 아님, 이동해야하는 길이
        m_Rigidbody.MoveRotation (m_Rotation);    //m_Rigidbody 을 왜 넣었을까?
                                                  //루트모션-> 진짜 애니메이션이 앞으로 나아감. 이유, 얼마나 애니메이션이 사실처럼 만들까에서 나온 의문.
                                                  //Rigidbody가 애니메이션 움직임에 맞추어 캐릭터를 쫒아갈수 있게 만들기 위해 넣어준것이다.
    }
}