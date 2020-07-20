// We use this namespace as it is where our GameLogicBehavior was generated

using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking.Unity;
using UnityEngine;
using UnityEngine.UI;

// We extend GameLogicBehavior which extends NetworkBehavior which extends MonoBehaviour
public class JumpStartGuideGameLogic : GameLogicBehavior
{
	public Text scoreLabel;

	private void Start()
	{
		// This will be called on every client so each client
		// will essentially instantiate their own player on the network.
		// We also pass in the position we want them to spawn at
		NetworkManager.Instance.InstantiatePlayer(position: new Vector3(0, 5, 0));
	}

	// Override the abstract RPC method that we made in the NCW
	public override void PlayerScored(RpcArgs args)
	{
		// Since there is only 1 argument and it is a string we can safely
		// cast the first argument to a string knowing that it is going to
		// be the name for the scoring player
		string playerName = args.GetNext<string>();

		// Update the UI to show the last player that scored
		scoreLabel.text = "Last player to score was: " + playerName;
	}
}
