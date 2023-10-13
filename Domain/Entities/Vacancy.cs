using Domain.Common;

namespace Domain.Entities
{
    public partial class Vacancy
    {
        public int Id { get; set; }
        public string TitleEn { get; set; } = null!;
        public string TitleAr { get; set; } = null!;
        public string DescriptionEN { get; set; } = null!;
        public string DescriptionAr { get; set; } = null!;
        public int MaxNumberOfApplicants { get; set; }
        public bool Status { get; set; }
        public DateTime ExpiryDate { get; set; }
        public virtual int EmployerId { get; set; }

        public virtual Employer Employer { get; set; }
        public ICollection<Applicant> Applicants { get; set; }
        //public virtual ICollection<VacancyApplicant> VacancyApplicants { get; set; }
    }
}
