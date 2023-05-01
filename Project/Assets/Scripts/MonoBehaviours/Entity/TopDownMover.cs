using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is a top-down movement controller
public class TopDownMover : MonoBehaviour
{
    public event Action StoppedMoving;
    public event Action StartedMoving;

    [SerializeField] float defaultMovementSpeed;
    [SerializeField] bool instantAcceleration;
    [SerializeField] float acceleration;
    [SerializeField] float deceleration;
    [SerializeField] Rigidbody2D rb;

    public bool MovementLock { get; set; }

    Vector2 lastDirection;
    Vector2 movement;
    float maxSpeed;
    bool moving;

    private void Awake()
    {
        maxSpeed = defaultMovementSpeed;
    }

    public virtual void RawInput(Vector2 inputs)
    {
        if (MovementLock) return;

        lastDirection = inputs.normalized;

        if (inputs.magnitude > 0)
        {
            SetMovement(inputs);

            if (moving == false)
            {
                StartedMoving?.Invoke();
                moving = true;
            }
        }
        else
        {
            // Slow to a stop
            if (instantAcceleration == false)
            {
                if (movement.magnitude <= .5f)
                    movement = Vector2.zero;
                else
                    SetMovement(-movement.normalized * deceleration);
            }
            else
                movement = Vector2.zero;

            if (moving == true)
            {
                moving = false;
                StoppedMoving?.Invoke();
            }
        }
    }

    void SetMovement(Vector2 inputs)
    {
        inputs.Normalize();

        if (instantAcceleration)
            movement = inputs * maxSpeed;
        else
        {
            movement += inputs * acceleration;

            movement = Vector2.ClampMagnitude(movement, maxSpeed);
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = movement;
    }

    public void SetMovementToMax() => movement = lastDirection.normalized * maxSpeed;
    public void SetMovementSpeedToDefault() => maxSpeed = defaultMovementSpeed;
    public void SetMaxMovementSpeed(float speed) => maxSpeed = speed;
}
