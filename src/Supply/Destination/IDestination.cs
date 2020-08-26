using System;
using Stars.Router;

namespace Stars.Supply.Destination
{
    public interface IDestination
    {
        Uri Uri { get; }

        IDestination RelativePath(IRoute route, IRoute subroute);
        IDestination To(IRoute route);
    }
}