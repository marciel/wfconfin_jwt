using System;
using System.Security.Cryptography;
using System.Text;

namespace WFConFin.Services
{
	public class MD5Hash
	{
		public static string CalcHash(string valor)
		{
			try
			{
				var md5 = MD5.Create();
				var inputBytes = Encoding.ASCII.GetBytes(valor);
				var hash = md5.ComputeHash(inputBytes);
				var sb = new StringBuilder();
				for(int i = 0; i < hash.Length; i++)
				{
					sb.Append(hash[i].ToString("X2"));
				}

				return sb.ToString();
			}catch(Exception e)
			{
				return null;
			}
		}
	}
}

