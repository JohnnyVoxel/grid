using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ActionPlayer1 : CharacterAction
{
    private CharacterAgent characterAgent;
    private CharacterStats characterStats;
    private NavMeshAgent agent;
    private Animator animator;

    private List<GameObject> selectableTileList = new List<GameObject>();
    private GameObject currentSelectedTile;
    private GameObject previousSelectedTile;
    private GameObject attackSelectedTile;
    private bool attacking = false;
    private bool basicAttackAvailable = true;
    public override bool Attacking
    {
        get { return attacking;}
        set{}
    }
    public override bool BasicAttackAvailable
    {
        get { return basicAttackAvailable;}
        set{}
    }

    private List<GameObject> enemyList = new List<GameObject>();

    void Start()
    {
        characterAgent = GetComponent<CharacterAgent>();
        characterStats = GetComponent<CharacterStats>();
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

    protected override void CharacterAutoAttack()
    {
        int enemyCount = 0;
        attackSelectedTile = null;
        // Check if enemies are on the same tile the character is on
        enemyCount = BoardController.Instance.GetEnemy(characterAgent.currentTile).Count;
        if (enemyCount > 0)
        {
            attackSelectedTile = characterAgent.currentTile;
            StopCoroutine("BasicAttackCoroutine");
            StartCoroutine("BasicAttackCoroutine");
        }
        else
        {
            selectableTileList = BoardController.Instance.Ring(1, characterAgent.currentTile);
            foreach (GameObject tile in selectableTileList)
            {
                enemyCount = BoardController.Instance.GetEnemy(tile).Count;
                if (enemyCount > 0)
                {
                    attackSelectedTile = tile;
                    break;
                }
            }
            if(attackSelectedTile)
            {
                StopCoroutine("BasicAttackCoroutine");
                StartCoroutine("BasicAttackCoroutine");
            }

        }
        // Check all tiles in a ring around the character
        // Decide to attack or not
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
        attacking = false;
        StopCoroutine("BasicAttackCoroutine");
        StartCoroutine("BasicAttackCooldown");
    }

    IEnumerator BasicAttackCoroutine()
    {
        attacking = true;
        basicAttackAvailable = false;
        animator = GetComponent<Animator>();
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
            foreach(GameObject enemy in enemyList)
            {
                if(enemy)
                {
                    enemy.GetComponent<EnemyStats>().TakeDamage(characterStats.attackDamage, this.gameObject);
                }
            }
        }
        CharacterBasicAttackCancel();
    }

    IEnumerator BasicAttackCooldown()
    {
        yield return new WaitForSeconds(characterStats.cooldownBasicAttack);
        basicAttackAvailable = true;
    }
}
