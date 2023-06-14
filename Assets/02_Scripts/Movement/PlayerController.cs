using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using GemTrader.Environment;
using UnityEngine;

namespace GemTrader.Control
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float verticalSpeed = 5f;
        [SerializeField] private float horizontalSpeed = 5f;
        [SerializeField] private Transform stackTransform;
        [SerializeField] private float gemStackLerpTime = 5f;

        private List<BaseGem> _gems = new();

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

        private void Update()
        {
            StackGems();
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
                SetMovementAnimation(false, false);
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

        private void OnTriggerStay(Collider other)
        {
            if (other.GetComponentInParent<GridSystem>())
            {
                BaseGem gem = other.GetComponent<BaseGem>();
                AddGems(gem);
            }
        }

        private void AddGems(BaseGem gem)
        {
            if (gem.isReadyToHarvest)
            {
                gem.GetComponentInParent<GridSystem>()
                    .RespawnGem(gem, gem.CellCoordinateX, gem.CellCoordinateY);
                
                var gemTransform = gem.transform;
                
                gemTransform.DOKill();
                gemTransform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

                _gems.Add(gem);
                
                //gem.transform.parent = transform;
            }
        }

        private void StackGems()
        {
            for (int i = 0; i < _gems.Count; i++)
            {
                var gemPos = _gems[i].transform.position;
                var stackPos = stackTransform.position;
                
                if (_gems.Count > 1 && i > 0)
                {
                    stackPos = _gems[i - 1].transform.position + new Vector3(0, 0.25f,0);
                }
                
                gemPos = new Vector3(Mathf.Lerp(gemPos.x, stackPos.x, Time.deltaTime * gemStackLerpTime),
                    Mathf.Lerp(gemPos.y, stackPos.y, Time.deltaTime * gemStackLerpTime),
                    Mathf.Lerp(gemPos.z, stackPos.z, Time.deltaTime * gemStackLerpTime));

                _gems[i].transform.position = gemPos;
            }
          
        }
    }
}