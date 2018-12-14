using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SwiftGoogleSheetParser.Demo
{
	[CreateAssetMenu(menuName = "Managers/UnitManager", fileName = "UnitManager")]
	public class UnitManager : SheetDictionarySingleton<Unit, UnitManager>
	{
		static readonly ICustomStringParser[] Parsers = 
		{
			new AbilityParser()
		};
		
		protected override ICustomStringParser[] GetCustomParsers()
		{
			return Parsers;
		}

		public override void OnItemsDownloaded(List<Unit> items)
		{
			foreach (var item in items)
			{
				Debug.Log("Unit with id " + item.Id + " downloaded!");
			}
		}

		protected override string ResourcesFolder
		{
			get { return "Configs/UnitManager"; }
		}
	}
}
