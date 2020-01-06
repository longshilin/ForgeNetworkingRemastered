﻿using System.Net;
using Forge.Networking.Messaging;

namespace Forge.Networking.Unity.Messages.Interpreters
{
	public class SpawnEntityInterpreter : IMessageInterpreter
	{
		public static SpawnEntityInterpreter Instance { get; private set; } = new SpawnEntityInterpreter();

		public bool ValidOnClient => true;
		public bool ValidOnServer => false;

		public void Interpret(INetworkMediator netMediator, EndPoint sender, IMessage message)
		{
			var msg = (SpawnEntityMessage)message;
			IEngineFacade engine = (IEngineFacade)netMediator.EngineProxy;
			EntitySpawnner.SpawnEntityFromMessage(engine, msg);
		}
	}
}