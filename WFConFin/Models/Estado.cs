using System;
using System.ComponentModel.DataAnnotations;

namespace WFConFin.Models
{
	public class Estado
	{
		[Key]
		public string sigla { get; set; }

		[Required(ErrorMessage = "Nome é obrigatório")]
		[StringLength(100,ErrorMessage ="Nome deve ter até 100 caracteres")]
		public string nome { get; set; }


		public Estado()
		{
		}
	}
}

