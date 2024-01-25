namespace AccountingSystem.Models.AccountDbModels
{
    public class ContactPerson
    {
        public int Id { get; set; }
        public int CId { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }
        public string PType { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
