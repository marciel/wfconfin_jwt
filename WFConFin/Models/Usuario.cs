using System;
using System.ComponentModel.DataAnnotations;

namespace WFConFin.Models
{
	public class Usuario
	{
        [Key]
        public Guid id { get; set; }

        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(100, ErrorMessage = "Nome deve ter até 100 caracteres")]
        public string nome { get; set; }

        [Required(ErrorMessage = "Login é obrigatório")]
        [StringLength(100, ErrorMessage = "Login deve ter até 20 caracteres")]
        public string login { get; set; }

        [Required(ErrorMessage = "Senha é obrigatório")]
        [StringLength(100, MinimumLength = 4, ErrorMessage = "Senha deve ter entre 4 e 20 caracteres")]
        public string password { get; set; }

        [Required(ErrorMessage = "Função é obrigatório")]
        [StringLength(20, ErrorMessage = "Função deve ter até 20 caracteres")]
        public string funcao { get; set; }

	}
}

