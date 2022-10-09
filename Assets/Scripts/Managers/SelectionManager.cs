using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SelectionManager : MonoBehaviour
{
    [SerializeField] private float min;
    [SerializeField] private float max;

    public static Action<EnemySelection> EnemySelectedEvent;
    public static Action EnemyDeselectedEvent;

    public EnemySelection SelectedEnemy { get; set; }


    private void Update()
    {
        SelectEnemy(min, max);
        CheckIfPlayerIsNearToSelectedEnemy();
    }

    private void CheckIfPlayerIsNearToSelectedEnemy()
    {
        if(SelectedEnemy != null)
        {
            Vector3 playerPosition = Player.Instance.gameObject.transform.position;
            float distancePlayerEnemy = (SelectedEnemy.transform.position - playerPosition).magnitude;
            if(distancePlayerEnemy > 6.5)
            {
                SelectedEnemy = null;
                EnemyDeselectedEvent?.Invoke();
            }
        }
    }

    private void SelectEnemy(float min, float max)
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {

            if(SelectedEnemy == null)
            {
                GameObject[] gos = GameObject.FindGameObjectsWithTag("Enemy");
                GameObject closestEnemy = null;
                float distance = Mathf.Infinity;
                Vector3 playerPosition = Player.Instance.gameObject.transform.position;

                // calculate squared distances
                min = min * min;
                max = max * max;
                foreach (GameObject go in gos)
                {
                    Vector3 diff = go.transform.position - playerPosition;
                    float curDistance = diff.sqrMagnitude;
                    if (curDistance < distance && curDistance >= min && curDistance <= max)
                    {
                        closestEnemy = go;
                        distance = curDistance;
                    }
                }
                if(closestEnemy != null)
                {
                    SelectedEnemy = closestEnemy.GetComponent<EnemySelection>();
                    EnemySelectedEvent?.Invoke(SelectedEnemy);
                }
                
            }
            else
            {
                SelectedEnemy = null;
                EnemyDeselectedEvent?.Invoke();
            }

            
        }

    }

}
