using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
	public Text statusMessage;
	public AudioClip btnSound;
	public Button continueButton;
	public Image arrow;
	public Image arrow2;
	public Text teachMarker;
	public TutorialShape_AI aiPlayer;
	public Player mainPlayer;
	public Canvas tutorialBox;
	public TutorialOrders tutorialInstructions;
	private Enums.TutorialStage stage = Enums.TutorialStage.Intro;
	public Text tutorialDialouge;
	public Text orderTitle;
    
	// Use this for initialization
	void Start()
	{
		arrow.gameObject.SetActive(false);
		arrow2.gameObject.SetActive(false);
		Tutorial();

	}

	// Update is called once per frame
	void Update()
	{
		if (mainPlayer.score > 0 && (int)stage < (int)Enums.TutorialStage.CapturedTiles)
		{
			teachMarker.enabled = false;
			stage = Enums.TutorialStage.CapturedTiles;
			toggletutorialBox();
			Tutorial();
		}
		if(mainPlayer.canMove == false && stage == Enums.TutorialStage.CapturedTiles)
		{
			stage = Enums.TutorialStage.GettingStunned;
			toggletutorialBox();
			Tutorial();
		}
		if(aiPlayer.canMove == false && stage == Enums.TutorialStage.Stun) 
		{
			stage = Enums.TutorialStage.SweetRevenge;
			toggletutorialBox();
			Tutorial();
		}
		if(mainPlayer.canMove == false && stage == Enums.TutorialStage.StatusBar)
		{
			stage = Enums.TutorialStage.EndTutorial;
			toggletutorialBox();
			Tutorial();
		}
	}

	private void Tutorial()
	{
		switch (stage)
		{
			case Enums.TutorialStage.Intro:
				playOrder((int)stage);
				break;
			case Enums.TutorialStage.Expanding:
				tutorialBox.enabled = true;
				Time.timeScale = 0.2f;
				playOrder((int)stage);
				break;
			case Enums.TutorialStage.Filling:
				playOrder((int)stage);
				break;
			case Enums.TutorialStage.CapturedTiles:
				playOrder((int)stage);
				break;
			case Enums.TutorialStage.GettingStunned:
				playOrder((int)stage);
				Time.timeScale = 0.2f;
				break;
			case Enums.TutorialStage.OtherPlayers:
				playOrder((int)stage);
				Time.timeScale = 0.2f;
				break;
			case Enums.TutorialStage.Stun:
				aiPlayer.Move();
				playOrder((int)stage);
				break;
			case Enums.TutorialStage.SweetRevenge:
				playOrder((int)stage);
				arrow.gameObject.SetActive(true);
				break;
			case Enums.TutorialStage.Collison:
				playOrder((int)stage);
				break;
			case Enums.TutorialStage.StatusBar:
				playOrder((int)stage);
				arrow.gameObject.SetActive(false);
				arrow2.gameObject.SetActive(true);
				statusMessage.text = "I'll tell you if you're doing something wrong";
				statusMessage.enabled = true;
				aiPlayer.Move();
				Time.timeScale = 0.2f;
				break;
			case Enums.TutorialStage.EndTutorial:
				arrow2.gameObject.SetActive(false);
				playOrder((int)stage);
				continueButton.GetComponentInChildren<Text>().text = "Main Menu";
				break;			
			default:
				break;
		}
	}

    public void playOrder( int currentStage)
	{
		orderTitle.text = tutorialInstructions.orderList[currentStage].orderName;
		tutorialDialouge.text = tutorialInstructions.orderList[currentStage].tutorialText;
	}

	public void ContinueButton()
	{
		SoundManager.Instance.EffectsSource.PlayOneShot(btnSound);
		if (ContinueDialog())
			Tutorial();
		else if(stage == Enums.TutorialStage.EndTutorial)
		{
			continueButton.GetComponentInChildren<Text>().text = "Main Menu";
			SceneManager.LoadScene((int)Enums.Scenes.Start);
		}
		else
		toggletutorialBox();
	}

	public void TutorialMarkerPressed()
	{
		teachMarker.enabled = false;
		stage = Enums.TutorialStage.Expanding;
		Tutorial();
	}
    
	private bool ContinueDialog()
	{
		
		if (tutorialInstructions.orderList[(int)stage].continueInstructions == true)
		{
			stage++;
			return true;
		}
		else
			return false;	    
    }

	private void toggletutorialBox()
	{
		if (tutorialBox.enabled == true)
		{
			tutorialBox.enabled = false;
            if((int)stage < 10)
			Time.timeScale = 1;
		}
		else
			tutorialBox.enabled = true;
	}

}

[System.Serializable]
public class Orders
{


	public string orderName;

	[TextArea]
	public string tutorialText;

	public bool continueInstructions = false;
       

	public void playOrder()
	{

	}

}




