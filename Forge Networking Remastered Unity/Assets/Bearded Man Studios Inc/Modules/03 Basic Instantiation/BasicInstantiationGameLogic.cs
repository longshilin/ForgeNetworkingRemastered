using UnityEngine;
using BeardedManStudios.Forge.Networking.Unity;

public class BasicInstantiationGameLogic : MonoBehaviour
{
	private void Start()
	{
		NetworkManager.Instance.InstantiatePlayerCube();
	}
}
