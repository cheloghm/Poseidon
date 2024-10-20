resource "aws_security_group" "eks_cluster_sg" {
  name        = "${var.project_name}-eks-sg"
  description = "EKS Cluster security group"
  vpc_id      = module.vpc.vpc_id

  ingress {
    from_port   = 443
    to_port     = 443
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"] # Adjust as needed for your use case
  }

  ingress {
    from_port   = 80
    to_port     = 80
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"] # Adjust as needed for your use case
  }

  egress {
    from_port   = 0
    to_port     = 0
    protocol    = "-1"
    cidr_blocks = ["0.0.0.0/0"]
  }

  tags = {
    Name = "${var.project_name}-eks-sg"
  }
}

output "eks_cluster_sg_id" {
  description = "Security Group ID for the EKS Cluster"
  value       = aws_security_group.eks_cluster_sg.id
}
