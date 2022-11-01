# Blazor ASP.NET website with Adaptive Cards for uploading text files
This is the result of a three week personal C# coding sprint, to creating website with text uploading, image conversion and downloading, and page view logging.

The website is implemented as a Blazor WebAssembly (WASM) app, with an Asp.NET Core server backend. The C# code is written using Visual Studio 2022 in .NET 6.

## Quick start - How to see the code in action
To add and display stories (articles with a title and text) on the website, you need to create a Google Firestore database on the cloud, and set up the service account credentials.

You can then build and then run the solution in Production (or Debug mode) in IIS Express.

For the purposes of the demo, the login email of the user with 'add story' privileges is 'john@stories.com' and password is 'hi'.


## Technologies used

### Google Firestore Cloud Database

Before you run the website, you will need to create a database to store the story and page view/download metadata on Google Cloud Firestore. Firestore exposes a RESTful service that is accessed via the C# Google.Cloud.Firestore .NET library.

***Why Firestore?***
As a non-SQL database, Firestore has the flexibility to allow freely inserting documents and fields without constraints. It feels like working with a file system. Firestore is organized into collections of documents. Documents hold fields and collections. Collections cannot hold collections, and Documents cannot hold documents. Fields hold only database values.

In the Stories web app, we work with the Firebase database this way:

***Adding a story:***
Add story metadata by defining a path to a story document (a document is a container for fields, which hold the database values) and then adding the field values.

***Incrementing page views and downloads:***
We create a reference to a ViewsDownloads collection in the story document. This collection does not need to exist yet. When a page is viewed, we request that a document (the document id is today's date) field is incremented. This document or field don't need to exist before the first increment request, and subsequent increment requests (when the field already exists) follow the exact same syntax. The increment operation is done atomically on the server using the FieldValue.Increment(int) function.

### System.Drawing.Graphics

The System.Drawing.Graphics library is used to create a (.png) graphic representation of the stories on the server. It is built on the GDI+ library, which is integrated into the Windows OS. This limits the server to running on Windows, and also results in  Third party graphics libraries for .NET exist as well, but were not chosen because some were not free or added watermarks to the graphics created.

The full story text and (.png) graphics are stored on the filesystem on the server. (With more time, it would have been nice to learn Firebase Storage and use it for the story text and .png graphic representation. Firebase Storage integrates with Firebase but is designed to hold large data blobs.)

Microsoft.AspNetCore.Components.Authorization and Microsoft.AspNetCore.Authorization .NET libraries

The Microsoft.AspNetCore.Authorization library provides authentication routing via [Authorize] and [AllowAnonymous] page attributes. The <AuthorizeView> component is used to show or hide page content using the <Authorized> or <NotAuthorized> subcomponents. This is how the Login and Logout links are selectively shown on the MainLayout.razor page, and the Add Story menu item on the NavMenu.razor page.

Note that when building the solution, you will notice CA1416 warnings. These are due to the Windows dependancy of the GDI+ library as described below. If you suppress these warnings (Stories.Server Properties | Build | General | Errors and warnings | Suppress specific warnings), then you should see no other warnings or errors.

### Adaptive Cards for Blazor

Stories are listed (in the StoryCollection.razor component) using Adaptive Cards for Blazor (AdaptiveCardsBlazor library v2.0.0), an opensource .NET library and project on Github. The Adaptive Cards use json Schemas to define card layout. (Version 1.2 from adaptivecards.io is the highest Schema version supported by .NET 6.)
The AdaptiveCardsBlazor library is not fully supported for Blazor Webassembly, so <CardCollection> component did not render and was not used, and the <AdaptiveCard> component did not populate the schema. The workaround was to insert the content for each story manually (in StoryModel.cs) and then pass the pre-populated schema to the <AdaptiveCard> component.

### SVG Adaptive Images

SVG is a vector graphics format that is based on XML, this makes it dynamic and responsive within a razor page. In the project, each story's statistics are loaded into and displayed as a line chart within an SVG graphic. Code for the SVG graphic is adapted from https://github.com/martijn/BlazorCharts, with a tip of the hat to @martijn.

## Not implemented

An authentication service was not used for the app, due to time constraints. Authentication services support issuing JWT tokens, user registration, email activation, multifactor authentication, etc. Blazor has some built in support for interacting with authentication services and Google has a role-based authentication service that integrates with Firebase.
