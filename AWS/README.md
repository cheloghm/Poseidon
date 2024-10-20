# Poseidon Project AWS Deployment Guide

Welcome to the **Poseidon Project** AWS Deployment Guide! This comprehensive manual will walk you through deploying the Poseidon backend project on AWS using **Terraform**, **Terragrunt**, **Kubernetes (EKS)**, and **Helm**. Whether you're a seasoned DevOps engineer or a beginner, this guide ensures a seamless setup.

## Table of Contents

1. [Prerequisites](#prerequisites)
2. [Directory Structure](#directory-structure)
3. [Setup Steps](#setup-steps)
   - [1. Clone the Repository](#1-clone-the-repository)
   - [2. Configure AWS Credentials](#2-configure-aws-credentials)
   - [3. Setup Environment](#3-setup-environment)
   - [4. Initialize and Deploy Infrastructure](#4-initialize-and-deploy-infrastructure)
   - [5. Deploy Applications to Kubernetes](#5-deploy-applications-to-kubernetes)
   - [6. Configure Logging and Monitoring](#6-configure-logging-and-monitoring)
   - [7. Implement Security Best Practices](#7-implement-security-best-practices)
4. [Site Reliability Engineering (SRE) Implementations](#site-reliability-engineering-sre-implementations)
5. [DevSecOps Implementations](#devsecops-implementations)
6. [Security/Attack Surface Mitigations](#securityattack-surface-mitigations)
7. [Tools and Technologies](#tools-and-technologies)
8. [Troubleshooting](#troubleshooting)
9. [Additional Resources](#additional-resources)

---

## Prerequisites

Before you begin, ensure you have the following installed and configured:

- **Operating System:** Linux, macOS, or Windows (using WSL)
- **AWS Account:** [Sign Up](https://aws.amazon.com/) if you don't have one.
- **SSH Key Pair:** For accessing EKS nodes. Create via AWS Console or CLI.
- **Docker:** [Install Docker](https://www.docker.com/get-started).
- **Git:** [Install Git](https://git-scm.com/downloads).
- **Helm:** [Install Helm](https://helm.sh/docs/intro/install/).
- **kubectl:** [Install kubectl](https://kubernetes.io/docs/tasks/tools/install-kubectl/).
- **Terraform & Terragrunt:** These will be installed via the setup script.
- **Access Permissions:** Ensure your AWS IAM user has necessary permissions for Terraform to create resources.

---

## Directory Structure

Understanding the project structure is crucial for efficient navigation and management.

\Poseidon\AWS
├── helm-charts
│ └── poseidon-app
│ ├── Chart.yaml │ ├── values.yaml │ └── templates
│ ├── api-deployment.yaml │ ├── api-service.yaml │ ├── frontend-deployment.yaml │ ├── frontend-service.yaml │ ├── ingress.yaml │ ├── mongodb-deployment.yaml │ ├── mongodb-service.yaml │ └── logstash
│ ├── logstash-deployment.yaml │ └── logstash-service.yaml ├── k8s
│ ├── configmaps
│ │ ├── poseidon-config.yaml │ │ └── poseidon-logstash-config.yaml │ ├── cronjobs
│ │ └── mongodb-backup-cronjob.yaml │ ├── deployments
│ │ ├── poseidon-api-deployment.yaml │ │ ├── poseidon-frontend-deployment.yaml │ │ └── poseidon-mongodb-deployment.yaml │ ├── ingress
│ │ └── poseidon-ingress.yaml │ ├── persistentvolumes
│ ├── persistenvolumeclaims
│ │ └── mongo-pvc.yaml │ ├── secrets
│ │ ├── poseidon-external-secret.yaml │ │ └── poseidon-secrets.yaml │ ├── services
│ │ ├── poseidon-api-service.yaml │ │ ├── poseidon-frontend-service.yaml │ │ └── poseidon-mongodb-service.yaml │ └── storageclasses
│ └── aws-ebs-sc.yaml ├── scripts
│ ├── deploy.sh │ └── setup.sh ├── terraform
│ ├── environments
│ │ └── prod
│ │ ├── eks
│ │ │ └── terragrunt.hcl │ │ └── networking
│ │ └── terragrunt.hcl │ ├── modules
│ │ ├── eks
│ │ │ ├── main.tf │ │ │ ├── variables.tf │ │ │ └── outputs.tf │ │ ├── kinesis
│ │ │ ├── main.tf │ │ │ ├── variables.tf │ │ │ └── outputs.tf │ │ ├── kms
│ │ │ └── main.tf │ │ ├── networking
│ │ │ └── main.tf │ │ └── vpc
│ │ ├── main.tf │ │ ├── variables.tf │ │ └── outputs.tf │ └── terragrunt.hcl └── README.md

yaml
Copy code

---

## Setup Steps

Follow these steps to set up and deploy the Poseidon project on AWS.

### 1. Clone the Repository

First, clone the backend project repository and navigate to the `AWS` directory.

```bash
git clone https://github.com/your-repo/backend-project.git
cd backend-project/AWS
2. Configure AWS Credentials
Ensure your AWS credentials are configured. You can set them using the AWS CLI:

bash
Copy code
aws configure
Provide your AWS Access Key ID, Secret Access Key, default region, and output format when prompted.

If you don't have the AWS CLI installed, the setup script will handle it.

3. Setup Environment
Run the setup script to install necessary tools such as AWS CLI, Terraform, Terragrunt, kubectl, and Helm.

bash
Copy code
chmod +x scripts/setup.sh
./scripts/setup.sh
What the Setup Script Does:

Installs AWS CLI: Command-line tool for interacting with AWS services.
Installs Terraform: Infrastructure as Code (IaC) tool for provisioning AWS resources.
Installs Terragrunt: A thin wrapper for Terraform that provides extra features like DRY configurations and remote state management.
Installs kubectl: Kubernetes command-line tool for interacting with the cluster.
Installs Helm: Kubernetes package manager for deploying applications.
Installs kubectx and kubens (optional): Tools for switching between Kubernetes contexts and namespaces.
Ensure you have administrative privileges to execute the script.

4. Initialize and Deploy Infrastructure
Navigate to the Terraform environment directory and deploy the infrastructure using Terragrunt.

bash
Copy code
cd terraform/environments/prod
terragrunt init
terragrunt apply -auto-approve
What This Step Does:

Provisions AWS Resources: VPC, subnets, EKS cluster, IAM roles, security groups, and Kinesis streams.
Manages Remote State: Uses S3 bucket and DynamoDB table for storing Terraform state and managing locks.
Note: Ensure that the S3 bucket (poseidon-terraform-state) and DynamoDB table (poseidon-terraform-lock) exist. If not, create them via the AWS Console or using Terraform scripts.

5. Deploy Applications to Kubernetes
After the infrastructure is up, deploy the Kubernetes applications using the deploy script.

A. Run the Deploy Script
bash
Copy code
cd ../../scripts
chmod +x deploy.sh
./deploy.sh
What the Deploy Script Does:

Applies Kubernetes Manifests: Deployments, Services, ConfigMaps, Secrets, PersistentVolumeClaims.
Installs NGINX Ingress Controller: Manages external access to services.
Deploys CronJobs: Sets up scheduled backups for MongoDB.
Deploys ELK Stack and DataDog: For logging and monitoring.
Deploys Poseidon Application via Helm: Manages API, Frontend, MongoDB, and Logstash.
Important: Replace <YOUR_DATADOG_API_KEY> and <YOUR_DATadog_APP_KEY> in the deploy.sh script with your actual DataDog credentials before executing.

B. Verify Deployments
Check the status of your deployments to ensure everything is running correctly.

bash
Copy code
kubectl get pods -A
You should see pods for the API, MongoDB, Frontend, Logstash, Ingress Controller, Elasticsearch, Kibana, DataDog agents, etc., all in the Running state.

C. Access the Application
After successful deployment, access your application via the Load Balancer URL associated with your Ingress. You can find the Load Balancer DNS in the poseidon-ingress service.

bash
Copy code
kubectl get ingress poseidon-ingress
Example Output:

css
Copy code
NAME               CLASS    HOSTS              ADDRESS                                         PORTS   AGE
poseidon-ingress   <none>   your-domain.com     abcdef1234567890.elb.amazonaws.com             80      10m
Navigate to http://your-domain.com in your browser to access the frontend and http://your-domain.com/api for the API.

6. Configure Logging and Monitoring
Integrate essential tools to ensure robust logging, monitoring, and incident management.

A. Logging with ELK Stack
1. Deploy Elasticsearch and Kibana
Use Helm charts to deploy Elasticsearch and Kibana on your EKS cluster.

bash
Copy code
helm repo add elastic https://helm.elastic.co
helm repo update

# Install Elasticsearch
helm install elasticsearch elastic/elasticsearch -n logging --create-namespace

# Install Kibana
helm install kibana elastic/kibana -n logging
2. Deploy Logstash via Helm
Logstash is now managed via Helm charts.

bash
Copy code
helm upgrade --install poseidon-app ../../helm-charts/poseidon-app --namespace poseidon --create-namespace
Benefits:

Centralized Management: All components managed through Helm.
Ease of Updates: Simplifies the process of upgrading or rolling back deployments.
B. Monitoring with DataDog
1. Install DataDog Agent
Use the official DataDog Helm chart to install the DataDog agent.

bash
Copy code
helm repo add datadog https://helm.datadoghq.com
helm repo update

helm install datadog-agent datadog/datadog \
  --set datadog.apiKey=<YOUR_DATADOG_API_KEY> \
  --set datadog.appKey=<YOUR_DATADog_APP_KEY> \
  --set agents.image.tag=latest \
  --namespace datadog --create-namespace
Replace <YOUR_DATADOG_API_KEY> and <YOUR_DATadog_APP_KEY> with your actual DataDog credentials.

2. Configure Dashboards and Alerts
Dashboards: Utilize DataDog's pre-built dashboards or create custom ones tailored to your application's metrics.
Alerts: Set up alerts to notify you via PagerDuty when certain thresholds or anomalies are detected.
C. Incident Management with PagerDuty
1. Integrate DataDog with PagerDuty
In DataDog:

Navigate to Integrations > Integrations.
Search for PagerDuty and click Install.
Configure the Integration:

Enter your PagerDuty Service Key.
Configure event rules to map DataDog alerts to PagerDuty incidents.
2. Set Up PagerDuty Services
Create a Service: In PagerDuty, create a new service for Poseidon.
Configure Integrations: Use the service key in DataDog to link alerts.
D. Data Streaming with AWS Kinesis
1. Create Kinesis Streams
Use Terraform to create Kinesis streams for real-time data ingestion.

Path: C:\Users\chelo\Poseidon\AWS\terraform\modules\kinesis\main.tf

hcl
Copy code
resource "aws_kinesis_stream" "poseidon_stream" {
  name        = "${var.project_name}-stream"
  shard_count = 1

  retention_period = 24

  tags = {
    Name = "${var.project_name}-stream"
  }
}
variables.tf

hcl
Copy code
variable "project_name" {
  description = "Name of the project"
  type        = string
}
outputs.tf

hcl
Copy code
output "kinesis_stream_name" {
  description = "Name of the Kinesis stream"
  value       = aws_kinesis_stream.poseidon_stream.name
}
2. Integrate Kinesis with Your Application
Modify your application code to publish and consume data from Kinesis streams as needed.

Example: Publishing to Kinesis in Node.js

javascript
Copy code
const AWS = require('aws-sdk');
const kinesis = new AWS.Kinesis({ region: 'us-west-2' });

const publishToKinesis = (data) => {
  const params = {
    Data: JSON.stringify(data),
    PartitionKey: 'partitionKey', // Replace with a suitable partition key
    StreamName: 'poseidon-stream'
  };

  kinesis.putRecord(params, (err, data) => {
    if (err) console.error(err);
    else console.log('Data published to Kinesis:', data);
  });
};
7. Implement Security Best Practices
Ensuring the security of your infrastructure and applications is paramount. Below are the key security measures implemented in the Poseidon project.

A. Site Reliability Engineering (SRE) Implementations
1. Monitoring and Alerting:

DataDog: Comprehensive monitoring of infrastructure and application metrics.
Prometheus & Grafana (Optional): For granular metrics and custom dashboards.
PagerDuty: Incident management ensuring timely responses to alerts.
2. Automation and CI/CD:

Terragrunt & Terraform: Automate infrastructure provisioning, ensuring consistency and repeatability.
Helm: Manages Kubernetes deployments, enabling reproducible and scalable deployments.
3. Resilience and Scalability:

Kubernetes (EKS): Provides auto-scaling capabilities for applications.
AWS Kinesis: Handles real-time data streaming, ensuring efficient data processing.
B. DevSecOps Implementations
1. Container Security:

Snyk & Aqua Security: Integrate tools to scan container images for vulnerabilities.
Regular Updates: Keep container images updated to patch known vulnerabilities.
2. Infrastructure as Code Security:

Terraform: Securely manage infrastructure configurations.
Terragrunt: Enforce DRY principles and manage remote state securely.
3. Secrets Management:

HashiCorp Vault: Centralized secrets management.
AWS Secrets Manager: Securely store and manage sensitive information.
External Secrets Operator: Synchronizes secrets between AWS Secrets Manager and Kubernetes.
C. Security/Attack Surface Mitigations
1. Network Security:

Security Groups: Configure to allow only necessary inbound and outbound traffic.
Network Policies: Implement Kubernetes Network Policies to restrict pod-to-pod communication.
2. Data Encryption:

AWS KMS: Encrypt data at rest.
TLS: Enable TLS for data in transit, especially for services exposed via Ingress.
3. Regular Updates and Patching:

Software Updates: Regularly update all software, dependencies, and Kubernetes components to mitigate vulnerabilities.
Automated Patch Management: Utilize automation tools to manage patches efficiently.
4. Logging and Auditing:

ELK Stack: Comprehensive logging for auditing and troubleshooting.
Log Monitoring: Continuously monitor logs for suspicious activities or anomalies.
5. Least Privilege Principle:

IAM Roles: Assign minimal necessary permissions to IAM roles.
Role-Based Access Control (RBAC): Implement RBAC in Kubernetes to control user access.
Tools and Technologies
Terraform & Terragrunt: Infrastructure as Code tools for AWS resource provisioning.
Kubernetes (EKS): Container orchestration platform.
Helm: Kubernetes package manager.
ELK Stack: Logging and log analysis.
DataDog: Monitoring and analytics.
PagerDuty: Incident response management.
AWS Kinesis: Real-time data streaming.
OpenSearch: Search and analytics engine.
Vault by HashiCorp: Secrets management.
Prometheus & Grafana: Metrics collection and visualization.
Istio/Linkerd: Service mesh for advanced traffic management and security.
Snyk/Aqua Security: Container security scanning.
External Secrets: Integrate AWS Secrets Manager with Kubernetes.
Troubleshooting
Encountering issues is natural. Here's how to address common problems:

1. Terraform Errors
Issue: Terraform or Terragrunt fails to initialize or apply.
Solution:
Check Versions: Ensure Terraform and Terragrunt versions are compatible.
AWS Permissions: Verify that your AWS IAM user has the necessary permissions.
Network Connectivity: Ensure stable internet connection and AWS service availability.
Error Logs: Review error messages for specific issues.
2. Kubernetes Issues
Issue: kubectl commands fail or pods are not running.
Solution:
Kubeconfig: Ensure your kubeconfig is correctly set.
EKS Cluster Status: Check the EKS cluster status in the AWS Console.
Pod Logs: Inspect pod logs using kubectl logs for errors.
Resource Limits: Verify that sufficient resources are allocated.
3. Helm Deployment Failures
Issue: Helm charts fail to install or upgrade.
Solution:
Helm Version: Ensure Helm is installed and updated correctly.
Chart Configurations: Validate Helm chart configurations for errors.
Resource Conflicts: Check for resource conflicts or missing dependencies.
Namespace Issues: Ensure the target namespace exists and is correct.
4. Logging and Monitoring Issues
Issue: Logs are not appearing in Kibana or metrics not showing in DataDog.
Solution:
Logstash Configuration: Verify Logstash is correctly configured to send logs to Elasticsearch.
Elasticsearch & Kibana Pods: Ensure Elasticsearch and Kibana pods are running.
DataDog Agent: Check DataDog agent logs for connectivity issues.
Permissions: Ensure proper permissions for accessing logs and metrics.
5. Ingress Issues
Issue: Application is not accessible via the Load Balancer URL.
Solution:
DNS Records: Ensure DNS records point to the correct Load Balancer.
SSL Certificates: Verify SSL certificates are correctly configured if using HTTPS.
Ingress Controller Logs: Check logs for any errors or misconfigurations.
Firewall Rules: Ensure firewall rules allow traffic on necessary ports.
Additional Resources
Terraform Documentation
Terragrunt Documentation
Kubernetes Documentation
Helm Documentation
DataDog Documentation
PagerDuty Documentation
AWS EKS Documentation
Elastic Helm Charts
DataDog Helm Charts
Logstash Configuration
AWS Kinesis Documentation
HashiCorp Vault Documentation
Prometheus Documentation
Grafana Documentation
Istio Documentation
Snyk Documentation
Aqua Security Documentation
External Secrets Documentation
kubectx and kubens Documentation
Summary
Reorganized Helm Charts: Separated Logstash manifests into their own directory within Helm charts.
Removed Redundant Manifests: Eliminated standalone Logstash deployment and service YAMLs to prevent duplication.
Updated Deployment Scripts: Modified deploy.sh to deploy the Poseidon application via Helm and removed redundant kubectl apply commands.
Refactored Terragrunt Configurations: Structured Terragrunt to manage multiple Terraform modules effectively.
Finalized Directory Structure: Ensured a clean and organized project structure.
Comprehensive README.md: Provided a detailed guide covering setup, deployment, SRE, DevSecOps, and security best practices.
This structured approach ensures that the Poseidon project is not only robust and scalable but also secure and maintainable. By following the README.md, even beginners can deploy and manage the project with ease.

Happy Deploying! 🚀

If you encounter any issues or have further questions, feel free to reach out for assistance.

markdown
Copy code

---

If you have any further questions or need additional assistance, feel free to ask!
