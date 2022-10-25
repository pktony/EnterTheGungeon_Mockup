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
        player = FindObjectOfType<Player>().GetComponent<IHealth>();
        player.OnTakeDamage += OnTakeDamage;
        player.OnHPUp += IncreaseHeart;

        heartSet = new RectTransform[(int)(player.MaxHP * 0.5f)];
        heartImg = new Image[player.MaxHP];

        int k = 0;
        for (int i = 0; i < heartSet.Length; i++)
        {
            heartSet[i] = transform.GetChild(i).GetComponent<RectTransform>();
            for (int j = 0; j < 2; j++)
            {
                heartImg[j + k] = heartSet[i].GetChild(j).GetComponent<Image>();
            }
            k += 2;
        }

        Initialize();
    }

    void Initialize()
    {
        for (int i = 0; i < heartImg.Length - player.HP; i++)
        {
            heartImg[heartImg.Length - 1 - i].color = Color.clear;
        }

        if (!IsEvenNumber(player.HP))
        {
            heartImg[player.HP].sprite = heart_Blank;
            heartImg[player.HP].color = Color.white;
            heartImg[player.HP].transform.SetAsFirstSibling();
        }
    }

    void OnTakeDamage(int hp, int _)
    {
        for (int i = 0; i < heartImg.Length - hp; i++)
        {
            heartImg[heartImg.Length - 1 - i].sprite = heart_Blank;
        }

        if (!IsEvenNumber(hp))
        {
            heartImg[hp].sprite = heart_Blank;
            heartImg[hp].color = Color.white;
            heartImg[hp].transform.SetAsFirstSibling();  // Set heart UI hierarchy order
        }
    }

    void IncreaseHeart(int hp)
    {
        for (int i = 0; i < heartImg.Length - hp; i++)
        {   // 전부 빈 하트 처리 
            heartImg[heartImg.Length - 1 - i].sprite = heart_Blank;
        }

        if (IsEvenNumber(hp))
        {
            heartImg[hp - 1].sprite = heart_Red;
            heartImg[hp - 1].color = Color.white;
            heartImg[hp - 1].transform.SetAsFirstSibling();  // Set heart UI hierarchy order
        }
        else
        {
            heartImg[hp - 1].sprite = heart_Red;
            heartImg[hp - 1].color = Color.white;
        }
    }

    bool IsEvenNumber(int num) => num % 2 == 0;
}
