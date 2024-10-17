# AWS Deployment Guide

Welcome to the **Poseidon** project deployment guide for **Amazon Web Services (AWS)**. This comprehensive, step-by-step guide will help you set up the entire Poseidon application stack on AWS, including the backend, frontend, database, and essential SRE and security tools. Whether you're a seasoned developer or a complete beginner, this guide is designed to be easy to follow, ensuring a smooth deployment experience.

---

## **Table of Contents**

1. [Prerequisites](#1-prerequisites)
2. [Setup AWS CLI](#2-setup-aws-cli)
3. [Install Required Tools](#3-install-required-tools)
4. [Clone the Poseidon Repository](#4-clone-the-poseidon-repository)
5. [Configure AWS Credentials](#5-configure-aws-credentials)
6. [Initialize and Apply Terraform Scripts](#6-initialize-and-apply-terraform-scripts)
7. [Configure kubectl for EKS](#7-configure-kubectl-for-eks)
8. [Build and Push Docker Images](#8-build-and-push-docker-images)
9. [Deploy Applications Using Helm](#9-deploy-applications-using-helm)
   - [9.1. Deploy Database](#91-deploy-database)
   - [9.2. Deploy Backend](#92-deploy-backend)
   - [9.3. Deploy Frontend](#93-deploy-frontend)
10. [Deploy SRE and Security Tools](#10-deploy-sre-and-security-tools)
    - [10.1. Deploy Elasticsearch](#101-deploy-elasticsearch)
    - [10.2. Deploy Kibana](#102-deploy-kibana)
    - [10.3. Deploy Logstash](#103-deploy-logstash)
    - [10.4. Deploy DataDog](#104-deploy-datadog)
    - [10.5. Integrate PagerDuty with DataDog](#105-integrate-pagerduty-with-datadog)
    - [10.6. Deploy Trivy for Vulnerability Scanning](#106-deploy-trivy-for-vulnerability-scanning)
11. [Configure Ingress and Secure Access](#11-configure-ingress-and-secure-access)
12. [Manage Secrets Securely](#12-manage-secrets-securely)
13. [Implement Kubernetes Network Policies and RBAC](#13-implement-kubernetes-network-policies-and-rbac)
14. [Testing and Validation](#14-testing-and-validation)
15. [Accessing Kibana](#15-accessing-kibana)
16. [Final Steps and Cleanup](#16-final-steps-and-cleanup)
17. [Troubleshooting](#17-troubleshooting)
18. [Additional Resources](#18-additional-resources)

---

## **1. Prerequisites**

Before you begin, ensure you have the following:

- **AWS Account:** [Sign up for AWS](https://aws.amazon.com/) if you don't have one.
- **Permissions:** Ensure your AWS IAM user has the necessary permissions to create and manage EKS clusters, VPCs, and other resources.
- **Basic Knowledge:** Familiarity with command-line interfaces and basic Kubernetes concepts.

---

## **2. Setup AWS CLI**

The AWS Command Line Interface (CLI) allows you to interact with AWS services from your terminal.

### **Step 1: Install AWS CLI**

- **For Windows:**
  1. Download the [AWS CLI MSI installer for Windows](https://awscli.amazonaws.com/AWSCLIV2.msi).
  2. Run the installer and follow the on-screen instructions.

- **For macOS:**
  ```bash
  curl "https://awscli.amazonaws.com/AWSCLIV2.pkg" -o "AWSCLIV2.pkg"
  sudo installer -pkg AWSCLIV2.pkg -target /
  ```

- **For Linux:**
  ```bash
  curl "https://awscli.amazonaws.com/awscli-exe-linux-x86_64.zip" -o "awscliv2.zip"
  unzip awscliv2.zip
  sudo ./aws/install
  ```

### **Step 2: Verify Installation**

```bash
aws --version
```

You should see output similar to `aws-cli/2.4.0 Python/3.8.8 Linux/5.4.0-1045-aws exe/x86_64.ubuntu.20 prompt/off`.

---

## **3. Install Required Tools**

Ensure you have the following tools installed on your machine:

1. **Terraform:** Infrastructure as Code (IaC) tool.
   - **Download:** [Terraform Downloads](https://www.terraform.io/downloads.html)
   - **Install:**
     - **For Windows:** Extract the `terraform.exe` to a directory included in your `PATH`.
     - **For macOS/Linux:**
       ```bash
       sudo mv terraform /usr/local/bin/
       ```

2. **kubectl:** Kubernetes command-line tool.
   - **Install via AWS CLI:**
     ```bash
     aws eks --help
     ```
     Alternatively, follow the [official kubectl installation guide](https://kubernetes.io/docs/tasks/tools/install-kubectl/).

3. **Helm:** Kubernetes package manager.
   - **Install via Script:**
     ```bash
     curl https://raw.githubusercontent.com/helm/helm/master/scripts/get-helm-3 | bash
     ```
   - **Verify Installation:**
     ```bash
     helm version
     ```

4. **Docker:** To build and push Docker images.
   - **Download:** [Docker Desktop](https://www.docker.com/products/docker-desktop)
   - **Install and Start Docker Desktop.**

5. **Git:** Version control system.
   - **Download:** [Git Downloads](https://git-scm.com/downloads)
   - **Install and Configure Git.**

6. **Lodash:** Utility library (already handled in project dependencies).

---

## **4. Clone the Poseidon Repository**

Clone the Poseidon project repository to your local machine.

```bash
git clone https://github.com/your-username/poseidon.git
cd poseidon/AWS
```

*Replace `your-username` with your actual GitHub username.*

---

## **5. Configure AWS Credentials**

Set up your AWS credentials to allow Terraform and AWS CLI to interact with your AWS account.

### **Step 1: Configure AWS CLI**

Run the following command and follow the prompts to enter your AWS Access Key ID, Secret Access Key, region, and output format.

```bash
aws configure
```

- **Access Key ID:** Your AWS access key.
- **Secret Access Key:** Your AWS secret key.
- **Default region name:** e.g., `us-west-2`.
- **Default output format:** e.g., `json`.

### **Step 2: Verify Credentials**

```bash
aws sts get-caller-identity
```

You should see details about your AWS account and user.

---

## **6. Initialize and Apply Terraform Scripts**

Terraform scripts will set up the necessary AWS infrastructure, including the EKS cluster, VPC, and other resources.

### **Step 1: Navigate to Terraform Directory**

```bash
cd terraform
```

### **Step 2: Initialize Terraform**

This command initializes the Terraform working directory, downloads the necessary providers, and sets up the backend.

```bash
terraform init
```

### **Step 3: Review Terraform Plan**

Generate and review the execution plan to understand what Terraform will create.

```bash
terraform plan -out=plan.out
```

### **Step 4: Apply Terraform Plan**

Apply the Terraform plan to create the AWS infrastructure.

```bash
terraform apply plan.out
```

*Note: This process may take several minutes.*

### **Step 5: Verify EKS Cluster Creation**

After Terraform completes, verify that the EKS cluster has been created.

```bash
aws eks list-clusters
```

You should see your cluster listed.

---

## **7. Configure kubectl for EKS**

Set up `kubectl` to interact with your newly created EKS cluster.

### **Step 1: Update kubeconfig**

Use the following command to update your kubeconfig with the EKS cluster details.

```bash
aws eks update-kubeconfig --region your-region --name your-cluster-name
```

*Replace `your-region` and `your-cluster-name` with your actual AWS region and EKS cluster name.*

### **Step 2: Verify kubectl Connection**

```bash
kubectl get nodes
```

You should see a list of worker nodes in your EKS cluster.

---

## **8. Build and Push Docker Images**

Build Docker images for the backend and frontend applications and push them to a container registry (e.g., Docker Hub or Amazon ECR).

### **Step 1: Log in to Docker Registry**

- **For Docker Hub:**
  ```bash
  docker login
  ```

- **For Amazon ECR:**
  ```bash
  aws ecr get-login-password --region your-region | docker login --username AWS --password-stdin your-aws-account-id.dkr.ecr.your-region.amazonaws.com
  ```

### **Step 2: Build Backend Docker Image**

```bash
cd ../backend
docker build -t your-dockerhub-username/poseidon-backend:latest .
```

*Replace `your-dockerhub-username` with your Docker Hub username.*

### **Step 3: Push Backend Docker Image**

```bash
docker push your-dockerhub-username/poseidon-backend:latest
```

### **Step 4: Build Frontend Docker Image**

```bash
cd ../frontend
docker build -t your-dockerhub-username/poseidon-frontend:latest .
```

### **Step 5: Push Frontend Docker Image**

```bash
docker push your-dockerhub-username/poseidon-frontend:latest
```

*Alternatively, push to Amazon ECR if preferred.*

---

## **9. Deploy Applications Using Helm**

Helm charts manage Kubernetes applications. We'll deploy the database, backend, and frontend using the provided Helm charts.

### **Prerequisites:**

- Ensure Helm is installed (`helm version`).
- Ensure you're in the AWS directory: `cd C:\Users\chelo\Poseidon\AWS`

### **Step 1: Add Helm Repositories**

Add any required Helm repositories (if not already added).

```bash
helm repo add elastic https://helm.elastic.co
helm repo add datadog https://helm.datadoghq.com
helm repo update
```

### **Step 2: Deploy Database**

Assuming you're using a Helm chart for the database (e.g., PostgreSQL).

```bash
cd helm/db
helm install poseidon-db . --namespace poseidon-demo --create-namespace
```

*Explanation:*
- **`poseidon-db`:** Release name.
- **`.`:** Current directory (contains Chart.yaml and values.yaml).
- **`--namespace poseidon-demo`:** Deploy into `poseidon-demo` namespace.
- **`--create-namespace`:** Create the namespace if it doesn't exist.

### **Step 3: Deploy Backend**

```bash
cd ../backend
helm install poseidon-backend . --namespace poseidon-demo
```

*Explanation:*
- **`poseidon-backend`:** Release name.
- **`--namespace poseidon-demo`:** Deploy into `poseidon-demo` namespace.

### **Step 4: Deploy Frontend**

```bash
cd ../frontend
helm install poseidon-frontend . --namespace poseidon-demo
```

*Explanation:*
- **`poseidon-frontend`:** Release name.
- **`--namespace poseidon-demo`:** Deploy into `poseidon-demo` namespace.

### **Step 5: Verify Deployments**

```bash
kubectl get all -n poseidon-demo
```

Ensure that all pods, services, and deployments are up and running.

---

## **10. Deploy SRE and Security Tools**

Deploy essential tools like Elasticsearch, Kibana, Logstash, DataDog, PagerDuty, and Trivy to ensure observability, monitoring, and security.

### **10.1. Deploy Elasticsearch**

```bash
cd ../../sre-security/elasticsearch
helm install poseidon-elasticsearch . --namespace poseidon-demo
```

### **10.2. Deploy Kibana**

```bash
cd ../kibana
helm install poseidon-kibana . --namespace poseidon-demo
```

### **10.3. Deploy Logstash**

```bash
cd ../logstash
helm install poseidon-logstash . --namespace poseidon-demo
```

### **10.4. Deploy DataDog**

```bash
cd ../datadog
helm install datadog-agent . --namespace poseidon-demo
```

*Ensure that you have set up DataDog API keys in `values.yaml` before deploying.*

### **10.5. Integrate PagerDuty with DataDog**

- **Step 1:** Obtain PagerDuty Integration Key.
- **Step 2:** Update DataDog Helm `values.yaml` with PagerDuty webhook URL.
- **Step 3:** Apply the updated Helm chart.

```bash
# After updating values.yaml
helm upgrade datadog-agent . --namespace poseidon-demo
```

### **10.6. Deploy Trivy for Vulnerability Scanning**

```bash
cd ../trivy
helm install trivy . --namespace poseidon-demo
```

---

## **11. Configure Ingress and Secure Access**

Set up Ingress to manage external access to your services securely.

### **Step 1: Deploy Ingress Controller**

Assuming you're using NGINX Ingress Controller.

```bash
kubectl apply -f ingress/ingress.yaml
```

### **Step 2: Configure Ingress Rules**

Ensure that `ingress.yaml` contains the necessary rules to route traffic to frontend and backend services.

### **Step 3: Secure Ingress with TLS**

- **Generate TLS Certificates:** Use Let's Encrypt or self-signed certificates.
- **Update Ingress Configuration:** Reference the TLS certificates in your `ingress.yaml`.

```yaml
# Example ingress.yaml snippet
apiVersion: networking.k8s.io/v1
kind: Ingress
metadata:
  name: poseidon-ingress
  namespace: poseidon-demo
  annotations:
    nginx.ingress.kubernetes.io/ssl-redirect: "true"
spec:
  tls:
    - hosts:
        - your-domain.com
      secretName: poseidon-tls
  rules:
    - host: your-domain.com
      http:
        paths:
          - path: /
            pathType: Prefix
            backend:
              service:
                name: poseidon-frontend
                port:
                  number: 80
          - path: /api
            pathType: Prefix
            backend:
              service:
                name: poseidon-backend
                port:
                  number: 80
```

### **Step 4: Implement Authentication**

- **Use Basic Auth:** Add annotations to enforce authentication.
- **Update Ingress Configuration:**

```yaml
# Example adding basic auth
metadata:
  annotations:
    nginx.ingress.kubernetes.io/auth-type: "basic"
    nginx.ingress.kubernetes.io/auth-secret: "poseidon-basic-auth"
    nginx.ingress.kubernetes.io/auth-realm: "Authentication Required"
```

- **Create Secret for Basic Auth:**

```bash
htpasswd -c auth poseidon-user
kubectl create secret generic poseidon-basic-auth --from-file=auth -n poseidon-demo
```

---

## **12. Manage Secrets Securely**

Use Kubernetes Secrets to manage sensitive information like API keys, passwords, and certificates.

### **Step 1: Create Secrets**

```bash
kubectl apply -f secrets/secrets.yaml -n poseidon-demo
```

*Ensure `secrets.yaml` contains all necessary secrets in a secure format.*

### **Step 2: Reference Secrets in Helm Charts**

Update your Helm `values.yaml` files to reference the created secrets.

```yaml
# Example in backend/values.yaml
env:
  - name: DATABASE_PASSWORD
    valueFrom:
      secretKeyRef:
        name: poseidon-secrets
        key: db-password
```

---

## **13. Implement Kubernetes Network Policies and RBAC**

Enhance the security of your Kubernetes cluster by defining network policies and role-based access controls.

### **13.1. Apply Network Policies**

```bash
cd ../../sre-security/network-policies
kubectl apply -f network-policy.yaml -n poseidon-demo
```

*Ensure `network-policy.yaml` defines the necessary ingress and egress rules.*

### **13.2. Apply RBAC Policies**

```bash
cd ../rbac
kubectl apply -f rbac.yaml -n poseidon-demo
```

*Ensure `rbac.yaml` defines roles and role bindings appropriately.*

---

## **14. Testing and Validation**

Ensure that all services are deployed correctly and functioning as expected.

### **Step 1: Verify Deployments**

```bash
kubectl get all -n poseidon-demo
```

Ensure that all pods are running without errors.

### **Step 2: Access the Application**

Navigate to `https://your-domain.com` in your browser. Test all functionalities:

- **Login and Registration**
- **Passenger Management**
- **Profile Editing**

### **Step 3: Check Monitoring Dashboards**

- **DataDog:** Access DataDog dashboard to monitor metrics.
- **Kibana:** Use Kibana to view logs and ensure logs are being captured.

### **Step 4: Test Alerts and Incident Workflows**

Trigger test alerts to ensure that PagerDuty receives notifications from DataDog.

---

## **15. Accessing Kibana**

Kibana provides a web interface to interact with Elasticsearch and view logs.

### **Step 1: Retrieve Kibana Service URL**

```bash
kubectl get svc poseidon-kibana -n poseidon-demo
```

*Note the `EXTERNAL-IP` and port.*

### **Step 2: Access Kibana**

Navigate to `http://<EXTERNAL-IP>:5601` in your browser.

### **Step 3: Configure Index Patterns**

1. **Login to Kibana** if authentication is enabled.
2. **Navigate to Management > Stack Management > Index Patterns.**
3. **Create an Index Pattern:** e.g., `poseidon-frontend-logs-*`
4. **Set the Timestamp Field:** Select the appropriate field for time-based filtering.

### **Step 4: Explore Logs**

Use Kibana's Discover, Dashboards, and Visualize sections to explore and analyze your application's logs.

---

## **16. Final Steps and Cleanup**

### **Step 1: Securely Share Credentials**

Provide recruiters with access credentials securely, avoiding plain text methods.

- **Use Encrypted Emails or Password Managers:**
  - Share AWS access details, Ingress URLs, and any other necessary information.

### **Step 2: Document Everything**

Ensure that all steps are documented in the `AWSDeploymentGuide.md` and `SREAndSecurity.md` within the `docs` directory.

### **Step 3: Regular Maintenance**

- **Update Helm Charts:** Keep your Helm charts up to date with the latest configurations.
- **Monitor Logs and Metrics:** Regularly check DataDog and Kibana for any anomalies.
- **Apply Security Patches:** Keep your Kubernetes cluster and applications updated with security patches.

---

## **17. Troubleshooting**

Encountering issues? Here are some common problems and their solutions.

### **Issue 1: Pods Not Running**

- **Check Pod Status:**
  ```bash
  kubectl get pods -n poseidon-demo
  ```
- **Describe Pod for Details:**
  ```bash
  kubectl describe pod <pod-name> -n poseidon-demo
  ```
- **Check Logs:**
  ```bash
  kubectl logs <pod-name> -n poseidon-demo
  ```

### **Issue 2: Unable to Access Services**

- **Verify Ingress Configuration:**
  ```bash
  kubectl describe ingress poseidon-ingress -n poseidon-demo
  ```
- **Check TLS Certificates:** Ensure certificates are correctly configured.

### **Issue 3: Logs Not Appearing in Kibana**

- **Check Logstash Pods:**
  ```bash
  kubectl get pods -n poseidon-demo | grep logstash
  kubectl logs <logstash-pod-name> -n poseidon-demo
  ```
- **Verify Elasticsearch Connection:** Ensure Logstash can communicate with Elasticsearch.

### **Issue 4: DataDog Metrics Not Visible**

- **Check DataDog Agent Status:**
  ```bash
  kubectl get pods -n poseidon-demo | grep datadog
  kubectl logs <datadog-pod-name> -n poseidon-demo
  ```
- **Verify API Keys:** Ensure DataDog API keys are correctly set in Helm `values.yaml`.

---

## **18. Additional Resources**

- **AWS EKS Documentation:** [Amazon EKS](https://docs.aws.amazon.com/eks/latest/userguide/what-is-eks.html)
- **Terraform AWS Provider:** [Terraform AWS Provider](https://registry.terraform.io/providers/hashicorp/aws/latest/docs)
- **Helm Documentation:** [Helm](https://helm.sh/docs/)
- **Kubernetes Documentation:** [Kubernetes](https://kubernetes.io/docs/home/)
- **DataDog Kubernetes Integration:** [DataDog EKS Integration](https://docs.datadoghq.com/integrations/amazon_eks/)
- **Trivy Vulnerability Scanner:** [Trivy](https://github.com/aquasecurity/trivy)

---

## **Conclusion**

Congratulations! You've successfully deployed the Poseidon application stack on AWS using Amazon EKS, Terraform, Helm, and essential SRE and security tools. This setup ensures a scalable, secure, and observable environment for your application.

**Key Takeaways:**

- **Infrastructure as Code:** Utilizing Terraform and Helm for reproducible deployments.
- **Observability:** Implementing Elasticsearch, Kibana, Logstash, and DataDog for comprehensive monitoring.
- **Security:** Enforcing best practices with Network Policies, RBAC, and secure secrets management.
- **Scalability:** Leveraging Kubernetes' powerful orchestration capabilities to scale your applications seamlessly.

**Next Steps:**

1. **Continuous Integration/Continuous Deployment (CI/CD):** Set up CI/CD pipelines for automated deployments.
2. **Enhance Security:** Implement advanced security measures like Pod Security Policies and Service Meshes.
3. **Optimize Performance:** Continuously monitor and optimize your application's performance based on collected metrics and logs.

For any further assistance or inquiries, feel free to reach out!

Happy Deploying!