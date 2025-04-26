using UnityEngine;

namespace Game.Player
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private FloatingJoystick joystick;

        private Rigidbody2D rb;
        private Animator anim;
        public Vector2 inputVec { get; private set; }
        private bool isMobile;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();

#if UNITY_ANDROID || UNITY_IOS
            isMobile = true;
#else
            isMobile = false;
#endif
        }

        private void Update()
        {
            if (isMobile && joystick != null)
            {
                inputVec = new Vector2(joystick.Horizontal, joystick.Vertical);
            }
            else
            {
                inputVec = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
                // TODO: Consider using Unity's new Input System for better input handling
            }

            /*anim.SetFloat("Speed", inputVec.magnitude);*/
            if (inputVec.x != 0)
                transform.localScale = new Vector3(Mathf.Sign(inputVec.x), 1, 1);
        }

        private void FixedUpdate()
        {
            if (!Game.Managers.GameManager.Instance.isGameActive)
                return;

            rb.MovePosition(rb.position + inputVec.normalized * moveSpeed * Time.fixedDeltaTime);
        }
    }
}