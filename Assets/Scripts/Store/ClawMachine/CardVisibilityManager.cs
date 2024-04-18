using UnityEngine;
using UnityEngine.Serialization;

public class CardVisibilityManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private CollectionManager collection;
    [SerializeField] private GameObject cardPanel;

    public void SetDelayFalse()
    {
        collection.delay = false;
    }

    public void CardHidden()
    {
        if (gameManager.currentLocation == "bbobgi")
            gameManager.newStore.GetComponent<StoreControl>().bbobgiPanel.bbobgiController.ShowObjs();
    }
}