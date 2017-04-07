using UnityEngine;
using System.Collections;

namespace FPSSystem
{
    public class Crouch : MonoBehaviour
    {
        public float crouchSpeed = 3;
        CharacterController charController;
        Transform player;
        private float charHeight;
        private Vector3 pos;

        void Start()
        {
            player = transform;
            charController = GetComponent<CharacterController>();
            charHeight = charController.height;
        }

        void Update()
        {
            float h = charHeight;

            if (Input.GetKey(KeyCode.LeftControl))
            {
                h = 1;
                Player.MoveMode = Player.MoveState.Sneak;
            }
            float lastHeight = charController.height;
            charController.height = Mathf.Lerp(charController.height, h, Time.deltaTime * 5);
            pos.x = player.position.x;
            pos.z = player.position.z;
            pos.y = player.position.y + (charController.height - lastHeight) / 2;
            player.position = pos;
        }
    }
}
