namespace PokeRPG.Feild.Player
{

    // # System
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.Tracing;

    // # Unity
    using UnityEngine;

    public class Player : MonoBehaviour
    {
        [Header("PlayerInfo")]
        [SerializeField] private float moveSpeed = default;

        private Vector2 dir = Vector2.zero;
        private Rigidbody2D rb = null;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }
        private void Update()
        {
            PlayerInput();
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

            dir = new Vector2(x, y).normalized; //�Է°��� ���� ������ ����
        }

        private void PlayerMovement()
        {
            rb.velocity = dir * moveSpeed;
        }
    }

}
