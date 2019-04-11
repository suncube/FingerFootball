using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FootballManager : MonoBehaviour
{
    // TODO ADD Event for ball move ended;!!!!!!!!!!!!!

    public static FootballManager runtime;

    public FootballPointer FootballPointer;

    public Text MatchScore;
    public FBall Ball;
    public FootballPlayer[] Players1;
    public FootballPlayer[] Players2;

    private int Player1Score = 0;
    private int Player2Score = 0;

    private bool isFirstPlayerMove;

    private void Awake()
    {
        runtime = this;

        Ball.SaveStartState();
        for (var index = 0; index < Players1.Length; index++)
        {
            Players1[index].SaveStartState();
            Players1[index].OnMoveEnded += OnFirstPlayerMoveEnded;
        }

        for (var index = 0; index < Players2.Length; index++)
        {
            Players2[index].SaveStartState();
            Players2[index].OnMoveEnded += OnSecondPlayerMoveEnded;
        }

        Initialize();
    }

    private void OnSecondPlayerMoveEnded()
    {
        SetPlayerMove(true);
    }

    private void OnFirstPlayerMoveEnded()
    {
        SetPlayerMove(false);
    }

    public void Initialize()
    {
        Ball.OnGoal += OnMakeGoal;
        FootballPointer.gameObject.SetActive(false);

        Player1Score = 0;
        Player2Score = 0;

        RestartGame();
    }

    public void RestartGame()
    {
        SetPlayerMove(true);
        MatchScore.text = string.Format("{0} : {1}", Player1Score, Player2Score);

        RestartSession();
    }

    public void RestartSession()
    {
        Ball.RestartStartState();

        for (var index = 0; index < Players1.Length; index++)
        {
            Players1[index].RestartStartState();
        }

        for (var index = 0; index < Players2.Length; index++)
        {
            Players2[index].RestartStartState();
        }
    }

    private void SetPlayerMove(bool isFirstPlayer)
    {
        Debug.Log(isFirstPlayer);
        isFirstPlayerMove = isFirstPlayer;
        if (isFirstPlayerMove)
        {
            SetCanMoved(Players1, true);
            SetCanMoved(Players2, false);
        }
        else
        {
            SetCanMoved(Players1,false);
            SetCanMoved(Players2, true);
        }
    }

    private void SetCanMoved(FootballPlayer[] players, bool isCanMoved)
    {
        for (var index = 0; index < players.Length; index++)
        {
            players[index].IsCanMoved = isCanMoved;
        }
    }

    private void OnMakeGoal(FGate fGate)
    {
       //TODO  need bloak all touches 

        if (fGate.FGateType == FGateType.Player1)
        {
            Player1Score++;
        }
        else
        {
            Player2Score++;
        }

        MatchScore.text = string.Format("{0} : {1}", Player1Score, Player2Score);

        Invoke("RestartSession",1f);
    }

    private void OnDestroy()
    {
        for (var index = 0; index < Players1.Length; index++)
        {
            Players1[index].OnMoveEnded += OnFirstPlayerMoveEnded;
        }
        for (var index = 0; index < Players2.Length; index++)
        {
            Players2[index].OnMoveEnded += OnSecondPlayerMoveEnded;
        }

        Ball.OnGoal -= OnMakeGoal;
        runtime = null;
    }
}


public interface IRestarted
{
    void SaveStartState();
    void RestartStartState();
}