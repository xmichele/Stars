using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Terradue.Stars.Interface.Router;
using Terradue.Stars.Interface.Supply.Asset;
using Terradue.Stars.Service.Router;
using Terradue.Stars.Service;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.DependencyInjection;
using System.Net;

namespace Terradue.Stars.Operations
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

        private RoutingService routingService;
        private int recursivity = 1;
        private string[] inputs = new string[0];

        public ListOperation()
        {

        }

        private void InitRoutingTask()
        {
            routingService.Parameters = new RoutingTaskParameters()
            {
                Recursivity = Recursivity,
                SkipAssets = SkippAssets
            };
            routingService.OnRoutingToNodeException((route, router, exception, state) => PrintRouteInfo(route, router, exception, state));
            routingService.OnBranchingNode((node, router, state) => PrintBranchingNode(node, router, state));
            routingService.OnLeafNode((node, router, state) => PrintLeafNode(node, router, state));
            routingService.OnBranching((parentRoute, route, siblings, state) => PrepareNewRoute(parentRoute, route, siblings, state));
        }

        private Task<object> PrepareNewRoute(IRoute parentRoute, IRoute route, IList<IRoute> siblings, object state)
        {
            if (state == null) return Task.FromResult<object>(new ListOperationState("", 1));

            ListOperationState operationState = state as ListOperationState;
            if (operationState.Depth == 0) return Task.FromResult<object>(state);

            string newPrefix = operationState.Prefix.Replace('─', ' ').Replace('└', ' ');
            int i = siblings.IndexOf(route);
            if (i == siblings.Count() - 1)
            {
                newPrefix += "└─";
            }
            else
                newPrefix += "│─";
            return Task.FromResult<object>(new ListOperationState(newPrefix, operationState.Depth + 1));
        }

        public static int OnValidationError(CommandLineApplication command, ValidationResult ve)
        {
            PhysicalConsole.Singleton.Error.WriteLine(ve.ErrorMessage);
            command.ShowHelp();
            return 1;
        }

        private async Task<object> PrintLeafNode(INode node, IRouter router, object state)
        {
            ListOperationState operationState = state as ListOperationState;
            string resourcePrefix1 = operationState.Prefix;
            await PrintAssets(node, router, operationState.Prefix);
            return state;
        }

        private async Task<object> PrintBranchingNode(INode node, IRouter router, object state)
        {
            console.ForegroundColor = GetColorFromType(node.ResourceType);
            // Print the information about the resource
            ListOperationState operationState = state as ListOperationState;
            string resourcePrefix1 = operationState.Prefix;
            if (router != null)
                resourcePrefix1 = string.Format("[{0}] {1}", router.Label, operationState.Prefix);
            await console.Out.WriteLineAsync(String.Format("{0,-80} {1,40}", (resourcePrefix1 + node.Label).Truncate(99), node.ContentType));
            await PrintAssets(node, router, operationState.Prefix);

            return state;
        }

        private async Task<object> PrintRouteInfo(IRoute route, IRouter router, Exception exception, object state)
        {
            ListOperationState operationState = state as ListOperationState;
            string resourcePrefix1 = operationState.Prefix;
            if (router != null)
                resourcePrefix1 = string.Format("[{0}] {1}", router.Label, operationState.Prefix);
            console.ForegroundColor = GetColorFromType(route.ResourceType);
            await console.Out.WriteAsync(String.Format("{0,-80} {1,40}", (resourcePrefix1 + route.Uri).Truncate(99), route.ContentType));
            console.ForegroundColor = ConsoleColor.Red;
            await console.Out.WriteLineAsync(String.Format(" -> {0}", exception.Message.Truncate(99)));
            return state;
        }

        protected override async Task ExecuteAsync()
        {
          

            this.routingService = ServiceProvider.GetService<RoutingService>();
            InitRoutingTask();
            await routingService.ExecuteAsync(Inputs);
        }

        private async Task PrintAssets(INode resource, IRouter router, string prefix)
        {
            // List assets
            if (!SkippAssets && resource is IAssetsContainer)
            {
                IDictionary<string, IAsset> assets = ((IAssetsContainer)resource).GetAssets();
                for (int i = 0; i < assets.Count(); i++)
                {
                    string newPrefix = prefix.Replace('─', ' ').Replace('└', ' ');
                    if (i == assets.Count() - 1)
                    {
                        newPrefix += '└';
                    }
                    else
                        newPrefix += '│';

                    console.ForegroundColor = ConsoleColor.DarkCyan;
                    var assetPrefix = newPrefix + new string('─', 1);
                    if (router != null)
                        assetPrefix = string.Format("[{0}] {1}", router.Label, assetPrefix);
                    var asset = assets.ElementAt(i).Value;
                    await console.Out.WriteLineAsync(String.Format("{0,-80} {1,40}", (assetPrefix + asset.Label).Truncate(99), asset.ContentType));
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