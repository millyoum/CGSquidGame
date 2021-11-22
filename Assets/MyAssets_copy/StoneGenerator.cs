using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneGenerator : MonoBehaviour
{
    
    public GameObject StonePrefeb;  //game object 선언
    public GameObject player;
    public Vector3 pos;
    public int stone_num = 3;

    // Start is called before the first frame update
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        pos = player.transform.position; //플레이어 센터 좌표

        //Debug.Log(stone_num);

        if (Input.GetMouseButtonDown(0)) //마우스 클릭하면
        {
            if (stone_num > 0 && stone_num <= 3)
            {
                GameObject stone = Instantiate(StonePrefeb, pos, Quaternion.identity) as GameObject; //플레이어 위치에서 게임 오브젝트 생성
                stone.transform.parent = player.transform; //플레이어 종속 아이템으로 함                                                                                  

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //ray를 이용해 마우스 클릭점 구함

                Vector3 shooting = ray.direction; //ray의 방향구하기

                shooting = shooting.normalized * 1000; //던지는 힘 설정

                stone.GetComponent<StoneController>().Shoot(shooting); //stone controller의 함수로 구슬 던지기
            }

            stone_num -= 1;
        }

        
    }
}