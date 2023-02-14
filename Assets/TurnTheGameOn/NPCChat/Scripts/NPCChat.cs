namespace TurnTheGameOn.NPCChat
{
	using UnityEngine;
	using UnityEngine.UI;
	using UnityEngine.Events;
	using System.Collections;
	using System.Collections.Generic;
	using TMPro;

	public class NPCChat : MonoBehaviour
	{
		public List<DialogueSettings> dialogueList = new List<DialogueSettings>(1);
		public bool playOnAwake = false;
		public float delay = 8;
		public bool scrollingText = true;
		public float speed = 0.5f;
		public bool timedPages = false;
		public UnityEvent chatStartEvent;
		public UnityEvent chatStopEvent;
		public bool useStartEvent = true;
		public bool useStopEvent = true;
		private bool startConversation;
		public bool talking { get; private set; }
		private int currentPage;
		private bool textIsScrolling, buttonPage;
		private TextMeshProUGUI chatText, speakerNameText;
		private GameObject tempClip;
		private int startLine;
		private string displayText = "";
		private int currentIndex;
		private float closeChatBoxTime;
		[HideInInspector] public int numberOfPages;
		[HideInInspector] public int pages;
		public bool canChat = true;
		private bool timedPage;
		private float pageTimer;

		void Awake()
		{
			if (playOnAwake) { Invoke("StartConversation", delay); }
		}

		void Update()
		{
			if (startConversation) StartChat();
			if (textIsScrolling)
			{
				if (chatText.text == dialogueList[currentPage].text)
				{
					textIsScrolling = false;
				}
			}
			if (textIsScrolling == false)
			{
				if (buttonPage)
				{
					for (int i = 0; i < (dialogueList[currentPage].buttons.buttonComponent.Length); i++)
					{
						if (dialogueList[currentPage].buttons.buttonComponent[i] == DialogueButtons.ItemType.enableButton)
						{
							dialogueList[currentPage].chatBox.buttonComponents[i].buttonObject.SetActive(true);
							Button tempButton = dialogueList[currentPage].chatBox.buttonComponents[i].button;
							tempButton.onClick = dialogueList[currentPage].buttons.NPCClick[i];
							TextMeshProUGUI tempButtonText = tempButton.GetComponentInChildren<TextMeshProUGUI>();
							tempButtonText.text = dialogueList[currentPage].buttons.buttonString[i];
						}
					}
					dialogueList[currentPage].pageStopEvent.Invoke();
					Invoke("EnableEventSystemNavigation", 0.1f);
				}
			}
			if (talking)
			{
				pageTimer += Time.deltaTime;
				if (pageTimer >= dialogueList[currentPage].duration)
				{
					FinishPage();
				}
			}
		}

		IEnumerator NPCChatUpdateCoroutine()
		{
			buttonPage = false;
			pageTimer = 0;
			if (dialogueList[currentPage].chatBox)
			{
				timedPage = dialogueList[currentPage].duration > 0 && timedPage ? true : false;
				speakerNameText = dialogueList[currentPage].chatBox.headerText;
				speakerNameText.text = dialogueList[currentPage].name;
				if (currentPage > 0)
				{
					dialogueList[currentPage - 1].chatBox.CloseChatBox();
					if (dialogueList[currentPage].chatBox.lerpSettings.useLerpOut)
					{
						closeChatBoxTime = dialogueList[currentPage].chatBox.lerpSettings.lerpDuration;
						yield return new WaitForSeconds(closeChatBoxTime + 0.1f);
					}
				}
				for (int ia = 0; ia < (dialogueList[currentPage].buttons.buttonComponent.Length); ia++)
				{
					dialogueList[currentPage].chatBox.buttonComponents[ia].buttonObject.SetActive(false);
				}
				dialogueList[currentPage].chatBox.bodyText.text = "";
				chatText = dialogueList[currentPage].chatBox.bodyText;
				dialogueList[currentPage].chatBox.OpenChatBox();
				for (int i = 0; i < (dialogueList[currentPage].buttons.buttonComponent.Length); i++)
				{
					if (dialogueList[currentPage].buttons.buttonComponent[i] == DialogueButtons.ItemType.enableButton)
					{
						buttonPage = true;
					}
				}
			}
			dialogueList[currentPage].pageStartEvent.Invoke();
			if (tempClip != null) Destroy(tempClip);
			if (dialogueList[currentPage].audio != null)
			{
				tempClip = new GameObject();
				tempClip.transform.parent = transform;
				tempClip.AddComponent<AudioSource>();
				tempClip.name = "NPC Page Audio";
				if (dialogueList[currentPage].loopAudio)
					tempClip.GetComponent<AudioSource>().loop = true;
				tempClip.AddComponent<DestroyAudioSource>();
				tempClip.GetComponent<AudioSource>().clip = dialogueList[currentPage].audio;
				tempClip.GetComponent<AudioSource>().Play();
			}
#if NPCCHAT_LIPSYNCPRO
			if (dialogueList[currentPage].lipSyncData != null && dialogueList[currentPage].lipSync != null)
			{
				dialogueList[currentPage].lipSync.Play(dialogueList[currentPage].lipSyncData);
			}
#endif
			StartCoroutine(ScrollPageText());
		}

		[ContextMenu("StartChat")]
		public void StartChat()
		{
			if (canChat && talking == false)
			{
				talking = true;
				Invoke("NPCChatStart", 0.1f);
			}
		}

		public void NPCChatStart()
		{
			colorString = tempFormattedString1 = tempFormattedString2 = finalFormattedString = "";
			startConversation = false;
			chatStartEvent.Invoke();
			currentPage = 0;
			StartCoroutine(NPCChatUpdateCoroutine());
		}

		public void FinishPage()
		{
			if (talking)
			{
				if (textIsScrolling)
				{
					textIsScrolling = false;
					chatText.text = dialogueList[currentPage].text;
					if (buttonPage)
					{
						for (int i = 0; i < (dialogueList[currentPage].buttons.buttonComponent.Length); i++)
						{
							if (dialogueList[currentPage].buttons.buttonComponent[i] == DialogueButtons.ItemType.enableButton)
							{
								dialogueList[currentPage].chatBox.buttonComponents[i].buttonObject.SetActive(true);
								Button tempButton = dialogueList[currentPage].chatBox.buttonComponents[i].button;
								tempButton.onClick = dialogueList[currentPage].buttons.NPCClick[i];
								TextMeshProUGUI tempButtonText = tempButton.GetComponentInChildren<TextMeshProUGUI>();
								tempButtonText.text = dialogueList[currentPage].buttons.buttonString[i];
							}
						}
						dialogueList[currentPage].pageStopEvent.Invoke();
						Invoke("EnableEventSystemNavigation", 0.1f);
					}
				}
				else if (pageTimer >= 1f) // min open time
				{
					if (currentPage < dialogueList.Count - 1)
					{
						currentPage++;
						colorString = tempFormattedString1 = tempFormattedString2 = finalFormattedString = "";
						StartCoroutine(NPCChatUpdateCoroutine());
					}
					else if (!buttonPage) StopChat();
				}
			}
		}

		public void StopChat()
		{
			dialogueList[currentPage].pageStopEvent.Invoke();
			if (tempClip) Destroy(tempClip);
#if NPCCHAT_LIPSYNCPRO
			if (dialogueList[currentPage].lipSync != null)
			{
				dialogueList[currentPage].lipSync.Stop(true);
			}
#endif
			currentPage = 0;
			if (dialogueList[currentPage].chatBox)
			{
				chatText.text = dialogueList[currentPage].text;
				for (var i = 0; i < dialogueList.Count; i++)
				{
					dialogueList[i].chatBox.CloseChatBox();
				}
				for (int j = 0; j < (dialogueList[currentPage].buttons.buttonComponent.Length); j++)
				{
					dialogueList[currentPage].chatBox.buttonComponents[j].buttonObject.SetActive(false);
				}
			}
			talking = false;
			chatStopEvent.Invoke();
		}

		IEnumerator ScrollPageText()
		{
			textIsScrolling = true;
			if (dialogueList[currentPage].chatBox)
			{
				startLine = currentPage;
				if (scrollingText)
				{
					for (int i = 0; i < dialogueList[currentPage].text.Length; i++)
					{
						if (talking && textIsScrolling && currentPage == startLine)
						{
							currentIndex = RTFCharacterCheck(i);
							if (currentIndex != i) i = currentIndex;
							for (int j = 0; j < 4; j++)
							{
								if (boldOpen || italicOpen || colorOpen || sizeOpen)
								{
									currentIndex = RTFCharacterCheck(i);
									if (currentIndex != i) i = currentIndex;
								}
							}
							if (boldOpen && !bold)
							{
								SetFormattedString();
								boldOpen = false;
								boldPrefix = boldSuffix = "";
							}
							if (italicOpen && !italic)
							{
								SetFormattedString();
								italicOpen = false;
								italicPrefix = italicSuffix = "";
							}
							if (colorOpen && !color)
							{
								SetFormattedString();
								colorOpen = false;
								colorString = colorPrefix = colorSuffix = "";
							}
							if (sizeOpen && !size)
							{
								SetFormattedString();
								sizeOpen = false;
								sizeString = sizePrefix = sizeSuffix = "";
							}
							if (boldOpen || italicOpen || colorOpen || sizeOpen)
							{
								tempFormattedString1 += dialogueList[currentPage].text[i];
								tempFormattedString2 = boldPrefix + italicPrefix + colorPrefix + sizePrefix + tempFormattedString1 + sizeSuffix + colorSuffix + italicSuffix + boldSuffix;
								charactersFormatted += 1;
							}
							else
							{
								SetFormattedString();
								finalFormattedString += dialogueList[currentPage].text[i];
							}
							chatText.text = finalFormattedString + tempFormattedString2;
							yield return new WaitForSeconds(speed / 100f);
						}
					}
				}
				else
				{
					displayText = dialogueList[currentPage].text;
					chatText.text = displayText;
				}
			}
			textIsScrolling = false;
		}

		void EnableEventSystemNavigation()
		{
			if (dialogueList[currentPage].chatBox.eventSystemNavigation.useEventSystemNavigation)
			{
				dialogueList[currentPage].chatBox.eventSystemNavigation.eventSystem.sendNavigationEvents = true;
			}
		}

#region Text Formatting
		private int charactersFormatted;
		private bool bold, boldOpen;
		private bool italic, italicOpen;
		private bool color, colorOpen;
		private bool size, sizeOpen;
		private string boldPrefix, boldSuffix;
		private string italicPrefix, italicSuffix;
		private string colorString, colorPrefix, colorSuffix;
		private string sizeString, sizePrefix, sizeSuffix;
		private string tempFormattedString1, tempFormattedString2, finalFormattedString;

		void SetFormattedString()
		{
			finalFormattedString += tempFormattedString2;
			if (tempFormattedString2 != "") tempFormattedString1 = tempFormattedString2 = "";
		}

		int RTFCharacterCheck(int index)
		{
			int newIndex;
			newIndex = RTFBoldCheck(index);
			newIndex = RTFItalicCheck(newIndex);
			newIndex = RTFColorCheck(newIndex);
			newIndex = RTFSizeCheck(newIndex);
			return newIndex;
		}

		int RTFBoldCheck(int index)
		{
			string s = dialogueList[currentPage].text[index].ToString();
			int newIndex = index;
			if (s == "<")
			{
				s += dialogueList[currentPage].text[index + 1].ToString();
				if (s == "<b")
				{
					s += dialogueList[currentPage].text[index + 2].ToString();
					if (s == "<b>")
					{
						newIndex += 3;
						bold = boldOpen = true;
						boldPrefix = "<b>";
						boldSuffix = "</b>";
						if (charactersFormatted > 0) SetFormattedString();
						charactersFormatted = 0;
					}
				}
				else if (s == "</")
				{
					s += dialogueList[currentPage].text[index + 2].ToString();
					if (s == "</b")
					{
						s += dialogueList[currentPage].text[index + 3].ToString();
						if (s == "</b>")
						{
							newIndex += 4;
							bold = false;
							boldSuffix = "";
						}
					}
				}
			}
			return newIndex;
		}

		int RTFItalicCheck(int index)
		{
			string s = dialogueList[currentPage].text[index].ToString();
			int newIndex = index;
			if (s == "<")
			{
				s += dialogueList[currentPage].text[index + 1].ToString();
				if (s == "<i")
				{
					s += dialogueList[currentPage].text[index + 2].ToString();
					if (s == "<i>")
					{
						newIndex += 3;
						italic = italicOpen = true;
						italicPrefix = "<i>";
						italicSuffix = "</i>";
						if (charactersFormatted > 0) SetFormattedString();
						charactersFormatted = 0;
					}
				}
				else if (s == "</")
				{
					s += dialogueList[currentPage].text[index + 2].ToString();
					if (s == "</i")
					{
						s += dialogueList[currentPage].text[index + 3].ToString();
						if (s == "</i>")
						{
							newIndex += 4;
							italic = false;
							italicSuffix = "";

						}
					}
				}
			}
			return newIndex;
		}

		int RTFColorCheck(int index)
		{
			string s = dialogueList[currentPage].text[index].ToString();
			int newIndex = index;
			if (s == "<")
			{
				s += dialogueList[currentPage].text[index + 1].ToString();
				if (s == "<c")
				{
					s += dialogueList[currentPage].text[index + 2].ToString();
					if (s == "<co")
					{
						color = colorOpen = true;
						newIndex += 7;
						StringCheck(dialogueList[currentPage].text[newIndex].ToString(), newIndex, "color");
						newIndex += colorString.Length + 1;
						colorPrefix = "<color=" + colorString + ">";
						colorSuffix = "</color>";
						if (charactersFormatted > 0) SetFormattedString();
						charactersFormatted = 0;
					}
				}
				else if (s == "</")
				{
					s += dialogueList[currentPage].text[index + 2].ToString();
					if (s == "</c")
					{
						s += dialogueList[currentPage].text[index + 3].ToString();
						if (s == "</co")
						{
							newIndex += 8;
							color = false;
							colorSuffix = "";
						}
					}
				}
			}
			return newIndex;
		}

		int RTFSizeCheck(int index)
		{
			string s = dialogueList[currentPage].text[index].ToString();
			int newIndex = index;
			if (s == "<")
			{
				s += dialogueList[currentPage].text[index + 1].ToString();
				if (s == "<s")
				{
					s += dialogueList[currentPage].text[index + 2].ToString();
					if (s == "<si")
					{
						size = sizeOpen = true;
						newIndex += 6;
						StringCheck(dialogueList[currentPage].text[newIndex].ToString(), newIndex, "size");
						newIndex += sizeString.Length + 1;
						sizePrefix = "<size=" + sizeString + ">";
						sizeSuffix = "</size>";
						if (charactersFormatted > 0) SetFormattedString();
						charactersFormatted = 0;
					}
				}
				else if (s == "</")
				{
					s += dialogueList[currentPage].text[index + 2].ToString();
					if (s == "</s")
					{
						s += dialogueList[currentPage].text[index + 3].ToString();
						if (s == "</si")
						{
							newIndex += 7;
							size = false;
							sizeSuffix = "";
						}
					}
				}
			}
			return newIndex;
		}

		void StringCheck(string s1, int index, string typeCheck)
		{
			if (typeCheck == "color")
			{
				if (s1 != ">")
				{
					colorString += s1;
					index += 1;
					StringCheck(dialogueList[currentPage].text[index].ToString(), index, "color");
				}
			}
			else if (typeCheck == "size")
			{
				if (s1 != ">")
				{
					sizeString += s1;
					index += 1;
					StringCheck(dialogueList[currentPage].text[index].ToString(), index, "size");
				}
			}
		}
#endregion
	}
}