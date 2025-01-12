using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CoWorkApi.Domain.Entities
{
    public class User
    {
        [Required]
        public int Id { get; set; } // Clave primaria
        [Required]
        public string Username { get; set; } = string.Empty; // Nombre de usuario único
        [Required]
        public string Password { get; set; } = string.Empty; // Contraseña
        [Required]
        public string Email { get; set; } = string.Empty; // Correo electrónico único
        public string Role { get; set; } = "user"; // Rol ('admin', 'user')
        [Required]
        public string Name {get; set;} = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Fecha de creación

        // Relación con las reservas realizadas por el usuario
        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}