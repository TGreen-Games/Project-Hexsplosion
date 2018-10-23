using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialShape_AI : Shape_AI
{
	
	protected override void Start()
	{
		startingScale = this.transform.localScale;
        capturedTiles = new List<GameObject>();
        shotColor = stunShot.GetComponent<RFX4_EffectSettingColor>();
        collisionColor = collisionStun.GetComponent<RFX4_EffectSettingColor>();
        coolDown.color = shapeColor;
        stunText.enabled = false;
		generateRandomNum = new System.Random(System.Environment.TickCount + this.gameObject.GetHashCode());
        grid = new List<GameObject>();
        grid.AddRange(GameObject.FindGameObjectsWithTag("Tile"));
        actionindicator.AiState = Enums.AiStage.Neutral;
		greedLimiter = 0.3f;
		detectionRadius = 2;
	}

	private void Update()
	{
		if (priorityTarget != null && canShoot)
        {
			BeingGreedy(priorityTarget);
            StartCoroutine(cooldownTimer(shotCooldown));
            canShoot = false;
            priorityTarget = null;
        }
	}
	protected override IEnumerator Expand()
	{
		while(canMove)
		{
			this.transform.localScale += new Vector3(scaleRate, scaleRate) * scaleSpeed;
            yield return new WaitForFixedUpdate();
		}
       
       
	}

	protected override IEnumerator Stun()
	{
		canMove = false;
        stunText.transform.position = this.transform.position;
        stunText.enabled = true;
        this.transform.localScale = startingScale;
        this.fillShape.SetActive(false);
        yield return new WaitForSeconds(collisionStunTime);
        stunText.enabled = false;
        canMove = true;
	}

	public void Move()
	{
		if(FoundRandomTile())
		StartCoroutine(Expand());
	}

	protected override void BeingGreedy(Shape hitPlayer)
	{
		base.BeingGreedy(hitPlayer);
	}
}
