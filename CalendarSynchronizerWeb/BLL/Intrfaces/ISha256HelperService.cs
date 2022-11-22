

namespace BLL.Intrfaces
{
    public interface ISha256HelperService
    {
        public string ComputeHash(string codeVerifier);
    }
}
