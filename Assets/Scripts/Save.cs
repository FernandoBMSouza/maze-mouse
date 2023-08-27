using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Xml.Linq;

public class Save : MonoBehaviour
{
    GameObject player, enemy, timer;
    GameObject[] cheeses;
    private void Awake() {
        SaveSystem.Init();
    }

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        enemy = GameObject.FindGameObjectWithTag("Enemy");
        timer = GameObject.Find("Manager");
        cheeses = GameObject.FindGameObjectsWithTag("Cheese");
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.S)) {
            Salvar();
            SalvarXML();
        }

        if (Input.GetKeyDown(KeyCode.L)) {
            //LoadXML();
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

    private void SalvarXML()
    {

        // Save
        float playerPositionX = player.transform.position.x;
        float playerPositionY = player.transform.position.y;
        float enemyPositionX = enemy.transform.position.x ;
        float enemyPositionY = enemy.transform.position.y;
        int playerPoints = player.GetComponent<Mouse>().Points;

        GameObject[] cheeses = GameObject.FindGameObjectsWithTag("Cheese");



        float currentTime = timer.GetComponent<Timer>().TimerValue;

        

        SaveObjectXML saveObject = new SaveObjectXML(playerPoints, playerPositionX, playerPositionY, enemyPositionX, enemyPositionY, cheeses, currentTime);
        
        XmlDocument xmlDocument = new XmlDocument();

        #region SaveXML

        XmlElement root = xmlDocument.CreateElement("Save");
        root.SetAttribute("FileName", "File_01");

        XmlElement positions = xmlDocument.CreateElement("Positions");

        XmlElement playerPositionXElement = xmlDocument.CreateElement("playerPositionX");
        playerPositionXElement.InnerText = saveObject.savePlayerPositionx.ToString();
        positions.AppendChild(playerPositionXElement);

        XmlElement playerPositionYElement = xmlDocument.CreateElement("playerPositionY");
        playerPositionYElement.InnerText = saveObject.savePlayerPositiony.ToString();
        positions.AppendChild(playerPositionYElement);

        XmlElement enemyPositionXElement = xmlDocument.CreateElement("enemyPositionX");
        enemyPositionXElement.InnerText = saveObject.saveEnemyPositionx.ToString();
        positions.AppendChild(enemyPositionXElement);

        XmlElement enemyPositionYElement = xmlDocument.CreateElement("enemyPositionY");
        enemyPositionYElement.InnerText = saveObject.saveEnemyPositionY.ToString();
        positions.AppendChild(enemyPositionYElement);

        root.AppendChild(positions);

        XmlElement playerPointsElement = xmlDocument.CreateElement("playerPoints");
        playerPointsElement.InnerText = saveObject.savePlayerPointsXML.ToString();
        root.AppendChild(playerPointsElement);

        XmlElement cheesesElement = xmlDocument.CreateElement("cheeses");
        cheesesElement.InnerText = saveObject.saveCheeses.ToString();
        root.AppendChild(cheesesElement);


        XmlElement cheese, cheesePositionX, cheesePositionY;

        for (int i = 0; i < cheeses.Length; i++)
        {
            cheese = xmlDocument.CreateElement("cheese" + (i + 1));
            cheesePositionX = xmlDocument.CreateElement("cheesePositionX");
            cheesePositionY = xmlDocument.CreateElement("cheesePositionY");

            cheesePositionX.InnerText = cheeses[i].transform.position.x.ToString();
            cheesePositionY.InnerText = cheeses[i].transform.position.y.ToString();

            cheese.AppendChild(cheesePositionX);
            cheese.AppendChild(cheesePositionY);

            root.AppendChild(cheese);
        }

        XmlElement currentTimeElement = xmlDocument.CreateElement("timer");
        currentTimeElement.InnerText = saveObject.saveTimerXML.ToString();
        root.AppendChild(currentTimeElement);

        xmlDocument.AppendChild(root);
        #endregion

        SaveSystem.SaveXML(xmlDocument);

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

    private void LoadXML()
    {
        if (File.Exists(Application.dataPath + "/Saves/XML/" + "saveXML_.txt"))
        {
            Debug.Log("Pegou mermo");
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(Application.dataPath + "/Saves/XML/" + "saveXML_.txt");

            XmlNodeList playerPositionX = xmlDocument.GetElementsByTagName("playerPositionX");
            float tempPlayerPositionX = float.Parse(playerPositionX[0].InnerText);

            XmlNodeList playerPositionY = xmlDocument.GetElementsByTagName("playerPositionY");
            float tempPlayerPositionY = float.Parse(playerPositionY[0].InnerText);

            XmlNodeList enemyPositionX = xmlDocument.GetElementsByTagName("enemyPositionX");
            float tempEnemyPositionX = float.Parse(enemyPositionX[0].InnerText);

            XmlNodeList enemyPositionY = xmlDocument.GetElementsByTagName("enemyPositionY");
            float tempEnemyPositionY = float.Parse(enemyPositionY[0].InnerText);

            XmlNodeList playerPoints = xmlDocument.GetElementsByTagName("playerPoints");
            int tempPlayerPoints = int.Parse(playerPoints[0].InnerText);

            XmlNodeList timer = xmlDocument.GetElementsByTagName("timer");
            float tempTimer = float.Parse(timer[0].InnerText);



            //colocando os Dados salvos nos GameObjects
            player.transform.position = new Vector3(tempPlayerPositionX, tempPlayerPositionY, 0);
            enemy.transform.position = new Vector3(tempEnemyPositionX, tempEnemyPositionY, 0);
            player.GetComponent<Mouse>().Points = tempPlayerPoints;
            this.timer.GetComponent<Timer>().TimerValue = tempTimer;
        }
    }

    private class SaveObjectXML
    {
        public int savePlayerPointsXML;
        public float savePlayerPositionx;
        public float savePlayerPositiony;
        public float saveEnemyPositionx;
        public float saveEnemyPositionY;
        public float saveTimerXML;
        public List<GameObject> saveCheeses = new List<GameObject>();

        public SaveObjectXML(int playerPoints, float playerPositionX, float playerPositionY, float enemyPositionX, float enemyPositionY, GameObject[] cheeses, float timer)
        {
            savePlayerPointsXML = playerPoints;
            savePlayerPositionx = playerPositionX;
            savePlayerPositiony = playerPositionY;
            saveEnemyPositionx = enemyPositionX;
            saveEnemyPositionY = enemyPositionY;
            saveTimerXML = timer;

            foreach (var item in cheeses)
            {
                saveCheeses.Add(item);
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
