using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineController : MonoBehaviour
{
    public CinemachineFreeLook cam;
    void Start()
    {
        cam.m_XAxis.m_MaxSpeed = 0;
    }
}
