namespace TurnTheGameOn.NPCChat
{
	using UnityEngine;
	using UnityEngine.UI;

	[System.Serializable]
	public class DialogueButtons
	{
		public enum ItemType { none, enableButton }
		public ItemType[] buttonComponent;
		public Button.ButtonClickedEvent[] NPCClick;
		[TextArea(1, 5)] public string[] buttonString;
	}
}