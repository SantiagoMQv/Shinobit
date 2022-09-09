using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearAttack : MonoBehaviour
{
    private Vector3 initialPosition;
    private Vector3 newPositionAttack;
    void Update()
    {
        initialPosition = Player.Instance.combatPlayer.CalculateInitialPositionAttack();
        newPositionAttack = Player.Instance.combatPlayer.RotateSpearWeapon() ;
        MeleeAttack();
    }

    public void MeleeAttack()
    {
        StartCoroutine(IEMelee());
    }

    private IEnumerator IEMelee()
    {
        Vector3 directionToPlayer = (newPositionAttack - initialPosition).normalized;


        float attackTransition = 0;
        while (attackTransition <= 1)
        {
            attackTransition += Time.deltaTime * 10;

            transform.position = Vector3.Lerp(initialPosition, newPositionAttack, attackTransition);
            yield return null; // Permite esperar 1 frame
        }


    }

    public void InitializeSpearAttack(Vector3 initialPosition, Vector3 newPositionAttack)
    {
        this.initialPosition = initialPosition;
        this.newPositionAttack = newPositionAttack;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy" )
        {
            collision.gameObject.GetComponent<EnemyHealth>().GetDamage(Player.Instance.combatPlayer.GetDamage(), collision.gameObject);
        }
    }

}
