using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    //HP
    public int hp = 3;
    //�̵��ӵ�
    public float speed = 0.5f;
    //���� �Ÿ�
    public float reactionDistance = 4.0f;
    //�ִϸ��̼� �̸�
    public string idleAnime = "EnemyIdle";      //����
    public string upAnime = "Enemyup";          //��
    public string downAnime = "EnemyDown";          //�Ʒ�
    public string rightAnime = "EnemyRight";          //������
    public string leftAnime = "EnemyLeft";          //����
    public string deadAnime = "EnemyDead";          //���


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
