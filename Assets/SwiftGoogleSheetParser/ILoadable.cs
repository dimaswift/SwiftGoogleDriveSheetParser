using System;
using System.Collections;
using System.Collections.Generic;

namespace SwiftGoogleSheetParser
{
	public interface ILoadable
	{
		void Load(Action onLoad);
	}
}
