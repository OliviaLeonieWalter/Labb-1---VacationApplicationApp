using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DatabaseHandler.Models
{
    public class VacApplication
    {
        [Key]
        public int applicationID { get; set; }
        [Required]
        public string VacationType { get; set; }
        [Required]

        public DateTime VacStartDate { get; set; }
        [Required]

        public DateTime VacEndDate { get; set; }

        public DateTime ApplicationSubmitDate { get; set; }

        public int EmployeeId { get; set; }
        public Employee employee { get; set; }
        
    }
}
