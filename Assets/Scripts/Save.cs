using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Save : MonoBehaviour
{
    GameObject player, enemy, timer;
    private void Awake() {
        SaveSystem.Init();
    }

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        enemy = GameObject.FindGameObjectWithTag("Enemy");
        timer = GameObject.Find("Manager");
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.S)) {
            Salvar();
        }

        if (Input.GetKeyDown(KeyCode.L)) {
            Load();
        }
    }

    private void Salvar() {

        // Save
        Vector3 playerPosition = player.transform.position;
        Vector3 enemyPosition = enemy.transform.position;
        int playerPoints = player.GetComponent<Mouse>().Points;

        GameObject[] cheeses = GameObject.FindGameObjectsWithTag("Cheese");
        float currentTime = timer.GetComponent<Timer>().TimerValue;
        
        SaveObject saveObject = new SaveObject(playerPoints, playerPosition, enemyPosition, cheeses, currentTime);

        string json = JsonUtility.ToJson(saveObject);
        SaveSystem.Save(json);

        Debug.Log("Salvou");
    }

    private void Load() {
        // Load
        string saveString = SaveSystem.Load();
        if (saveString != null) {
            SaveObject saveObject = JsonUtility.FromJson<SaveObject>(saveString);

            player.transform.position = saveObject.savePlayerPosition;
            enemy.transform.position = saveObject.saveEnemyPosition;
            player.GetComponent<Mouse>().Points = saveObject.savePlayerPoints;
            
            timer.GetComponent<Timer>().TimerValue = saveObject.saveTimer;

            foreach (var item in saveObject.saveCheeses)
            {
                item.SetActive(true);
            }
        }
    }


    private class SaveObject {
        public int savePlayerPoints;
        public Vector3 savePlayerPosition;
        public Vector3 saveEnemyPosition;
        public float saveTimer;
        public List<GameObject> saveCheeses = new List<GameObject>();

        public SaveObject(int playerPoints, Vector3 playerPosition, Vector3 enemyPosition, GameObject[] cheeses, float timer)
        {
            savePlayerPoints = playerPoints;
            savePlayerPosition = playerPosition;
            saveEnemyPosition = enemyPosition;
            saveTimer = timer;

            foreach (var item in cheeses)
            {
                saveCheeses.Add(item);
            }
        }
    }
}
