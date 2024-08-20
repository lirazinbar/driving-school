using System.Collections.Generic;
using Audio;
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
        [SerializeField] private Canvas keyboard;     
        [SerializeField] private TMP_InputField playerNameInput;     
        [SerializeField] private Canvas chooseRouteMenuCanvas;     
        [SerializeField] private GameObject routeComponentPrefab;
        [SerializeField] private GameObject gridContainerGameObject;
        [SerializeField] private Image soundOnImage;
        //[SerializeField] private Canvas mainMenuCanvas;

        private string _backgroundMusicName;
    

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

        public void SaveNameAndLoadRoutes()
        {
            PlayerPrefs.SetString("PlayerName", this.playerNameInput.text);

            playModeMenuCanvas.gameObject.SetActive(false);
            if (vrPlatform)
            {
                keyboard.gameObject.SetActive(false);
            }
            
            // TODO:
            // StartCoroutine(DatabaseManager.Instance.GetRoutes(OnRoutesFetched));
            StartCoroutine(DatabaseManager.Instance.GetData(OnRoutesFetched));
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
            AudioManager.Instance.Stop(_backgroundMusicName);
        
            int routeIndex = int.Parse(name.Substring(5));

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
