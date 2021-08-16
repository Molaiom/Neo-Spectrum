using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
//using UnityEngine.Advertisements;


public class BannerAdManager : MonoBehaviour
{
    /*    public static BannerAdManager instance;
        public bool testMode = false;

        private string bannerId = "BannerAD";

    #if UNITY_IOS
        private string storeId = "3083044";
    #elif UNITY_ANDROID
        private string storeId = "3083045";
    #else
        private string storeId = null;
    #endif

        private void Awake()
        {
            CheckForInstance();

            if (storeId != null)
                Advertisement.Initialize(storeId, testMode);      
        }

        private void CheckForInstance() // CHECK FOR SINGLETON INSTANCE
        {
            if (instance != null)
                Destroy(gameObject);
            else
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        private void Update()
        {
            if (storeId != null)
                StartCoroutine(ShowBannerWhenReady()); 
        }

        private IEnumerator ShowBannerWhenReady() // DISPLAYS THE BANNER IF CONDITIONS ARE MET
        {
            if(IsSceneValid())
            {
                yield return new WaitForSeconds(0.3f);

                if (Advertisement.IsReady(bannerId) && IsSceneValid())
                {
                    Advertisement.Banner.Hide(false);

                    if(!Advertisement.Banner.isLoaded)
                        Advertisement.Banner.Show(bannerId);
                }
            }
            else
            {
                Advertisement.Banner.Hide(true);
            }       
        }

        private bool IsSceneValid() // RETURNS TRUE IF CURRENT SCENE IS "EXTRAS" OR "LEVEL_SELECT"
        {
            string sceneName = SceneManager.GetActiveScene().name;

            if(sceneName == "Extras" || sceneName == "Level_Select")
            {
                return true;            
            }
            else
            {
                return false;
            }
        }
    */
}
