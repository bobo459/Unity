using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    //=========test========
    public int Effect = 1;
    //=========test========




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(player != null){
            //�÷��̾��� ��ġ�� ����
            transform.position = new Vector3(player.transform.position.x, 
                                             player.transform.position.y, -10);
        }
    }
}
