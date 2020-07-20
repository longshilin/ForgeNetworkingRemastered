// We use this namespace as it is where our PlayerBehavior was generated

using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

// We extend PlayerBehavior which extends NetworkBehavior which extends MonoBehaviour
public class Player : PlayerBehavior
{
	// These strings are to be used to construct a player's name
	// by randomly combining 2 strings
	private string[] nameParts = new string[] {"crazy", "cat", "dog", "homie", "bobble", "mr", "ms", "mrs", "castle", "flip", "flop"};

	public string Name { get; private set; }


	// NetworkStart() is **automatically** called, when a networkObject
	// has been fully setup on the network and ready/finalized on the network!
	// In simpler words, think of it like Unity's Start() but for the network ;)
	protected override void NetworkStart()
	{
		base.NetworkStart();

		// If this networkObject is actually the **enemy** Player
		// hence not the one we will control and own
		if (!networkObject.IsOwner)
		{
			// Don't render through a camera that is not ours
			// Don't listen to audio through a listener that is not ours
			transform.GetChild(0).gameObject.SetActive(false);

			// Don't accept inputs from objects that are not ours
			GetComponent<FirstPersonController>().enabled = false;

			// There is no reason to try and simulate physics since
			// the position is being sent across the network anyway
			Destroy(GetComponent<Rigidbody>());
		}

		// Assign the name when this object is setup on the network
		ChangeName();
	}

	public void ChangeName()
	{
		// Only the owning client of this object can assign the name
		if (!networkObject.IsOwner)
			return;

		// Get a random index for the first name
		int first = Random.Range(0, nameParts.Length - 1);
		// Get a random index for the last name
		int last = Random.Range(0, nameParts.Length - 1);

		// Assign the name to the random selection
		Name = nameParts[first] + " " + nameParts[last];

		// Send an RPC to let everyone know what the name is for this player
		// We use "AllBuffered" so that if people come late they will get the
		// latest name for this object
		// We pass in "Name" for the args because we have 1 argument that
		// is to be a string as it is set in the NCW
		networkObject.SendRpc(RPC_UPDATE_NAME, Receivers.AllBuffered, Name);
	}

	// Default Unity update method
	private void Update()
	{
		// Check to see if we are NOT the owner of this player
		if (!networkObject.IsOwner)
		{
			// Set this object's transform.position
			// to the position that is syndicated across the network
			// In simpler words, its position is updated via the network.
			transform.position = networkObject.position;
			return;
		}

		// When our position changes, the networkObject.position
		// will detect the change based on this assignment automatically
		// and this data will then be syndicated across the network
		// on the next update pass for this networkObject.
		// In simpler words, our local position (transform.position) is
		// registered and shared onto the network -> see above if
		networkObject.position = transform.position;
	}

	// Override the abstract RPC method that we made in the NCW
	public override void UpdateName(RpcArgs args)
	{
		// Since there is only 1 argument and it is a string we can safely
		// cast the first argument to a string, knowing that it is going to
		// be the name for this player
		Name = args.GetNext<string>();
	}
}
