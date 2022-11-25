using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TecnocampusTimeScalerDebug : MonoBehaviour
{
    public KeyCode m_FastKeyCode = KeyCode.Minus;
    public KeyCode m_SlowKeyCode = KeyCode.RightShift;

    // Start is called before the first frame update
    void Start()
    {
        
    }
#if UNITY_EDITOR
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(m_FastKeyCode))
        {
            Time.timeScale = 10.0f;
        }
        if (Input.GetKeyDown(m_SlowKeyCode))
        {
            Time.timeScale = 0.1f;
        }
        if (Input.GetKeyUp(m_FastKeyCode))
        {
            Time.timeScale = 1.0f;
        }
        if (Input.GetKeyUp(m_SlowKeyCode))
        {
            Time.timeScale = 1.0f;
        }
    }
#endif
}
