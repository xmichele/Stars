<h1 align="center">Stars</h1>


<h2 align="center">
Spatio Temporal Asset Router Services
<br/>
<img src="docs/logo/Stars_logo.png" height="200" />

</h2>

<h3 align="center">

  <!-- [![Build Status](https://travis-ci.com/Terradue/DotNetStac.svg?branch=develop)](https://travis-ci.com/Terradue/DotNetStac) -->
  [![NuGet](https://img.shields.io/nuget/vpre/Terradue.Stars.Services)](https://www.nuget.org/packages/Terradue.Stars.Services/)
  [![License](https://img.shields.io/badge/license-AGPL3-blue.svg)](LICENSE)
  <!-- [![Binder](https://mybinder.org/badge_logo.svg)](https://mybinder.org/v2/gh/Terradue/DotNetStac/develop?filepath=example.ipynb) -->

</h3>

<h3 align="center">
  <a href="#Services">Services</a>
  <span> · </span>
  <a href="#Getting-Started">Getting started</a>
  <span> · </span>
  <a href="#Documentation">Documentation</a>
  <span> · </span>
  <a href="#Developing">Developing</a>
</h3>

**Stars** is a set of services for working with Spatio Temporal Catalog such as [STAC](https://stacspec.org) but not only.

All [services are built around the **Catalog**](#Services) :

* **Router** is a service for navigating a catalog.
* **Supplier** service enables Data Provider for the *assets* of the catalog.
* **Harvester** allows the assets to be processed by various modules for extracting additional information.
* **Processing** the *assets* to enhance their availibility.
* **Coordinator** for linking the different resources in a catalog. For instance, by gathering items in a collection.

***

## Services

**Stars** is basically a collection of services implemented in .Net that can be used to implement command line tools, web services or any programmtic logic arounf Spatio Temporal Catalogs.
They can be combined togheter to perform simples operations like listing a catalog to complex processing of assets.

<h4 align="center">
<IMG SRC="docs/diagrams/servicesStarsConcepts.svg" ALIGN=”left” HSPACE=”50” VSPACE=”50” height="300"/>
</h4>

### Router

This is a recursive function for trigger functions during the navigation of a Catalog. Basically it reads a catalog as a tree and crawl in every node of the catalog allowing the programmer to set functions to be executed when it meets a new node, before and after branching to the node children or when the parser comes to a leaf node.
This service uses the plugins manager to find the appropriate router for a catalog data model and encoding.

> :mag: a Router plugin implements readers for various catalog data model and encoding. [Stars CLI](#Stars-CLI) implements natively
> * Atom feed from an [OpenSearch](https://github.com/dewitt/opensearch) results
> * [STAC](https://stacspec.org) ([catalog](https://github.com/radiantearth/stac-spec/tree/master/catalog-spec), [collection](https://github.com/radiantearth/stac-spec/tree/master/collection-spec), [item](https://github.com/radiantearth/stac-spec/tree/master/catalog-spec))

### Supplier

Service managing a collection of suppliers for providing with the assets. From a resource description (e.g. uid, AOI, time...) it requests them for the data availability and organize the delivery of the assets using **Carriers** (e.g. HTTP/FTP/S3 download). It also allows to make orders to suppliers that offers offline datasets. Suppliers and Carriers in this service are managed as configurable plugins.

    E.g. Copernicus datasets providers like [DIAS](https://www.copernicus.eu/en/access-data/dias) can be implemented as a supllier and managed in this service.

### Harvester

Collection of executors that perform a scan of the data to extract useful infromation that can be added to the catalog. Executors are managed as configurable plugins.



### Processing

An abstracted service enabling a trigger for procesing *items* of a catalog 

