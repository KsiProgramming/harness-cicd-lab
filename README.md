# harness-cicd-lab

Hands-on Harness CI/CD lab: a .NET 10 minimal API built, tested and pushed to Docker Hub by
Harness CI (Harness Cloud), then deployed to a local minikube cluster by Harness CD (canary).

| Path | Content |
|------|---------|
| `app/` | ASP.NET Core minimal API, xUnit tests, multi-stage `Dockerfile` |
| `harness/pipelines/` | Pipeline YAML (CI stage; CD stage added later) |
| `harness/connectors/` | GitHub and Docker Hub connector YAML (secrets referenced, never stored) |
| `harness/delegate/` | Sanitized Kubernetes delegate manifest template |
| `k8s/` | Kubernetes manifests for CD (coming next) |

## Run locally

```bash
dotnet test app/HarnessLab.slnx -c Release
docker build -t harness-lab-api:local --build-arg APP_VERSION=local app/
docker run --rm -p 8080:8080 harness-lab-api:local   # GET / , /api/greeting/{name}, /healthz
```

Image: [`essaidlab/harness-lab-api`](https://hub.docker.com/r/essaidlab/harness-lab-api)
