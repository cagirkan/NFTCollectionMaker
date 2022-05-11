using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Concrete
{
    public class ContextSettings
    {
        public static IConfiguration Configuration { get; set; }
        public static string ConnectionString { get; set; }
        public static string JWTKey { get; set; }
    }
}
