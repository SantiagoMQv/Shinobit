using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    [SerializeField] private Vector3[] points;
    public Vector3[] Points => points;

    public Vector3 CurrentPosition { get; set; }
    private bool gameStarted;

    private void Start()
    {
        gameStarted = true;
        CurrentPosition = transform.position;
    }

    public Vector3 GetMovementPosition(int index)
    {
        return CurrentPosition + points[index];
    }

    private void OnDrawGizmos()
    {
        // Para reubicar la posicion del personaje si lo movemos en el editor y usarlo para que los puntos lo sigan 
        if (!gameStarted && transform.hasChanged)
        {
            CurrentPosition = transform.position;
        }

        // Solo dibuja los puntos, si hay puntos
        if(points != null || points.Length > 0)
        {
            for (int i = 0; i < points.Length; i++)
            {
                // Dibuja esfera en la posicion de cada punto con un radio 0.5f
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(points[i] + CurrentPosition, 0.5f);
                // Ahora dibujaremos las lineas que unen las esferas, que serán n-1 siendo n el numero de esferas
                if (i < points.Length - 1)
                {
                    Gizmos.color = Color.gray;
                    Gizmos.DrawLine(points[i] + CurrentPosition, points[i + 1] + CurrentPosition);
                }
            }
        }
    }

}
