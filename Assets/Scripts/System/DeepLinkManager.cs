using UnityEngine;

public class DeepLinkManager : MonoBehaviour
{
    [SerializeField] private PhoneMessageController msg;
    [SerializeField] private LiveEventMessagePanel tanguru, bossam, dove;
    public string deeplinkURL;
    public static DeepLinkManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Application.deepLinkActivated += onDeepLinkActivated;
            if (!string.IsNullOrEmpty(Application.absoluteURL))
                // Cold start and Application.absoluteURL not null so process Deep Link.
                onDeepLinkActivated(Application.absoluteURL);
            // Initialize DeepLink Manager global variable.
            else deeplinkURL = "[none]";
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void onDeepLinkActivated(string url)
    {
        // Update DeepLink Manager global variable, so URL can be accessed from anywhere.
        deeplinkURL = url;

// Decode the URL to determine action. 
// In this example, the application expects a link formatted like this:
// unitydl://mylink?scene1
        // msg.SetMsg(url, 1);
        var sceneName = url.Split('?')[1];
        // msg.SetMsg(sceneName, 1);
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