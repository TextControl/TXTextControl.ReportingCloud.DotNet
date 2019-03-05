![Logo](https://raw.githubusercontent.com/TextControl/TXTextControl.ReportingCloud.Dotnet/master/Resources/rc_logo_512.png)

#  ReportingCloud .NET SDK

This is the official .NET SDK for ReportingCloud, which is authored, maintained and fully supported by [Text Control](http://www.textcontrol.com).

[http://www.reporting.cloud](http://www.reporting.cloud)

Before using ReportingCloud, please sign up to the service:

[https://portal.reporting.cloud](https://portal.reporting.cloud)

## API Documentation

The complete API documentation can be found in the GitHub page of this project:

[ReportingCloud .NET SDK API Documentation](http://textcontrol.github.io/TXTextControl.ReportingCloud.DotNet/api.html)

## Getting Started

This getting started tutorial helps to create your first application using the ReportingCloud .NET SDK:

[.NET Framework Quickstart Tutorial](https://docs.reporting.cloud/docs/chapter/quickstart/dotnet)

## Username and password for unittests

The ReportingCloud .NET SDK ships with a number of .NET unit tests. The scripts in each of these directories require a username and password for ReportingCloud in order to be executed. So that your username and password are not made inadvertently publicly available via a public GIT repository, you will first need to specify them.

## Running unit tests under .NET Core

There is a seperate test project for the Reporting Cloud .NET Core SDK. The project can be found in the UnitTests.DotNetCore folder. As the test project is a .NET Core project, you can run it on any platform that is supported by .NET Core like macOS oder Linux using the `dotnet test` command. This assumes that you have .NET Core installed on your system. 

## Installation

You will find the Text Control ReportingCloud .NET SDK as a NuGet package available on NuGet:

    PM> Install-Package TXTextControl.ReportingCloud

## Contributing

Bug reports and pull requests are welcome on GitHub at https://github.com/TextControl/TXTextControl.ReportingCloud.DotNet.
