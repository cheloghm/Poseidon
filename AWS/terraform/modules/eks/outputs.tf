output "eks_cluster_id" {
  description = "EKS Cluster ID"
  value       = module.eks.cluster_id
}

output "eks_cluster_endpoint" {
  description = "EKS Cluster endpoint"
  value       = module.eks.cluster_endpoint
}

output "eks_kubeconfig" {
  description = "Kubeconfig for EKS Cluster"
  value       = module.eks.kubeconfig
}
