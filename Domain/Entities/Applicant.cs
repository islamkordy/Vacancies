namespace Domain.Entities
{
    public partial class Applicant
    {
        public int Id { get; set; }
        public string NameEn { get; set; } = null!;
        public string NameAr { get; set; } = null!;
        public int Age { get; set; }
        public Guid UserAccountId { get; set; }

        public virtual UserAccount UserAccount { get; set; }
        public ICollection<Vacancy> Vacancies { get; set; }
        //public virtual ICollection<VacancyApplicant> VacancyApplicants { get; set; }
    }
}
