using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ActionPlayer1 : CharacterAction
{
    private CharacterAgent characterAgent;
    private NavMeshAgent agent;
    private Animator animator;

    private List<GameObject> selectableTileList = new List<GameObject>();
    private GameObject currentSelectedTile;
    private GameObject previousSelectedTile;
    private GameObject attackSelectedTile;

    private List<GameObject> enemyList = new List<GameObject>();

    void Start()
    {
        characterAgent = GetComponent<CharacterAgent>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }
    
    // Start is called before the first frame update
    protected override void CharacterBasicAttack()
    {
        //// Highlight Available Tiles ////
        if((characterAgent.currentTile != characterAgent.lastTile) || (selectableTileList.Count == 0))
        {
            selectableTileList = BoardController.Instance.Ring(1, characterAgent.lastTile);
            BoardController.Instance.HighlightRangeOff(selectableTileList);
            selectableTileList = BoardController.Instance.Ring(1, characterAgent.currentTile);
            BoardController.Instance.HighlightRangeOn(selectableTileList, "cyan");
        }
        //// Highlight Selected Tile ////
        if(CameraCaster.Instance.SelectedTile().gameObject)
        {
            currentSelectedTile = CameraCaster.Instance.SelectedTile().gameObject;
        }
        if((currentSelectedTile != previousSelectedTile) || (characterAgent.currentTile != characterAgent.lastTile))
        {
            if(selectableTileList.Contains(currentSelectedTile))
            {
                currentSelectedTile.GetComponent<Hex>().HighlightOn("red");
            }
            if((currentSelectedTile != previousSelectedTile) && (selectableTileList.Contains(previousSelectedTile)))
            {
                previousSelectedTile.GetComponent<Hex>().HighlightOn("cyan");
            }
        }
        previousSelectedTile = currentSelectedTile;
    }

    protected override void CharacterBasicAttackExecute()
    {
        if(selectableTileList.Contains(currentSelectedTile))
        {
            attackSelectedTile = currentSelectedTile;
            StopCoroutine("BasicAttackCoroutine");
            StartCoroutine("BasicAttackCoroutine");
        }
    }

    protected override void CharacterBasicAttackCancel()
    {
        BoardController.Instance.HighlightAllOff();
        selectableTileList.Clear();
        characterAgent.basicAttackEnabled = false;
        CharacterController.Instance.currentCommand = 'I';
        previousSelectedTile = null;
        attackSelectedTile = null;
    }

    IEnumerator BasicAttackCoroutine()
    {
        animator = GetComponent<Animator>();
        //attacking = true;
        // Stop navigating
        agent.ResetPath();
        animator.SetBool("Run", false);
        // Turn towards enemy
        Vector3 targetDirection = attackSelectedTile.transform.position - transform.position;
        targetDirection.y = 0.0f;
        while(Vector3.Angle(transform.forward, targetDirection) > 1.0f)
        {
            float singleStep = characterAgent.rotSpeed * Time.deltaTime;
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDirection);
            yield return null;
        }
        // Beging attack sequence
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(0.87f);
        enemyList = BoardController.Instance.GetEnemy(attackSelectedTile);
        if(enemyList.Count > 0)
        {
            Debug.Log(enemyList.Count);
            foreach(GameObject enemy in enemyList)
            {
                if(enemy)
                {
                    enemy.GetComponent<EnemyStats>().TakeDamage(50, this.gameObject);
                }
            }
        }
        CharacterBasicAttackCancel();
        //attacking = false;
    }
}
