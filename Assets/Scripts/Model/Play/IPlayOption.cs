using System.Threading.Tasks;

namespace model.play
{
    public interface IPlayOption
    {
        bool Legal { get; }
        bool Mandatory { get; }
        Task Resolve();
    }
}
