# Learn about building .NET container images:
# https://github.com/dotnet/dotnet-docker/blob/main/samples/README.md
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Get target architecture
ARG TARGETARCH

# Set working directory name
WORKDIR /src

# Copy project/ file and restore as distinct layers
COPY --link . .

# Install dependencies
RUN dotnet restore LibraryManagement.sln -a $TARGETARCH

# Copy source code
COPY --link . .

# publish the app
RUN dotnet publish /p:UseAppHost=false -a $TARGETARCH --no-restore --property:PublishDir=/src

# Install SDK 
FROM mcr.microsoft.com/dotnet/sdk:8.0

# Set working directory name
WORKDIR /src

# Copy files
COPY --link --from=build /src .

# Install EF Tool for migrations
RUN dotnet tool install --global dotnet-ef --version 9.0.11

# Update PATH enviroment variable
ENV PATH=$PATH:/root/.dotnet/tools

# Run the bash script
ENTRYPOINT ["./build.sh"]