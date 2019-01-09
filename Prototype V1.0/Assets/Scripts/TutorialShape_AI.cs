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
		while(true)
		{
			this.transform.localScale += new Vector3(scaleRate, scaleRate) * scaleSpeed;
            yield return new WaitForFixedUpdate();
		}
       
       
	}

	protected override IEnumerator Stun()
	{
		StopAllCoroutines();
		canMove = false;
        this.transform.localScale = startingScale;
        this.fillShape.SetActive(false);
		isShapeActive(false);
        yield return new WaitForSeconds(collisionStunTime);
        canMove = true;
	}

	public void Move(Color playerColor)
	{
		foreach (GameObject tile in grid)
		{
			GetTileInfo(tile);
			if (tileColor == playerColor && isPlayerDetected(tilePosition, detectionRadius) == null)
			{
				this.transform.position = (Vector2)tile.transform.position;
				break;
			}
				
		}
		StartCoroutine(Expand());			
	}

	protected override void BeingGreedy(Shape hitPlayer)
	{
		base.BeingGreedy(hitPlayer);
	}
}
