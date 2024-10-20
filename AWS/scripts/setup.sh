#!/bin/bash

# Exit on any error
set -e

# Update and install necessary packages
sudo apt-get update -y
sudo apt-get install -y unzip curl git

# Install AWS CLI
curl "https://awscli.amazonaws.com/awscli-exe-linux-x86_64.zip" -o "awscliv2.zip"
unzip awscliv2.zip
sudo ./aws/install
rm -rf awscliv2.zip aws/

# Install Terraform
TERRAFORM_VERSION=1.3.9
curl -LO "https://releases.hashicorp.com/terraform/${TERRAFORM_VERSION}/terraform_${TERRAFORM_VERSION}_linux_amd64.zip"
unzip "terraform_${TERRAFORM_VERSION}_linux_amd64.zip"
sudo mv terraform /usr/local/bin/
rm "terraform_${TERRAFORM_VERSION}_linux_amd64.zip"

# Install Terragrunt
TERRAGRUNT_VERSION=0.40.12
curl -LO "https://github.com/gruntwork-io/terragrunt/releases/download/v${TERRAGRUNT_VERSION}/terragrunt_linux_amd64"
chmod +x terragrunt_linux_amd64
sudo mv terragrunt_linux_amd64 /usr/local/bin/terragrunt

# Install kubectl
curl -LO "https://dl.k8s.io/release/$(curl -L -s https://dl.k8s.io/release/stable.txt)/bin/linux/amd64/kubectl"
chmod +x kubectl
sudo mv kubectl /usr/local/bin/

# Install Helm
HELM_VERSION=v3.10.2
curl -LO "https://get.helm.sh/helm-${HELM_VERSION}-linux-amd64.tar.gz"
tar -zxvf "helm-${HELM_VERSION}-linux-amd64.tar.gz"
sudo mv linux-amd64/helm /usr/local/bin/
rm -rf linux-amd64 helm-${HELM_VERSION}-linux-amd64.tar.gz

# Install kubectx and kubens (optional)
git clone https://github.com/ahmetb/kubectx.git
sudo ln -s "$(pwd)/kubectx/kubectx" /usr/local/bin/kubectx
sudo ln -s "$(pwd)/kubectx/kubens" /usr/local/bin/kubens
rm -rf kubectx

echo "Setup completed successfully."
