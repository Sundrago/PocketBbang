using UnityEngine;

public class DeepLinkManager : MonoBehaviour
{
    public static DeepLinkManager Instance { get; private set; }
    
    [SerializeField] private PhoneMessageController msg;
    [SerializeField] private LiveEventMessagePanel tanguru, bossam, dove;
    private string deeplinkURL;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Application.deepLinkActivated += OnDeepLinkActivated;
            if (!string.IsNullOrEmpty(Application.absoluteURL))
                // Cold start and Application.absoluteURL not null so process Deep Link.
                OnDeepLinkActivated(Application.absoluteURL);
            // Initialize DeepLink Manager global variable.
            else deeplinkURL = "[none]";
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDeepLinkActivated(string url)
    {
        deeplinkURL = url;
        var sceneName = url.Split('?')[1];
        switch (sceneName)
        {
            case "tanghuru":
                tanguru.ShowEventPanel();
                break;
            case "bossam":
                bossam.ShowEventPanel();
                break;
            case "dove":
                dove.ShowEventPanel();
                break;
        }
    }
}