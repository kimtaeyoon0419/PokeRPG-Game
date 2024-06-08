namespace PokeRPG.Feild.Player
{
    using PokeRPG.Manager;

    // # System
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.Tracing;

    // # Unity
    using UnityEngine;

    public class Player : MonoBehaviour
    {
        [Header("PlayerInfo")]
        [SerializeField] private float nomalSpeed = default;
        [SerializeField] private float sprintSpeed = default;


        private float moveSpeed = default;
        private Vector2 dir = Vector2.zero;
        private Rigidbody2D rb = null;
        private Animator animator = null;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
        }
        private void Update()
        {
            PlayerInput();

            if(dir != Vector2.zero)
            {
                animator.SetBool("isMove", true);
            }
            else
            {
                animator.SetBool("isMove", false);
            }
        }

        private void FixedUpdate()
        {
            PlayerMovement();
        }

        /// <summary>
        /// �÷��̾��� �Է°��� ó���ϴ� �Լ�
        /// </summary>
        private void PlayerInput()
        {
            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");

            //��, �Ʒ�, ������, ���� ���θ� �����̵��� ����
            if(x != 0)
            {
                y = 0;
            }
            else if (y != 0)
            {
                x = 0;
            }

            if(Input.GetKey(KeyCode.LeftShift))
            {
                moveSpeed = sprintSpeed;
                animator.SetTrigger("Sprint");
            }
            else
            {
                moveSpeed = nomalSpeed;
                animator.SetTrigger("Walk");
            }

            dir = new Vector2(x, y).normalized; //�Է°��� ���� ������ ����

            animator.SetFloat("Horizontal", x);
            animator.SetFloat("Vertical", y);
        }

        private void PlayerMovement()
        {
            rb.velocity = dir * moveSpeed;
        }
    }

}
