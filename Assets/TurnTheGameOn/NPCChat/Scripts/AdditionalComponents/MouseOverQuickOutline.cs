namespace TurnTheGameOn.NPCChat
{
	using System.Collections.Generic;
	using UnityEngine;

	public class MouseOverQuickOutline : MonoBehaviour
	{
#if NPCCHAT_QUICKOUTLINE
		public Outline.Mode outlineMode = Outline.Mode.OutlineVisible;
		private List<Outline> quickOutlineList = new List<Outline>();
#endif
		public List<Renderer> outlineRendererList;

#if NPCCHAT_QUICKOUTLINE
		private void Awake()
		{
			for (int i = 0; i < outlineRendererList.Count; i++)
			{
				Outline quickOutline = outlineRendererList[i].gameObject.AddComponent<Outline>();
				quickOutline.enabled = false;
				quickOutline.OutlineMode = outlineMode;
				quickOutlineList.Add(quickOutline);
			}
		}

		private void OnMouseExit()
		{
			for (int i = 0; i < quickOutlineList.Count; i++)
			{
				quickOutlineList[i].enabled = false;
			}
		}

		private void OnMouseEnter()
		{
			for (int i = 0; i < quickOutlineList.Count; i++)
			{
				quickOutlineList[i].enabled = true;
			}
		}
#endif
	}
}