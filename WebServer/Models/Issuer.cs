﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace WebServer.Models
{
    [Table("issuer")]
    [PrimaryKey("Id")]
    public class Issuer
    {
        [Column("id")]
        [Key]
        [Required]
        public uint? Id { get; set; }

        [Column("uuid")]
        [Key]
        [Required]
        public Guid? UUID { get; set; }

        [Column("name")]
        [Key]
        [Required]
        public string Name { get; set; }

        [Column("city")]
        [Required]
        public string City { get; set; }

        [Column("area")]
        [Required]
        public string Area { get; set; }
    }
}