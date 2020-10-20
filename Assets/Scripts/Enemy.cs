using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;
    private Transform _player;
    private Animator _animator;

    // Chase
    [SerializeField] private float _chaseDistance = 7.0f;
    [SerializeField] private float _chaseSpeed = 4.0f;
    [SerializeField] private float _acceleration = 10.0f;
    [SerializeField] private float _lookSpeed = 12.0f;

    // Patrol
    [SerializeField] private float _patrolSpeed = 0.5f;
    [SerializeField] private Transform _moveSpot;
    [SerializeField] private float _startWaitTime = 3.0f;
    private float _waitTime;

    void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        if (_navMeshAgent == null) Debug.LogError("Navmesh Agent is NULL");
        _player = GameObject.Find("Player").GetComponent<Transform>();
        if (_player == null) Debug.LogError("Player is NULL");
        _animator = gameObject.transform.Find("Model").GetComponent<Animator>();
        if (_animator == null) Debug.LogError("Zombie Animator is NULL");
    }

    void Start()
    {
        _waitTime = _startWaitTime;
        _moveSpot.position = GetRandomLocation();
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, _player.transform.position);
        if (distance < _chaseDistance)
        {
            Chase();
        } 
        else 
        {
            Patrol();
        }
    }

    void Chase() 
    {
        _animator.SetBool("chasing", true);
        _animator.SetBool("patroling", false);
        _navMeshAgent.isStopped = false;
        _navMeshAgent.speed = _chaseSpeed;
        _navMeshAgent.acceleration = _acceleration;
        Quaternion targetRotation = Quaternion.LookRotation(_player.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _lookSpeed * Time.deltaTime);
        _navMeshAgent.SetDestination(_player.transform.position);
    }

    void Patrol()
    {
        _animator.SetBool("chasing", false);
        _animator.SetBool("patroling", true);
        _navMeshAgent.speed = _patrolSpeed;
        _navMeshAgent.acceleration = _acceleration;
        _navMeshAgent.SetDestination(_moveSpot.position);
        if (Vector3.Distance(transform.position, _moveSpot.position) < 0.1f)
        {
            if (_waitTime <= 0)
            {
                _navMeshAgent.isStopped = false;
                _moveSpot.position = GetRandomLocation();
                _waitTime = _startWaitTime;
            }
            else
            {
                _animator.SetBool("patroling", false);
                _navMeshAgent.isStopped = true;
                _waitTime -= Time.deltaTime;
            }
        }
    }

     Vector3 GetRandomLocation()
     {
         NavMeshTriangulation navMeshData = NavMesh.CalculateTriangulation();
         int t = Random.Range(0, navMeshData.indices.Length-3);
         Vector3 point = Vector3.Lerp(navMeshData.vertices[navMeshData.indices[t]], navMeshData.vertices[navMeshData.indices[t+1]], Random.value);
         Vector3.Lerp(point, navMeshData.vertices[navMeshData.indices[t+2]], Random.value);
         return point;
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
