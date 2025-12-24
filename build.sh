#!/bin/bash

# CoreHub - Build and Validation Script

echo "╔════════════════════════════════════════════════════════════════╗"
echo "║                    CoreHub Build Script                        ║"
echo "╚════════════════════════════════════════════════════════════════╝"
echo ""

# Color codes
GREEN='\033[0;32m'
RED='\033[0;31m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

# Function to print status
print_status() {
    if [ $1 -eq 0 ]; then
        echo -e "${GREEN}✓${NC} $2"
    else
        echo -e "${RED}✗${NC} $2"
        exit 1
    fi
}

# Check prerequisites
echo "Checking prerequisites..."
command -v dotnet >/dev/null 2>&1
print_status $? "dotnet SDK installed"

dotnet --version | grep -q "^9\."
print_status $? "dotnet 9.0 or higher"

echo ""

# Clean previous builds
echo "Cleaning previous builds..."
dotnet clean --nologo > /dev/null 2>&1
print_status $? "Clean completed"

echo ""

# Restore dependencies
echo "Restoring NuGet packages..."
dotnet restore --nologo
print_status $? "Restore completed"

echo ""

# Build solution
echo "Building solution..."
dotnet build --configuration Release --no-restore --nologo
print_status $? "Build completed"

echo ""

# Run tests
echo "Running tests..."
dotnet test --configuration Release --no-build --verbosity quiet --nologo
print_status $? "Tests passed"

echo ""

# Build Docker image (optional)
if command -v docker &> /dev/null; then
    echo "Building Docker image..."
    docker build -t corehub:latest . > /dev/null 2>&1
    print_status $? "Docker image created"
    echo ""
fi

echo "╔════════════════════════════════════════════════════════════════╗"
echo "║                    Build Successful! 🎉                        ║"
echo "╚════════════════════════════════════════════════════════════════╝"
echo ""
echo "Next steps:"
echo "  1. Set up database:  cd src/CoreHub.Infrastructure && dotnet ef database update --startup-project ../CoreHub.Web"
echo "  2. Seed demo data:   cd src/CoreHub.Seed && dotnet run"
echo "  3. Run application:  cd src/CoreHub.Web && dotnet run"
echo ""
echo "Then open: https://localhost:5001"
echo "Login as:  admin@demo.com / Admin123!"
echo ""
