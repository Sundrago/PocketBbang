using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageCapture_crop : MonoBehaviour
{
    public void CaptureAndCrop()
    {
        createCanvasWithRectTransform();
        //StartCoroutine(takeScreenShot());
    }

    public RectTransform rectT; // Assign the UI element which you wanna capture

    private int width; // width of the object to capture
    private int height; // height of the object to capture
    private Transform rectTransformParent;  //RectTransform parent to reasign after usage
    private GameObject screenShotGO;    //Tempory GameObject to copy recttransform
                                        //RectTransform values to reasign after usage
    private Vector2 offsetMinValues;
    private Vector2 offsetMaxValues;
    private Vector3 localScaleValues;

    private void createCanvasWithRectTransform()
    {
        rectTransformParent = rectT.parent; //Assigning Parent transform to reasign after usage

        //Copying RectTransform values to reasign after switching parent
        offsetMinValues = rectT.offsetMin;
        offsetMaxValues = rectT.offsetMax;
        localScaleValues = rectT.localScale;

        //Creating secondary CANVAS with required fields
        screenShotGO = new GameObject("ScreenShotGO");
        screenShotGO.transform.parent = null;
        Canvas canvas = screenShotGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        CanvasScaler canvasScalar = screenShotGO.AddComponent<CanvasScaler>();
        canvasScalar.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
        screenShotGO.AddComponent<GraphicRaycaster>();

        rectT.SetParent(screenShotGO.transform);    //Assigning capture recttransform to temporary parent gameobject

        //Reasigning recttransform values
        rectT.offsetMin = offsetMinValues;
        rectT.offsetMax = offsetMaxValues;
        rectT.localScale = localScaleValues;

        Canvas.ForceUpdateCanvases();   // Forcing all canvas to update the UI

        StartCoroutine(takeScreenShot());   //Once everything was set ready, Capture the screenshot
    }

    private IEnumerator takeScreenShot()
    {
        yield return new WaitForEndOfFrame(); // it must be a coroutine 

        //Calcualtion for the width and height of the screenshot from recttransform
        width = System.Convert.ToInt32(rectT.rect.width);
        height = System.Convert.ToInt32(rectT.rect.height);

        //Calcualtion for the starting position of the recttransform to be captured
        Vector2 temp = rectT.transform.position;
        var startX = temp.x - width / 2;
        var startY = temp.y - height / 2;

        // Read the pixels from the texture
        var tex = new Texture2D(width, height, TextureFormat.RGB24, false);
        tex.ReadPixels(new Rect(startX, startY, width, height), 0, 0);
        tex.Apply();

        // split the process up--ReadPixels() and the GetPixels() call inside of the encoder are both pretty heavy
        yield return 0;

        var bytes = tex.EncodeToPNG();
        Destroy(tex);

        //Writing bytes to a file
        //System.IO.File.WriteAllBytes(Application.persistentDataPath + "/ScreenShotRect/ScreenShot.png", bytes);
        //print(Application.persistentDataPath + "/ScreenShotRect/ScreenShot.png");

        //In case of ScaleMode was not ScaleWithScreenSize, parent will not be assigned then no need to revert the changes
        if (rectTransformParent != null)
        {
            //Reasigning gameobject to its original parent group
            rectT.SetParent(rectTransformParent);

            //Reasigning recttransform values
            rectT.offsetMin = offsetMinValues;
            rectT.offsetMax = offsetMaxValues;
            rectT.localScale = localScaleValues;

            //Destorying temporary created gameobject after usage
            Destroy(screenShotGO);
        }

        Debug.Log("Picture taken");
    }
}
