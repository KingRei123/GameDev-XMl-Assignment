using UnityEngine;
using UnityEngine.UI;
using System.Text;
using UnityEngine.Networking;
using System.Collections;
using System.Net.Sockets;
using System.Net;
using System.Collections.Generic; //Needed for Lists
using System.Xml; //Needed for XML functionality
using System.Xml.Serialization; //Needed for XML functionality
using System.IO;
using TMPro;

public class XMLLoader : MonoBehaviour
{

    public InputField playerNameInput;
    public InputField playerPowerLevel;

    private string filePath;

    // Start is called before the first frame update
    void Start()
    {
        filePath = Application.dataPath + "/XMLData.xml";
        LoadPlayerData();


    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SaveButton()
    {
        SavePlayerData();
    }

    public void LoadPlayerData()
    {

    }



    public void SavePlayerData()
    {
        string playerName = playerNameInput.text;
        string powerLevel = playerPowerLevel.text;
        XmlDocument xmlDoc = new XmlDocument();

        XmlElement root = xmlDoc.CreateElement("Player");

        XmlElement name = xmlDoc.CreateElement("name");
        name.InnerText = playerName;
        XmlElement power = xmlDoc.CreateElement("power");
        power.InnerText = powerLevel;


        root.AppendChild(name);
        root.AppendChild(power);


        xmlDoc.AppendChild(root);

        xmlDoc.Save(filePath);
        if (File.Exists(filePath))
        {
            Debug.Log("XML FILE SAVED");
        }
    }

    private void OnGUI()
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(filePath);

        XmlNodeList name = xmlDoc.GetElementsByTagName("name");
        XmlNodeList power = xmlDoc.GetElementsByTagName("power");

        string nameStr = name[0].InnerText;
        string powerStr = power[0].InnerText;

        GUILayout.BeginArea(new Rect(10, 10, 200, 100));
        GUILayout.Label("Player Stats: ");
        GUILayout.Label(nameStr);
        GUILayout.Label(powerStr);
        GUILayout.EndArea();

    }

    public void SendToServer() => StartCoroutine(SendForm());

    IEnumerator SendForm()
    {
        string xml = System.IO.File.ReadAllText(filePath);

        string url = "http://localhost/Server%20Side%20Programming/gamedev.php";
        UnityWebRequest request = UnityWebRequest.Post(url, "POST");

        byte[] bodyRaw = Encoding.UTF8.GetBytes(xml);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.SetRequestHeader("Content-Type", "application/xml");

        yield return request.SendWebRequest();


        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error sending XML file: " + request.error);
        }
        else
        {
            Debug.Log("XML file sent successfully.");
        }
    }

}
