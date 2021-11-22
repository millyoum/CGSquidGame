using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Work : MonoBehaviour
{
    //환경 설정
    public float speed = 1.0F;
    public float jumpSpeed = 0.001F;
    public float gravity = 0.001F;
    private Vector3 moveDirection = Vector3.zero;

    //플레이어와 유리발판 사이 인터렉션 위해 게임 오브젝트 변수 생성
    public GameObject glass;

    private void Start()
    {
        //태그로 깨지는 유리 찾음
        glass = GameObject.FindGameObjectWithTag("BreakGlass");
    }

    void Update()
    {
        CharacterController controller = GetComponent<CharacterController>();
        Animator mAvartar = GetComponent<Animator>();

        if (controller.isGrounded)
        {
            //점프 기능 구현
            if (Input.GetKey(KeyCode.Space))
            {
                moveDirection.y = jumpSpeed;
                mAvartar.SetTrigger("Jump");
            }

            //쉬프트 점프 기능 구현
            if (Input.GetKey(KeyCode.LeftShift))
            {
                if (Input.GetKey(KeyCode.Space))
                {
                    moveDirection.y = jumpSpeed * 2;
                    mAvartar.SetTrigger("Jump");
                }
            }

            //방향키에 따르는 캐릭터의 모션 변경(W,A,D는 걷는 모션)
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            {
                mAvartar.SetBool("MoveAction0", true);

            }
            else
            {
                mAvartar.SetBool("MoveAction0", false);

            }
            //S는 뒤로 걷는 모션
            if (Input.GetKey(KeyCode.S))
            {
                mAvartar.SetBool("BackMotion", true);
            }
            else
            {
                mAvartar.SetBool("BackMotion", false);
            }

        }

        //중력으로 캐릭터가 y축을 따라 내려오도록 구현
        moveDirection.y -= gravity * Time.deltaTime * 2;
        controller.Move(moveDirection * Time.deltaTime / 20);

        float move_side = 0f;
        float move_forth = 0f;

        //방향키에 따르는 캐릭터의 이동거리 구현
        if (Input.GetKey(KeyCode.W))
        {
            move_forth += 1f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            move_forth -= 1f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            move_side -= 1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            move_side += 1f;
        }
        //R키를 누를 시 처음 자리로 부활
        if (Input.GetKey(KeyCode.R))
        {
            transform.position = new Vector3(0, 0, 0);
        }
        transform.Translate(new Vector3(move_side, 0f, move_forth) * Time.deltaTime * 10);

    }

    //플레이어와 충돌 물체 처리
    void OnControllerColliderHit(ControllerColliderHit hit)
    {

        /*플레이어와 충돌한 물체의 rigidbody를 불러오고 그 바디가 null이 아닐 경우에 원점으로 돌아감
         * 유리발판은 rigidbody가 없기 때문에 여기에 해당되지 않음 
         * 보완필요한 부분:
         * 1)강화유리에 있다가 다리 밖으로 떨어져 바닥에 닿음 -> 리스폰 -> 깨지는유리를 밟으면 오류남 
         * 에러코드: glass splinter 게임 오브젝트에 rigidbody가 없으나 스크립트가 자꾸 접근하려함 
         * 2)깨지는 유리를 먼저 밟음 -> 깨져서 떨어짐 -> 바닥에 떨어졌을때 충돌 인지가 되지 않음*/

        Rigidbody body = hit.collider.attachedRigidbody;
        if (body != null)
            transform.position = new Vector3(0, 0.05f, 0);


        //플레이어가 깨지는 유리발판을 밟는 경우
        if (hit.collider.CompareTag("BreakGlass"))
        {
            Debug.Log("플레이어가 일반 유리 밟음");
            glass.GetComponent<BreakableWindow>().breakWindow();
        }

    }
}