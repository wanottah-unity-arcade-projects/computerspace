
using UnityEngine;

//
// Computer Space 1971 v2020.10.24
//
// created 2020.10.23
//


public class StarfieldController : MonoBehaviour
{
	private int maximumStars;

	private float minimumStarSize;

	//private float maximumStarSize;

	private float starfieldWidth;

	private float starfieldHeight;

	//public bool Colorize = false;

	private float xOffset;
	private float yOffset;

	ParticleSystem Particles;
	ParticleSystem.Particle[] Stars;


	private void Awake()
	{
		Initialise();

		Stars = new ParticleSystem.Particle[maximumStars];

		Particles = GetComponent<ParticleSystem>();
	}


    private void Initialise()
    {
		maximumStars = 1000;

		minimumStarSize = 0.05f;
		//maximumStarSize = 0.5f;

		starfieldHeight = 11f;
		starfieldWidth = 19f;

		// offset the coordinates to distribute the spread around the object's center
		xOffset = starfieldWidth * 0.5f;
		yOffset = starfieldHeight * 0.5f;                                                                                                       
    }


	public void CreateStarfield()
    {
		for (int i = 0; i < maximumStars; i++)
		{
			// randomize star size within parameters
			//float randSize = Random.Range(maximumStarSize, maximumStarSize + 1f);

			// if coloration is desired, color based on size
			//float scaledColor = (true == Colorize) ? randSize - maximumStarSize : 1f;

			Stars[i].position = GetRandomStarPosition(starfieldWidth, starfieldHeight) + transform.position;

			Stars[i].startSize = minimumStarSize; // * randSize;

			//Stars[i].startColor = new Color(1f, scaledColor, scaledColor, 1f);
			Stars[i].startColor = new Color(1f, 1f, 1f, 1f);
		}

		// write data to the particle system
		Particles.SetParticles(Stars, Stars.Length);
	}


	Vector3 GetRandomStarPosition(float width, float height)
	{
		float x = Random.Range(0, width);

		float y = Random.Range(0, height);

		return new Vector3(x - xOffset, y - yOffset, 0);
	}


} // end of class
