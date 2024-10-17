# terraform/providers.tf

provider "aws" {
  region = var.aws_region
  # Optionally, specify profile if using AWS CLI profiles
  # profile = var.aws_profile
}

terraform {
  required_version = ">= 1.0.0"

  required_providers {
    aws = {
      source  = "hashicorp/aws"
      version = "~> 4.0"
    }
  }
}
