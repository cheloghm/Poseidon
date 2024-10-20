include {
  path = find_in_parent_folders()
}

terraform {
  source = "../../modules/eks"
}

inputs = {
  aws_region        = local.project_name
  project_name      = local.project_name
  cluster_name      = "poseidon-eks-cluster"
  kubernetes_version = "1.21"
  vpc_cidr          = "10.0.0.0/16"
  public_subnets    = ["10.0.1.0/24", "10.0.2.0/24", "10.0.3.0/24"]
  private_subnets   = ["10.0.101.0/24", "10.0.102.0/24", "10.0.103.0/24"]
  availability_zones = ["us-west-2a", "us-west-2b", "us-west-2c"]
  key_name          = "your-ssh-key"  # Replace with your SSH key name
}

# terragrunt.hcl in terraform/environments/prod/

include {
  path = find_in_parent_folders()
}

terraform {
  source = "../../modules/networking"
}

inputs = {
  project_name = "poseidon"
  # Add other necessary variables here
}
