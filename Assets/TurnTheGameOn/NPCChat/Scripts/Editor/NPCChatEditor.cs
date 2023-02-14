namespace TurnTheGameOn.NPCChat
{
	using UnityEngine;
	using UnityEngine.UI;
	using UnityEditor;

	[CustomEditor(typeof(NPCChat))]
	public class NPCChatEditor : Editor
	{
		int editorPage;
		static int tab;
		static bool showButtonSettings;
		static bool showPageEventSettings;
		GUIContent _GUIContent;

		public override void OnInspectorGUI()
		{
			GUI.enabled = false;
			EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour((NPCChat)target), typeof(NPCChat), false);
			GUI.enabled = true;

			NPCChat _NPCChat = (NPCChat)target;

			EditorGUILayout.BeginVertical("Box");
			tab = GUILayout.Toolbar(tab, new string[] { "Conversation", "Settings" });
			EditorGUILayout.EndVertical();

			EditorGUILayout.Space();

			switch (tab)
			{
				#region Conversation
				case 0:
					if (_NPCChat.dialogueList.Count > 0)
					{
						#region Number of pages
						EditorGUILayout.BeginVertical("Box");
						EditorGUILayout.BeginHorizontal();

						/// Number Of Pages
						SerializedProperty numberOfPages = serializedObject.FindProperty("numberOfPages");
						_GUIContent = new GUIContent("Number Of Pages", "Defines the number of pages in the conversation, press update to apply changes.");
						EditorGUI.BeginChangeCheck();
						EditorGUILayout.PropertyField(numberOfPages, _GUIContent);
						_NPCChat.numberOfPages = _NPCChat.numberOfPages < 1 ? 1 : _NPCChat.numberOfPages;

						_GUIContent = new GUIContent("Update", "Set the current number of pages in the conversation.");
						if (GUILayout.Button(_GUIContent))
						{
							if (_NPCChat.dialogueList.Count != _NPCChat.numberOfPages)
							{
								while (_NPCChat.dialogueList.Count > _NPCChat.numberOfPages) _NPCChat.dialogueList.RemoveAt(_NPCChat.dialogueList.Count - 1);
								while (_NPCChat.dialogueList.Count < _NPCChat.numberOfPages) _NPCChat.dialogueList.Add(new DialogueSettings());
							}
							if (_NPCChat.dialogueList.Count <= editorPage)
								editorPage = _NPCChat.dialogueList.Count - 1;
							GUIUtility.hotControl = 0;
							GUIUtility.keyboardControl = 0;
							EditorUtility.SetDirty(target);
							serializedObject.Update();
						}

						if (EditorGUI.EndChangeCheck())
							serializedObject.ApplyModifiedProperties();

						EditorGUILayout.EndHorizontal();
						EditorGUILayout.EndVertical();
						#endregion

						#region Page Selection
						EditorGUILayout.BeginHorizontal();
						if (editorPage >= 1)
						{
							_GUIContent = new GUIContent("-", "Show previous page.");
							if (GUILayout.Button(_GUIContent, GUILayout.MaxWidth(30)))
							{
								editorPage -= 1;
								EditorGUI.FocusTextInControl(null);
								GUIUtility.hotControl = 0;
								GUIUtility.keyboardControl = 0;
							}
						}
						else
						{
							GUILayout.Box("", GUILayout.MaxWidth(30));
						}
						if (editorPage < _NPCChat.dialogueList.Count - 1)
						{
							_GUIContent = new GUIContent("+", "Show next page.");
							if (GUILayout.Button(_GUIContent, GUILayout.MaxWidth(30)))
							{
								editorPage += 1;
								EditorGUI.FocusTextInControl(null);
								GUIUtility.hotControl = 0;
								GUIUtility.keyboardControl = 0;
							}
						}
						else
						{
							GUILayout.Box("", GUILayout.MaxWidth(30));
						}
						EditorGUILayout.LabelField("  Page   " + (editorPage + 1).ToString(), EditorStyles.label, GUILayout.MaxWidth(60));
						EditorGUILayout.EndHorizontal();
						#endregion

						if (_NPCChat.timedPages)
						{
							EditorGUILayout.BeginVertical("box");
							/// Duration
							SerializedProperty duration = serializedObject.FindProperty("dialogueList").GetArrayElementAtIndex(editorPage).FindPropertyRelative("duration");
							_GUIContent = new GUIContent("Duration", "Amount of time page will be displayed before closing. Setting a page's duration to 0 will bypass the timer.");
							EditorGUI.BeginChangeCheck();
							EditorGUILayout.PropertyField(duration, _GUIContent);
							if (EditorGUI.EndChangeCheck())
								serializedObject.ApplyModifiedProperties();
							EditorGUILayout.EndVertical();
						}

						#region Pegae Content
						EditorGUILayout.BeginVertical("box");

						/// Name
						SerializedProperty name = serializedObject.FindProperty("dialogueList").GetArrayElementAtIndex(editorPage).FindPropertyRelative("name");
						_GUIContent = new GUIContent("Name", "Text displayed in the chat box name field.");
						EditorGUI.BeginChangeCheck();
						EditorGUILayout.PropertyField(name, _GUIContent);
						if (EditorGUI.EndChangeCheck())
							serializedObject.ApplyModifiedProperties();
						
						/// Text
						SerializedProperty text = serializedObject.FindProperty("dialogueList").GetArrayElementAtIndex(editorPage).FindPropertyRelative("text");
						_GUIContent = new GUIContent("Text", "Text displayed in the chat box text field.");
						EditorGUI.BeginChangeCheck();
						EditorGUILayout.PropertyField(text, _GUIContent);
						if (EditorGUI.EndChangeCheck())
							serializedObject.ApplyModifiedProperties();

						/// Chat Box
						SerializedProperty chatBox = serializedObject.FindProperty("dialogueList").GetArrayElementAtIndex(editorPage).FindPropertyRelative("chatBox");
						_GUIContent = new GUIContent("Chat Box", "Reference to a chat box to use for the conversation page.");
						EditorGUI.BeginChangeCheck();
						EditorGUILayout.PropertyField(chatBox, _GUIContent);
						if (EditorGUI.EndChangeCheck())
							serializedObject.ApplyModifiedProperties();

#if NPCCHAT_LIPSYNCPRO
						/// Lip Sync Data
						SerializedProperty lipSyncData = serializedObject.FindProperty("dialogueList").GetArrayElementAtIndex(editorPage).FindPropertyRelative("lipSyncData");
						_GUIContent = new GUIContent("Lip Sync Data", "Reference to a LipSyncData object for the conversation page, will be played on the page's LipSync reference.");
						EditorGUI.BeginChangeCheck();
						EditorGUILayout.PropertyField(lipSyncData, _GUIContent);
						if (EditorGUI.EndChangeCheck())
							serializedObject.ApplyModifiedProperties();

						/// Lip Sync Object
						SerializedProperty lipSync = serializedObject.FindProperty("dialogueList").GetArrayElementAtIndex(editorPage).FindPropertyRelative("lipSync");
						_GUIContent = new GUIContent("Lip Sync", "Reference to a LipSync object for the conversation page, will be played on the page's LipSync reference.");
						EditorGUI.BeginChangeCheck();
						EditorGUILayout.PropertyField(lipSync, _GUIContent);
						if (EditorGUI.EndChangeCheck())
							serializedObject.ApplyModifiedProperties();
#endif
						/// Audio
						SerializedProperty audio = serializedObject.FindProperty("dialogueList").GetArrayElementAtIndex(editorPage).FindPropertyRelative("audio");
						_GUIContent = new GUIContent("Audio", "Audio Clip to use for the conversation page.");
						EditorGUI.BeginChangeCheck();
						EditorGUILayout.PropertyField(audio, _GUIContent);
						if (EditorGUI.EndChangeCheck())
							serializedObject.ApplyModifiedProperties();

						/// Loop Audio
						if (_NPCChat.dialogueList[editorPage].audio)
						{
							SerializedProperty loopAudio = serializedObject.FindProperty("dialogueList").GetArrayElementAtIndex(editorPage).FindPropertyRelative("loopAudio");
							_GUIContent = new GUIContent("Loop Audio", "Controls if the page audio clip should loop.");
							EditorGUI.BeginChangeCheck();
							EditorGUILayout.PropertyField(loopAudio, _GUIContent);
							if (EditorGUI.EndChangeCheck())
								serializedObject.ApplyModifiedProperties();
						}

						/// Page Events
						showPageEventSettings = EditorGUI.Foldout(EditorGUILayout.GetControlRect(), showPageEventSettings, "Page Events", true);
						if (showPageEventSettings)
						{
							/// Page Start Event
							SerializedProperty pageStartEvent = serializedObject.FindProperty("dialogueList").GetArrayElementAtIndex(editorPage).FindPropertyRelative("pageStartEvent");
							_GUIContent = new GUIContent("Page Start Event", "UnityEvent that is triggered when the page starts.");
							EditorGUI.BeginChangeCheck();
							EditorGUILayout.PropertyField(pageStartEvent, _GUIContent);
							if (EditorGUI.EndChangeCheck())
								serializedObject.ApplyModifiedProperties();
							
							/// Page Stop Event
							SerializedProperty pageStopEvent = serializedObject.FindProperty("dialogueList").GetArrayElementAtIndex(editorPage).FindPropertyRelative("pageStopEvent");
							_GUIContent = new GUIContent("Page Stop Event", "UnityEvent that is triggered when the page is closed.");
							EditorGUI.BeginChangeCheck();
							EditorGUILayout.PropertyField(pageStopEvent, _GUIContent);
							if (EditorGUI.EndChangeCheck())
								serializedObject.ApplyModifiedProperties();
						}

						/// Page Buttons
						if (_NPCChat.dialogueList[editorPage].buttons == null || _NPCChat.dialogueList[editorPage].buttons.buttonComponent == null)
						{
							if (Event.current.type == EventType.Repaint)
							{
								_NPCChat.dialogueList[editorPage].buttons = new DialogueButtons();
								_NPCChat.dialogueList[editorPage].buttons.buttonComponent = new DialogueButtons.ItemType[6];
								_NPCChat.dialogueList[editorPage].buttons.buttonString = new string[6];
								_NPCChat.dialogueList[editorPage].buttons.NPCClick = new Button.ButtonClickedEvent[6];
							}
						}
						else
						{
							_GUIContent = new GUIContent("Page Buttons", "Configure button settings for the page's chat box.");
							showButtonSettings = EditorGUI.Foldout(EditorGUILayout.GetControlRect(), showButtonSettings, _GUIContent, true);
							if (showButtonSettings)
							{
								for (int i = 0; i < 6; i++)
								{
									EditorGUILayout.BeginVertical("Box");

									/// Button
									SerializedProperty enableButton = serializedObject.FindProperty("dialogueList").GetArrayElementAtIndex(editorPage).FindPropertyRelative("buttons").FindPropertyRelative("buttonComponent").GetArrayElementAtIndex(i);
									_GUIContent = new GUIContent("Button " + (i + 1).ToString(), "Controls if this button is enabled on the chat box.");
									EditorGUI.BeginChangeCheck();
									EditorGUILayout.PropertyField(enableButton, _GUIContent);
									if (EditorGUI.EndChangeCheck())
										serializedObject.ApplyModifiedProperties();

									if (_NPCChat.dialogueList[editorPage].buttons.buttonComponent[i] == DialogueButtons.ItemType.enableButton)
									{
										/// Button Text
										SerializedProperty buttonString = serializedObject.FindProperty("dialogueList").GetArrayElementAtIndex(editorPage).FindPropertyRelative("buttons").FindPropertyRelative("buttonString").GetArrayElementAtIndex(i);
										_GUIContent = new GUIContent("Button Text", "Text displayed in the chat box button text field.");
										EditorGUI.BeginChangeCheck();
										EditorGUILayout.PropertyField(buttonString, _GUIContent, true);
										if (EditorGUI.EndChangeCheck()) serializedObject.ApplyModifiedProperties();

										/// Button Event
										SerializedProperty NPCClick = serializedObject.FindProperty("dialogueList").GetArrayElementAtIndex(editorPage).FindPropertyRelative("buttons").FindPropertyRelative("NPCClick").GetArrayElementAtIndex(i);
										_GUIContent = new GUIContent("Button Event", "UnityEvent that is triggered when the button is pressed.");
										EditorGUI.BeginChangeCheck();
										EditorGUILayout.PropertyField(NPCClick, _GUIContent, true);
										if (EditorGUI.EndChangeCheck())
											serializedObject.ApplyModifiedProperties();
									}
									EditorGUILayout.EndVertical();
								}
							}
						}

						EditorGUILayout.EndVertical();
						#endregion
					}
					else
					{
						if (Event.current.type == EventType.Repaint)
						{
							_NPCChat.dialogueList.Add(new DialogueSettings());
						}
					}
					break;
				#endregion

				#region Settings
				case 1:
					EditorGUILayout.BeginVertical("box");

					#region Play On Awake
					EditorGUILayout.BeginHorizontal("Box");

					/// Play On Awake
					SerializedProperty playOnAwake = serializedObject.FindProperty("playOnAwake");
					_GUIContent = new GUIContent("Play On Awake", "Play the conversation when the scene loads.");
					EditorGUIUtility.labelWidth = 110;
					EditorGUI.BeginChangeCheck();
					EditorGUILayout.PropertyField(playOnAwake, _GUIContent, GUILayout.MaxWidth(130));
					if (EditorGUI.EndChangeCheck())
						serializedObject.ApplyModifiedProperties();

					/// Start Delay
					if (_NPCChat.playOnAwake)
					{
						SerializedProperty delay = serializedObject.FindProperty("delay");
						_GUIContent = new GUIContent("Delay", "Amount of time to wait before the conversation is started through Play On Awake.");
						EditorGUIUtility.labelWidth = 50;
						EditorGUI.BeginChangeCheck();
						EditorGUILayout.PropertyField(delay, _GUIContent);
						if (EditorGUI.EndChangeCheck())
							serializedObject.ApplyModifiedProperties();
					}
					EditorGUILayout.EndHorizontal();
					#endregion

					#region Scrolling Text
					EditorGUILayout.BeginHorizontal("Box");

					/// Scrolling Text
					SerializedProperty scrollingText = serializedObject.FindProperty("scrollingText");
					_GUIContent = new GUIContent("Scrolling Text", "Print display text over time for each page.");
					EditorGUIUtility.labelWidth = 110;
					EditorGUI.BeginChangeCheck();
					EditorGUILayout.PropertyField(scrollingText, _GUIContent, GUILayout.MaxWidth(130));
					if (EditorGUI.EndChangeCheck())
						serializedObject.ApplyModifiedProperties();

					/// Speed
					if (_NPCChat.scrollingText)
					{
						SerializedProperty speed = serializedObject.FindProperty("speed");
						_GUIContent = new GUIContent("Speed", "Controls the rate at which scrolling text is printed.");
						EditorGUIUtility.labelWidth = 50;
						EditorGUI.BeginChangeCheck();
						EditorGUILayout.PropertyField(speed, _GUIContent);
						if (EditorGUI.EndChangeCheck())
							serializedObject.ApplyModifiedProperties();
					}
					EditorGUILayout.EndHorizontal();
					#endregion

					/// Timed Pages
					EditorGUILayout.BeginVertical("Box");
					SerializedProperty timedPages = serializedObject.FindProperty("timedPages");
					_GUIContent = new GUIContent("Timed Pages", "Auto-complete the page after the pages duration");
					EditorGUIUtility.labelWidth = 110;
					EditorGUI.BeginChangeCheck();
					EditorGUILayout.PropertyField(timedPages, _GUIContent);
					if (EditorGUI.EndChangeCheck())
						serializedObject.ApplyModifiedProperties();
					EditorGUILayout.EndVertical();



					/// NOTE: UnityEvent does not support tooltips as of 11/16/2020

					#region StartEvent

					/// Use Start Event
					EditorGUILayout.BeginVertical("Box");
					EditorGUILayout.BeginHorizontal();
					SerializedProperty useStartEvent = serializedObject.FindProperty("useStartEvent");
					_GUIContent = new GUIContent("Use Start Event", "Controls if the Chat Start Event can be triggered.");
					EditorGUIUtility.labelWidth = 110;
					EditorGUI.BeginChangeCheck();
					EditorGUILayout.PropertyField(useStartEvent, _GUIContent, GUILayout.MaxWidth(130));
					if (EditorGUI.EndChangeCheck())
						serializedObject.ApplyModifiedProperties();
					EditorGUILayout.EndHorizontal();

					/// Chat Start Event
					SerializedProperty chatStartEvent = serializedObject.FindProperty("chatStartEvent");
					//_GUIContent = new GUIContent("Chat Start Event", "Event that's triggered when the conversation is started.");
					EditorGUI.BeginChangeCheck();
					EditorGUILayout.PropertyField(chatStartEvent);//, _GUIContent);
					if (EditorGUI.EndChangeCheck())
						serializedObject.ApplyModifiedProperties();
					EditorGUILayout.EndVertical();
					#endregion

					#region StopEvent
					EditorGUILayout.BeginVertical("Box");

					EditorGUILayout.BeginHorizontal();

					/// Use Stop Event
					SerializedProperty useStopEvent = serializedObject.FindProperty("useStopEvent");
					_GUIContent = new GUIContent("Use Stop Event", "Controls if the Chat Stop Event can be triggered.");
					EditorGUIUtility.labelWidth = 110;
					EditorGUI.BeginChangeCheck();
					EditorGUILayout.PropertyField(useStopEvent, _GUIContent, GUILayout.MaxWidth(130));
					if (EditorGUI.EndChangeCheck())
						serializedObject.ApplyModifiedProperties();
					EditorGUILayout.EndHorizontal();

					/// Chat Stop Event
					SerializedProperty chatStopEvent = serializedObject.FindProperty("chatStopEvent");
					//_GUIContent = new GUIContent("Chat Stop Event", "Event that's triggered when the conversation ends.");
					EditorGUI.BeginChangeCheck();
					EditorGUILayout.PropertyField(chatStopEvent);//, _GUIContent);
					if (EditorGUI.EndChangeCheck())
						serializedObject.ApplyModifiedProperties();
					EditorGUILayout.EndVertical();
					#endregion

					EditorGUILayout.EndVertical();
					break;
					#endregion
			}
		}
	}
}