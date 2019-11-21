﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class PlayerController : BeatBehaviour
{
    [BoxGroup("Camera")]
    [Required]
    [SerializeField] new Camera camera;

    [BoxGroup("PlayerController")]
    [Required]
    [SerializeField] VirtualController virtualController;

    [BoxGroup("PlayerController")]
    [SerializeField] bool secondPrototype;

    [BoxGroup("PlayerController")]
    [SerializeField] float walkForce, dashForce, attackForce, rayCastMaxDist;

    [BoxGroup("PlayerController")]
    [SerializeField] float walkVelocity, dashVelocity, attackVelocity, rigidbodyDrag, maxRotPerAttackPerSec, maxRotPerWalkPerSec;

    [BoxGroup("States")]
    [ReadOnly]
    [SerializeField] State state;

    [BoxGroup("States")]
    [SerializeField] float walkToIdleTime, attackTime, dashTime;

    [BoxGroup("States")]
    [ReadOnly]
    [SerializeField] float timeTracker;

    [BoxGroup("Animation")]
    [Required]
    [SerializeField] Animator animator;

    [BoxGroup("Animation")]
    [SerializeField] string walkFloatParameter, dashBoolParameter, attackBoolParameter;

    [BoxGroup("Animation")]
    [SerializeField] float animationVelocityLerpSpeed = 0.1f;

    [BoxGroup("Weapon")]
    [SerializeField] GameObject weapon;

    new Rigidbody rigidbody;
    float animationVelocity;
    Vector3 camStartingOffset;
    float prevRotationAngle;


    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        camStartingOffset = camera.transform.position - transform.position;
    }

    protected override void Update()
    {
        base.Update();

        var input =  virtualController.GetPackage();
        timeTracker -= Time.deltaTime;

        UpdateNone(input);
        UpdateMove(input);
        UpdateDash(input, noOnBeatRequired: secondPrototype);

        if (secondPrototype)
        {

        }
        else
        {
            UpdateAutoAttack(input);
        }


        rigidbody.velocity = new Vector3(rigidbody.velocity.x * rigidbodyDrag, rigidbody.velocity.y, rigidbody.velocity.z * rigidbodyDrag);
    }

    private void LateUpdate()
    {
        camera.transform.position = rigidbody.position + camStartingOffset;
    }

    private void Move(Vector3 inputDir, float force, float maxSpeed)
    {

        Ray r = new Ray(transform.position, inputDir);

        bool hit = Physics.Raycast(r, rayCastMaxDist);

        if (hit)
        {
            Debug.DrawLine(r.origin, r.origin + r.direction, Color.red, 1f);
        }
        else
        {
            rigidbody.AddForce(inputDir * force);
        }

        Vector3 xzVel = rigidbody.velocity;
        xzVel.y = 0;

        if(xzVel.magnitude > walkVelocity)
        {
            xzVel = xzVel.normalized * walkVelocity;
            rigidbody.velocity = new Vector3(xzVel.x, rigidbody.velocity.y, xzVel.z);
        }
        
    }

    private void UpdateRotation(InputPackage input, float maxRotangle)
    {
        Vector3 forward = new Vector3(input.LeftStick.x, 0, input.LeftStick.y);

        if(forward.magnitude > 0.01f)
        {
            transform.rotation = Quaternion.identity;
            float angle = Vector3.Angle(Vector3.forward, forward);
            angle = (forward.x > 0 ? angle : 360 - angle);
            angle = Mathf.MoveTowardsAngle(prevRotationAngle, angle, maxRotangle);
            prevRotationAngle = angle;
            transform.Rotate(Vector3.up, angle);
        }
    }

    private void UpdateAnimation()
    {
        animationVelocity = Mathf.Lerp(animationVelocity, rigidbody.velocity.magnitude / walkVelocity, animationVelocityLerpSpeed);
        animator.SetFloat(walkFloatParameter, animationVelocity);
    }

    private void UpdateNone(InputPackage input)
    {
        if(timeTracker <= 0)
        {
            if(state == State.Dash)
            {
                rigidbody.velocity = Vector3.zero;
            }

            state = State.None;
            animator.SetBool(dashBoolParameter, false);
            animator.SetBool(attackBoolParameter, false);
            animator.SetFloat(walkFloatParameter, 0);
            weapon.SetActive(false);
        }
    }

    private void UpdateMove(InputPackage input)
    {
        if(state == State.None && input.LeftStickMoved())
        {
            state = State.Move;
        }

        if (state == State.Move)
        {
            UpdateRotation(input, maxRotPerWalkPerSec * Time.deltaTime);
            UpdateAnimation();

            var inputDir = CameraDirectionFromInput(input.LeftStick);
            Move(inputDir, walkForce, walkVelocity);

            if (input.LeftStickMoved())
            {
                timeTracker = walkToIdleTime;
            }
        }
    }

    private Vector3 CameraDirectionFromInput(Vector2 movInput)
    {
        Vector3 camForward = camera.transform.forward;
        camForward.y = 0;
        camForward.Normalize();

        Vector3 camRight = camera.transform.right;
        camRight.y = 0;
        camRight.Normalize();

        Vector3 inputDir = camForward * movInput.y + camRight * movInput.x;
        return inputDir;
    }

    private void UpdateDash(InputPackage input, bool noOnBeatRequired)
    {
        if(noOnBeatRequired)
        {
            if ((input.LB || input.A) )
            {
                TryDash();
            }
        }
        else
        {
            if ((input.LB || input.A) && beatRangeCloseness > 0)
            {
                TryDash();
            }
        }

        if (state == State.Dash)
        {
            rigidbody.velocity = transform.forward * dashVelocity;
        }
    }

    private void UpdateAutoAttack(InputPackage input)
    {
        if ((input.RB || input.X) && beatRangeCloseness > 0)
        {
            TryAttack();
        }

        if(state == State.Attack)
        {
            UpdateRotation(input, maxRotPerAttackPerSec * Time.deltaTime);

            Vector2 inputLeftStick = new Vector2(transform.forward.x, transform.forward.z);
            var inputDir = CameraDirectionFromInput(inputLeftStick);
            Move(inputDir, attackForce, attackVelocity);
        }
    }

    private void UpdateMultiBeatAttack(InputPackage input)
    {
        if ((input.RB || input.X))
        {
            TryAttack();
        }

        if (state == State.Attack)
        {

        }
    }

    [Button]
    private void TryDash()
    {
        if(state != State.Move && state != State.None)
        {
            return;
        }

        state = State.Dash;
        timeTracker = dashTime;
        animator.SetBool(dashBoolParameter, true);
    }

    [Button]
    private void TryAttack()
    {
        if(state != State.Move && state != State.None)
        {
            return;
        }

        state = State.Attack;
        timeTracker = attackTime;
        animator.SetBool(attackBoolParameter, true);
        weapon.SetActive(true);
    }

    public enum State
    {
        None,
        Move,
        Attack,
        Dash
    }

}
