using System.Collections.Generic;
using Audio;
using RouteEditors;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Menus
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private bool vrPlatform;
        
        [Header("Canvas")]
        [SerializeField] private Canvas playModeMenuCanvas;  
        [SerializeField] private Canvas choosePracticeMenuCanvas;  
        [SerializeField] private Canvas routeSettingsCanvas;     
        [SerializeField] private Canvas parkingSettingsCanvas;     
        [SerializeField] private Canvas keyboard;     
        [SerializeField] private TMP_InputField playerNameInput;     
        [SerializeField] private Canvas chooseRouteMenuCanvas;     
        [SerializeField] private GameObject routeComponentPrefab;
        [SerializeField] private GameObject gridContainerGameObject;
        [SerializeField] private Image soundOnImage;
        //[SerializeField] private Canvas mainMenuCanvas;

        
        [Header("RouteSettings")]
        [SerializeField] private Slider pedestrianDifficulty;
        [SerializeField] private Slider autoCarsDifficulty;
        [SerializeField] private UISwitcher.UISwitcher nightMode;
        [SerializeField] private Slider numberOfTurnsToWin;
        [SerializeField] private UISwitcher.UISwitcher instructor;
        
        [Header("ParkSettings")]
        [SerializeField] private Slider parkType;
        [SerializeField] private Slider numberOfParksToWin;
        
        private string _backgroundMusicName;
        private string routeName;
    

        /*public void EnterPlayModeMenu()
    {
        mainMenuCanvas.gameObject.SetActive(false);
        playModeMenuCanvas.gameObject.SetActive(true);
    }
    */

        private void Start()
        {
            _backgroundMusicName = AudioManager.Instance.GetBackgroundMusicName();
            AudioManager.Instance.SetVolume(_backgroundMusicName, 0.5f);
            AudioManager.Instance.Play(_backgroundMusicName);
            Debug.Log("Background music is playing: " + AudioManager.Instance.IsPlaying(_backgroundMusicName));
        }

        public void SaveNameAndChoosePractice()
        {
            PlayerPrefs.SetString("PlayerName", this.playerNameInput.text);

            playModeMenuCanvas.gameObject.SetActive(false);
            if (vrPlatform)
            {
                keyboard.gameObject.SetActive(false);
            }
            
            choosePracticeMenuCanvas.gameObject.SetActive(true);
        }

        public void OnChooseDrivingTest()
        {
            choosePracticeMenuCanvas.gameObject.SetActive(false);
            
            StartCoroutine(DatabaseManager.Instance.GetData(OnRoutesFetched));
        }
        
        public void OnChooseParkingTest()
        {
            choosePracticeMenuCanvas.gameObject.SetActive(false);
            
            parkingSettingsCanvas.gameObject.SetActive(true);
        }
        
        public void OnSetParkingTestSettings()
        {
            choosePracticeMenuCanvas.gameObject.SetActive(false);
            
            PlayerPrefs.SetInt("ParkingsToWin", (int)numberOfParksToWin.value);
            int parkingTypeVal = 0;
            
            if ((int)parkType.value == 1) parkingTypeVal = 2;
            else if ((int)parkType.value == 2) parkingTypeVal = 1;
            
            PlayerPrefs.SetInt("ParkingType", parkingTypeVal);
            
            UnityEngine.SceneManagement.SceneManager.LoadScene("ParkingTest");
            
            AudioManager.Instance.Stop(_backgroundMusicName);
        }

        private void OnRoutesFetched(List<MapMatrixObject> routeList)
        {
            for (int i = gridContainerGameObject.transform.childCount - 1; i > 0; i--)
            {
                Destroy(gridContainerGameObject.transform.GetChild(i).gameObject);
            }
        
            if (routeList != null)
            {
                for (int index = 0; index < routeList.Count; index++)
                {
                    MapMatrixObject route = routeList[index];
                    GameObject newComponent = Instantiate(routeComponentPrefab, gridContainerGameObject.transform);
                    newComponent.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = route.name;
                    newComponent.name = "Route" + (index+1);
            
                    Button buttonComponent = newComponent.GetComponent<Button>();
                    buttonComponent.onClick.AddListener(() => OnChooseRoute(newComponent.name));
                }
            }
        
            chooseRouteMenuCanvas.gameObject.SetActive(true);
        }
    
        /*public void LoadEnvironmentEditor()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("RoutesEditor");
        }*/
    
        public void OnChooseRoute(string name)
        {
            chooseRouteMenuCanvas.gameObject.SetActive(false);
            routeName = name;
            routeSettingsCanvas.gameObject.SetActive(true);
        }

        public void OnSaveRouteSettings()
        {
            routeSettingsCanvas.gameObject.SetActive(false);
            
            AudioManager.Instance.Stop(_backgroundMusicName);
        
            int routeIndex = int.Parse(routeName.Substring(5));
            
            switch (pedestrianDifficulty.value)
            {
                case 1:
                    PlayerPrefs.SetInt("PedestrianDifficulty", 0);
                    break;
                case 2:
                    PlayerPrefs.SetInt("PedestrianDifficulty", 60);
                    break;
                case 3:
                    PlayerPrefs.SetInt("PedestrianDifficulty", 40);
                    break;
                default:
                    PlayerPrefs.SetInt("PedestrianDifficulty", 20);
                    break;
            }
            
            switch (autoCarsDifficulty.value)
            {
                case 1:
                    PlayerPrefs.SetInt("CarsDifficulty", 0);
                    break;
                case 2:
                    PlayerPrefs.SetInt("CarsDifficulty", 15);
                    break;
                case 3:
                    PlayerPrefs.SetInt("CarsDifficulty", 10);
                    break;
                default:
                    PlayerPrefs.SetInt("CarsDifficulty", 5);
                    break;
            }
            
            PlayerPrefs.SetInt("NightMode", nightMode.isOn ? 1 : 0);
            PlayerPrefs.SetInt("NumberOfTurnsToWin", (int)numberOfTurnsToWin.value);
            PlayerPrefs.SetInt("Instructor", instructor.isOn ? 1 : 0);

            if (routeIndex == 0)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("GameEnv");
            }
            else
            {
                PlayerPrefs.SetString("RouteNumber", (routeIndex-1)+"");
                UnityEngine.SceneManagement.SceneManager.LoadScene("RoutesCreatorEnv");
            }
        }

        public void OnBackgroundMusicButtonClicked()
        {
            if (AudioManager.Instance.IsPlaying(_backgroundMusicName))
            {
                AudioManager.Instance.Stop(_backgroundMusicName);
                soundOnImage.enabled = false;
            }
            else
            {
                AudioManager.Instance.Play(_backgroundMusicName);
                soundOnImage.enabled = true;
            }
        }
    }
}
