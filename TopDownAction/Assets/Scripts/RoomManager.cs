using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomManager : MonoBehaviour
{ 

    //static ����
public static int doorNumber = 0;   //����ȣ

    // Start is called before the first frame update
    void Start()
    {
        //�÷��̾� ĳ���� ��ġ
        //���Ա��� �迭�� ���
        GameObject[] enters = GameObject.FindGameObjectsWithTag("Exit");    //Objects ������ ����ҰŸ� ������Ʈ �ڿ� s�� ����. ������Ʈ�� ����ũ�� ģ���� ĳ���Ϳ��� �ο���.
        for (int i = 0; i< enters.Length; i++)
        {
            GameObject doorObj = enters[i];            //�迭���� ������
            Exit exit = doorObj.GetComponent<Exit>();   //Exit Ŭ���� ����
            if (doorNumber == exit.doorNumber)
            {
                //==���� �� ��ȣ ==
                //�÷��̾� ĳ���� ���Ա��� �̵�
                float x = doorObj.transform.position.x;
                float y = doorObj.transform.position.y;
                if(exit.direction == ExitDirection.up)
                {
                    y += 1;
                }
                else if (exit.direction == ExitDirection.right)
                {
                    x += 1;
                }
                else if (exit.direction == ExitDirection.down)
                {
                    y -= 1;
                }
                else if (exit.direction == ExitDirection.left)
                {
                    x -= 1;
                }
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                player.transform.position = new Vector3(x, y);
                break;      //�ݺ��� ����������

            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //�� �̵�
    public static void ChangeScene(string scnename, int doornum)
    {
        doorNumber = doornum;               //�� ��ȣ�� static ������ ����
        SceneManager.LoadScene(scnename);   //�� �̵�
    }
}
