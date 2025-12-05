#!/usr/bin/env bash

dotnet ef database update --project LibraryManagement.Infrastructure --startup-project LibraryManagement.Api --verbose
dotnet dev-certs https
dotnet dev-certs https --trust
dotnet run --project LibraryManagement.Api/LibraryManagement.Api.csproj
