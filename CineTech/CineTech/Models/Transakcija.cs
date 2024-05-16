﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CineTech.Models
{
    public abstract class Transakcija
    {
        [Key]
        public int id { get; set; }
        public DateOnly datum { get; set; }
        public TimeOnly vrijeme { get; set; }
        [ForeignKey("Korisnik")]
        public String KorisnikId { get; set; }
        public Korisnik Korisnik { get; set; }
        [ForeignKey("ZauzetaSjedista")]
        public int ZauzetaSjedistaId {  get; set; }
        public ZauzetaSjedista ZauzetaSjedista { get; set; }
        public Transakcija() { }
    }
}