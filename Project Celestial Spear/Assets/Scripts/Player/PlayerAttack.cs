using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public MainCharacterBasicMovement playerControllerReferenceWhichHasATurnOnAndTurnOffBoolean;
    public PlayerAnimationController playerAnimationControllerReference;
    public BoxCollider2D myAtackBoxCollider;

    private Statuses statusSciptOfEnemy;
    private Rigidbody2D enemyRigidBody;
    
    private bool canAttack = true;
    private int attackIDCounterWhichIsUsedToControlWhichAttackIsToBeExecuted = 10; // In case the user doesn't input a number. This represents the number of attacks, starting from zero
    private float playerFacingDirection;


    public float maximumAllowedDelayBetweenAttackButtonPresses = 1f;
    private float timeStampWhenAttackButtonWasLastPressed = 0f;
   

    public float pushBackForceOfFirstAttack;
    public int damageOfFirstAttack;
    public float windingUpTimeOfFirstAttack;
    public int damageOfSecondAttack;
    public float windingUpTimeOfSecondAttack;
    public int damageOfThirdAttack;
    public float windingUpTimeOfThirdAttack;

    // Start is called before the first frame update
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
       
        CheckPlayerFacingDirection();
        CheckIfPlayerCanAttackAndExecuteAttackIfThePlayerCan();
    }

    private void CheckIfPlayerCanAttackAndExecuteAttackIfThePlayerCan()
    {
        if (canAttack)
        {
            StartCoroutine(AttackWhenverAttackButtonIsPressedAndEnemyRigidbodyWithAnAttachedStatusScriptIsAvailable());
        }
    }



   





    public void IfEnemyHasBeenDetectedThenPushTheEnemyAndAlsoPlayTheAppropriateAnimation()
    {
        if (enemyRigidBody)
        {
            CalculateTheForceAsVectorAndAddItToTheEnemyRigicbody();

            playerAnimationControllerReference.ChangeAnimationState(playerAnimationControllerReference.PUNCH_AND_PUSH_ANIMATION);

            enemyRigidBody = null;

        }

        else
        {
            playerAnimationControllerReference.ChangeAnimationState(playerAnimationControllerReference.PUNCH_AND_PUSH_ANIMATION);
        }
    }

    private void CalculateTheForceAsVectorAndAddItToTheEnemyRigicbody()
    {
        Vector2 pushBackForceToAddAsVector = new Vector2(playerFacingDirection * pushBackForceOfFirstAttack, 0f);
        enemyRigidBody.AddForce(pushBackForceToAddAsVector, ForceMode2D.Impulse);
    }

    


    private void OnTriggerStay2D(Collider2D collision) // This function is automatically called by unity like the update function
    {
        if (collision.tag == "Enemy")
        {
            Debug.Log("Collision with enemy successfully detected");

            enemyRigidBody = collision.gameObject.GetComponent<Rigidbody2D>();

        }
    }


    public void CheckPlayerFacingDirection()
    {
        if (gameObject.transform.rotation.y == 0)
        {
            playerFacingDirection = 1; // 1 means right

        }
        else if (gameObject.transform.rotation.y == -1)
        {
            playerFacingDirection = -1;

        }

    }





    public IEnumerator AttackWhenverAttackButtonIsPressedAndEnemyRigidbodyWithAnAttachedStatusScriptIsAvailable()
    {
        if (enemyRigidBody)
        {
            statusSciptOfEnemy = enemyRigidBody.gameObject.GetComponent<Statuses>();

        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {

            attackIDCounterWhichIsUsedToControlWhichAttackIsToBeExecuted += 1;

            ResetAttackIDCounterToZeroIfTooMuchDelayBetweenButtonPresses();


            if (attackIDCounterWhichIsUsedToControlWhichAttackIsToBeExecuted > 2)
            {
                attackIDCounterWhichIsUsedToControlWhichAttackIsToBeExecuted = 0;
            }


            if (attackIDCounterWhichIsUsedToControlWhichAttackIsToBeExecuted == 0)
            {
                SetMainCharacterVelocityToZeroToStopTheLeftOverMovementWhenCanMoveIsTurnedOff();
                playerControllerReferenceWhichHasATurnOnAndTurnOffBoolean.canMove = false;
                canAttack = false;

                IfEnemyHasBeenDetectedThenPushTheEnemyAndAlsoPlayTheAppropriateAnimation();
                yield return new WaitForSeconds(windingUpTimeOfFirstAttack);

                

                if (statusSciptOfEnemy)
                {
                    statusSciptOfEnemy.DecreaseHealthByTheNumber(damageOfFirstAttack);
                }

                timeStampWhenAttackButtonWasLastPressed = Time.time;

                statusSciptOfEnemy = null;
                playerControllerReferenceWhichHasATurnOnAndTurnOffBoolean.canMove = true;
                canAttack = true;
            }

            if (attackIDCounterWhichIsUsedToControlWhichAttackIsToBeExecuted == 1)
            {
                SetMainCharacterVelocityToZeroToStopTheLeftOverMovementWhenCanMoveIsTurnedOff();
                canAttack = false;
                playerControllerReferenceWhichHasATurnOnAndTurnOffBoolean.canMove = false;

                playerAnimationControllerReference.ChangeAnimationState(playerAnimationControllerReference.PLAYER_ATTACK_ONE_ANIMATION);
                yield return new WaitForSeconds(windingUpTimeOfSecondAttack);

                if (statusSciptOfEnemy)
                {
                    statusSciptOfEnemy.DecreaseHealthByTheNumber(damageOfSecondAttack);
                }

                timeStampWhenAttackButtonWasLastPressed = Time.time;                

                statusSciptOfEnemy = null;
                playerControllerReferenceWhichHasATurnOnAndTurnOffBoolean.canMove = true;
                canAttack = true;
            }

            if (attackIDCounterWhichIsUsedToControlWhichAttackIsToBeExecuted == 2)
            {
                SetMainCharacterVelocityToZeroToStopTheLeftOverMovementWhenCanMoveIsTurnedOff();
                canAttack = false;
                playerControllerReferenceWhichHasATurnOnAndTurnOffBoolean.canMove = false;

                playerAnimationControllerReference.ChangeAnimationState(playerAnimationControllerReference.PLAYER_ATTACK_TWO_ANIMATION);
                yield return new WaitForSeconds(windingUpTimeOfThirdAttack);

                if (statusSciptOfEnemy)
                {
                    statusSciptOfEnemy.DecreaseHealthByTheNumber(damageOfThirdAttack);
                }

                timeStampWhenAttackButtonWasLastPressed = Time.time;



                statusSciptOfEnemy = null;
                playerControllerReferenceWhichHasATurnOnAndTurnOffBoolean.canMove = true;
                canAttack = true;
            }


        }

    }

    private void ResetAttackIDCounterToZeroIfTooMuchDelayBetweenButtonPresses()
    {
        if(Time.time - timeStampWhenAttackButtonWasLastPressed > maximumAllowedDelayBetweenAttackButtonPresses)
        {
            attackIDCounterWhichIsUsedToControlWhichAttackIsToBeExecuted = 0;
        }
    }

   public void ResetAttackIDCounterToRightBeforeZeroWheneverRequired()
    {
        attackIDCounterWhichIsUsedToControlWhichAttackIsToBeExecuted = 10; 
    }

    void SetMainCharacterVelocityToZeroToStopTheLeftOverMovementWhenCanMoveIsTurnedOff()
    {
        playerControllerReferenceWhichHasATurnOnAndTurnOffBoolean.SetMainCharacterVelocityToZeroWhenCalled();
    }
}
