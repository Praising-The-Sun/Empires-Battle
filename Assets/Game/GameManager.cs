using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/**
 * Реализует события игры
 */
public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    [Header("Steps")]

    [SerializeField]
    private int m_step = 0;
    public int step { get => m_step; }

    [SerializeField]
    private UnityEvent m_nextStep = new UnityEvent();
    public UnityEvent nextStep { get => m_nextStep; }

    public List<Player> players { get; private set; } = new List<Player>();
    public int activePlayerId { get; private set; } = -1;
    public Player activePlayer {
        get { return activePlayerId == -1 ? null : players[activePlayerId]; }
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        Initialize();
    }

    private void Initialize()
    {

    }

    public void NextPlayer()
    {
        if (players.Count == 0)
        {
            activePlayerId = -1;
            return;
        }

        activePlayerId = ++activePlayerId % players.Count;
        if (activePlayerId == 0)
        {
            ++m_step;
            m_nextStep.Invoke();
        }
    }
}
