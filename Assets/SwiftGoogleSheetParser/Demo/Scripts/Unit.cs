using System;

namespace SwiftGoogleSheetParser.Demo
{
    [Serializable]
    public class Unit : IUniqueItem
    {
        public string Id;
        public int Hp;
        public float Damage;
        public Ability Ability;
		
        public string UniqueId
        {
            get { return Id; }
        }
    }
}