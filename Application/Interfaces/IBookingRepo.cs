using Domain;
using System.Threading.Tasks;

namespace Application
{
    public interface IBookingRepo
    {
        Task Save(Booking booking);
        Task<Booking> GetById(long id);
    }
}
