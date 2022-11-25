using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MarioPlayerController : MonoBehaviour, IRestartGameElement
{
    Animator m_Animator;
    CharacterController m_CharacterController;
    float m_VerticalSpeed = 0.0f;

    public Camera m_Camera;
    public float m_LerpRotation = 0.85f;
    public float m_WalkSpeed = 2.5f;
    public float m_RunSpeed = 6.5f;

    [Header("Punch")]
    public float m_ComboPunchTime = 2.5f;
    float m_ComboPunchCurrentTime;
    TPunchType m_CurrentPunchType;
    public Collider m_RightHandCollider;
    public Collider m_LeftHandCollider;
    public Collider m_KickFootCollider;
    bool m_PunchActive = false;

    Vector3 m_StartPosition;
    Quaternion m_StartRotation;

    public Collider m_CurrentElevatorCollider;
    public float m_ElevatorDotAngle = 0.95f;

    public float m_BridgeForce = 0.05f;

    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
        m_CharacterController = GetComponent<CharacterController>();
    }
    void Start()
    {
        m_ComboPunchCurrentTime = -m_ComboPunchTime;
        m_RightHandCollider.gameObject.SetActive(false);
        m_LeftHandCollider.gameObject.SetActive(false);
        m_KickFootCollider.gameObject.SetActive(false);
        m_StartPosition = transform.position;
        m_StartRotation = transform.rotation;
        GameController.GetGameController();
    }

    void Update()
    {
        float l_Speed = 0.0f;

        Vector3 l_ForwardCamera = m_Camera.transform.forward;
        Vector3 l_RightCamera = m_Camera.transform.right;
        l_ForwardCamera.y = 0.0f;
        l_RightCamera.y = 0.0f;
        l_ForwardCamera.Normalize();
        l_RightCamera.Normalize();
        bool l_HasMovenent = false;

        Vector3 l_Movement = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            l_HasMovenent = true;
            l_Movement = l_ForwardCamera;
        }
        if (Input.GetKey(KeyCode.S))
        {
            l_HasMovenent = true;
            l_Movement = -l_ForwardCamera;
        }
        if (Input.GetKey(KeyCode.A))
        {
            l_HasMovenent = true;
            l_Movement -= l_RightCamera;
        }
        if (Input.GetKey(KeyCode.D))
        {
            l_HasMovenent = true;
            l_Movement += l_RightCamera;
        }
        l_Movement.Normalize();

        float l_MovementSpeed = 0.0f;
        if (l_HasMovenent)
        {
            Quaternion l_LookRotation = Quaternion.LookRotation(l_Movement);
            transform.rotation = Quaternion.Lerp(transform.rotation, l_LookRotation, m_LerpRotation);

            l_Speed = 0.5f;
            l_MovementSpeed = m_WalkSpeed;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                l_Speed = 1.0f;
                l_MovementSpeed = m_RunSpeed;
            }
        }
        l_Movement = l_Movement * l_MovementSpeed * Time.deltaTime;
        m_Animator.SetFloat("Speed", l_Speed);
        
        //m_CharacterController.Move(l_Movement);
        m_VerticalSpeed += Physics.gravity.y * Time.deltaTime;
        if (Input.GetMouseButtonDown(0) && CanPunch())
        {
            if (RestartComboPunch())
            {
                SetPunchCombo(TPunchType.RIGHT_PUNCH);
            }
            else
            {
                NextComboPunch();
            }
        }
        l_Movement.y = m_VerticalSpeed * Time.deltaTime;
        CollisionFlags l_CollisionFlags = m_CharacterController.Move(l_Movement);
        if ((l_CollisionFlags & CollisionFlags.Below) != 0 && m_VerticalSpeed < 0.0f)
        {
            m_VerticalSpeed = 0.0f;
        }
    }
    public enum TPunchType
    {
        LEFT_PUNCH, RIGHT_PUNCH, KICK
    }

    public void SetPunchActive(TPunchType PunchType, bool isActive)
    {
        if (PunchType == TPunchType.RIGHT_PUNCH)
        {
            m_RightHandCollider.gameObject.SetActive(isActive);
        }
        else if (PunchType == TPunchType.LEFT_PUNCH)
        {
            m_LeftHandCollider.gameObject.SetActive(isActive);
        }
        else if (PunchType == TPunchType.KICK)
        {
            m_KickFootCollider.gameObject.SetActive(isActive);
        }
    }

    bool CanPunch()
    {
        return !m_PunchActive;
    }

    public void SetPunchEnabled(bool isPunchActive)
    {
        m_PunchActive = isPunchActive;
    }

    bool RestartComboPunch()
    {
        return (Time.time - m_ComboPunchCurrentTime) > m_ComboPunchTime;
    }
    
    void NextComboPunch()
    {
        if (m_CurrentPunchType == TPunchType.RIGHT_PUNCH)
        {
            SetPunchCombo(TPunchType.LEFT_PUNCH);
        }
        else if(m_CurrentPunchType == TPunchType.LEFT_PUNCH)
        {
            SetPunchCombo(TPunchType.KICK);
        }
        else if (m_CurrentPunchType == TPunchType.KICK)
        {
            SetPunchCombo(TPunchType.RIGHT_PUNCH);
        }
    }

    void SetPunchCombo(TPunchType PunchType)
    {
        m_CurrentPunchType = PunchType;
        m_ComboPunchCurrentTime = Time.deltaTime;
        m_PunchActive = true;
        if (m_CurrentPunchType == TPunchType.RIGHT_PUNCH)
        {
            m_Animator.SetTrigger("PunchRightHand");
        }
        else if (m_CurrentPunchType == TPunchType.LEFT_PUNCH)
        {
            m_Animator.SetTrigger("PunchLeftHand");
        }
        else if (m_CurrentPunchType == TPunchType.KICK)
        {
            m_Animator.SetTrigger("PunchKick");
        }
    }

    public void RestartGame()
    {
        Debug.Log("rs");
        m_CharacterController.enabled = false;
        m_StartPosition = transform.position;
        m_StartRotation = transform.rotation;
        m_CharacterController.enabled = true;
    }

    

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Elevator" && CanAttachToElevator(other))
        {

        }
    }
    bool CanAttachToElevator(Collider other)
    {
        return m_CurrentElevatorCollider == null && Vector3.Dot(other.transform.up, Vector3.up) >= m_ElevatorDotAngle;
    }

    void DetachElevator()
    {
        transform.SetParent(null);
        m_CurrentElevatorCollider = null;
    }
}
