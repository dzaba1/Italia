using Italia.Lib.Model;
using System.Threading.Tasks;

namespace Italia.Lib
{
    public interface IDataProvider
    {
        Task<Offer[]> GetOffersAsync();
    }
}
