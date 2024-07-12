using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
/*    public GameObject impactEffect; // 충돌 이펙트 프리팹

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
    }*/
}
