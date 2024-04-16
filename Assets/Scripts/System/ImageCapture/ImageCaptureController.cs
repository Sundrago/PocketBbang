using UnityEngine;
using UnityEngine.Serialization;
using VoxelBusters.EssentialKit;

public class ImageCaptureController : MonoBehaviour
{
    [SerializeField] private Canvas canvasToSreenShot;
    [FormerlySerializedAs("screenShot")] public CanvasScreenManager screenManager;
    private string userNote;

    public void OnEnable()
    {
        CanvasScreenManager.OnPictureTaken -= receivePNGScreenShot;
    }

    public void TakeScreenShot(string text)
    {
        screenManager.gameObject.SetActive(true);
        userNote = text;
        CanvasScreenManager.OnPictureTaken += receivePNGScreenShot;
        screenManager.takeScreenShot(canvasToSreenShot, SCREENSHOT_TYPE.IMAGE_AND_TEXT, false);
    }

    private void receivePNGScreenShot(Texture2D pngArray)
    {
        var shareSheet = ShareSheet.CreateInstance();
        shareSheet.AddText(userNote);
        shareSheet.AddImage(pngArray);
        shareSheet.SetCompletionCallback((result, error) =>
        {
            Debug.Log("Share Sheet was closed. Result code: " + result.ResultCode);
        });
        shareSheet.Show();

        screenManager.gameObject.SetActive(false);
    }
}