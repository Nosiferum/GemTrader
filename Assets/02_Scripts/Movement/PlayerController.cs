using GemTrader.Environment;
using UnityEngine;

namespace GemTrader.Control
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float verticalSpeed = 5f;
        [SerializeField] private float horizontalSpeed = 5f;

        private FloatingJoystick _floatingJoystick;
        private Rigidbody _rb;
        private Animator _animator;

        private static readonly int IsRunning = Animator.StringToHash("isRunning");
        private static readonly int IsMoving = Animator.StringToHash("isMoving");

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _rb = GetComponent<Rigidbody>();
            _floatingJoystick = FindObjectOfType<FloatingJoystick>();
        }

        private void FixedUpdate()
        {
            Move();
        }

        private void Move()
        {
            _rb.velocity = new Vector3(_floatingJoystick.Horizontal * horizontalSpeed, 0,
                _floatingJoystick.Vertical * verticalSpeed);

            if (_floatingJoystick.Horizontal != 0 || _floatingJoystick.Vertical != 0)
            {
                SetRotation();
            }

            if (new Vector2(Mathf.Abs(_floatingJoystick.Horizontal), Mathf.Abs(_floatingJoystick.Vertical))
                    .sqrMagnitude is > 0 and <= 0.5f)
            {
                SetMovementAnimation(true, false);
            }

            else if (new Vector2(Mathf.Abs(_floatingJoystick.Horizontal), Mathf.Abs(_floatingJoystick.Vertical))
                         .sqrMagnitude is > 0 and >= 0.5f)
            {
                SetMovementAnimation(false, true);
            }

            else
            {
                SetMovementAnimation(false,false);
            }
        }

        private void SetMovementAnimation(bool isMoving, bool isRunning)
        {
            _animator.SetBool(IsMoving, isMoving);
            _animator.SetBool(IsRunning, isRunning);
        }

        private void SetRotation()
        {
            //this vector is already normalized by its values
            Vector3 forward = new Vector3(_floatingJoystick.Horizontal, 0, _floatingJoystick.Vertical);
            transform.rotation = Quaternion.LookRotation(forward, transform.up);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponentInParent<GridSystem>())
            {
                BaseGem baseGem = other.GetComponent<BaseGem>();
                other.GetComponentInParent<GridSystem>()
                    .RemoveAndRespawnGem(baseGem, baseGem.CellCoordinateX, baseGem.CellCoordinateY);
            }
        }
    }
}