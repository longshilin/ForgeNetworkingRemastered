using BeardedManStudios.Forge.Networking.Unity;
using UnityEngine;

// Do note that this script is just a Monobehaviour
// but we have access to NetworkManager.Instance which is very helpful!
public class StartTrigger : MonoBehaviour
{
	private void Update()
	{
		// If the game started we will remove this trigger from the scene
		if (FindObjectOfType<GameBall>() != null)
			Destroy(gameObject);
	}

	private void OnTriggerEnter(Collider triggeringCollider)
	{
		// Only allow the server player to start the game
		// so the server is the owner of the ball
		// because if a client is the owner of the ball
		// and that client disconnects, the ball will be destroyed.
		if (!NetworkManager.Instance.IsServer)
			return;

		// We detect if the colliding gameobject has
		// the Component/Script Player.cs and if it doesn't have it
		// we simply don't do anything by using return;
		if (triggeringCollider.GetComponent<Player>() == null)
			return;


		// We need to create the ball on the network
		GameBall ball = NetworkManager.Instance.InstantiateGameBall() as GameBall;

		// Reset the ball position and give it a random velocity
		ball.Reset();

		// We destroy this trigger gameobject since we dont need it anymore.
		// This gameobject is destroyed only for the server.
		// However, it is destroyed for the client via Update() because
		// the ball is spawned.
		Destroy(gameObject);
	}
}
