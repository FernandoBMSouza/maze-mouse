using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Interface : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI pointsText;
    [SerializeField] GameObject player;
    [SerializeField] GameObject[] cheeses;
    [SerializeField] Canvas canvas;


    private void Start() {
        canvas = GameObject.FindObjectOfType<Canvas>();
        pointsText = canvas.transform.Find("PointsText").GetComponent<TextMeshProUGUI>();
        player = GameObject.FindGameObjectWithTag("Player");
        cheeses = GameObject.FindGameObjectsWithTag("Cheese");
    }

    private void Update() {
        pointsText.text = player.GetComponent<Mouse>().Points + "/" + cheeses.Length;
    }
}
