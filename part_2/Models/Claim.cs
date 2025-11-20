using System.ComponentModel.DataAnnotations;

namespace part_2.Models
{
    public class Claim
    {
        // int Id is going to be the primary key by the entity framework
        [Key]
        public int Id { get; set; }

        public required string FacultyName { get; set; }

        public required string ModuleName { get; set; }

        public required int Hours { get; set; }

        public required int Rate { get; set; }

        public required int TotalAmount { get; set; }

        public string SupportingDocuments { get; set; }

        public required string Status { get; set; } = "Pending";
    }
}
