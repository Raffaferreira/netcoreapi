using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class ApplicationSetup
    {
        public string? DisplayMessage { get; set; }

        public TokenConfigurations? tokenConfigurations { get; set; }

        public ConnectionStrings? ConnectionStrings { get; set; }
    }
}
