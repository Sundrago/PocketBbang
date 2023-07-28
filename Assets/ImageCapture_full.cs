using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.EssentialKit;

public class ImageCapture_full : MonoBehaviour
{
    public Canvas canvasToSreenShot;
    public CanvasScreenShot screenShot;
    string myText;  

    public void OnEnable()
    {
        CanvasScreenShot.OnPictureTaken -= receivePNGScreenShot;
    }

    public void TakeScreenShot(string text)
    {
        screenShot.gameObject.SetActive(true);
        myText = text;
        CanvasScreenShot.OnPictureTaken += receivePNGScreenShot;
        screenShot.takeScreenShot(canvasToSreenShot, SCREENSHOT_TYPE.IMAGE_AND_TEXT, false);
    }

    void receivePNGScreenShot(Texture2D pngArray)
    {
        print("received");
     
        ShareSheet shareSheet = ShareSheet.CreateInstance();
        shareSheet.AddText(myText);
        shareSheet.AddImage(pngArray);
        shareSheet.SetCompletionCallback((result, error) => {
            Debug.Log("Share Sheet was closed. Result code: " + result.ResultCode);
        });
        shareSheet.Show();

        screenShot.gameObject.SetActive(false);

        //Debug.Log("Picture taken");

        //Do Something With the Image (Save)
        //string path = Application.persistentDataPath + "/CanvasScreenShot.png";
        //System.IO.File.WriteAllBytes(path, pngArray);
        //Debug.Log(path);
    }
}
