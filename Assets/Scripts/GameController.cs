using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    static GameController m_GameController = null;
    MarioPlayerController m_Mario;
    List<IRestartGameElement> m_RestartGameElements = new List<IRestartGameElement>();
    // Start is called before the first frame update

    

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddRestartGameElement(IRestartGameElement RestartGameElement)
    {
        m_RestartGameElements.Add(RestartGameElement);
    }

    public void RestartGame()
    {
        foreach(IRestartGameElement l_RestartGameElements in m_RestartGameElements)
        {
            l_RestartGameElements.RestartGame();
        }
    }

    static public GameController GetGameController()
    {
        return m_GameController;
    }

    public MarioPlayerController GetPlayer()
    {
        return m_Mario;
    }
    
    public void SetPlayer(MarioPlayerController m_Player)
    {
        m_Mario = m_Player;
    }
}
