using System.Collections.Generic;
using System.Threading.Tasks;

namespace Terradue.Stars.Interface.Router
{
    public interface IRouter
    {
        string Label { get; }

        bool CanRoute(INode node);

        Task<IRoutable> Route(INode node);
    }
}