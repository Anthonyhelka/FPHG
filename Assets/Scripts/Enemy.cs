using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;
    private Transform _player;
    [SerializeField] private float _enemyChaseDistance = 7.0f;
    [SerializeField] private float _speed = 4.0f;
    [SerializeField] private float _acceleration = 10.0f;
    [SerializeField] private float _lookSpeed = 8.0f;
    private Animator _animator;

    void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _player = GameObject.Find("Player").GetComponent<Transform>();
        _navMeshAgent.speed = _speed;
        _animator = gameObject.transform.Find("Zombie").GetComponent<Animator>();
        if (_animator == null) Debug.LogError("Zombie Animator is NULL");
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, _player.transform.position);

        if (distance < _enemyChaseDistance)
        {
            _animator.SetBool("chasing", true);
            _navMeshAgent.isStopped = false;
            Quaternion targetRotation = Quaternion.LookRotation(_player.transform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _lookSpeed * Time.deltaTime);
            _navMeshAgent.SetDestination(_player.transform.position);
            _navMeshAgent.acceleration = _acceleration;
        } else {
            _navMeshAgent.isStopped = true;
            _animator.SetBool("chasing", false);
        }
    }

        void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameObject player = GameObject.Find("Player");
            Destroy(player);
        }
    }
}
