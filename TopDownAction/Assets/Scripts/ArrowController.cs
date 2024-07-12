using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    public float deleteTime = 3;  // ���� �ð�


    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, deleteTime);    //���� �ð� �� �����ϱ�   
    }

    //���� ������Ʈ�� ����
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //������ ���� ������Ʈ�� �ڽ����� �����ϱ�

        //������ ���� �浹�ϴ� ���, ���Ϳ� ���� ���·� �������� �ϱ� ������
        //�浹��ü�� �ڽ����� ������
        transform.SetParent(collision.transform);    ///SetParent �θ�

        //�浹 ������ ��Ȱ��
        GetComponent<CircleCollider2D>().enabled = false;
        //���� �ùķ��̼��� ��Ȱ��
        GetComponent<Rigidbody2D>().simulated = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }



/*

    //test
    public GameObject impactEffect; // �浹 ����Ʈ ������

    void OnCollisionEnter2D(Collision2D collision)
    {
        // �浹�� ����� "Wall" �±׸� ���� ��쿡�� �浹 ����Ʈ ����
        if (collision.gameObject.tag.Equals("Wall"))
        {
            ContactPoint2D contact = collision.contacts[0]; // �浹 ���� ���� ��������
            Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal); // ���� ���͸� �������� ȸ�� ����

            // �浹 ����Ʈ�� �浹 ������ ����
            GameObject impact = Instantiate(impactEffect, contact.point, rot);
            Destroy(impact, 1.0f); // ���÷� ���� �� 1�� �Ŀ� ���� (���ϴ� �ð� ���� ����)
        }

        Destroy(gameObject); // �浹�� ȭ�� ������Ʈ ����
    }

*/

}
