using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
	public Text currentObjective;
	public Text statusMessage;
	public AudioClip btnSound;
	public Button continueButton;
	public Image arrow;
	public Image arrow2;
	public TutorialShape_AI aiPlayer;
	public Player mainPlayer;
	public Canvas tutorialBox;
	public TutorialOrders tutorialInstructions;
	private Enums.TutorialStage stage = Enums.TutorialStage.Intro;
	private List<Orders> orderList;
	private Queue<string> sentaces = new Queue<string>();
	private DigitalRubyShared.FingersScript touchScript;
	public Text tutorialDialouge;
	public Text orderTitle;
    
	// Use this for initialization
	void Start()
	{
		arrow.gameObject.SetActive(false);
		arrow2.gameObject.SetActive(false);
		orderList = tutorialInstructions.orderList;
		aiPlayer.enabled = false;
		touchScript = mainPlayer.GetComponent<DigitalRubyShared.FingersScript>();
		Tutorial();

	}

	// Update is called once per frame
	void Update()
	{
		if (mainPlayer.score > 0 && (int)stage < (int)Enums.TutorialStage.CapturedTiles)
		{
			stage = Enums.TutorialStage.CapturedTiles;
			toggletutorialBox();
			Tutorial();
		}
		if(stage == Enums.TutorialStage.CapturedTiles)
		{
			currentObjective.text =  mainPlayer.score + " out of 150";
		}
		if (mainPlayer.score > 150 && stage == Enums.TutorialStage.CapturedTiles)
		{
			stage = Enums.TutorialStage.GettingStunned;
			toggletutorialBox();
			Tutorial();
		}
		if(mainPlayer.canMove == false && stage == Enums.TutorialStage.GettingStunned)
		{
			stage = Enums.TutorialStage.OtherPlayers;
			toggletutorialBox();
			Tutorial();
		}
		if(aiPlayer.canMove == false && stage == Enums.TutorialStage.Stun) 
		{
			Debug.Log("this isnt going through");
			stage = Enums.TutorialStage.SweetRevenge;
			toggletutorialBox();
			Tutorial();
		}
		if(mainPlayer.canMove == false && stage == Enums.TutorialStage.Collison)
		{
			stage = Enums.TutorialStage.StatusBar;
			toggletutorialBox();
			Tutorial();
		}
		if (tutorialBox.enabled == true)
			touchScript.enabled = false;
		else
			touchScript.enabled = true;
	}

	private void Tutorial()
	{
		switch (stage)
		{
			case Enums.TutorialStage.Intro:				
				playOrder((int)stage);
				break;
			case Enums.TutorialStage.CapturedTiles:
				playOrder((int)stage);
				break;
			case Enums.TutorialStage.GettingStunned:
				aiPlayer.enabled = true;
				playOrder((int)stage);
				//Time.timeScale = 0.2f;
				break;
			case Enums.TutorialStage.OtherPlayers:
				playOrder((int)stage);
				//Time.timeScale = 0.2f;
				break;
			case Enums.TutorialStage.Stun:

				playOrder((int)stage);
				aiPlayer.Move(mainPlayer.shapeColor);
				break;
			case Enums.TutorialStage.SweetRevenge:
				playOrder((int)stage);
				arrow.gameObject.SetActive(true);
				break;
			case Enums.TutorialStage.Collison:
				playOrder((int)stage);
				aiPlayer.Move(mainPlayer.shapeColor);
				//Time.timeScale = 0.3f;
				mainPlayer.stunDisabled = true;
				break;
			case Enums.TutorialStage.StatusBar:
				Time.timeScale = 1;
				playOrder((int)stage);
				arrow.gameObject.SetActive(false);
				arrow2.gameObject.SetActive(true);
				statusMessage.text = "I'll tell you if you're doing something wrong";
				statusMessage.enabled = true;
				break;
			case Enums.TutorialStage.EndTutorial:
				arrow2.gameObject.SetActive(false);
				playOrder((int)stage);
				break;			
			default:
				break;
		}
	}

    public void playOrder( int currentStage)
	{
		orderList[currentStage].LoadOrders(sentaces);
		orderTitle.text = orderList[currentStage].orderName;
		tutorialDialouge.text = sentaces.Dequeue();
		if (tutorialInstructions.orderList[currentStage].currentObjective != string.Empty)
			currentObjective.text = tutorialInstructions.orderList[currentStage].currentObjective;

	}

	public void ContinueButton()
	{
		SoundManager.Instance.EffectsSource.PlayOneShot(btnSound);
		if(sentaces.Count != 0)
		{
			tutorialDialouge.text = sentaces.Dequeue();
		}			
		else if(ContinueDialog())
		{
			Tutorial();
		}		    
		else if(stage == Enums.TutorialStage.EndTutorial && sentaces.Count == 0)
		{
			continueButton.GetComponentInChildren<Text>().text = "Main Menu";
			SceneManager.LoadScene((int)Enums.Scenes.Start);
		}
		else
		{
			toggletutorialBox();
		}

	}

	//public void TutorialMarkerPressed()
	//{
	//	teachMarker.enabled = false;
	//	stage = Enums.TutorialStage.Expanding;
	//	Tutorial();
	//}
    
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
   //         if((int)stage < 10)
			//Time.timeScale = 1;
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
	public string[] tutorialText;

	public bool continueInstructions = false;

	public string currentObjective;

	public void LoadOrders(Queue<string> sentaces)
	{
		foreach(string sentace in tutorialText)
		{
			sentaces.Enqueue(sentace);
		}
	}

}




