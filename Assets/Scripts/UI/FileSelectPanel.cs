using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

namespace UI.Panel.FileSelet
{
    public class FileSelectPanel : MonoBehaviour
    {
        public static FileSelectPanel currentFileSelectPanel;

        public delegate void Work4String();
        public Work4String work4String;

        public bool isReady=false;
        public string filePath;

        private Vector2 contentSize;
        private Vector2 filePosition;

        private DirectoryInfo currentDirectory;
        private DirectoryInfo[] directoryInfos;
        
        private FileInfo[] fileInfos;
        private float fileHeight;
        [SerializeField]
        private Button fileButton;
        [SerializeField]
        private RectTransform content;
        [SerializeField]
        private Scrollbar scrollbar;
        [SerializeField]
        private InputField pathInputField;
        [SerializeField]
        private InputField fileInputField;
       
        

        private void Awake()
        {
            currentFileSelectPanel = this;
            contentSize = content.sizeDelta;
            filePosition = fileButton.GetComponent<RectTransform>().anchoredPosition;
            
            currentDirectory = new DirectoryInfo(Application.persistentDataPath);

            pathInputField.text = currentDirectory.FullName;
            fileHeight = fileButton.GetComponent<RectTransform>().rect.height;
            UpdatePath();
        }

        public void UpdatePath()
        {

            content.sizeDelta = contentSize;
            fileButton.GetComponent<RectTransform>().anchoredPosition = filePosition;
            Button[] childrenTransforms = content.GetComponentsInChildren<Button>();
            foreach (Button btn in childrenTransforms)
            {
                if (btn.gameObject.activeSelf)
                    Destroy(btn.gameObject);
            }

            if (Directory.Exists(pathInputField.text))
            {
                currentDirectory = new DirectoryInfo(pathInputField.text);
                
            }
            else
            {
                

            }
            pathInputField.text = currentDirectory.FullName;

            directoryInfos = currentDirectory.GetDirectories();
            fileInfos = currentDirectory.GetFiles();
            foreach (DirectoryInfo di in directoryInfos)
            {
                Button nbtn = Instantiate(fileButton, content);
                nbtn.gameObject.SetActive(true);
                nbtn.GetComponentInChildren<Text>().text = "   [文件夹]   " + di.Name;
                fileButton.GetComponent<RectTransform>().anchoredPosition -= new Vector2(0, fileHeight + 5);
                content.sizeDelta += new Vector2(0, fileHeight + 5);
                nbtn.onClick.AddListener(delegate () { pathInputField.text += @"\" + di.Name; UpdatePath(); });
            }
            foreach (FileInfo fi in fileInfos)
            {
                Button nbtn = Instantiate(fileButton, content);
                nbtn.gameObject.SetActive(true);
                nbtn.GetComponentInChildren<Text>().text = "    [文件]    " + fi.Name;
                fileButton.GetComponent<RectTransform>().anchoredPosition -= new Vector2(0, fileHeight + 5);
                content.sizeDelta = new Vector2(0, fileHeight + 5);
                nbtn.onClick.AddListener(delegate () { fileInputField.text = fi.Name; });

            }
            scrollbar.value = 1;
        }
        public void BackUp()
        {
            try
            {
                pathInputField.text = currentDirectory.Parent.FullName;
            }
            catch
            {

            }
            UpdatePath();
        }

        public void Activate(Work4String w4s)
        {
            work4String = w4s;
            gameObject.SetActive(true);
            isReady = false;
        }

        public void Affirm()
        {
            if (File.Exists(currentDirectory.FullName + @"\" + fileInputField.text))
            {
                
                isReady = true;
                filePath = currentDirectory.FullName + @"\" + fileInputField.text;
                gameObject.SetActive(false);
                work4String();
                
            }
        }

        public void Cancel()
        {
            gameObject.SetActive(false);
        }
    }
}