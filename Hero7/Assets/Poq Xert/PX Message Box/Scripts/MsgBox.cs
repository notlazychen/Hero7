//Created by PoqXert (poqxert@gmail.com)

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace PoqXert.MessageBox
{
	public class MsgBox : MonoBehaviour
	{
		//Рект канваса, для определения максимального размера
		public RectTransform canvasRect;
		//Главная панел
		public RectTransform mainPanel;
		//Панель для блокирования других GUI
		public GameObject blockPanel;
		//Заголовок
		public Text captionText;
		//Сообщение
		public Text mainText;
		/* Оформление */
		//Заголовок
		public Image captionImg;
		//Фон
		public Image backgroundImg;
		//Иконка
		public Image iconImg;
		//Кнопка "Да"/"Ок"
		public Image btnYesImg;
		//Кнопка "Нет"
		public Image btnNoImg;
		//Кнопка "Отмена"
		public Image btnCancelImg;
		//EventSystem
		public EventSystem eventer;
		//Стили
		public List<MSGBoxStyle> boxStyles;

		//Последнее окно сообщения
		private static GameObject _lastMsgBox;
		//Message Box ID
		private int messageID = 0;

		private DialogResultMethod calledMethod;

		[System.Serializable]
		public struct MSGBoxStyle
		{
			//Имя
			[TooltipAttribute("Name of style")]
			public string name;
			//Цвет заголовка
			[TooltipAttribute("Color of caption")]
			public Color caption;
			//Цвет фона
			[TooltipAttribute("Color of background")]
			public Color background;
			//Цвет кнопки "Да"/"Ок"
			[TooltipAttribute("Color of button \"Yes/Ok\"")]
			public Color btnYesColor;
			//Цвет кнопки "Нет"
			[TooltipAttribute("Color of button \"No\"")]
			public Color btnNoColor;
			//Цвет кнопки "Отмена"
			[TooltipAttribute("Color of button \"Cancel\"")]
			public Color btnCancelColor;
			//Иконка
			[TooltipAttribute("Icon for MessageBox")]
			public Sprite icon;
		}

		void ButtonClickEvent(int btn)
		{
			calledMethod(messageID, (DialogResult)btn);
		}

		/// <summary>
		/// Генерация окна сообщения.
		/// </summary>
		/// <param name="mess">Сообщение.</param>
		/// <param name="caption">Заголовок.</param>
		/// <param name="btns">Кнопки.</param>
		/// <param name="style">Стиль.</param>
		/// <param name="btnText0">Текст кнопки "Да"/"Ок".</param>
		/// <param name="btnText1">Текст кнопки "Нет".</param>
		/// <param name="btnText2">Текст кнопки "Отмена".</param>
		public void BuildMessageBox(int id, string mess, string caption, MsgBoxButtons btns, MsgBoxStyle style, DialogResultMethod method, bool modal = false, string btnText0 = "", string btnText1 = "", string btnText2 = "")
		{
			//Сохраняем ID
			messageID = id;
			//Статус блокировки других GUI
			blockPanel.SetActive(modal);
			//Сохранение метода
			calledMethod = method;
			//Узнаём ID стиля
			int styleId = (int)style;
			//Устанавливаем заголовок
			this.captionText.text = caption;
			//Устанавливаем сообщение
			this.mainText.text = mess;
			//Устанавливаем цвет заголовка
			this.captionImg.color = boxStyles[styleId].caption;
			//Устанавливаем цвет фона
			this.backgroundImg.color = boxStyles[styleId].background;
			//Устанавливаем цвет кнопок
			this.btnYesImg.color = boxStyles[styleId].btnYesColor;
			this.btnNoImg.color = boxStyles[styleId].btnNoColor;
			this.btnCancelImg.color = boxStyles[styleId].btnCancelColor;
			//Устанавливаем иконку
			this.iconImg.sprite = boxStyles[styleId].icon;
			//Устанавливаем размер
			this.mainPanel.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Mathf.Clamp(Mathf.Max(captionText.preferredWidth, 150 + mainText.preferredWidth), 512, canvasRect.rect.width));
			this.mainPanel.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Mathf.Clamp(mainText.preferredHeight, 256, canvasRect.rect.height));

			RectTransform btnTr;
			Text btnText;
			float cancelW;
			float noW;
			//Устанавливаем нужные кнопки
			switch(btns)
			{
				//Кнопка "Ок"
				case MsgBoxButtons.OK:
					//Получаем рект кнопки "Ок"
					btnTr = btnYesImg.rectTransform;
					//Устанавливаем позицию
					btnTr.anchoredPosition = new Vector2(-10.0f, -10.0f);
					//Получаем элемент "текст"
					btnText = btnYesImg.GetComponentInChildren<Text>();
					//Устанавливаем текст
					btnText.text = (btnText0 == "") ? "Ok" : btnText0;
					//Устанавливаем размер
					btnTr.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Mathf.Clamp(btnText.preferredWidth + 10, 130, mainPanel.rect.width/2 - 30));
					//Отключаем кнопку "Нет"
					btnNoImg.gameObject.SetActive(false);
					//Отключаем кнопку "Отмена"
					btnCancelImg.gameObject.SetActive(false);
					break;
				//Кнопки "Ок" и "Отмена"
				case MsgBoxButtons.OK_CANCEL:
					//Получаем элемент "текст"
					btnText = btnCancelImg.GetComponentInChildren<Text>();
					//Устанавливаем текст
					btnText.text = (btnText2 == "") ? "Cancel" : btnText2;
					//Получаем рект кнопки "Отмена"
					btnTr = btnCancelImg.rectTransform;
					//Устанавливаем размер
					cancelW = Mathf.Clamp(btnText.preferredWidth + 10, 130, mainPanel.rect.width/3 - 30);
					btnTr.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, cancelW);
					//Устанавливаем позицию
					btnTr.anchoredPosition = new Vector2(-10, -10);
					//Получаем рект кнопки "Ок"
					btnTr = btnYesImg.rectTransform;
					//Устанавливаем позицию
					btnTr.anchoredPosition = new Vector2(-(20.0f + cancelW), -10.0f);
					//Получаем элемент "текст"
					btnText = btnYesImg.GetComponentInChildren<Text>();
					//Устанавливаем текст
					btnText.text = (btnText0 == "") ? "Ok" : btnText0;
					//Устанавливаем размер
					btnTr.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Mathf.Clamp(btnText.preferredWidth + 10, 130, mainPanel.rect.width/2 - 30));
					//Отключаем кнопку "Нет"
					btnNoImg.gameObject.SetActive(false);
					break;
				//Кнопки "Да" и "Нет"
				case MsgBoxButtons.YES_NO:
					//Получаем элемент "текст"
					btnText = btnNoImg.GetComponentInChildren<Text>();
					//Устанавливаем текст
					btnText.text = (btnText1 == "") ? "No" : btnText1;
					//Получаем рект кнопки "Нет"
					btnTr = btnNoImg.rectTransform;
					//Устанавливаем размер
					noW = Mathf.Clamp(btnText.preferredWidth + 10, 130, mainPanel.rect.width/2 - 30);
					btnTr.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, noW);
					//Устанавливаем позицию
					btnTr.anchoredPosition = new Vector2(-10, -10);
					//Получаем рект кнопки "Ок"
					btnTr = btnYesImg.rectTransform;
					//Устанавливаем позицию
					btnTr.anchoredPosition = new Vector2(-(20.0f + noW), -10.0f);
					//Получаем элемент "текст"
					btnText = btnYesImg.GetComponentInChildren<Text>();
					//Устанавливаем текст
					btnText.text = (btnText0 == "") ? "Yes" : btnText0;
					//Устанавливаем размер
					btnTr.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Mathf.Clamp(btnText.preferredWidth + 10, 130, mainPanel.rect.width/2 - 30));
					//Отключаем кнопку "Отмена"
					btnCancelImg.gameObject.SetActive(false);
					break;
				case MsgBoxButtons.YES_NO_CANCEL:
					//Получаем элемент "текст"
					btnText = btnCancelImg.GetComponentInChildren<Text>();
					//Устанавливаем текст
					btnText.text = (btnText2 == "") ? "Cancel" : btnText2;
					//Получаем рект кнопки "Отмена"
					btnTr = btnCancelImg.rectTransform;
					//Устанавливаем размер
					cancelW = Mathf.Clamp(btnText.preferredWidth + 10, 130, mainPanel.rect.width/3 - 30);
					btnTr.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, cancelW);
					//Устанавливаем позицию
					btnTr.anchoredPosition = new Vector2(-10, -10);
					//Получаем элемент "текст"
					btnText = btnNoImg.GetComponentInChildren<Text>();
					//Устанавливаем текст
					btnText.text = (btnText1 == "") ? "No" : btnText1;
					//Получаем рект кнопки "Нет"
					btnTr = btnNoImg.rectTransform;
					//Устанавливаем размер
					noW = Mathf.Clamp(btnText.preferredWidth + 10, 130, mainPanel.rect.width/3 - 30);
					btnTr.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, noW);
					btnTr.anchoredPosition = new Vector2(-(20.0f + cancelW), -10);
					//Получаем рект кнопки "Ок"
					btnTr = btnYesImg.rectTransform;
					//Устанавливаем позицию
					btnTr.anchoredPosition = new Vector2(-(30.0f + cancelW + noW), -10.0f);
					//Получаем элемент "текст"
					btnText = btnYesImg.GetComponentInChildren<Text>();
					//Устанавливаем текст
					btnText.text = (btnText0 == "") ? "Ok" : btnText0;
					//Устанавливаем размер
					btnTr.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Mathf.Clamp(btnText.preferredWidth + 10, 130, mainPanel.rect.width/3 - 30));
					break;
				default:
					break;
			}
		}

		/// <summary>
		/// Show the message box.
		/// </summary>
		/// <param name="id">Message Box ID</param>
		/// <param name="message">Message.</param>
		/// <param name="method">Called method with result</param>
		/// <param name="modal">if <c>true</c> then blocked other GUI elements</param>
		/// <param name="btnText0">Text for button Yes/Ok. "" - use default value</param>
		/// <param name="btnText1">Text for button No. "" - use default value</param>
		/// <param name="btnText2">Text for button Cancel. "" - use default value</param>
		public static void Show(int id, string message, DialogResultMethod method, bool modal = false, string btnText0 = "", string btnText1 = "", string btnText2 = "")
		{
			Show(id, message, "", MsgBoxButtons.OK, MsgBoxStyle.Information, method, modal, btnText0, btnText1, btnText2);
		}

		/// <summary>
		/// Show the message box with caption.
		/// </summary>
		/// <param name="id">Message Box ID</param>
		/// <param name="message">Message.</param>
		/// <param name="caption">Caption.</param>
		/// <param name="method">Called method with result</param>
		/// <param name="modal">if <c>true</c> then blocked other GUI elements</param>
		/// <param name="btnText0">Text for button Yes/Ok. "" - use default value</param>
		/// <param name="btnText1">Text for button No. "" - use default value</param>
		/// <param name="btnText2">Text for button Cancel. "" - use default value</param>
		public static void Show(int id, string message, string caption, DialogResultMethod method, bool modal = false, string btnText0 = "", string btnText1 = "", string btnText2 = "")
		{
			Show(id, message, caption, MsgBoxButtons.OK, MsgBoxStyle.Information, method, modal, btnText0, btnText1, btnText2);
		}

		/// <summary>
		/// Show the message box with caption.
		/// </summary>
		/// <param name="id">Message Box ID</param>
		/// <param name="message">Message.</param>
		/// <param name="caption">Caption.</param>
		/// <param name="buttons">Buttons.</param>
		/// <param name="method">Called method with result</param>
		/// <param name="modal">if <c>true</c> then blocked other GUI elements</param>
		/// <param name="btnText0">Text for button Yes/Ok. "" - use default value</param>
		/// <param name="btnText1">Text for button No. "" - use default value</param>
		/// <param name="btnText2">Text for button Cancel. "" - use default value</param>
		public static void Show(int id, string message, string caption, MsgBoxButtons buttons, DialogResultMethod method, bool modal = false, string btnText0 = "", string btnText1 = "", string btnText2 = "")
		{
			Show(id, message, caption, buttons, MsgBoxStyle.Information, method, modal, btnText0, btnText1, btnText2);
		}

		/// <summary>
		/// Show the message box with caption and buttons.
		/// </summary>
		/// <param name="id">Message Box ID</param>
		/// <param name="message">Message.</param>
		/// <param name="caption">Caption.</param>
		/// <param name="buttons">Buttons.</param>
		/// <param name="style">Style of message box</param>
		/// <param name="method">Called method with result</param>
		/// <param name="modal">if <c>true</c> then blocked other GUI elements</param>
		/// <param name="btnText0">Text for button Yes/Ok. "" - use default value</param>
		/// <param name="btnText1">Text for button No. "" - use default value</param>
		/// <param name="btnText2">Text for button Cancel. "" - use default value</param>
		public static void Show(int id, string message, string caption, MsgBoxButtons buttons, MsgBoxStyle style, DialogResultMethod method, bool modal = false, string btnText0 = "", string btnText1 = "", string btnText2 = "")
		{
			Close();
			_lastMsgBox = (GameObject)Instantiate(Resources.Load("PXMSG/MSG"));
			MsgBox box = _lastMsgBox.GetComponent<MsgBox>();
			box.BuildMessageBox(id, message, caption, buttons, style, method, modal, btnText0, btnText1, btnText2);
			EventSystem[] es = GameObject.FindObjectsOfType<EventSystem>();
			if(es.Length > 1 || es[0] != box.eventer)
				DestroyObject(box.eventer.gameObject);
		}

		/// <summary>
		/// Close the last message box.
		/// </summary>
		public static void Close()
		{
			if(_lastMsgBox != null)
			{
				DestroyObject(_lastMsgBox);
			}
		}
	}

	public delegate void DialogResultMethod(int id, DialogResult result);

	public enum DialogResult
	{
		YES_OK = 0,
		NO = 1,
		CANCEL = 2
	}

	public enum MsgBoxStyle
	{
		Information = 0,
		Question = 1,
		Warning = 2,
		Error = 3,
		Custom = 4
	}

	public enum MsgBoxButtons
	{
		OK = 0,
		OK_CANCEL = 1,
		YES_NO = 2,
		YES_NO_CANCEL = 3
	}
}