using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Terradue.Stars.Interface.Router;

using Terradue.Stars.Services.Router;
using Terradue.Stars.Services;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using Terradue.Stars.Interface;
using System.IO;
using System.Threading;

namespace Terradue.Stars.Console.Operations
{
    [Command(Name = "list", Description = "List the routing from the input catalog")]
    internal class ListOperation : BaseOperation
    {
        [Argument(0)]
        public string[] Inputs { get => inputs; set => inputs = value; }

        [Option("-r|--recursivity", "Resource recursivity depth routing", CommandOptionType.SingleValue)]
        public int Recursivity { get => recursivity; set => recursivity = value; }

        [Option("-sa|--skip-assets", "Do not list assets", CommandOptionType.NoValue)]
        public bool SkippAssets { get; set; }

        private RouterService routingService;
        private IResourceServiceProvider resourceServiceProvider;
        private int recursivity = 1;
        private string[] inputs = new string[0];

        public ListOperation(IConsole console): base(console)
        {

        }

        private void InitRoutingTask()
        {
            routingService.Parameters = new RouterServiceParameters()
            {
                Recursivity = Recursivity,
                SkipAssets = SkippAssets
            };
            routingService.OnRoutingException((route, router, exception, state, ct) => PrintRouteInfo(route, router, exception, state, ct));
            routingService.OnBeforeBranching((node, router, state, subroutes, ct) => PrintBranchingNode(node, router, state, ct));
            routingService.OnItem((node, router, state, ct) => PrintItem(node, router, state, ct));
            routingService.OnBranching((parentRoute, route, siblings, state, ct) => PrepareNewRoute(parentRoute, route, siblings, state, ct));
        }

        private Task<object> PrepareNewRoute(IResource parentRoute, IResource route, IEnumerable<IResource> siblings, object state, System.Threading.CancellationToken ct)
        {
            if (state == null) return Task.FromResult<object>(new ListOperationState("", 1));

            ListOperationState operationState = state as ListOperationState;
            if (operationState.Depth == 0) return Task.FromResult<object>(state);

            string newPrefix = operationState.Prefix.Replace('─', ' ').Replace('└', ' ');
            int i = siblings.ToList().IndexOf(route);
            if (i == siblings.Count() - 1)
            {
                newPrefix += "└─";
            }
            else
                newPrefix += "│─";
            return Task.FromResult<object>(new ListOperationState(newPrefix, operationState.Depth + 1));
        }

        private async Task<object> PrintItem(IItem node, IRouter router, object state, System.Threading.CancellationToken ct)
        {
            ListOperationState operationState = state as ListOperationState;
            string resourcePrefix1 = operationState.Prefix;
            if (router != null)
                resourcePrefix1 = string.Format("[{0}] {1}", router.Label, operationState.Prefix);
            _console.ForegroundColor = GetColorFromType(node.ResourceType);
            await _console.Out.WriteLineAsync(String.Format("{0,-80} {1,40}", (resourcePrefix1 + node.Title).Truncate(99), node.ContentType));
            _console.ForegroundColor = ConsoleColor.White;
            await PrintAssets(node, router, operationState.Prefix);
            return state;
        }

        private async Task<object> PrintBranchingNode(ICatalog node, IRouter router, object state, System.Threading.CancellationToken ct)
        {
            _console.ForegroundColor = GetColorFromType(node.ResourceType);
            // Print the information about the resource
            ListOperationState operationState = state as ListOperationState;
            string resourcePrefix1 = operationState.Prefix;
            if (router != null)
                resourcePrefix1 = string.Format("[{0}] {1}", router.Label, operationState.Prefix);
            await _console.Out.WriteLineAsync(String.Format("{0,-80} {1,40}", (resourcePrefix1 + node.Title).Truncate(99), node.ContentType));
            // await PrintAssets(node, router, operationState.Prefix);

            return state;
        }

        private async Task<object> PrintRouteInfo(IResource route, IRouter router, Exception exception, object state, System.Threading.CancellationToken ct)
        {
            ListOperationState operationState = state as ListOperationState;
            string resourcePrefix1 = operationState.Prefix;
            if (router != null)
                resourcePrefix1 = string.Format("[{0}] {1}", router.Label, operationState.Prefix);
            _console.ForegroundColor = GetColorFromType(route.ResourceType);
            await _console.Out.WriteAsync(String.Format("{0,-80} {1,40}", (resourcePrefix1 + route.Uri).Truncate(99), route.ContentType));
            if (exception != null)
            {
                _console.ForegroundColor = ConsoleColor.Red;
                await _console.Out.WriteLineAsync(String.Format(" -> {0}", exception.Message.Truncate(99)));
            }
            else
                await _console.Out.WriteLineAsync();
            _console.ForegroundColor = ConsoleColor.White;
            return state;
        }

        protected override async Task ExecuteAsync(CancellationToken ct)
        {
            this.routingService = ServiceProvider.GetService<RouterService>();
            this.resourceServiceProvider = ServiceProvider.GetService<IResourceServiceProvider>();
            InitRoutingTask();
            var tasks = Inputs.Select(input => resourceServiceProvider.CreateStreamResourceAsync(new GenericResource(new Uri(input)), ct));
            List<IResource> routes = (await Task.WhenAll(tasks)).Cast<IResource>().ToList();

            foreach (var route in routes)
            {
                object state = await PrepareNewRoute(null, route, null, null, ct);
                await routingService.RouteAsync(route, recursivity, null, state, ct);
            }

        }

        private async Task PrintAssets(IItem resource, IRouter router, string prefix)
        {
            // List assets
            if (!SkippAssets && resource is IAssetsContainer)
            {
                IReadOnlyDictionary<string, IAsset> assets = ((IAssetsContainer)resource).Assets;
                for (int i = 0; i < assets.Count(); i++)
                {
                    string newPrefix = prefix.Replace('─', ' ').Replace('└', ' ');
                    if (i == assets.Count() - 1)
                    {
                        newPrefix += '└';
                    }
                    else
                        newPrefix += '│';

                    _console.ForegroundColor = ConsoleColor.DarkCyan;
                    var assetPrefix = newPrefix + new string('─', 1);
                    if (router != null)
                        assetPrefix = string.Format("[{0}] {1}", router.Label, assetPrefix);
                    var asset = assets.ElementAt(i).Value;
                    string title = asset.Title;
                    if ( string.IsNullOrEmpty(title) )
                        title = Path.GetFileName(asset.Uri.ToString());
                    title = string.Format("[{0}] {1}", assets.ElementAt(i).Key, title);
                    await _console.Out.WriteLineAsync(String.Format("{0,-80} {1,40}", (assetPrefix + title).Truncate(99), asset.ContentType));
                    _console.ForegroundColor = ConsoleColor.White;
                }
            }
        }

        private ConsoleColor GetColorFromType(ResourceType resourceType)
        {
            switch (resourceType)
            {
                case ResourceType.Catalog:
                    return ConsoleColor.Blue;
                case ResourceType.Collection:
                    return ConsoleColor.Green;
                case ResourceType.Item:
                    return ConsoleColor.Yellow;
                case ResourceType.Asset:
                    return ConsoleColor.DarkCyan;
            }
            return ConsoleColor.White;
        }


        protected override void RegisterOperationServices(ServiceCollection collection)
        {
        }
    }
}