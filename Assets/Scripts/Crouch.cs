using UnityEngine;
using System.Collections;

namespace FPSSystem
{
    public class Crouch : MonoBehaviour
    {
        public float crouchSpeed = 3;
        CharacterController charController;
        Transform Player;
        private float charHeight;
        private Vector3 pos;

        void Start()
        {
            Player = transform;
            charController = GetComponent<CharacterController>();
            charHeight = charController.height;
        }

        void Update()
        {
            float h = charHeight;

            if (Input.GetKey(KeyCode.LeftControl))
            {
                h = 1;
            }
            float lastHeight = charController.height;
            charController.height = Mathf.Lerp(charController.height, h, Time.deltaTime * 5);
            pos.x = Player.position.x;
            pos.z = Player.position.z;
            pos.y = Player.position.y + (charController.height - lastHeight) / 2;
            Player.position = pos;
        }
    }
}
