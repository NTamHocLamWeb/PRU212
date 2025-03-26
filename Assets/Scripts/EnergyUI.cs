using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyUI : MonoBehaviour
{
	public GameEvent playerDamagedEvent;
	public Image energyImage;
	[SerializeField] public Sprite energy6;
	[SerializeField] public Sprite energy5;
	[SerializeField] public Sprite energy4;
	[SerializeField] public Sprite energy3;
	[SerializeField] public Sprite energy2;
	[SerializeField] public Sprite energy1;
    [SerializeField] public Sprite energy0;


    private void OnEnable()
	{
		playerDamagedEvent.OnEventRaised += UpdateHealthUI;
	}

	private void OnDisable()
	{
		playerDamagedEvent.OnEventRaised -= UpdateHealthUI;
	}

	public void UpdateHealthUI(int energy)
	{
		if (energy == 6)
		{
			energyImage.sprite = energy6;
		}
		else if (energy == 5)
		{
			energyImage.sprite = energy5;
		}
		else if (energy == 4)
		{
			energyImage.sprite = energy4;
		}
		else if (energy == 3)
		{
			energyImage.sprite = energy3;
		}
		else if (energy == 2)
		{
			energyImage.sprite = energy2;
		}
        else if (energy == 1)
        {
            energyImage.sprite = energy1;
        }
        else
		{
			energyImage.sprite = energy0;
		}
	}
}
