using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterAction : MonoBehaviour
{
    abstract public bool Attacking {get; set;}
    abstract public bool BasicAttackAvailable {get; set;}
    
    public void ActionBasicAttack()
    {
        this.CharacterBasicAttack();
    }
    protected abstract void CharacterBasicAttack();

    public void ActionBasicAttackExecute()
    {
        this.CharacterBasicAttackExecute();
    }
    protected abstract void CharacterBasicAttackExecute();

    public void ActionBasicAttackCancel()
    {
        this.CharacterBasicAttackCancel();
    }
    protected abstract void CharacterBasicAttackCancel();

    public void ActionAutoAttack()
    {
        this.CharacterAutoAttack();
    }
    protected abstract void CharacterAutoAttack();
}
