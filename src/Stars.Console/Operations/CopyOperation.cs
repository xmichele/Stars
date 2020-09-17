using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Stars.Interface.Router;
using Stars.Interface.Supply;
using Stars.Interface.Supply.Destination;
using Stars.Service.Catalog;
using Stars.Service.Model.Stac;
using Stars.Service.Router;
using Stars.Service.Supply;
using Stars.Service.Supply.Destination;

namespace Stars.Operations
{
    [Command(Name = "copy", Description = "Copy the tree of resources and assets by routing from the input reference")]
    internal class CopyOperation : BaseOperation
    {
        [Argument(0)]
        public string[] Inputs { get => inputs; set => inputs = value; }

        [Option("-r|--recursivity", "Resource recursivity depth routing", CommandOptionType.SingleValue)]
        public int Recursivity { get => recursivity; set => recursivity = value; }

        [Option("-sa|--skip-assets", "Do not list assets", CommandOptionType.NoValue)]
        public bool SkippAssets { get; set; }

        [Option("-o|--output_dir", "Output Directory", CommandOptionType.SingleValue)]
        public string OutputDirectory { get; set; }

        [Option("-ao|--allow-ordering", "Allow ordering assets", CommandOptionType.NoValue)]
        public bool AllowOrdering { get; set; }


        private RoutingTask routingTask;
        private DestinationManager destinationManager;
        private CarrierManager carrierManager;

        private string[] inputs = new string[0];
        private int recursivity = 1;

        public CopyOperation()
        {

        }
        private void InitRoutingTask()
        {
            routingTask.Parameters = new RoutingTaskParameters()
            {
                Recursivity = Recursivity,
                SkipAssets = SkippAssets
            };
            // routingTask.OnRoutingToNodeException((route, router, exception, state) => PrintRouteInfo(route, router, exception, state));
            routingTask.OnBranchingNode((node, router, state) => CopyNode(node, router, state));
            routingTask.OnLeafNode((node, router, state) => CopyNode(node, router, state));
            routingTask.OnBranching(async (parentRoute, route, siblings, state) => await PrepareNewRoute(parentRoute, route, siblings, state));
            routingTask.OnAfterBranching(async (parentRoute, router, parentState, subStates) => await LinkNodes(parentRoute, router, parentState, subStates));
        }

        private async Task<object> LinkNodes(IRoutable parentRoute, IRouter router, object state, IEnumerable<object> subStates)
        {
            CopyOperationState operationState = state as CopyOperationState;

            CatalogingTask catalogingTask = ServiceProvider.GetService<CatalogingTask>();

            StacNode stacNode = await catalogingTask.ExecuteAsync(parentRoute, subStates.Cast<CopyOperationState>().Select(s => s.LastRoute));

            var stacDeliveries = carrierManager.GetSingleDeliveryQuotations(null, stacNode, operationState.Destination);

            IRoute stacRoute = null;

            foreach (var delivery in stacDeliveries)
            {
                stacRoute = await delivery.Carrier.Deliver(delivery);
                if (stacRoute != null) break;
            }

            operationState.LastRoute = stacRoute;

            return operationState;

        }

        private async Task<object> PrepareNewRoute(IRoute parentRoute, IRoute newRoute, IList<IRoute> siblings, object state)
        {
            if (state == null)
            {
                var destination = await destinationManager.CreateDestination(OutputDirectory);
                if (destination == null)
                    throw new InvalidProgramException("No destination found for " + OutputDirectory);
                return new CopyOperationState(1, destination);
            }

            CopyOperationState operationState = state as CopyOperationState;
            if (operationState.Depth == 0) return state;

            var newDestination = operationState.Destination.RelativePath(parentRoute, newRoute);

            return new CopyOperationState(operationState.Depth + 1, newDestination);
        }

        private async Task<object> CopyNode(INode node, IRouter router, object state)
        {
            CopyOperationState operationState = state as CopyOperationState;

            SupplyingTask supplyTask = ServiceProvider.GetService<SupplyingTask>();

            supplyTask.Parameters = new SupplyTaskParameters();

            DeliveryForm deliveryForm = await supplyTask.ExecuteAsync(node, operationState.Destination);

            operationState.LastRoute = deliveryForm.NodeDeliveredRoute;

            return operationState;
        }


        protected override async Task ExecuteAsync()
        {
            this.routingTask = ServiceProvider.GetService<RoutingTask>();
            this.destinationManager = ServiceProvider.GetService<DestinationManager>();
            this.carrierManager = ServiceProvider.GetService<CarrierManager>();
            InitRoutingTask();
            await routingTask.ExecuteAsync(Inputs);
        }

        protected override void RegisterOperationServices(ServiceCollection collection)
        {
            if (AllowOrdering)
                collection.AddTransient<ICarrier, OrderingCarrier>();
        }
    }
}