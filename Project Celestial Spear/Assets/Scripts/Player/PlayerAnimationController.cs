using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    

    public Animator myAnimator;

    private string currentState;

    

    public  string IDLE_ANIMATION = "Idle";
    public  string RUNNING_ANIMATION = "Running";
    public string PUNCH_AND_PUSH_ANIMATION = "PunchAndPush";
    public string PLAYER_ATTACK_ONE_ANIMATION = "PlayerAttackOne";
    public string PLAYER_ATTACK_TWO_ANIMATION = "PlayerAttackTwo";


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        


    }

    



    public void ChangeAnimationState(string newState)
    {
        if (currentState == newState)
        {
            return;
        }
        else
        {
            myAnimator.Play(newState);

            currentState = newState;
        }
    }
}
