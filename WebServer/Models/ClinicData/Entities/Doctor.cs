using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WebServer.Models.UserData;
using Microsoft.EntityFrameworkCore;

namespace WebServer.Models.ClinicData.Entities
{
    [Table("doctor")]
    public class Doctor : User
    {
        [Required]
        [ForeignKey("DoctorUUID")]
        public List<Notes> Notes { get; set; } = new List<Notes>();

        [Required]
        [ForeignKey("DoctorUUID")]
        public List<Prescription> Prescriptions { get; set; } = new List<Prescription>();

        [Required]
        [ForeignKey("DoctorUUID")]
        public List<Schedule> Schedules { get; set; } = new List<Schedule>();

        [Column("area_of_expertise")]
        [StringLength(100)]
        public string AreaOfExpertise { get; set; }

        [Column("room_number")]
        [Range(101, 999)]
        public int RoomNumber { get; set; }
    }
}
