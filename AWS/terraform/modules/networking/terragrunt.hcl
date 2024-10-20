include {
  path = find_in_parent_folders()
}

terraform {
  source = "../../../modules/networking"
}

inputs = {
  project_name = local.project_name
  aws_region   = local.aws_region
  # Add other necessary variables here
}
