using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickeringLight : MonoBehaviour
{
    private Light _light;
    [SerializeField] private float _minWaitTime = 0.1f;
    [SerializeField] private float _maxWaitTime = 0.5f;

    // Start is called before the first frame update
    void Awake()
    {
       _light = GetComponent<Light>();
        if (_light == null) Debug.LogError("Light is NULL");
    }

    void Start()
    {
        StartCoroutine(Flickering());
    }

    IEnumerator Flickering()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(_minWaitTime, _maxWaitTime));
            _light.enabled = !_light.enabled;
        }
    }
}
