using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_control : MonoBehaviour
{
    public GameObject parent, selected_ui;

    public void RemoveSelf()
    {
        //Destroy(gameObject);
        parent.SetActive(false);
    }

    public void ShowCard(int index)
    {
        gameObject.GetComponent<Animator>().SetTrigger("show");
    }

    public void HideCard()
    {
        gameObject.GetComponent<Animator>().SetTrigger("hide");
    }

    public void SetSelectedUI(bool selected)
    {
        //print(selected);
        selected_ui.SetActive(selected);
    }
}
