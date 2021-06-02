using Dal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebProject.Common
{
	public static class DbConfig
	{
		public static void AddDb(this IServiceCollection services, IConfiguration config)
		{
			services.AddDbContext<MyContext>(opts =>
				opts.UseSqlServer(config.GetConnectionString("Default"))
			);
		}
	}
}
