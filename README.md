# ⛽ cs-project

ASP.NET Core 9 API for managing stations, pumps, sales and suppliers.

## 🚀 What's inside
- `cs-project` – Web API entry point.
- `cs-project.Core` – entities, DTOs & validators.
- `cs-project.Infrastructure` – EF Core persistence & services.
- `cs-project.Tests` – xUnit & Moq tests.

## 🧩 Layered architecture
- **Core** – domain entities, DTOs, interfaces and custom exceptions.
- **Infrastructure** – data access with EF Core, repository implementations and supporting services.
- **API** – ASP.NET Core project exposing controllers, middleware and validators.
- **Tests** – xUnit project with Moq-based tests covering controllers, services and validators.

## 🛠️ Quick start
```bash
 dotnet restore
 dotnet build
dotnet run --project cs-project        # start API
dotnet test                            # run tests
```

## 🐳 Docker
### Build the image
```bash
docker build -f DockerFile.txt -t cs-project-api .
```

### docker-compose
Create a `.env` file supplying database and JWT values:

```
SA_PASSWORD=<strong password>
CSAPP_PASSWORD=<app user password>
JWT_KEY=<signing key>
JWT_ISSUER=<issuer>
JWT_AUDIENCE=<audience>
CORS_ORIGINS=<comma separated origins>
DISABLE_HTTPS_REDIRECT=true
```

Then start the stack:

```bash
docker compose up -d          # uses docker-compose.yml
# docker compose -f docker-compose.prod.yml up -d   # production
```

## ☸️ Kubernetes
Manifests live in `k8s/` and use kustomize.  Update or patch the secrets with your real values:

```bash
kubectl create secret generic api-conn --from-literal=DefaultConnection="Server=..."
kubectl create secret generic jwt-secret \
  --from-literal=Jwt__Key=... \
  --from-literal=Jwt__Issuer=cs-project-api \
  --from-literal=Jwt__Audience=cs-project-client
kubectl create secret tls cs-project-tls --cert=path/to/cert.crt --key=path/to/key.key
```

Deploy using an overlay:

```bash
kubectl apply -k k8s/overlays/dev     # or k8s/overlays/prod
```

## 🤝 Contributing
Got an idea? Open an issue or PR! ✨
