using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasScreenManager : MonoBehaviour
{
    /*
 CanvasScreenManager by programmer.
 http://stackoverflow.com/questions/36555521/unity3d-build-png-from-panel-of-a-unity-ui#36555521
 http://stackoverflow.com/users/3785314/programmer
 */

    //Events
    public delegate void takePictureHandler(Texture2D pngArray);

    private Image[] allImages;

    //Store all other canvas that will be disabled and re-anabled after screenManager
    private Canvas[] allOtherCanvas;
    private Text[] allTexts;

    private GameObject duplicatedTargetUI;
    public static event takePictureHandler OnPictureTaken;

    //takes Screenshot
    public void takeScreenShot(Canvas canvasPanel, SCREENSHOT_TYPE screenShotType = SCREENSHOT_TYPE.IMAGE_AND_TEXT,
        bool createNewInstance = true)
    {
        StartCoroutine(_takeScreenShot(canvasPanel, screenShotType, createNewInstance));
    }

    private IEnumerator _takeScreenShot(Canvas canvasPanel,
        SCREENSHOT_TYPE screenShotType = SCREENSHOT_TYPE.IMAGE_AND_TEXT, bool createNewInstance = true)
    {
        //Get Visible Canvas In the Scene
        allOtherCanvas = getAllCanvasInScene();

        //Hide all the other Visible Canvas except the one that is passed in as parameter(Canvas we want to take Picture of)
        showCanvasExcept(allOtherCanvas, canvasPanel, false);
        //Reset the position so that both UI will be in the-same place if we make the duplicate a child
        resetPosAndRot(gameObject);

        //Check if we should operate on the original image or make a duplicate of it
        if (createNewInstance)
        {
            //Duplicate the Canvas we want to take Picture of
            duplicatedTargetUI = duplicateUI(canvasPanel.gameObject, "ScreenShotUI");
            //Make this game object the parent of the Canvas
            duplicatedTargetUI.transform.SetParent(gameObject.transform);

            //Hide the orginal Canvas we want to take Picture of
            showCanvas(canvasPanel, false);
        }
        else
        {
            //No duplicate. Use original GameObject
            //Make this game object the parent of the Canvas
            canvasPanel.transform.SetParent(gameObject.transform);
        }

        RenderMode defaultRenderMode;

        //Change the duplicated Canvas to RenderMode to overlay
        Canvas duplicatedCanvas = null;
        if (createNewInstance)
        {
            duplicatedCanvas = duplicatedTargetUI.GetComponent<Canvas>();
            defaultRenderMode = duplicatedCanvas.renderMode;
            duplicatedCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        }
        else
        {
            defaultRenderMode = canvasPanel.renderMode;
            canvasPanel.renderMode = RenderMode.ScreenSpaceOverlay;
        }


        if (screenShotType == SCREENSHOT_TYPE.IMAGE_AND_TEXT)
        {
            //No Action Needed
        }
        else if (screenShotType == SCREENSHOT_TYPE.IMAGE_ONLY)
        {
            if (createNewInstance)
            {
                //Get all images on the duplicated visible Canvas
                allTexts = getAllTextsFromCanvas(duplicatedTargetUI);
                //Hide those images
                showTexts(allTexts, false);
            }
            else
            {
                //Get all images on the duplicated visible Canvas
                allTexts = getAllTextsFromCanvas(canvasPanel.gameObject);
                //Hide those images
                showTexts(allTexts, false);
            }
        }
        else if (screenShotType == SCREENSHOT_TYPE.TEXT_ONLY)
        {
            if (createNewInstance)
            {
                //Get all images on the duplicated visible Canvas
                allImages = getAllImagesFromCanvas(duplicatedTargetUI);
                //Hide those images
                showImages(allImages, false);
            }
            else
            {
                //Get all images on the duplicated visible Canvas
                allImages = getAllImagesFromCanvas(canvasPanel.gameObject);
                //Hide those images
                showImages(allImages, false);
            }
        }

        //////////////////////////////////////Finally Take ScreenShot///////////////////////////////
        yield return new WaitForEndOfFrame();
        var screenImage = new Texture2D(Screen.width, Screen.height);
        //Get Image from screen
        screenImage.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenImage.Apply();

        //Convert to png
        //byte[] pngBytes = croppedTexture.EncodeToPNG();

        /*FOR TESTING/DEBUGGING PURPOSES ONLY. COMMENT THIS
        string path = Application.persistentDataPath + "/CanvasScreenManager.png";
        System.IO.File.WriteAllBytes(path, pngBytes);
        Debug.Log(path);*/

        //Notify functions that are subscribed to this event that picture is taken then pass in image bytes as png
        if (OnPictureTaken != null) OnPictureTaken(screenImage);


        ///////////////////////////////////RE-ENABLE OBJECTS

        //Change the duplicated Canvas RenderMode back to default Value
        if (createNewInstance)
            duplicatedCanvas.renderMode = defaultRenderMode;
        else
            canvasPanel.renderMode = defaultRenderMode;
        //Un-Hide all the other Visible Canvas except the one that is passed in as parameter(Canvas we want to take Picture of)
        showCanvas(allOtherCanvas);
        if (screenShotType == SCREENSHOT_TYPE.IMAGE_AND_TEXT)
        {
            //No Action Needed
        }
        else if (screenShotType == SCREENSHOT_TYPE.IMAGE_ONLY)
        {
            //Un-Hide those images
            showTexts(allTexts);
        }
        else if (screenShotType == SCREENSHOT_TYPE.TEXT_ONLY)
        {
            //Un-Hide those images
            showImages(allImages);
        }

        //Un-hide the orginal Canvas we want to take Picture of
        showCanvas(canvasPanel);

        if (createNewInstance)
            //Destroy the duplicated GameObject
            Destroy(duplicatedTargetUI, 1f);
        else
            //Remove the Canvas as parent 
            canvasPanel.transform.SetParent(null);
    }

    private GameObject duplicateUI(GameObject parentUICanvasOrPanel, string newOBjectName)
    {
        var tempObj = Instantiate(parentUICanvasOrPanel);
        tempObj.name = newOBjectName;
        return tempObj;
    }


    private Image[] getAllImagesFromCanvas(GameObject canvasParentGameObject, bool findDisabledCanvas = false)
    {
        var tempImg = canvasParentGameObject.GetComponentsInChildren<Image>(findDisabledCanvas);
        if (findDisabledCanvas)
        {
            return tempImg;
        }

        var canvasList = new List<Image>();
        for (var i = 0; i < tempImg.Length; i++)
            if (tempImg[i].enabled)
                canvasList.Add(tempImg[i]);
        return canvasList.ToArray();
    }

    private Text[] getAllTextsFromCanvas(GameObject canvasParentGameObject, bool findDisabledCanvas = false)
    {
        var tempImg = canvasParentGameObject.GetComponentsInChildren<Text>(findDisabledCanvas);
        if (findDisabledCanvas)
        {
            return tempImg;
        }

        var canvasList = new List<Text>();
        for (var i = 0; i < tempImg.Length; i++)
            if (tempImg[i].enabled)
                canvasList.Add(tempImg[i]);
        return canvasList.ToArray();
    }

    private Canvas[] getAllCanvasFromCanvas(Canvas canvasParentGameObject, bool findDisabledCanvas = false)
    {
        var tempImg = canvasParentGameObject.GetComponentsInChildren<Canvas>(findDisabledCanvas);
        if (findDisabledCanvas)
        {
            return tempImg;
        }

        var canvasList = new List<Canvas>();
        for (var i = 0; i < tempImg.Length; i++)
            if (tempImg[i].enabled)
                canvasList.Add(tempImg[i]);
        return canvasList.ToArray();
    }

    //Find Canvas.
    private Canvas[] getAllCanvasInScene(bool findDisabledCanvas = false)
    {
        var tempCanvas = FindObjectsOfType<Canvas>();
        if (findDisabledCanvas)
        {
            return tempCanvas;
        }

        var canvasList = new List<Canvas>();
        for (var i = 0; i < tempCanvas.Length; i++)
            if (tempCanvas[i].enabled)
                canvasList.Add(tempCanvas[i]);
        return canvasList.ToArray();
    }

    //Disable/Enable Images
    private void showImages(Image[] imagesToDisable, bool enableImage = true)
    {
        for (var i = 0; i < imagesToDisable.Length; i++) imagesToDisable[i].enabled = enableImage;
    }

    //Disable/Enable Texts
    private void showTexts(Text[] imagesToDisable, bool enableTexts = true)
    {
        for (var i = 0; i < imagesToDisable.Length; i++) imagesToDisable[i].enabled = enableTexts;
    }


    //Disable/Enable Canvas
    private void showCanvas(Canvas[] canvasToDisable, bool enableCanvas = true)
    {
        for (var i = 0; i < canvasToDisable.Length; i++) canvasToDisable[i].enabled = enableCanvas;
    }


    //Disable/Enable one canvas
    private void showCanvas(Canvas canvasToDisable, bool enableCanvas = true)
    {
        canvasToDisable.enabled = enableCanvas;
    }

    //Disable/Enable Canvas Except
    private void showCanvasExcept(Canvas[] canvasToDisable, Canvas ignoreCanvas, bool enableCanvas = true)
    {
        for (var i = 0; i < canvasToDisable.Length; i++)
            if (!(canvasToDisable[i] == ignoreCanvas))
                canvasToDisable[i].enabled = enableCanvas;
    }

    //Disable/Enable Canvas Except
    private void showCanvasExcept(Canvas[] canvasToDisable, Canvas[] ignoreCanvas, bool enableCanvas = true)
    {
        for (var i = 0; i < canvasToDisable.Length; i++)
        for (var j = 0; j < ignoreCanvas.Length; j++)
            if (!(canvasToDisable[i] == ignoreCanvas[j]))
                canvasToDisable[i].enabled = enableCanvas;
    }

    //Reset Position
    private void resetPosAndRot(GameObject posToReset)
    {
        posToReset.transform.position = Vector3.zero;
        posToReset.transform.rotation = Quaternion.Euler(Vector3.zero);
    }
}

public enum SCREENSHOT_TYPE
{
    IMAGE_AND_TEXT,
    IMAGE_ONLY,
    TEXT_ONLY
}