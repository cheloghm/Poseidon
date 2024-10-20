locals {
  project_name = "poseidon"
  aws_region   = "us-west-2"
}

remote_state {
  backend = "s3"
  config = {
    bucket         = "poseidon-terraform-state"
    key            = "${path_relative_to_include()}/terraform.tfstate"
    region         = local.aws_region
    encrypt        = true
    dynamodb_table = "poseidon-terraform-lock"
  }
}
