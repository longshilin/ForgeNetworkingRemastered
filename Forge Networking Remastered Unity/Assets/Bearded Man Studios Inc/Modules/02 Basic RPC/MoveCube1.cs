using UnityEngine;
using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Unity;
public class MoveCube1 : MoveCube1Behavior
{
	private void Update()
	{
		// Move the cube up in world space if the up arrow was pressed
		if (Input.GetKeyDown(KeyCode.UpArrow))
			networkObject.SendRpc(RPC_MOVE_UP, Receivers.All);

		// Move the cube down in world space if the down arrow was pressed
		else if (Input.GetKeyDown(KeyCode.DownArrow))
			networkObject.SendRpc(RPC_MOVE_DOWN, Receivers.All);
	}

	/// <summary>
	/// Used to move the cube that this script is attached to up
	/// </summary>
	/// <param name="args">null</param>
	public override void MoveUp(RpcArgs args)
	{
		// RPC calls are not made from the main thread for performance, since we
		// are interacting with Unity engine objects, we will need to make sure
		// to run the logic on the main thread
		MainThreadManager.Run(() =>
		{
			transform.position += Vector3.up;
		});
	}

	/// <summary>
	/// Used to move the cube that this script is attached to down
	/// </summary>
	/// <param name="args">null</param>
	public override void MoveDown(RpcArgs args)
	{
		// RPC calls are not made from the main thread for performance, since we
		// are interacting with Unity engine objects, we will need to make sure
		// to run the logic on the main thread
		MainThreadManager.Run(() =>
		{
			transform.position += Vector3.down;
		});
	}
}
