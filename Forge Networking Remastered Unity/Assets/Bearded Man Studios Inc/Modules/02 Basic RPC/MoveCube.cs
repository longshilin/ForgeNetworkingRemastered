using UnityEngine;
using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Unity;

public class MoveCube : MoveCubeBehavior
{
	private void Update()
	{
		// Move the cube up in world space if the up arrow was pressed
		if (Input.GetKeyDown(KeyCode.UpArrow))
			networkObject.SendRpc(RPC_MOVE, Receivers.All, Vector3.up);

		// Move the cube down in world space if the down arrow was pressed
		else if (Input.GetKeyDown(KeyCode.DownArrow))
			networkObject.SendRpc(RPC_MOVE, Receivers.All, Vector3.down);
	}

	/// <summary>
	/// Used to move the cube that this script is attached to
	/// </summary>
	/// <param name="args">
	/// [0] Vector3 The direction/distance to move this cube by
	/// </param>
	public override void Move(RpcArgs args)
	{
		// RPC calls are not made from the main thread for performance, since we
		// are interacting with Unity engine objects, we will need to make sure
		// to run the logic on the main thread
		MainThreadManager.Run(() =>
		{
			transform.position += args.GetNext<Vector3>();
		});
	}
}
