using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingText : MonoBehaviour
{
    [SerializeField] private Text textToShow;

    // Sirve para mostrar el texto en posiciones distintas segun un valor aleatorio.
    public Vector3 Offset = new Vector3(0, 0.8f, 0);
    public Vector3 RandomizeIntensity = new Vector3(0.2f, 0.2f, 0.2f);

    private void Start()
    {
        // Sirve para mostrar el texto en posiciones distintas segun un valor aleatorio.
        transform.localPosition += Offset;
        transform.localPosition += new Vector3(Random.Range(-RandomizeIntensity.x, RandomizeIntensity.x), 
            Random.Range(-RandomizeIntensity.y, RandomizeIntensity.y), 
            Random.Range(-RandomizeIntensity.z, RandomizeIntensity.z));

    }

    public void SetUpText(string text, Color color)
    {
        textToShow.text = text;
        textToShow.color = color;

    }
}
