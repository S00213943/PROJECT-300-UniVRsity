using UnityEngine;

public class TogglePlayerControlSample : MonoBehaviour
{
    private Behaviour playerController;
	private Animator playerAnimator;

	void Start()
	{
		playerController = FindObjectOfType<PlayerControllerInput>();
		playerAnimator = playerController.GetComponent<Animator>();
	}

	public void EnablePlayer()
	{
		playerController.enabled = true;
	}

	public void DisablePlayerControl()
	{
		playerController.enabled = false;
		playerAnimator.SetFloat("Forward", 0);
		playerAnimator.SetFloat("Turn", 0);
		playerAnimator.SetFloat("JumpLeg", 0);
	}

}