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
    [SerializeField] float walkForce, dashForce, rayCastMaxDist;

    [BoxGroup("PlayerController")]
    [SerializeField] float walkVelocity, dashVelocity;

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

    new Rigidbody rigidbody;
    Vector3 camStartingOffset;


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
        UpdateDash(input);
        UpdateAttack(input);
    }

    private void LateUpdate()
    {
        camera.transform.position = rigidbody.position + camStartingOffset;
    }

    protected override void OnBeatRangeStay()
    {
        if(state == State.None || state == State.Move)
        {
            if (virtualController.GetPackage().A)
            {
                //attack
            }
        }
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

    private void UpdateRotation()
    {
        Vector3 forward = rigidbody.velocity;
        forward.y = 0;

        if(forward.magnitude > 0.01f)
        {
            transform.rotation = Quaternion.identity;
            float angle = Vector3.Angle(Vector3.forward, forward); 
            transform.Rotate(Vector3.up, (forward.x>0 ? angle : 360 - angle));
        }
    }

    private void UpdateAnimation()
    {
        animator.SetFloat(walkFloatParameter, rigidbody.velocity.magnitude / walkVelocity);
    }

    private void UpdateNone(InputPackage input)
    {
        if(timeTracker <= 0)
        {
            state = State.None;
            animator.SetBool(dashBoolParameter, false);
            animator.SetBool(attackBoolParameter, false);
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
            UpdateRotation();
            UpdateAnimation();

            Vector3 camForward = camera.transform.forward;
            camForward.y = 0;
            camForward.Normalize();

            Vector3 camRight = camera.transform.right;
            camRight.y = 0;
            camRight.Normalize();

            Vector3 inputDir = camForward * input.LeftStick.y + camRight * input.LeftStick.x;
            Move(inputDir, walkForce, walkVelocity);

            if (input.LeftStickMoved())
            {
                timeTracker = walkToIdleTime;
            }
        }
    }

    private void UpdateDash(InputPackage input)
    {
        if (input.RB)
        {
            TryDash();
        }
        if (state == State.Dash)
        {
            Move(transform.forward, dashForce, dashVelocity);
        }
    }

    private void UpdateAttack(InputPackage input)
    {
        if (input.LB)
        {
            TryAttack();
        }

        if(state == State.Attack)
        {
            UpdateRotation();
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

    }

    public enum State
    {
        None,
        Move,
        Attack,
        Dash
    }

}
