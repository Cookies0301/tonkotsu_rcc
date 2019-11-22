using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using NaughtyAttributes;

public class Enemy : MonoBehaviour, IDamagable
{
    [BoxGroup("Enemy")]
    [SerializeField] float attackRange, ragdollForce, hitRange;

    [BoxGroup("Enemy")]
    [SerializeField] GameObject ragdoll;

    [BoxGroup("Animation")]
    [SerializeField] string movingBoolParameter, attackingBoolParameter;

    [SerializeField] GameObject weapon;

    NavMeshAgent agent;
    Animator animator;
    bool hit;


    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!agent.isOnNavMesh)
        {
            return;
        }

        if(Vector3.Distance(transform.position, PlayerHandler.Player.position) < attackRange)
        {
            agent.SetDestination(PlayerHandler.Player.position);
            animator.SetBool(movingBoolParameter, true);
            weapon.SetActive(true);
        }
        else
        {
            agent.SetDestination(transform.position);
            animator.SetBool(movingBoolParameter, false);
            weapon.SetActive(false);
        }

        if(Vector3.Distance(transform.position, PlayerHandler.Player.position) < hitRange)
        {
            animator.SetBool(attackingBoolParameter, true);
            Debug.Log("Attack");
        }
        else
        {
            animator.SetBool(attackingBoolParameter, false);
        }

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    public void TakeDamage(Damager source, ContactPoint contact)
    {
        Destroy(gameObject);
        var rd = Instantiate(ragdoll, transform.position, transform.rotation);

        var rbs = rd.GetComponentsInChildren<Rigidbody>();

        foreach (var rb in rbs)
        {
            Vector3 dir = rb.transform.position - source.transform.position;

            Vector3 force = dir.normalized * ragdollForce;

            rb.AddForce(force);
        }
    }
}