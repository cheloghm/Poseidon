# SRE and Security Guide

Welcome to the **Poseidon** project's **SRE (Site Reliability Engineering)** and **Security** guide for **Amazon Web Services (AWS)**. This comprehensive documentation outlines the setup and configuration of essential SRE and security tools to ensure the reliability, scalability, and security of your Poseidon application deployed on AWS. Whether you're a seasoned DevOps engineer or a beginner, this guide is designed to be straightforward and easy to follow.

---

## **Table of Contents**

1. [Introduction](#1-introduction)
2. [Overview of SRE and Security Tools](#2-overview-of-sre-and-security-tools)
3. [Prerequisites](#3-prerequisites)
4. [Deploying SRE and Security Tools Using Helm](#4-deploying-sre-and-security-tools-using-helm)
    - [4.1. Deploy Elasticsearch](#41-deploy-elasticsearch)
    - [4.2. Deploy Kibana](#42-deploy-kibana)
    - [4.3. Deploy Logstash](#43-deploy-logstash)
    - [4.4. Deploy DataDog](#44-deploy-datadog)
    - [4.5. Deploy Trivy](#45-deploy-trivy)
    - [4.6. Deploy PagerDuty](#46-deploy-pagerduty)
5. [Configuring Network Policies](#5-configuring-network-policies)
6. [Implementing Role-Based Access Control (RBAC)](#6-implementing-role-based-access-control-rbac)
7. [Managing Secrets Securely](#7-managing-secrets-securely)
8. [Integrating PagerDuty with DataDog](#8-integrating-pagerduty-with-datadog)
9. [Setting Up Vulnerability Scanning with Trivy](#9-setting-up-vulnerability-scanning-with-trivy)
10. [Monitoring and Logging](#10-monitoring-and-logging)
11. [Testing and Validation](#11-testing-and-validation)
12. [Troubleshooting](#12-troubleshooting)
13. [Additional Resources](#13-additional-resources)
14. [Conclusion](#14-conclusion)

---

## **1. Introduction**

In the realm of cloud-native applications, ensuring the reliability and security of your infrastructure is paramount. This guide provides detailed instructions to deploy and configure essential SRE and security tools within your AWS environment using **Helm** charts. These tools include **Elasticsearch**, **Kibana**, **Logstash**, **DataDog**, **Trivy**, **PagerDuty**, **Network Policies**, and **Role-Based Access Control (RBAC)**.

By following this guide, you'll establish a robust monitoring, logging, and security framework that enhances the observability and protection of your Poseidon application.

---

## **2. Overview of SRE and Security Tools**

Before diving into the deployment steps, it's essential to understand the purpose of each tool:

- **Elasticsearch:** A distributed search and analytics engine for log and data storage.
- **Kibana:** A visualization tool that works with Elasticsearch to provide dashboards and insights.
- **Logstash:** A data processing pipeline that ingests, transforms, and sends logs to Elasticsearch.
- **DataDog:** A monitoring and analytics platform for infrastructure and applications.
- **Trivy:** A vulnerability scanner for container images and filesystems.
- **PagerDuty:** An incident management platform that integrates with monitoring tools to notify teams of issues.
- **Network Policies:** Kubernetes resources that control the traffic flow between pods.
- **Role-Based Access Control (RBAC):** Kubernetes mechanism for regulating access to resources based on user roles.

---

## **3. Prerequisites**

Before proceeding, ensure you have the following:

- **AWS CLI Installed and Configured:**
  - [AWS CLI Installation Guide](https://docs.aws.amazon.com/cli/latest/userguide/cli-chap-install.html)
- **kubectl Installed and Configured:**
  - [kubectl Installation Guide](https://kubernetes.io/docs/tasks/tools/install-kubectl/)
- **Helm Installed:**
  - [Helm Installation Guide](https://helm.sh/docs/intro/install/)
- **Terraform Installed:**
  - [Terraform Installation Guide](https://learn.hashicorp.com/tutorials/terraform/install-cli)
- **Docker Installed:**
  - [Docker Installation Guide](https://docs.docker.com/get-docker/)
- **Poseidon Repository Cloned:**
  - Ensure you've cloned the Poseidon repository and navigated to the AWS directory.
  
  ```bash
  git clone https://github.com/your-username/poseidon.git
  cd poseidon/AWS
  ```

*Replace `your-username` with your actual GitHub username.*

---

## **4. Deploying SRE and Security Tools Using Helm**

Helm charts simplify the deployment and management of Kubernetes applications. We'll use Helm to deploy each SRE and security tool.

### **4.1. Deploy Elasticsearch**

Elasticsearch serves as the backend for storing and searching logs.

1. **Navigate to Elasticsearch Helm Directory:**

    ```bash
    cd sre-security/elasticsearch
    ```

2. **Review and Update `values.yaml`:**

    Open `values.yaml` and configure settings such as resource allocations, replicas, and storage options as needed.

    ```yaml
    # Example settings in values.yaml
    replicas: 3
    minimumMasterNodes: 2
    persistence:
      enabled: true
      size: 30Gi
    ```

3. **Install Elasticsearch Helm Chart:**

    ```bash
    helm install poseidon-elasticsearch . --namespace poseidon-demo --create-namespace
    ```

4. **Verify Deployment:**

    ```bash
    kubectl get pods -n poseidon-demo
    ```

    Ensure that all Elasticsearch pods are in the `Running` state.

### **4.2. Deploy Kibana**

Kibana provides a web interface for visualizing Elasticsearch data.

1. **Navigate to Kibana Helm Directory:**

    ```bash
    cd ../kibana
    ```

2. **Review and Update `values.yaml`:**

    Configure Kibana settings, including Elasticsearch host details.

    ```yaml
    # Example settings in values.yaml
    elasticsearchHosts: "http://poseidon-elasticsearch-master:9200"
    replicas: 2
    ingress:
      enabled: true
      annotations:
        kubernetes.io/ingress.class: "nginx"
      hosts:
        - host: kibana.your-domain.com
          paths:
            - /
      tls:
        - secretName: kibana-tls
          hosts:
            - kibana.your-domain.com
    ```

3. **Install Kibana Helm Chart:**

    ```bash
    helm install poseidon-kibana . --namespace poseidon-demo
    ```

4. **Verify Deployment:**

    ```bash
    kubectl get pods -n poseidon-demo
    ```

    Ensure that all Kibana pods are in the `Running` state.

### **4.3. Deploy Logstash**

Logstash processes and forwards logs to Elasticsearch.

1. **Navigate to Logstash Helm Directory:**

    ```bash
    cd ../logstash
    ```

2. **Review and Update `values.yaml`:**

    Configure Logstash settings, including input, filters, and output configurations.

    ```yaml
    # Example settings in values.yaml
    pipeline:
      logstash.conf: |
        input {
          http {
            port => 5000
            codec => json
            ssl => true
            ssl_certificate => "/etc/logstash/certs/logstash.crt"
            ssl_key => "/etc/logstash/certs/logstash.key"
          }
        }
        filter {
          mutate {
            add_field => { "source" => "frontend" }
          }
        }
        output {
          elasticsearch {
            hosts => ["http://poseidon-elasticsearch-master:9200"]
            index => "poseidon-frontend-logs-%{+YYYY.MM.dd}"
            user => "elastic"
            password => "changeme"
          }
        }
    service:
      type: ClusterIP
      port: 5000
    ```

    *Ensure that SSL certificates are properly mounted and paths are correct.*

3. **Install Logstash Helm Chart:**

    ```bash
    helm install poseidon-logstash . --namespace poseidon-demo
    ```

4. **Verify Deployment:**

    ```bash
    kubectl get pods -n poseidon-demo
    ```

    Ensure that all Logstash pods are in the `Running` state.

### **4.4. Deploy DataDog**

DataDog provides comprehensive monitoring and analytics for your applications and infrastructure.

1. **Navigate to DataDog Helm Directory:**

    ```bash
    cd ../datadog
    ```

2. **Review and Update `values.yaml`:**

    Configure DataDog agent settings, including API keys and integrations.

    ```yaml
    # Example settings in values.yaml
    datadog:
      apiKey: YOUR_DATADOG_API_KEY
      site: datadoghq.com
      logs:
        enabled: true
        containerCollectAll: true
    agents:
      image:
        tag: latest
    ```

    *Replace `YOUR_DATADOG_API_KEY` with your actual DataDog API key.*

3. **Install DataDog Helm Chart:**

    ```bash
    helm install datadog-agent . --namespace poseidon-demo
    ```

4. **Verify Deployment:**

    ```bash
    kubectl get pods -n poseidon-demo
    ```

    Ensure that all DataDog agent pods are in the `Running` state.

### **4.5. Deploy Trivy**

Trivy scans container images for vulnerabilities, ensuring the security of your deployments.

1. **Navigate to Trivy Helm Directory:**

    ```bash
    cd ../trivy
    ```

2. **Review and Update `values.yaml`:**

    Configure Trivy settings as needed.

    ```yaml
    # Example settings in values.yaml
    image:
      repository: aquasecurity/trivy
      tag: latest
    cronjob:
      schedule: "0 */6 * * *" # Every 6 hours
    ```

3. **Install Trivy Helm Chart:**

    ```bash
    helm install trivy . --namespace poseidon-demo
    ```

4. **Verify Deployment:**

    ```bash
    kubectl get pods -n poseidon-demo
    ```

    Ensure that the Trivy cronjob is scheduled and runs as expected.

### **4.6. Deploy PagerDuty**

PagerDuty manages incident responses and alerts based on monitoring data.

1. **Navigate to PagerDuty Helm Directory:**

    ```bash
    cd ../pagerduty
    ```

2. **Review and Update `values.yaml`:**

    Configure PagerDuty integration settings.

    ```yaml
    # Example settings in values.yaml
    pagerduty:
      serviceKey: YOUR_PAGERDUTY_SERVICE_KEY
    ```

    *Replace `YOUR_PAGERDUTY_SERVICE_KEY` with your actual PagerDuty service key.*

3. **Install PagerDuty Helm Chart:**

    ```bash
    helm install poseidon-pagerduty . --namespace poseidon-demo
    ```

4. **Verify Deployment:**

    ```bash
    kubectl get pods -n poseidon-demo
    ```

    Ensure that all PagerDuty pods are in the `Running` state.

---

## **5. Configuring Network Policies**

Network Policies in Kubernetes control the traffic flow between pods, enhancing the security of your cluster by restricting unauthorized access.

1. **Navigate to Network Policies Directory:**

    ```bash
    cd ../../sre-security/network-policies
    ```

2. **Review and Update `network-policy.yaml`:**

    Define ingress and egress rules to control pod communication.

    ```yaml
    # network-policy.yaml
    apiVersion: networking.k8s.io/v1
    kind: NetworkPolicy
    metadata:
      name: poseidon-network-policy
      namespace: poseidon-demo
    spec:
      podSelector: {}
      policyTypes:
        - Ingress
        - Egress
      ingress:
        - from:
            - podSelector:
                matchLabels:
                  app: poseidon-frontend
          ports:
            - protocol: TCP
              port: 80
      egress:
        - to:
            - podSelector:
                matchLabels:
                  app: poseidon-backend
          ports:
            - protocol: TCP
              port: 80
    ```

    *This example allows ingress traffic from the frontend to any pod in the `poseidon-demo` namespace and egress traffic to the backend.*

3. **Apply Network Policies:**

    ```bash
    kubectl apply -f network-policy.yaml -n poseidon-demo
    ```

4. **Verify Network Policies:**

    ```bash
    kubectl get networkpolicies -n poseidon-demo
    ```

    Ensure that the `poseidon-network-policy` is listed.

---

## **6. Implementing Role-Based Access Control (RBAC)**

RBAC restricts access to Kubernetes resources based on user roles, enhancing the security and management of your cluster.

1. **Navigate to RBAC Directory:**

    ```bash
    cd ../rbac
    ```

2. **Review and Update `rbac.yaml`:**

    Define roles and role bindings to manage permissions.

    ```yaml
    # rbac.yaml
    apiVersion: rbac.authorization.k8s.io/v1
    kind: Role
    metadata:
      namespace: poseidon-demo
      name: poseidon-role
    rules:
      - apiGroups: [""]
        resources: ["pods", "services", "deployments"]
        verbs: ["get", "list", "watch", "create", "update", "patch", "delete"]
      - apiGroups: ["apps"]
        resources: ["deployments", "replicasets"]
        verbs: ["get", "list", "watch", "create", "update", "patch", "delete"]
    
    ---
    
    apiVersion: rbac.authorization.k8s.io/v1
    kind: RoleBinding
    metadata:
      name: poseidon-rolebinding
      namespace: poseidon-demo
    subjects:
      - kind: User
        name: poseidon-user
        apiGroup: rbac.authorization.k8s.io
    roleRef:
      kind: Role
      name: poseidon-role
      apiGroup: rbac.authorization.k8s.io
    ```

    *This example grants comprehensive permissions to the `poseidon-user` within the `poseidon-demo` namespace.*

3. **Apply RBAC Policies:**

    ```bash
    kubectl apply -f rbac.yaml -n poseidon-demo
    ```

4. **Verify RBAC Policies:**

    ```bash
    kubectl get roles -n poseidon-demo
    kubectl get rolebindings -n poseidon-demo
    ```

    Ensure that the `poseidon-role` and `poseidon-rolebinding` are listed.

---

## **7. Managing Secrets Securely**

Secure management of secrets is crucial to protect sensitive information like API keys, passwords, and certificates.

1. **Navigate to Secrets Directory:**

    ```bash
    cd ../../secrets
    ```

2. **Review and Update `secrets.yaml`:**

    Define Kubernetes Secrets to store sensitive data.

    ```yaml
    # secrets.yaml
    apiVersion: v1
    kind: Secret
    metadata:
      name: poseidon-secrets
      namespace: poseidon-demo
    type: Opaque
    data:
      db-password: bXlfc3VwZXJzZWNyZXRfcGFzc3dvcmQ= # base64 encoded
      logstash-password: cGFzc3dvcmRfZmxvc3RzdGFjaA== # base64 encoded
      datadog-api-key: ZGF0YWRvZ19hcGlfa2V5X3ZhbHVl # base64 encoded
      pagerduty-service-key: cGFnZXJkdXB0eV9zZXJ2aWNlX2tleQ== # base64 encoded
    ```

    *Encode your actual secret values using base64 before updating the YAML.*

    ```bash
    echo -n 'your_db_password' | base64
    ```

3. **Apply Secrets:**

    ```bash
    kubectl apply -f secrets.yaml -n poseidon-demo
    ```

4. **Verify Secrets:**

    ```bash
    kubectl get secrets -n poseidon-demo
    ```

    Ensure that the `poseidon-secrets` are listed.

5. **Reference Secrets in Helm Charts:**

    Update your Helm `values.yaml` files to reference the created secrets.

    ```yaml
    # Example in backend/helm/backend/values.yaml
    env:
      - name: DATABASE_PASSWORD
        valueFrom:
          secretKeyRef:
            name: poseidon-secrets
            key: db-password
    ```

---

## **8. Integrating PagerDuty with DataDog**

Integrating PagerDuty with DataDog ensures that critical alerts trigger incident responses effectively.

1. **Obtain PagerDuty Integration Key:**

    - **Step 1:** Log in to your PagerDuty account.
    - **Step 2:** Navigate to **Services > Service Directory**.
    - **Step 3:** Select the service you want to integrate.
    - **Step 4:** Under **Integrations**, add a new **API Integration** and obtain the **Integration Key**.

2. **Update DataDog Helm `values.yaml`:**

    ```yaml
    # sre-security/pagerduty/values.yaml
    pagerduty:
      serviceKey: YOUR_PAGERDUTY_SERVICE_KEY
    ```

    *Replace `YOUR_PAGERDUTY_SERVICE_KEY` with the key obtained from PagerDuty.*

3. **Upgrade DataDog Helm Release:**

    ```bash
    cd ../../helm/datadog
    helm upgrade datadog-agent . --namespace poseidon-demo
    ```

4. **Verify Integration:**

    - **Trigger a Test Alert in DataDog:**
      - Navigate to DataDog dashboard.
      - Create a test monitor that triggers an alert.
    - **Check PagerDuty:**
      - Ensure that the alert appears in PagerDuty as an incident.

---

## **9. Setting Up Vulnerability Scanning with Trivy**

Trivy scans your container images for known vulnerabilities, ensuring the security of your deployments.

1. **Navigate to Trivy Helm Directory:**

    ```bash
    cd ../../trivy
    ```

2. **Review and Update `values.yaml`:**

    Configure Trivy settings, such as scan schedules and image repositories.

    ```yaml
    # values.yaml
    image:
      repository: aquasecurity/trivy
      tag: latest
    cronjob:
      schedule: "0 */6 * * *" # Every 6 hours
      image: aquasecurity/trivy
      args:
        - "--quiet"
        - "image"
        - "--no-progress"
        - "poseidon-backend:latest"
        - "poseidon-frontend:latest"
    ```

3. **Install Trivy Helm Chart:**

    ```bash
    helm install trivy . --namespace poseidon-demo
    ```

4. **Verify Deployment:**

    ```bash
    kubectl get pods -n poseidon-demo
    ```

    Ensure that the Trivy pods are scheduled and running as per the cronjob schedule.

5. **Check Trivy Reports:**

    Access Trivy scan results to review any identified vulnerabilities.

    ```bash
    kubectl logs <trivy-pod-name> -n poseidon-demo
    ```

    *Replace `<trivy-pod-name>` with the actual pod name.*

---

## **10. Monitoring and Logging**

Effective monitoring and logging are crucial for maintaining the health and performance of your applications.

1. **Elasticsearch, Kibana, Logstash Setup:**

    - **Elasticsearch:** Stores and indexes logs.
    - **Kibana:** Visualizes logs and provides dashboards.
    - **Logstash:** Processes and forwards logs from the frontend and other sources to Elasticsearch.

2. **DataDog Integration:**

    - **Metrics Collection:** DataDog collects metrics from your Kubernetes cluster, applications, and infrastructure.
    - **Dashboards:** Create customized dashboards in DataDog to monitor key performance indicators.
    - **Alerts:** Set up alerts to notify your team of any anomalies or critical issues.

3. **PagerDuty Integration:**

    - **Incident Management:** PagerDuty manages incident responses based on alerts from DataDog.
    - **Notifications:** Receive real-time notifications via email, SMS, or other channels when incidents occur.

---

## **11. Testing and Validation**

After deploying all SRE and security tools, it's essential to verify that everything is functioning correctly.

### **11.1. Verify Deployments**

1. **Check All Pods:**

    ```bash
    kubectl get pods -n poseidon-demo
    ```

    Ensure that all pods across Elasticsearch, Kibana, Logstash, DataDog, Trivy, and PagerDuty are in the `Running` state.

2. **Check Services:**

    ```bash
    kubectl get svc -n poseidon-demo
    ```

    Verify that all services are correctly exposed, especially Elasticsearch, Kibana, and Logstash.

### **11.2. Test Monitoring and Alerts**

1. **Generate Test Metrics:**

    - **Simulate High Load:** Deploy additional replicas or generate traffic to observe DataDog metrics.
    - **Simulate Failures:** Temporarily shut down a pod to trigger alerts.

2. **Check DataDog Dashboards:**

    - Verify that metrics are being collected and displayed correctly.
    - Ensure that alerts are configured and triggered as expected.

3. **Verify PagerDuty Alerts:**

    - Confirm that PagerDuty receives alerts from DataDog.
    - Test notification channels (email, SMS, etc.) to ensure they function correctly.

### **11.3. Test Logging Pipeline**

1. **Generate Logs:**

    - Perform actions in the Poseidon application (e.g., login, register, passenger management) to generate logs.

2. **Check Elasticsearch:**

    ```bash
    kubectl port-forward svc/poseidon-elasticsearch-master 9200:9200 -n poseidon-demo
    ```

    Access Elasticsearch at `http://localhost:9200` and verify that logs are being indexed.

3. **Access Kibana:**

    ```bash
    kubectl port-forward svc/poseidon-kibana 5601:5601 -n poseidon-demo
    ```

    Navigate to `http://localhost:5601` and verify that logs are visible in Kibana dashboards.

### **11.4. Test Vulnerability Scanning**

1. **Check Trivy Reports:**

    ```bash
    kubectl logs <trivy-pod-name> -n poseidon-demo
    ```

    Review the scan reports to identify any vulnerabilities in your container images.

---

## **12. Troubleshooting**

Encountering issues during deployment? Here's how to address common problems.

### **12.1. Pods Not Running**

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

### **12.2. Unable to Access Kibana**

- **Verify Service Exposure:**

    Ensure that the Kibana service is exposed correctly and that Ingress rules are properly configured.

- **Check TLS Certificates:**

    Confirm that TLS certificates are correctly referenced and valid.

### **12.3. DataDog Metrics Not Visible**

- **Check DataDog Agent Logs:**

    ```bash
    kubectl logs <datadog-agent-pod-name> -n poseidon-demo
    ```

- **Verify API Keys:**

    Ensure that the DataDog API key in `values.yaml` is correct.

### **12.4. Trivy Scan Failures**

- **Check Trivy Pod Logs:**

    ```bash
    kubectl logs <trivy-pod-name> -n poseidon-demo
    ```

- **Verify Image References:**

    Ensure that the images specified in the Trivy scan are correct and accessible.

### **12.5. PagerDuty Alerts Not Triggering**

- **Verify Integration Key:**

    Ensure that the PagerDuty service key in `values.yaml` is accurate.

- **Test Alert Configuration:**

    Generate a test alert in DataDog and verify that it triggers an incident in PagerDuty.

---

## **13. Additional Resources**

- **AWS EKS Documentation:** [Amazon EKS](https://docs.aws.amazon.com/eks/latest/userguide/what-is-eks.html)
- **Helm Documentation:** [Helm](https://helm.sh/docs/)
- **Terraform AWS Provider:** [Terraform AWS Provider](https://registry.terraform.io/providers/hashicorp/aws/latest/docs)
- **DataDog Kubernetes Integration:** [DataDog EKS Integration](https://docs.datadoghq.com/integrations/amazon_eks/)
- **Trivy Vulnerability Scanner:** [Trivy GitHub Repository](https://github.com/aquasecurity/trivy)
- **PagerDuty Documentation:** [PagerDuty](https://support.pagerduty.com/docs)
- **Kubernetes Network Policies:** [Kubernetes Network Policies](https://kubernetes.io/docs/concepts/services-networking/network-policies/)
- **Kubernetes RBAC:** [Kubernetes RBAC](https://kubernetes.io/docs/reference/access-authn-authz/rbac/)

---

## **14. Conclusion**

Congratulations! You've successfully deployed and configured the essential SRE and security tools for your Poseidon application on AWS. This setup ensures that your application is not only reliable and scalable but also secure and well-monitored. By implementing these tools, you enhance the observability, security, and incident management capabilities of your application, aligning with best practices in Site Reliability Engineering.

**Key Benefits:**

- **Enhanced Monitoring:** Real-time metrics and logs provide deep insights into application performance and health.
- **Proactive Incident Management:** Integration with PagerDuty ensures swift response to critical issues.
- **Security Assurance:** Vulnerability scanning with Trivy and secure access controls protect your infrastructure.
- **Scalability and Reliability:** Kubernetes, coupled with robust monitoring tools, ensures that your application can scale seamlessly while maintaining reliability.

**Next Steps:**

1. **Continuous Improvement:** Regularly update and refine your monitoring and security configurations based on evolving requirements.
2. **Automate Deployments:** Integrate CI/CD pipelines to automate the deployment of updates and configurations.
3. **Expand Observability:** Incorporate additional monitoring tools and dashboards to cover more aspects of your application.
4. **Regular Audits:** Conduct periodic security audits and vulnerability assessments to maintain a secure environment.

For any further assistance or inquiries, feel free to reach out or consult the additional resources provided.

Happy Deploying and Securing!

---

*This guide is maintained and updated regularly to reflect best practices and new features. Ensure you refer to the latest documentation for any changes or updates.*