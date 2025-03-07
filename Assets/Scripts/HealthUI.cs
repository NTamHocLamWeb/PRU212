using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
	public GameEvent playerDamagedEvent;
	public Image healthImage;
	[SerializeField] public Sprite life5;
	[SerializeField] public Sprite life4;
	[SerializeField] public Sprite life3;
	[SerializeField] public Sprite life2;
	[SerializeField] public Sprite life1;


	private void OnEnable()
	{
		playerDamagedEvent.OnEventRaised += UpdateHealthUI;
	}

	private void OnDisable()
	{
		playerDamagedEvent.OnEventRaised -= UpdateHealthUI;
	}

	public void UpdateHealthUI(int health)
	{
		if (health == 5)
		{
			healthImage.sprite = life5;
		}
		else if (health == 4)
		{
			healthImage.sprite = life4;
		}
		else if (health == 3)
		{
			healthImage.sprite = life3;
		}
		else if (health == 2)
		{
			healthImage.sprite = life2;
		}
		else
		{
			healthImage.sprite = life1;
		}
	}
}
