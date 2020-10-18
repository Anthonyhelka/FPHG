using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightController : MonoBehaviour
{
    public GameObject _light;
    private bool _status;
    private bool _mouseInput, _tInput;

    void Start()
    {
        _light = gameObject.transform.Find("Light").gameObject;
        if (_light == null) Debug.LogError("Light is NULL");
    }

    void Update()
    {
        GetInput();
        if (_mouseInput || _tInput)
        {
            _status = !_status;
            _light.SetActive(_status);
        }
    }

    void GetInput()
    {
         _mouseInput = Input.GetKeyDown(KeyCode.Mouse0);
         _tInput = Input.GetKeyDown(KeyCode.T);
    }
}
