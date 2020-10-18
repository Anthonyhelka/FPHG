using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;
    private Transform _player;
    [SerializeField] private float _enemyChaseDistance = 7.0f;
    [SerializeField] private float _speed = 3.5f;
    [SerializeField] private float _lookSpeed = 8.0f;

    void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _player = GameObject.Find("Player").GetComponent<Transform>();
        _navMeshAgent.speed = _speed;
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, _player.transform.position);

        if (distance < _enemyChaseDistance)
        {
            Quaternion targetRotation = Quaternion.LookRotation(_player.transform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _lookSpeed * Time.deltaTime);
            _navMeshAgent.SetDestination(_player.transform.position);
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
