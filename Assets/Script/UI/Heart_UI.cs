using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Heart_UI : MonoBehaviour
{
    public Sprite heart_Red;
    public Sprite heart_Blank;

    RectTransform[] heartSet;
    Image[] heartImg;

    IHealth player = null;

    private void Awake()
    {
        player = GameManager.Inst.Player.GetComponent<IHealth>();
        player.OnTakeDamage += OnTakeDamage;
        player.OnHPUp += IncreaseHeart;
    }

    private void Start()
    {
        heartSet = new RectTransform[(int)(player.MaxHP * 0.5f)];
        heartImg = new Image[player.MaxHP];

        int k = 0;
        for (int i = 0; i < heartSet.Length; i++)
        {
            heartSet[i] = transform.GetChild(i).GetComponent<RectTransform>();
            for (int j = 0; j < 2; j++)
            {
                heartImg[j+k] = heartSet[i].GetChild(j).GetComponent<Image>();
            }
            k += 2;
        }

        //Debug.Log(heartSet.Length);
        //for (int i = 0; i < heartImg.Length; i++)
        //{
        //    Debug.Log(heartImg[i].transform.parent.name);
        //}

        Initialize();
    }

    void Initialize()
    {
        for (int i = 0; i < heartImg.Length - player.HP; i++)
        {
            heartImg[heartImg.Length - 1 - i].color = Color.clear;
        }

        if (!IsEvenNumber())
        {
            heartImg[player.HP].sprite = heart_Blank;
            heartImg[player.HP].color = Color.white;
            heartImg[player.HP].transform.SetAsFirstSibling();
        }
    }

    void OnTakeDamage()
    {
        for (int i = 0; i < heartImg.Length - player.HP; i++)
        {
            heartImg[heartImg.Length - 1 - i].sprite = heart_Blank;
        }

        if (!IsEvenNumber())
        {
            heartImg[player.HP].sprite = heart_Blank;
            heartImg[player.HP].color = Color.white;
            heartImg[player.HP].transform.SetAsFirstSibling();  // Set heart UI hierarchy order
        }
    }

    void IncreaseHeart()
    {
        for (int i = 0; i < heartImg.Length - player.HP; i++)
        { 
            heartImg[heartImg.Length - 1 - i].sprite = heart_Blank;
        }

        if (IsEvenNumber())
        {
            heartImg[player.HP].sprite = heart_Red;
            heartImg[player.HP].color = Color.white;
        }
        else
        {
            heartImg[player.HP].sprite = heart_Red;
            heartImg[player.HP].transform.SetAsFirstSibling();
        }
    }

    bool IsEvenNumber() => player.HP % 2 == 0;
}
