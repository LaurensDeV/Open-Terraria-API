﻿using OTAPI.Patcher.Engine.Extensions;
using OTAPI.Patcher.Engine.Modification;
using System.Linq;

namespace OTAPI.Patcher.Engine.Modifications.Hooks.Item
{
	public class SetDefaultsByName : ModificationBase
	{
		public override System.Collections.Generic.IEnumerable<string> AssemblyTargets => new[]
		{
			"TerrariaServer, Version=1.3.3.3, Culture=neutral, PublicKeyToken=null",
			"Terraria, Version=1.3.3.1, Culture=neutral, PublicKeyToken=null"
		};
		public override string Description => "Hooking Item.SetDefaults(string)...";
		public override void Run()
		{
			var vanilla = this.SourceDefinition.Type("Terraria.Item").Methods.Single(
				x => x.Name == "SetDefaults"
				&& x.Parameters.First().ParameterType == this.SourceDefinition.MainModule.TypeSystem.String
			);

			var cbkBegin = this.ModificationDefinition.Type("OTAPI.Callbacks.Terraria.Item").Method("SetDefaultsByNameBegin", parameters: vanilla.Parameters);
			var cbkEnd = this.ModificationDefinition.Type("OTAPI.Callbacks.Terraria.Item").Method("SetDefaultsByNameEnd", parameters: vanilla.Parameters);

			vanilla.Wrap
			(
				beginCallback: cbkBegin,
				endCallback: cbkEnd,
				beginIsCancellable: true,
				noEndHandling: false,
				allowCallbackInstance: true
			);
		}
	}
}
