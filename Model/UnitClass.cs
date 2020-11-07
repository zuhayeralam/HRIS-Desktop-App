namespace Human_Resource_Information_System.Model
{
    public class UnitClass : Event
    {
        public string UnitName { get; set; }
        public string ClassUnitCode { get; set; }
        public string Room { get; set; }
        public ClassType ClassType { get; set; }
        public int StaffID { get; set; }
    }
}
