using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowShoot : MonoBehaviour
{
    public float shootSpeed = 12.0f;    //화살 속도
    public float shootDelay = 0.25f;    //발사 간격  => 연타를 막기 위해 사용하는 것, 연타시 오류가 발생 할 수도 있기때문에 0.25초 간격에는 공격키를 받지 않는다는 의미. 
                                                       //0.3초는 체감상 키가 안먹는 다고 생각이 들기때문에 0.15~0.25초 사이를 추천한다.
    public GameObject bowPrefab;        //활의 프리팹
    public GameObject arrowPrefab;      //화살의 프리팹

    bool inAttack = false;              //공격 중 여부
    GameObject bowObj;                  //활의 게임 오브젝트

    // Start is called before the first frame update
    void Start()
    {
        //활을 플레이어 캐릭터에 배치
        Vector3 pos = transform.position;
        bowObj = Instantiate(bowPrefab, pos, Quaternion.identity);
        bowObj.transform.SetParent(transform);    //플레이어 캐릭터를 활의 부모로 설정
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetButtonDown("Fire3")))  //Fire3 = shift 공격키 Fire1 or Fire2 키로 바꾸어도 된다. 
        {
            //공격 키가 눌림
            Attack();
        }
        //활의 회전과 우선순위
        float bowZ = -1;   //활의 Z 값(캐릭터보다 앞으로 설정)
        PlayerController plmv = GetComponent<PlayerController>();
        if(plmv.angleZ > 30 && plmv.angleZ < 150)
        {
            //위 방향
            bowZ = 1;       //활의 Z값(캐릭터보다 뒤로 설정)
        }
        //활의 회전
        bowObj.transform.rotation = Quaternion.Euler(0, 0, plmv.angleZ);  //z값으로만 회전하라
        //활의 우선순위
        bowObj.transform.position = new Vector3(transform.position.x, transform.position.y, bowZ);
    }
    //공격
    public void Attack()
    {
        //화살을 가지고 있음& 공격 중이 아님
        if(ItemKeeper.hasArrows > 0 && inAttack == false)
        {
            ItemKeeper.hasArrows -= 1;  //화살을 소모
            inAttack = true;            //공격 중으로 설정
            //화살발사
            PlayerController playerCnt = GetComponent < PlayerController>();
            float angleZ = playerCnt.angleZ;    //회전 각도
            //화살의 게임 오브젝트 만들기(진행 방향으로 회전)
            Quaternion r = Quaternion.Euler(0, 0, angleZ);
            GameObject arrowObj = Instantiate(arrowPrefab, transform.position, r);    //3차원 개발환경이여서. 오일러(Euler) 회전 공식 => 짐벌락 : 회전축이 겹쳐서 동일한 모습으로 회전을 하는 문제가 있다.
                                                                                      //Quaternion(쿼터니언) : 4차원의 내용
                                                                                      //실제 계산식에는 쿼터니언을 사용하므로 쿼터니언을 바꿔서 사용할수 있게 식을 만든것.
                                                                                      //실제로 다루는 3차원은 오일러 방식이 이닌 쿼터니언 방식을 사용한다.
            //화살을 발사할 벡터 생성
            float x = Mathf.Cos(angleZ * Mathf.Deg2Rad);
            float y = Mathf.Sin(angleZ * Mathf.Deg2Rad);
            Vector3 v = new Vector3(x, y) * shootSpeed;
            //화살에 힘을 가하기
            Rigidbody2D body = arrowObj.GetComponent<Rigidbody2D>();
            body.AddForce(v, ForceMode2D.Impulse);
            //공격 중이 아님으로 설정
            Invoke("StopAttack", shootDelay);   //Invoke: 비동기 
        }
    }
    //공격중지
    public void StopAttack()
    {
        inAttack = false;   //공격 중이 아님으로 설정
    }
}
