namespace TurnTheGameOn.NPCChat
{
	using UnityEngine;
	using UnityEngine.UI;
	using UnityEngine.EventSystems;
	using System.Collections;
	using TMPro;

	public class ChatBox : MonoBehaviour
	{
		public bool useRenderTexture;
		public GameObject chatBoxObject;
		public RectTransform chatBoxRectTransform;
		public Image iconImage;
		public RawImage renderTextureImage;
		public GameObject renderTextureCamera;
		public TextMeshProUGUI headerText;
		public TextMeshProUGUI bodyText;
		public ButtonComponents[] buttonComponents;

		[System.Serializable]
		public class ButtonComponents
		{
			public GameObject buttonObject;
			public Button button;
		}
		[System.Serializable]
		public class EventSystemNavigation
		{
			public bool useEventSystemNavigation;
			public EventSystem eventSystem;
			public GameObject firstSelection;
			public GameObject selected;
		}
		public EventSystemNavigation eventSystemNavigation;
		[System.Serializable]
		public class LerpSettings
		{
			public bool useLerpIn;
			public bool useLerpOut;
			public AnimationCurve lerpCurve;
			public Vector2 startPosition;
			public Vector2 endPosition;
			public float lerpDuration = 0.5f;
		}
		public LerpSettings lerpSettings;
		
		private float elapsedTime;
		private float lerpProgress;
		private Vector2 tempPosition;

		private void Awake()
		{
			if (renderTextureCamera != null)
			{
				renderTextureCamera.gameObject.SetActive(false);
			}
		}

		void OnDisable()
		{
			if (eventSystemNavigation.eventSystem) eventSystemNavigation.eventSystem.sendNavigationEvents = true;
			StopAllCoroutines();
		}

		void Update()
		{
			if (chatBoxObject.activeInHierarchy)
			{
				if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
				{
					if (!eventSystemNavigation.eventSystem)
					{
						SetChatBoxButtonSelection();
					}
					else
					{
						eventSystemNavigation.eventSystem.SetSelectedGameObject(null);
						eventSystemNavigation.eventSystem.SetSelectedGameObject(eventSystemNavigation.selected);
						//selected.GetComponent<Button> ().Select ();
					}
				}
				else if (eventSystemNavigation.eventSystem)
				{
					if (eventSystemNavigation.eventSystem.currentSelectedGameObject)
					{
						eventSystemNavigation.selected = eventSystemNavigation.eventSystem.currentSelectedGameObject;
						eventSystemNavigation.eventSystem.SetSelectedGameObject(null);
						eventSystemNavigation.eventSystem.SetSelectedGameObject(eventSystemNavigation.selected);
						eventSystemNavigation.selected.GetComponent<Button>().Select();
					}
					else
					{
						SetChatBoxButtonSelection();
					}
				}
				else
				{
					SetChatBoxButtonSelection();
				}
			}
		}

		[ContextMenu("SetChatBoxButtonSelection")]
		public void SetChatBoxButtonSelection()
		{
			if (eventSystemNavigation.useEventSystemNavigation)
			{
				if (eventSystemNavigation.eventSystem == null)
				{
					eventSystemNavigation.eventSystem = GameObject.FindObjectOfType<EventSystem>();
					if (eventSystemNavigation.eventSystem == null)
					{
						Debug.LogWarning("[NPC Chat - Chat Box] " + gameObject.name + " - Use Event System Navigation is enabled, but an event system was not found in the scene. Please add one to your scene to use this feature.");
					}
				}
				if (eventSystemNavigation.eventSystem != null)
				{
					eventSystemNavigation.eventSystem.sendNavigationEvents = false;
					if (eventSystemNavigation.firstSelection) eventSystemNavigation.eventSystem.firstSelectedGameObject = eventSystemNavigation.firstSelection;
					if (!eventSystemNavigation.eventSystem.firstSelectedGameObject) Debug.LogWarning("The chat box has useeventSystemNavigation enabled but the first selection is not assigned.");
					eventSystemNavigation.eventSystem.SetSelectedGameObject(eventSystemNavigation.firstSelection);
					eventSystemNavigation.selected = eventSystemNavigation.firstSelection;
					eventSystemNavigation.selected.GetComponent<Button>().Select();
				}
			}
		}


		#region Open Chat Box
		public void OpenChatBox()//Sprite _spriteIcon)
		{
			//iconImage.sprite = _spriteIcon;
			if (renderTextureCamera != null && useRenderTexture)
			{
				renderTextureCamera.gameObject.SetActive(true);
			}
			chatBoxObject.SetActive(true);
			if (lerpSettings.useLerpIn)
			{
				StartCoroutine(LerpRectFromStartPositionToEndPosition(lerpSettings.startPosition, lerpSettings.endPosition, lerpSettings.lerpDuration));
			}
		}

		private IEnumerator LerpRectFromStartPositionToEndPosition(Vector2 startPosition, Vector2 endPosition, float time)
		{
			elapsedTime = 0;
			while (elapsedTime < time)
			{
				lerpProgress = lerpSettings.lerpCurve.Evaluate(Mathf.InverseLerp(0.0f, lerpSettings.lerpDuration, (elapsedTime / time)));
				tempPosition = Vector2.Lerp(startPosition, endPosition, lerpProgress);
				chatBoxRectTransform.anchoredPosition = tempPosition;
				elapsedTime += Time.deltaTime;
				yield return new WaitForEndOfFrame();
			}
			tempPosition = endPosition;
			chatBoxRectTransform.anchoredPosition = tempPosition;
		}
        #endregion

        #region Close Chat Box
        public void CloseChatBox()
		{
			if (lerpSettings.useLerpOut && chatBoxObject.activeInHierarchy)
			{
				if (chatBoxRectTransform == null) chatBoxRectTransform = GetComponent<RectTransform>();
				StartCoroutine(LerpRectFromEndPositionToStartPosition(lerpSettings.startPosition, lerpSettings.endPosition, lerpSettings.lerpDuration));
			}
			else
			{
				if (renderTextureCamera != null && useRenderTexture)
				{
					renderTextureCamera.gameObject.SetActive(false);
				}
				chatBoxObject.SetActive(false);
			}
		}

		private IEnumerator LerpRectFromEndPositionToStartPosition(Vector2 startPosition, Vector2 endPosition, float time)
		{
			elapsedTime = 0;
			while (elapsedTime < time)
			{
				lerpProgress = lerpSettings.lerpCurve.Evaluate(Mathf.InverseLerp(0.0f, lerpSettings.lerpDuration, (elapsedTime / time)));
				tempPosition = Vector2.Lerp(endPosition, startPosition, lerpProgress);
				chatBoxRectTransform.anchoredPosition = tempPosition;
				elapsedTime += Time.deltaTime;
				yield return new WaitForEndOfFrame();
			}
			tempPosition = startPosition;
			chatBoxRectTransform.anchoredPosition = tempPosition;
			chatBoxObject.SetActive(false);
		}
        #endregion

	}
}