using System;
using System.ComponentModel.DataAnnotations;

namespace WFConFin.Models
{
	public class UsuarioLogin
	{
        [Required(ErrorMessage = "Login é obrigatório")]
        [StringLength(100, ErrorMessage = "Login deve ter até 20 caracteres")]
        public string login { get; set; }

        [Required(ErrorMessage = "Senha é obrigatório")]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "Senha deve ter entre 4 e 20 caracteres")]
        public string password { get; set; }

    }
}

