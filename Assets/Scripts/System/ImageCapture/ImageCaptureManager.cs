using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using VoxelBusters.EssentialKit;

public class ImageCaptureManager : MonoBehaviour
{
    [SerializeField] private BalloonUIManager balloonUIManager;

    [SerializeField] public Canvas canvasToSreenShot;

    [FormerlySerializedAs("scaler")] [SerializeField]
    private RectTransform rectTransformScaler;

    [FormerlySerializedAs("img1")] [SerializeField]
    private Image imageFrom;

    [FormerlySerializedAs("img1to")] [SerializeField]
    private Image imageTo;

    [FormerlySerializedAs("title")] [SerializeField]
    private Text titleText;

    private CanvasScreenManager screenManager;

    private void Start()
    {
        CanvasScreenManager.OnPictureTaken += receivePNGScreenShot;
        screenManager = gameObject.GetComponent<CanvasScreenManager>();

        var myRect = rectTransformScaler.rect;
        myRect.height = myRect.width;

        rectTransformScaler.sizeDelta = new Vector2(rectTransformScaler.sizeDelta.x, rectTransformScaler.rect.width);
        gameObject.SetActive(false);
    }

    public void OnEnable()
    {
        CanvasScreenManager.OnPictureTaken -= receivePNGScreenShot;
    }

    public void TakeScreenShot()
    {
        Start();
        imageTo.sprite = imageFrom.sprite;
        gameObject.SetActive(true);
        screenManager.takeScreenShot(canvasToSreenShot, SCREENSHOT_TYPE.IMAGE_AND_TEXT, false);
    }

    private void receivePNGScreenShot(Texture2D pngArray)
    {
        //image processing
        var squareSize = 950;
        var xPos = 0;
        var yPos = 0;
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

        var c = pngArray.GetPixels(xPos, yPos, squareSize, squareSize);
        var croppedTexture = new Texture2D(squareSize, squareSize);
        croppedTexture.SetPixels(c);
        croppedTexture.Apply();

        balloonUIManager.ShowMsg("수집한 스티커 이미지는 자유롭게 사용해도 좋습니다 :)");
        var rnd = Random.Range(0, 3);
        var myText = "";

        switch (rnd)
        {
            case 0:
                myText = GetCompleteWorld(titleText.text, "을", "를") + " 얻었다-!\n#포켓볼빵_더게임";
                break;
            case 1:
                myText = "[" + titleText.text + "] 갓챠☆ \n#포켓볼빵_더게임";
                break;
            case 2:
                myText = "#" + titleText.text + " #포켓볼빵_더게임";
                break;
        }

        var shareSheet = ShareSheet.CreateInstance();
        shareSheet.AddText(myText);
        shareSheet.AddImage(croppedTexture);
        shareSheet.SetCompletionCallback((result, error) =>
        {
            Debug.Log("Share Sheet was closed. Result code: " + result.ResultCode);
        });
        shareSheet.Show();

        gameObject.SetActive(false);
    }

    public string GetCompleteWorld(string name, string firstVal, string secondVal)
    {
        var lastName = name.ElementAt(name.Length - 1);
        var index = (lastName - 0xAC00) % 28;

        if (lastName < 0xAC00 || lastName > 0xD7A3) return name;

        var selectVal = (lastName - 0xAC00) % 28 > 0 ? firstVal : secondVal;

        return name + selectVal;
    }
}