using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterAction : MonoBehaviour
{
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
/*
    public void ActionAutoAttack()
    {
        this.CharacterAutoAttack();
    }
    protected abstract void CharacterAutoAttack();*/
}
