using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _cam;
    [SerializeField] private CinemachineVirtualCamera _cam2;
    [SerializeField] private Transform _lookDownPos;
    [SerializeField] private Transform _lookUpPos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.wKey.isPressed)
        {
            Debug.Log("Look Down");
            //_cam.m_LookAt = _lookDownPos;
            _cam2.MoveToTopOfPrioritySubqueue();
        }

        if (Keyboard.current.sKey.isPressed)
        {
            Debug.Log("Look Up");
            //_cam.m_LookAt = _lookUpPos;
            _cam.MoveToTopOfPrioritySubqueue();
        }
    }
}
