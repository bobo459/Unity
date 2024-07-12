using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBox : MonoBehaviour
{
    public Sprite openImage;            //���� �̹���
    public GameObject itemPrefad;       //��� ������ ������
    public bool isClosed = true;        //true= ��������, false= ��������.
    public int arrangeId = 0;           //��ġ �ĺ��� ���

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //����(����)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(isClosed && collision.gameObject.tag == "Player")
        {
            //���ڰ� ���� �ִ� ���¿��� �÷��̾�� ����
            GetComponent<SpriteRenderer>().sprite = openImage;
            isClosed = false;       //���� ���·� �ϱ�
            if(itemPrefad != null)  //�������� �����ϸ� ������
            {
                //���������� ������ �����
                Instantiate(itemPrefad, transform.position, Quaternion.identity);
            }
        }
    }
}
