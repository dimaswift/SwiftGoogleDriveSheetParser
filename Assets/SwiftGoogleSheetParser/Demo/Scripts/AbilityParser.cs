using System;

namespace SwiftGoogleSheetParser.Demo
{
    public class AbilityParser : ICustomStringParser
    {
        public bool TryParse(string field, out object result)
        {
            Ability ability = new Ability();
            result = ability;
            if (string.IsNullOrEmpty(field))
                return false;
		
            var values = field.Split('-');
            if (values.Length < 2)
                return false;
            int.TryParse(values[0], out ability.Value);
            ability.Id = values[1];
            return true;
        }

        public bool IsOfType(Type type)
        {
            return typeof(AbilityParser) == type;
        }
    }
}