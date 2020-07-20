// We use this namespace as it is where our BallBehavior was generated

using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking.Unity;
using UnityEngine;

// We extend GameBallBehavior which extends NetworkBehavior which extends MonoBehaviour
public class GameBall : GameBallBehavior
{
	private Rigidbody rigidbodyRef;
	private JumpStartGuideGameLogic gameLogic;

	private void Awake()
	{
		rigidbodyRef = GetComponent<Rigidbody>();
		gameLogic = FindObjectOfType<JumpStartGuideGameLogic>();
	}

	// Default Unity update method
	private void Update()
	{
		// Check to see if we are the owner of this ball
		if (!networkObject.IsOwner)
		{
			// If we are not the owner then we set the position to the
			// position that is syndicated across the network
			// for this ball
			transform.position = networkObject.position;
			return;
		}

		// Registers and syndicates transform.position
		// across the network, on the next update pass
		networkObject.position = transform.position;
	}

	private void OnCollisionEnter(Collision triggeringCollision)
	{
		// We are making this authoritative by only
		// allowing the server to call it
		if (!networkObject.IsServer)
			return;

		// Only continue, if a player touches the ball
		// otherwise normal collision/bounciness happens.
		if (triggeringCollision.gameObject.GetComponent<Player>() == null)
			return;

		// **Call an RPC from gameLogic** to print the player's name
		// as the last player to touch the ball
		gameLogic.networkObject.SendRpc(
			GameLogicBehavior.RPC_PLAYER_SCORED,
			Receivers.All,
			triggeringCollision.gameObject.GetComponent<Player>().Name
		);

		// Reset the ball
		Reset();
	}

	/* Minor note on this function:
	/// Check out the invokes and references of this function.
	/// This function is called always from a server.
	///
	/// The velocity or force dont matter for other clients
	/// since the server relays the **position** across the network.
	/// And we only really care about the position.
	*/
	public void Reset()
	{
		// Move the ball to 0, 10, 0
		transform.position = Vector3.up * 10;

		// Reset the velocity for this object to zero
		rigidbodyRef.velocity = Vector3.zero;

		// Create a random force to apply to this object
		// between 300 to 500 or -300 to -500
		Vector3 force = new Vector3(0, 0, 0);
		force.x = Random.Range(300, 500);
		force.z = Random.Range(300, 500);

		// 50% chance to make the force.x inverted/negative.
		if (Random.value < 0.5f)
			force.x *= -1;

		// 50% chance to make the force.z inverted/negative.
		if (Random.value < 0.5f)
			force.z *= -1;

		// Add the random force to the ball
		rigidbodyRef.AddForce(force);
	}
}
