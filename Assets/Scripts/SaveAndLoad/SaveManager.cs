using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveManager : MonoBehaviour
{
    public TMP_InputField SaveName;
    public GameObject saveButtonPrefab;
    public GameObject scrollContentArea;
    public GameObject loadObject;
    public string[] saveFileName;

    public void OnSave()
    {
        SerializationManager.Save(SaveName.text, SaveData.current);
    }

    public void OnLoad()
    {
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
                getObjectFromLoadFile(saveFileName[index]);
            });
            buttonObject.GetComponentInChildren<TextMeshProUGUI>().text = saveFileName[index].Replace($"{Application.persistentDataPath}/saves\\", "");
        }

    }

    public void GetLoadFile()
    {
        if(!Directory.Exists(Application.persistentDataPath + "/saves"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/saves");
        }

        saveFileName = Directory.GetFiles(Application.persistentDataPath + "/saves");
    }

    public void getObjectFromLoadFile(string loadFile)
    {
        SaveData.current = (SaveData)SerializationManager.Load(loadFile);

        GameEvent.current.OnLoad();
        
        foreach(var newObject in SaveData.current.objects)
        {
            Instantiate(loadObject, newObject.position, newObject.rotation);
        }
    }
}
