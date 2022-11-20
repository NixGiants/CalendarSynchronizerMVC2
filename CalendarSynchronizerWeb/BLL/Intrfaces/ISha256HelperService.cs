using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Intrfaces
{
    public interface ISha256HelperService
    {
        public string ComputeHash(string codeVerifier);
    }
}
