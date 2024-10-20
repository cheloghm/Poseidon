#!/bin/bash

# Exit on any error
set -e

# Navigate to Terraform environment directory
cd terraform/environments/prod

# Initialize and Apply EKS Module
cd eks
terragrunt init
terragrunt apply -auto-approve
cd ..

# Initialize and Apply Networking Module
cd networking
terragrunt init
terragrunt apply -auto-approve
cd ..

# Output kubeconfig
terragrunt output -raw kubeconfig > ../../k8s/kubeconfig.yaml

# Set KUBECONFIG environment variable
export KUBECONFIG=$(pwd)/../../k8s/kubeconfig.yaml

# Install NGINX Ingress Controller (if not already installed)
kubectl apply -f https://raw.githubusercontent.com/kubernetes/ingress-nginx/controller-v1.2.1/deploy/static/provider/aws/deploy.yaml

# Wait for Ingress Controller to be ready
kubectl rollout status deployment/ingress-nginx-controller -n ingress-nginx

# Apply StorageClass
kubectl apply -f ../../k8s/storageclasses/aws-ebs-sc.yaml

# Apply PersistentVolumeClaim
kubectl apply -f ../../k8s/persistenvolumeclaims/mongo-pvc.yaml

# Apply ConfigMap and Secrets
kubectl apply -f ../../k8s/configmaps/poseidon-config.yaml
kubectl apply -f ../../k8s/secrets/poseidon-secrets.yaml
kubectl apply -f ../../k8s/secrets/poseidon-external-secret.yaml

# Deploy MongoDB
kubectl apply -f ../../k8s/deployments/poseidon-mongodb-deployment.yaml
kubectl apply -f ../../k8s/services/poseidon-mongodb-service.yaml

# Deploy API
kubectl apply -f ../../k8s/deployments/poseidon-api-deployment.yaml
kubectl apply -f ../../k8s/services/poseidon-api-service.yaml

# Deploy Frontend
kubectl apply -f ../../k8s/deployments/poseidon-frontend-deployment.yaml
kubectl apply -f ../../k8s/services/poseidon-frontend-service.yaml

# Deploy CronJob
kubectl apply -f ../../k8s/cronjobs/mongodb-backup-cronjob.yaml

# Deploy Ingress
kubectl apply -f ../../k8s/ingress/poseidon-ingress.yaml

# Deploy Helm Charts for Elasticsearch and Kibana
helm repo add elastic https://helm.elastic.co
helm repo update

helm install elasticsearch elastic/elasticsearch -n logging --create-namespace
helm install kibana elastic/kibana -n logging

# Deploy Helm Chart for DataDog
helm repo add datadog https://helm.datadoghq.com
helm repo update

helm install datadog-agent datadog/datadog \
  --set datadog.apiKey=<YOUR_DATADOG_API_KEY> \
  --set datadog.appKey=<YOUR_DATadog_APP_KEY> \
  --set agents.image.tag=latest \
  --namespace datadog --create-namespace

# Deploy Poseidon Application via Helm
helm upgrade --install poseidon-app ../../helm-charts/poseidon-app --namespace poseidon --create-namespace

echo "Deployment completed successfully."
