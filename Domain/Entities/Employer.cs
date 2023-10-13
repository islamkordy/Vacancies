namespace Domain.Entities
{
    public partial class Employer
    {
        public Employer()
        {
            Vacancies = new HashSet<Vacancy>();
        }

        public int Id { get; set; }
        public string NameEn { get; set; } = null!;
        public string NameAr { get; set; } = null!;
        public int Age { get; set; }
        public Guid UserAccountId { get; set; }

        public virtual UserAccount UserAccount { get; set; }
        public virtual ICollection<Vacancy> Vacancies { get; set; }
    }
}
