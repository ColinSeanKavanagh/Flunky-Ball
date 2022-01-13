using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// #region State

// public abstract class State
// {

//     public virtual IEnumerator Start()
//     {
//         yield break;
//     }
//     public virtual IEnumerator Throw()
//     {
//         yield break;
//     }

//     public virtual IEnumerator Drink()
//     {
//         yield break;
//     }

//     public virtual IEnumerator Run()
//     {
//         yield break;
//     }
// }

// #endregion State

// #region StateMachine

// public abstract class StateMachine : MonoBehaviour
// {
//     protected State State;

//     public void SetState(State state)
//     {
//         State = state;
//         StartCoroutine(State.Start());
//     }
// }

// #endregion StateMachineClass

// public class Begin : State 
// {
//     public override IEnumerator Start()
//     {
//         return base.Start();
//     }
// }

public enum GameStates { Menu, Onboarding, SwitchTurn, Throw, CheckHit, Drink, End };

public enum PlayerStates { Player1, Player2 };

public class GameRulesController : MonoBehaviour
{

    public GameStates GameStatus;
    public PlayerStates ActivePlayer;
    public GameObject OSCController;

    public bool thrown;
    public bool hit;
    public float timeSinceThrown;

    public int Round;

    // Start is called before the first frame update

    // public void
    void Start()
    {
        GameStatus = GameStates.Throw;

        thrown = OSCController.GetComponent<OSCController>().thrown;
        timeSinceThrown = OSCController.GetComponent<OSCController>().timeSinceThrown;
        ActivePlayer = PlayerStates.Player1;

        Round = 1;
    }

    // Update is called once per frame
    void Update()
    {
        switch (GameStatus)
        {
            case GameStates.Menu:
                // Debug.Log("Menu");
                break;
            case GameStates.Onboarding:
                // Debug.Log("Onboarding");
                break;
            case GameStates.SwitchTurn:
                // Debug.Log("SwitchTurn");
                if(ActivePlayer == PlayerStates.Player1)ActivePlayer = PlayerStates.Player2;
                else ActivePlayer = PlayerStates.Player1;

                // To Do:
                // Also change camera etc.

                break;
            case GameStates.Throw:
                // Debug.Log("Throw");
                break;
            case GameStates.CheckHit:

                if (hit)
                {
                    // Go in Drinking Event
                    GameStatus = GameStates.Drink;
                    break;
                }
                else
                {
                    // Wait 2 Secs after throw if bottle is hit
                    if(timeSinceThrown >= 2f)
                    {
                        // No hit
                        GameStatus = GameStates.SwitchTurn;
                    }
                }
                break;
            case GameStates.End:
                // Debug.Log("End");
                break;
        }

    }

    // public void onThrow()
    // {
    //     StartCoroutine(State.Throw());
    // }
}
