using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VoxelBusters.CoreLibrary;
using VoxelBusters.EssentialKit;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ImageCapture : MonoBehaviour
{
    [SerializeField] public Canvas canvasToSreenShot;
    [SerializeField] Image img1, img1to;
    [SerializeField] Text title;
    [SerializeField] RectTransform scaler;
    [SerializeField] GameObject balloon;

    CanvasScreenShot screenShot;

    // Use this for initialization
    void Start()
    {
        CanvasScreenShot.OnPictureTaken += receivePNGScreenShot;
        screenShot = gameObject.GetComponent<CanvasScreenShot>();

        Rect myRect = scaler.rect;
        myRect.height = myRect.width;

        scaler.sizeDelta = new Vector2(scaler.sizeDelta.x, scaler.rect.width);
        gameObject.SetActive(false);
    }

    public void TakeScreenShot()
    {
        Start();
        img1to.sprite = img1.sprite;
        gameObject.SetActive(true);
        screenShot.takeScreenShot(canvasToSreenShot, SCREENSHOT_TYPE.IMAGE_AND_TEXT, false);
    }

    public void OnEnable()
    {
        CanvasScreenShot.OnPictureTaken -= receivePNGScreenShot;
    }

    void receivePNGScreenShot(Texture2D pngArray)
    {
        print("received");

        //image processing
        int squareSize = 950;
        int xPos = 0;
        int yPos = 0;
        if (pngArray.height < pngArray.width)
        {
            squareSize = pngArray.height;
            xPos = (pngArray.width - pngArray.height) / 2;
        }
        else
        {
            squareSize = pngArray.width;
            yPos = (pngArray.height - pngArray.width) / 2;
        }

        Color[] c = ((Texture2D)pngArray).GetPixels(xPos, yPos, squareSize, squareSize);
        Texture2D croppedTexture = new Texture2D(squareSize, squareSize);
        croppedTexture.SetPixels(c);
        croppedTexture.Apply();


        balloon.GetComponent<BalloonControl>().ShowMsg("수집한 스티커 이미지는 자유롭게 사용해도 좋습니다 :)");
        int rnd = Random.Range(0, 3);
        string myText = "";

        switch (rnd)
        {
            case 0:
                myText = GetCompleteWorld(title.text, "을", "를") + " 얻었다-!\n#포켓볼빵_더게임";
                break;
            case 1:
                myText = "[" + title.text + "] 갓챠☆ \n#포켓볼빵_더게임";
                break;
            case 2:
                myText = "#" + title.text + " #포켓볼빵_더게임";
                break;
        }

        print(myText);

        ShareSheet shareSheet = ShareSheet.CreateInstance();
        shareSheet.AddText(myText);
        shareSheet.AddImage(croppedTexture);
        shareSheet.SetCompletionCallback((result, error) => {
            Debug.Log("Share Sheet was closed. Result code: " + result.ResultCode);
        });
        shareSheet.Show();

        gameObject.SetActive(false);

        //Debug.Log("Picture taken");

        //Do Something With the Image (Save)
        //string path = Application.persistentDataPath + "/CanvasScreenShot.png";
        //System.IO.File.WriteAllBytes(path, pngArray);
        //Debug.Log(path);
    }

    public string GetCompleteWorld(string name, string firstVal, string secondVal)
    {
        char lastName = name.ElementAt(name.Length - 1);
        int index = (lastName - 0xAC00) % 28;

        if (lastName < 0xAC00 || lastName > 0xD7A3)
        {
            return name;
        }

        string selectVal = (lastName - 0xAC00) % 28 > 0 ? firstVal : secondVal;

        return name + selectVal;
    }

}