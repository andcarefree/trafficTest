using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField saveName;
    [SerializeField] private GameObject carPrefab;
    [SerializeField] private GameObject loadObject;
    [SerializeField] private GameObject loadPanel;
    [SerializeField] private GameObject saveButtonPrefab;
    [SerializeField] private GameObject scrollContentArea;
    [SerializeField] private GameObject savePanel;
    [SerializeField] private GameObject warningPanel;
    [SerializeField] private GameObject warningText;
    private string[] saveFileName;
    
    public void OnSave()
    {
        try
        {
            SerializationManager.Save(saveName.text, SaveData.current);
            savePanel.SetActive(false);
            warningPanel.SetActive(true);
            warningText.GetComponent<TextMeshProUGUI>().SetText("保存成功！");
        }
        catch (System.Exception exception)
        {
            savePanel.SetActive(false);
            warningPanel.SetActive(true);
            warningText.GetComponent<TextMeshProUGUI>().SetText(exception.ToString());
            throw;
        }
        
    }

    public void OnLoad()
    {
        loadPanel.SetActive(true);
        GetLoadFile();

        foreach(Transform button in scrollContentArea.transform)
        {
            Destroy(button.gameObject);
        }

        for(int i = 0; i<saveFileName.Length; i++)
        {
            GameObject buttonObject = Instantiate(saveButtonPrefab);
            buttonObject.transform.SetParent(scrollContentArea.transform, false);

            var index = i;
            buttonObject.GetComponent<Button>().onClick.AddListener(() =>
            {
                GetObjectFromLoadFile(saveFileName[index]);
            });
            buttonObject.GetComponentInChildren<TextMeshProUGUI>().text = saveFileName[index].Replace($"{Application.dataPath}/saves\\", "");
        }
    }

    private void GetLoadFile()
    {
        if(!Directory.Exists(Application.dataPath + "/saves"))
        {
            Directory.CreateDirectory(Application.dataPath + "/saves");
        }

        saveFileName = Directory.GetFiles(Application.dataPath + "/saves");
    }

    private void GetObjectFromLoadFile(string loadFile)
    {
        try
        {
            SaveData.current = (SaveData)SerializationManager.Load(loadFile);

            GameEvents.current.DispatchOnLoad();
            
            foreach(var newObject in SaveData.current.objects)
            {
                var gameObject = Instantiate(loadObject, newObject.position, newObject.rotation);
                gameObject.GetComponent<Road>().Id = newObject.id;

                if (newObject.roadType == RoadTypeEnum.SOURCE)
                {
                    var originRoad = gameObject.AddComponent<OriginRoad>();
                    originRoad.Car = carPrefab;
                }
            }

            loadPanel.SetActive(false);
            warningPanel.SetActive(true);
            warningText.GetComponent<TextMeshProUGUI>().SetText("读取成功！");
        }
        catch (System.Exception exception)
        {
            warningPanel.SetActive(true);
            warningText.GetComponent<TextMeshProUGUI>().SetText(exception.ToString());
            throw;
        }

    }
}
