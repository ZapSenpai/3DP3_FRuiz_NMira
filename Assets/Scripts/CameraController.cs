using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform m_LookAtTransform;
    float m_Pitch = 0.0f;
    public float m_Distance = 15.0f;
    public float m_YawRotSpeed = 720.0f;
    public float m_PitchRotSpeed = 360.0f;
    public float m_MaxPitch = 85f;
    public float m_MinPitch = -85f;
    public float m_MinDistance = 2.0f;
    public float m_MaxDistance = 5.0f;
    public LayerMask m_AvoidObjectsLayerMask;
    public float m_AvoidObjectOffset = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float l_MouseX = Input.GetAxis("Mouse X");
        float l_MouseY = Input.GetAxis("Mouse Y");
        float l_Distance = Vector3.Distance(transform.position, m_LookAtTransform.position);
        l_Distance = Mathf.Clamp(l_Distance, m_MinDistance, m_MaxDistance);
        transform.LookAt(m_LookAtTransform.position);
        Vector3 l_EulerAngles = transform.rotation.eulerAngles;
        float l_Yaw = l_EulerAngles.y;

        l_Yaw += l_MouseX * m_YawRotSpeed * Time.deltaTime;
        m_Pitch += l_MouseY * m_PitchRotSpeed * Time.deltaTime;
        m_Pitch = Mathf.Clamp(m_Pitch, m_MinPitch, m_MaxPitch);

        Vector3 l_ForwardCamera = new Vector3(Mathf.Sin(l_Yaw * Mathf.Deg2Rad) * Mathf.Cos(m_Pitch * Mathf.Deg2Rad), Mathf.Sin(m_Pitch * Mathf.Deg2Rad), Mathf.Cos(l_Yaw * Mathf.Deg2Rad) * Mathf.Cos(m_Pitch * Mathf.Deg2Rad));
        Vector3 l_DesiredPosition = m_LookAtTransform.position - l_ForwardCamera * l_Distance;

        Ray l_Ray = new Ray(m_LookAtTransform.position, -l_ForwardCamera);
        RaycastHit l_RaycastHit;
        if (Physics.Raycast(l_Ray, out l_RaycastHit, l_Distance, m_AvoidObjectsLayerMask.value))
        {
            l_DesiredPosition = l_RaycastHit.point + l_ForwardCamera * m_AvoidObjectOffset;
        }

        transform.position = l_DesiredPosition;
        transform.LookAt(m_LookAtTransform.position);
    }
}
