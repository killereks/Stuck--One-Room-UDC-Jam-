﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;

namespace Michsky.UI.ModernUIPack
{
    [AddComponentMenu("Modern UI Pack/Context Menu/Context Menu Content")]
    public class ContextMenuContent : MonoBehaviour, IPointerClickHandler
    {
        [Header("Resources")]
        public ContextMenuManager contextManager;
        public Transform itemParent;

        [Header("Items")]
        public List<ContextItem> contexItems = new List<ContextItem>();

        Animator contextAnimator;
        GameObject selectedItem;
        Image setItemImage;
        TextMeshProUGUI setItemText;
        Sprite imageHelper;
        string textHelper;

        [System.Serializable]
        public class ContextItem
        {
            public string itemText = "Item Text";
            public Sprite itemIcon;
            public ContextItemType contextItemType;
            public UnityEvent onClick;
        }

        public enum ContextItemType
        {
            BUTTON
            // SUB_MENU
        }

        void Start()
        {
            if (contextManager == null)
            {
                try
                {
                    contextManager = (ContextMenuManager)GameObject.FindObjectsOfType(typeof(ContextMenuManager))[0];
                    itemParent = contextManager.transform.Find("Content/Item List").transform;
                }

                catch { Debug.Log("<b>[Context Menu]</b> Context Manager is missing.", this); return; }
            }

            contextAnimator = contextManager.contextAnimator;

            foreach (Transform child in itemParent)
                Destroy(child.gameObject);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (contextManager.isContextMenuOn == true)
            {
                contextAnimator.Play("Menu Out");
                contextManager.isContextMenuOn = false;
            }

            else if (eventData.button == PointerEventData.InputButton.Right && contextManager.isContextMenuOn == false)
            {
                foreach (Transform child in itemParent)
                    Destroy(child.gameObject);

                for (int i = 0; i < contexItems.Count; ++i)
                {
                    if (contexItems[i].contextItemType == ContextItemType.BUTTON)
                        selectedItem = contextManager.contextButton;

                    GameObject go = Instantiate(selectedItem, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                    go.transform.SetParent(itemParent, false);

                    setItemText = go.GetComponentInChildren<TextMeshProUGUI>();
                    textHelper = contexItems[i].itemText;
                    setItemText.text = textHelper;

                    Transform goImage;
                    goImage = go.gameObject.transform.Find("Icon");
                    setItemImage = goImage.GetComponent<Image>();
                    imageHelper = contexItems[i].itemIcon;
                    setItemImage.sprite = imageHelper;

                    Button itemButton;
                    itemButton = go.GetComponent<Button>();
                    itemButton.onClick.AddListener(contexItems[i].onClick.Invoke);
                    itemButton.onClick.AddListener(CloseOnClick);
                    StartCoroutine(ExecuteAfterTime(0.01f));
                }

                contextManager.SetContextMenuPosition();
                contextAnimator.Play("Menu In");
                contextManager.isContextMenuOn = true;
                contextManager.SetContextMenuPosition();
            }
        }

        IEnumerator ExecuteAfterTime(float time)
        {
            yield return new WaitForSeconds(time);
            itemParent.gameObject.SetActive(false);
            itemParent.gameObject.SetActive(true);
            StopCoroutine(ExecuteAfterTime(0.01f));
            StopCoroutine("ExecuteAfterTime");
        }

        public void CloseOnClick()
        {
            contextAnimator.Play("Menu Out");
            contextManager.isContextMenuOn = false;
        }
    }
}