using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceTracker.Models
{
    public class LoginRequestDto
    {
        public string EmailorUsername { get; set; }
        public string Password { get; set; }
    }
}
