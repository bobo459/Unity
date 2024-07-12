using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    public float deleteTime = 3;  // 제거 시간


    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, deleteTime);    //일정 시간 후 제거하기   
    }

    //게임 오브젝트에 접촉
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //접촉한 게임 오브젝트의 자식으로 설정하기

        //몬스터의 몸에 충돌하는 경우, 몬스터에 꽂힌 상태로 움직여야 하기 때문에
        //충돌객체의 자식으로 설정함
        transform.SetParent(collision.transform);    ///SetParent 부모

        //충돌 판정을 비활성
        GetComponent<CircleCollider2D>().enabled = false;
        //물리 시뮬레이션을 비활성
        GetComponent<Rigidbody2D>().simulated = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }



/*

    //test
    public GameObject impactEffect; // 충돌 이펙트 프리팹

    void OnCollisionEnter2D(Collision2D collision)
    {
        // 충돌한 대상이 "Wall" 태그를 가진 경우에만 충돌 이펙트 생성
        if (collision.gameObject.tag.Equals("Wall"))
        {
            ContactPoint2D contact = collision.contacts[0]; // 충돌 지점 정보 가져오기
            Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal); // 법선 벡터를 기준으로 회전 설정

            // 충돌 이펙트를 충돌 지점에 생성
            GameObject impact = Instantiate(impactEffect, contact.point, rot);
            Destroy(impact, 1.0f); // 예시로 생성 후 1초 후에 삭제 (원하는 시간 설정 가능)
        }

        Destroy(gameObject); // 충돌한 화살 오브젝트 삭제
    }

*/

}
