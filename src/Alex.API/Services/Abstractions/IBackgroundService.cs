using System.Threading.Tasks;

namespace Alex.API.Services
{
    public interface IBackgroundService
    {
        void Start();
        void Stop();
    }
}