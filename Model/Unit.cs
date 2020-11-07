using System.Collections.Generic;

namespace Human_Resource_Information_System.Model
{
    public class Unit
    {
        public string Code { get; set; }
        public string Title { get; set; }
        public int UnitCoordinatorID { get; set; }
        public string UnitNameCode { get { return ToString(); } }
        public List<UnitClass> UnitClasses { get; set; }
        public override string ToString()
        {
            return $"{Code}{Title}";
        }
    }
}
