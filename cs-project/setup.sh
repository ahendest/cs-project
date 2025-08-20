#!/bin/bash

# 1. Check for .NET 9 SDK
if ! dotnet --list-sdks | grep -q "9.0"; then
  echo "ERROR: .NET 9 SDK is not installed."
  echo "Please install .NET 9 SDK from https://dotnet.microsoft.com/en-us/download/dotnet/9.0"
  exit 1
fi

# 2. Restore NuGet packages for all projects
dotnet restore

# 3. Build the solution
dotnet build --configuration Release

# 4. Run tests (if you have test projects)
dotnet test

# 5. (Optional) If you have a frontend project, install npm dependencies
if [ -d "frontend" ]; then
  cd frontend
  if command -v npm &> /dev/null; then
    npm install
    npm run build
  else
    echo "npm not found. Skipping frontend setup."
  fi
  cd ..
fi

# 6. Run the API project (optional)
dotnet run --project cs-project/cs-project.API.csproj

echo "Setup complete. Your .NET 9 project is ready to run."