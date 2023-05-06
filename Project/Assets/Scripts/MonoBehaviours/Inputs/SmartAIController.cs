using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartAIController : AiController
{
    bool fleeing;

    protected override void Start()
    {
        base.Start();
        StartCoroutine(Vary());
    }

    protected override void NavigateTarget()
    {
        Gun gun = itemHolder.Item as Gun;

        float dist = Vector2.Distance(transform.position, Target.position);

        Vector2 direction = (Target.position - transform.position).normalized;

        // How far am I from the optimal position?
        float optimality = dist - (gun.Range / 1.1f);
        float value = Mathf.Abs(optimality);
        // Cube it so it has a cubic graph
        value *= value * value;

        if (fleeing == true)
        {
            // If I am closer than I need to be and I am fleeing
            if (optimality < 0)
                value = .5f;
        }
        // If I am further than I need to be and I am coming closer
        else if (optimality > 0)
            value = .2f;

        if (Random.Range(0f, 100) <= value)
            fleeing = !fleeing;

        if (fleeing)
            direction = -direction;


        direction.x += -Mathf.Clamp(Mathf.Cos(Time.time * frequency.x), -0.5f, 0.5f);
        direction.y += Mathf.Clamp(Mathf.Sin(Time.time * frequency.y), -0.5f, 0.5f);

        mover.RawInput(direction);
    }

    Vector2 frequency;

    void SetVariationFrequency()
    {
        frequency = new Vector2(Random.value, Random.value);
    }

    void Update()
    {
        if (Target)
            Attack();
        else
        {
            mover.RawInput(Vector2.zero);
            TryChooseTarget();
        }
    }

    protected override void Aim()
    {
        Vector3 aimAhead = Vector3.zero;

        if (targetRb != null)
            if (Vector2.Distance(transform.position, Target.position) > 1.5f)
                aimAhead = targetRb.velocity.normalized;

        //Gun g = itemHolder.Item as Gun;
        //float attackSpeed = g.AttackForce;

        //float distToPredicted = targetDirection * (distToPredicted / attackSpeed);
        //float time = distToPredicted / attackSpeed;

        //Vector2 predictedPosition = target.position + (targetDirection * (distToPredicted / attackSpeed));
        //float distToTarget = Vector2.Distance(transform.position, target.position);

        //float time = Mathf.Sqrt((distToTarget * distToTarget) + (distToPredicted * distToPredicted))

        //Vector3 predictedPosition = target.position + (targetDirection * time);
        //Vector2 toPredictedPosition = predictedPosition - transform.position;

        //toPredictedPosition * time = target.position + (targetDirection * time);

        //toPredictedPosition - targetDirection = target.position / time;

        itemHolder.Aim(Target.position + aimAhead, Vector2.one /** Mathf.Cos(Time.time)*/);
    }

    IEnumerator Vary()
    {
        yield return new WaitForSeconds(2);
        SetVariationFrequency();
        StartCoroutine(Vary());
    }
}
